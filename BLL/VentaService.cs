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
        private readonly PersonaService _personaService;
        private readonly BitacoraService _bitacora;

        public VentaService()
        {
            _mapperVenta = new MapperVentaUnidad();
            _mapperUnidad = new MapperUnidad();
            _unidadService = new UnidadService();
            _personaService = new PersonaService();
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
            var comprador = ObtenerCompradorOFallar(idPersonaComprador);

            // Si venía de Reservado, cancelo la reserva activa (queda registrada como inactiva).
            if (unidad.EstadoActual == EstadoUnidad.Reservado)
                _mapperVenta.CancelarVentaActiva(idUnidad, "Reserva concretada en venta.");

            _mapperVenta.Insertar(idUnidad, new VentaUnidad
            {
                Tipo = TipoOperacionVenta.Venta,
                Comprador = comprador,
                Vendedor = SesionUsuario.GetInstancia().Usuario,
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
            var comprador = ObtenerCompradorOFallar(idPersonaComprador);

            _mapperVenta.Insertar(idUnidad, new VentaUnidad
            {
                Tipo = TipoOperacionVenta.Reserva,
                Comprador = comprador,
                Vendedor = SesionUsuario.GetInstancia().Usuario,
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

            if (string.IsNullOrWhiteSpace(comentario))
                throw new BLLException("ERR_MOTIVO_CANCELACION_OBLIGATORIO",
                    "El motivo de la cancelación de la reserva es obligatorio.");

            var unidad = ObtenerOFallar(idUnidad);
            if (unidad.EstadoActual != EstadoUnidad.Reservado)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede cancelar una reserva de una unidad en estado Reservada.");

            _mapperVenta.CancelarVentaActiva(idUnidad, comentario.Trim());

            _unidadService.TransicionarConHistorial(idUnidad, EstadoUnidad.Reservado, EstadoUnidad.EnVenta,
                $"Reserva cancelada. {comentario.Trim()}", motivoObligatorio: true);
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

        private Persona ObtenerCompradorOFallar(int idPersonaComprador)
        {
            var persona = idPersonaComprador > 0 ? _personaService.ObtenerPorId(idPersonaComprador) : null;
            if (persona == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná una persona compradora.");
            return persona;
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
