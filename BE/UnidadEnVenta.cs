using BE.Enums;
using System;

namespace BE
{
    // Vista plana de una unidad En venta con los datos necesarios para el matching de N03.
    public class UnidadEnVenta
    {
        public int IdUnidad { get; set; }
        public string Dominio { get; set; }
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public int IdModelo { get; set; }
        public string Modelo { get; set; }
        public TipoCarroceria TipoCarroceria { get; set; }
        public int Anio { get; set; }
        public int Kilometraje { get; set; }
        public decimal? PrecioPublicacion { get; set; }
        public DateTime FechaEnVenta { get; set; }

        public override string ToString() => $"{Marca} {Modelo} {Anio} ({Dominio})";
    }
}
