namespace BE
{
    // Configuración de N03 (registro único, la administra el Gerente).
    public class ParametrosCaptacion
    {
        // Interruptor global del envío automático de avisos.
        public bool NotificacionesActivas { get; set; }
        public int DiasVigenciaBusqueda { get; set; }
        // A partir de cuántos días publicada una unidad sin interesados se marca como alerta.
        public int DiasAlertaSinInteresados { get; set; }
    }
}
