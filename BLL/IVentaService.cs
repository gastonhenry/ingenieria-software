using BE;
using System;
using System.Collections.Generic;

namespace BLL
{
    public interface IVentaService
    {
        VentaUnidad ObtenerActiva(int idUnidad);
        List<VentaUnidad> ListarPorUnidad(int idUnidad);

        // Vender: EnVenta | Reservado → Vendido
        void Vender(int idUnidad, int idPersonaComprador, DateTime fechaOperacion,
                    decimal precioFinal, string descripcion);

        // Reservar: EnVenta → Reservado
        void Reservar(int idUnidad, int idPersonaComprador, DateTime fechaOperacion,
                      decimal precioAcordado, decimal montoSena, DateTime fechaEstimadaFin, string descripcion);

        // Cancelar reserva: Reservado → EnVenta (operación queda inactiva en el historial)
        void CancelarReserva(int idUnidad, string comentario);

        // Pausar: EnVenta → Pausado (comentario obligatorio)
        void Pausar(int idUnidad, string motivo);

        // Reanudar: Pausado → EnVenta
        void Reanudar(int idUnidad);
    }
}
