namespace BE
{
    // Resumen del envío de avisos al pasar una unidad a En venta (N03 — CU-CAP-03).
    public class ResultadoNotificacion
    {
        public bool EnvioDesactivado { get; set; }
        public int Enviados { get; set; }
        public int Fallidos { get; set; }
        // Error general (ej. no se pudo leer la configuración); la publicación igual quedó hecha.
        public string Error { get; set; }
    }
}
