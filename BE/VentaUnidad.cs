using System;
using BE.Enums;

namespace BE
{
    // Operación comercial sobre una unidad (venta o reserva).
    // Si Tipo = Reserva y la reserva se cae, Activa pasa a false y queda registrada
    // en el historial de operaciones.
    public class VentaUnidad
    {
        public int Id { get; set; }
        public int IdUnidad { get; set; }
        public TipoOperacionVenta Tipo { get; set; }
        public bool Activa { get; set; }
        public int IdPersonaComprador { get; set; }
        public int IdVendedorUsuario { get; set; }
        public DateTime FechaOperacion { get; set; }

        // Venta: precio final al que se concretó.
        // Reserva: precio total acordado para la futura venta.
        public decimal PrecioAcordado { get; set; }

        // Solo para Reserva: monto entregado como seña y fecha estimada de finalización.
        public decimal? MontoSena { get; set; }
        public DateTime? FechaEstimadaFin { get; set; }

        public string Descripcion { get; set; }
        public string ComentarioCancelacion { get; set; }
    }
}
