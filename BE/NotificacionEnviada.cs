using System;

namespace BE
{
    // Aviso por mail enviado (o intentado) a un cliente por una unidad en venta (N03 — CU-CAP-03).
    public class NotificacionEnviada
    {
        public int Id { get; set; }
        public int IdBusqueda { get; set; }
        public int IdPersona { get; set; }
        public int IdUnidad { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Exitoso { get; set; }
        public string Error { get; set; }

        // Datos de visualización (vienen del JOIN en el listado).
        public string ClienteNombre { get; set; }
        public string ClienteDocumento { get; set; }
        public string ClienteEmail { get; set; }
        public string Unidad { get; set; }
    }
}
