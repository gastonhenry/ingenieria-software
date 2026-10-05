using System;
using BE.Enums;

namespace BE
{
    // Vista plana de una publicación (datos de la Unidad + datos comerciales) para el listado de Publicaciones.
    public class PublicacionListado
    {
        public int IdUnidad { get; set; }
        public string Dominio { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public int Kilometraje { get; set; }
        public EstadoUnidad EstadoActual { get; set; }
        public decimal? PrecioPublicacion { get; set; }
        public string DescripcionPublicacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimaEdicion { get; set; }
        public int CantidadImagenes { get; set; }

        public DateTime FechaActualizacion => FechaUltimaEdicion ?? FechaCreacion;
    }
}
