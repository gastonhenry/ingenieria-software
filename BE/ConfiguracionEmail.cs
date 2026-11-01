namespace BE
{
    // Cuenta SMTP desde la que salen los avisos a clientes (registro único, la administran Admin y Gerente).
    public class ConfiguracionEmail
    {
        public string Host { get; set; }
        public int Puerto { get; set; }
        public bool Ssl { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int TimeoutMs { get; set; }
        public string Remitente { get; set; }
    }
}
