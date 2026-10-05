using System;

namespace BE
{
    // Datos comerciales (precio, descripción) de la unidad para su publicación.
    // Vida 1-1 con Unidad; se crea al pasar a PendienteVenta y el Gerente la edita.
    public class PublicacionUnidad
    {
        public int Id { get; set; }
        public int IdUnidad { get; set; }
        public decimal? PrecioPublicacion { get; set; }
        public string DescripcionPublicacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimaEdicion { get; set; }
    }
}
