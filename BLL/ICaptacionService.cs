using BE;
using BE.Enums;
using System;
using System.Collections.Generic;

namespace BLL
{
    // N03 — Gestión de captación de clientes potenciales.
    public interface ICaptacionService
    {
        // CU-CAP-01: registra la búsqueda del cliente y devuelve su Id.
        // Marcas, modelos y carrocerías son multiselección; lista vacía = cualquiera.
        int RegistrarEntrevista(int idPersona, List<int> idsMarca, List<int> idsModelo, List<TipoCarroceria> carrocerias,
                                int? anioDesde, int? anioHasta, decimal? precioMaximo,
                                string observaciones, bool aceptaAvisos);

        // Unidades En venta que coinciden con una búsqueda (paso 7 de CU-CAP-01).
        List<UnidadEnVenta> ListarUnidadesCoincidentes(int idBusqueda);

        // CU-CAP-02
        TableroCaptacion ObtenerTablero(DateTime desde, DateTime hasta);

        // CU-CAP-05
        List<Persona> BuscarClientes(string texto);      // por DNI/CUIT o nombre (parcial)
        List<BusquedaCliente> ListarBusquedasDeCliente(int idPersona);
        List<NotificacionEnviada> ListarAvisosDeCliente(int idPersona);
        SuspensionAvisos ObtenerSuspension(int idPersona);
        void SuspenderAvisos(int idPersona, string motivo);
        void ReanudarAvisos(int idPersona);
        void CerrarBusqueda(int idBusqueda, string motivo);

        // Configuración (Gerente)
        ParametrosCaptacion ObtenerParametros();
        void ActualizarParametros(bool notificacionesActivas, int diasVigenciaBusqueda, int diasAlertaSinInteresados);
    }
}
