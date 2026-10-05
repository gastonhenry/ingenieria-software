using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class VentaService : IVentaService
    {
        private readonly MapperVentaUnidad _mapperVenta;
        private readonly MapperUnidad _mapperUnidad;
        private readonly UnidadService _unidadService;
        private readonly BitacoraService _bitacora;

        public VentaService()
        {
            _mapperVenta = new MapperVentaUnidad();
            _mapperUnidad = new MapperUnidad();
            _unidadService = new UnidadService();
            _bitacora = new BitacoraService();
        }

        public VentaUnidad ObtenerActiva(int idUnidad) => _mapperVenta.ObtenerActivaDeUnidad(idUnidad);

        public List<VentaUnidad> ListarPorUnidad(int idUnidad) => _mapperVenta.ListarPorUnidad(idUnidad);

        public void Vender(int idUnidad, int idPersonaComprador, DateTime fechaOperacion,
                           decimal precioFinal, string descripcion)
        {
            RequerirAdminOPermiso("REGISTRAR_VENTA", "Vender");

            var unidad = ObtenerOFallar(idUnidad);
            if (unidad.EstadoActual != EstadoUnidad.EnVenta && unidad.EstadoActual != EstadoUnidad.Reservado)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede vender una unidad en estado En venta o Reservada.");

            if (precioFinal <= 0)
                throw new BLLException("ERR_PRECIO_INVALIDO", "El precio final debe ser mayor a cero.");
            if (idPersonaComprador <= 0)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná una persona compradora.");

            // Si venía de Reservado, cancelo la reserva activa (queda registrada como inactiva).
            if (unidad.EstadoActual == EstadoUnidad.Reservado)
                _mapperVenta.CancelarVentaActiva(idUnidad, "Reserva concretada en venta.");

            _mapperVenta.Insertar(new VentaUnidad
            {
                IdUnidad = idUnidad,
                Tipo = TipoOperacionVenta.Venta,
                IdPersonaComprador = idPersonaComprador,
                IdVendedorUsuario = SesionUsuario.GetInstancia().Usuario.Id,
                FechaOperacion = fechaOperacion,
                PrecioAcordado = precioFinal,
                Descripcion = descripcion
            });

            _unidadService.TransicionarConHistorial(idUnidad, unidad.EstadoActual, EstadoUnidad.Vendido,
                $"Venta registrada por ${precioFinal:N2}.", motivoObligatorio: false);
        }

        public void Reservar(int idUnidad, int idPersonaComprador, DateTime fechaOperacion,
                             decimal precioAcordado, decimal montoSena, DateTime fechaEstimadaFin, string descripcion)
        {
            RequerirAdminOPermiso("GESTIONAR_RESERVA", "Reservar");

            var unidad = ObtenerOFallar(idUnidad);
            if (unidad.EstadoActual != EstadoUnidad.EnVenta)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede reservar una unidad en estado En venta.");
            if (precioAcordado <= 0)
                throw new BLLException("ERR_PRECIO_INVALIDO", "El precio acordado debe ser mayor a cero.");
            if (montoSena <= 0 || montoSena > precioAcordado)
                throw new BLLException("ERR_SENA_INVALIDA", "La seña debe ser mayor a cero y menor o igual al precio acordado.");
            if (fechaEstimadaFin.Date < fechaOperacion.Date)
                throw new BLLException("ERR_FECHA_FIN_INVALIDA", "La fecha estimada de fin no puede ser anterior a la fecha de la operación.");
            if (idPersonaComprador <= 0)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná una persona compradora.");

            _mapperVenta.Insertar(new VentaUnidad
            {
                IdUnidad = idUnidad,
                Tipo = TipoOperacionVenta.Reserva,
                IdPersonaComprador = idPersonaComprador,
                IdVendedorUsuario = SesionUsuario.GetInstancia().Usuario.Id,
                FechaOperacion = fechaOperacion,
                PrecioAcordado = precioAcordado,
                MontoSena = montoSena,
                FechaEstimadaFin = fechaEstimadaFin,
                Descripcion = descripcion
            });

            _unidadService.TransicionarConHistorial(idUnidad, EstadoUnidad.EnVenta, EstadoUnidad.Reservado,
                $"Reserva registrada. Seña ${montoSena:N2} sobre ${precioAcordado:N2}.", motivoObligatorio: false);
        }

        public void CancelarReserva(int idUnidad, string comentario)
        {
            RequerirAdminOPermiso("GESTIONAR_RESERVA", "CancelarReserva");

            var unidad = ObtenerOFallar(idUnidad);
            if (unidad.EstadoActual != EstadoUnidad.Reservado)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede cancelar una reserva de una unidad en estado Reservada.");

            _mapperVenta.CancelarVentaActiva(idUnidad, comentario);

            _unidadService.TransicionarConHistorial(idUnidad, EstadoUnidad.Reservado, EstadoUnidad.EnVenta,
                string.IsNullOrWhiteSpace(comentario)
                    ? "Reserva cancelada."
                    : $"Reserva cancelada. {comentario}",
                motivoObligatorio: false);
        }

        public void Pausar(int idUnidad, string motivo)
        {
            RequerirAdminOPermiso("PAUSAR_UNIDAD", "Pausar");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new BLLException("ERR_MOTIVO_PAUSA_OBLIGATORIO",
                    "El motivo de la pausa es obligatorio.");

            var unidad = ObtenerOFallar(idUnidad);
            if (unidad.EstadoActual != EstadoUnidad.EnVenta)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede pausar una unidad en estado En venta.");

            _unidadService.TransicionarConHistorial(idUnidad, EstadoUnidad.EnVenta, EstadoUnidad.Pausado,
                motivo.Trim(), motivoObligatorio: true);
        }

        public void Reanudar(int idUnidad)
        {
            RequerirAdminOPermiso("PAUSAR_UNIDAD", "Reanudar");

            var unidad = ObtenerOFallar(idUnidad);
            if (unidad.EstadoActual != EstadoUnidad.Pausado)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede reanudar una unidad en estado Pausada.");

            _unidadService.TransicionarConHistorial(idUnidad, EstadoUnidad.Pausado, EstadoUnidad.EnVenta,
                "Unidad reanudada.", motivoObligatorio: false);
        }

        private Unidad ObtenerOFallar(int idUnidad)
        {
            var u = _mapperUnidad.Obtener(idUnidad);
            if (u == null) throw new BLLException("ERR_UNIDAD_NO_ENCONTRADA", "No se encontró la unidad.");
            return u;
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;
            if (new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;
            _bitacora.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
