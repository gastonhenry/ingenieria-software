using System;

namespace BE
{
    public class ImagenUnidad
    {
        public int Id { get; set; }
        public string RutaArchivo { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCarga { get; set; }
    }
}
