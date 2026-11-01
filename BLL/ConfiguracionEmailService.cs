using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Net;
using System.Net.Mail;

namespace BLL
{
    // Cuenta SMTP de los avisos. Vive en la tabla ConfiguracionEmail; solo Admin y Gerente la modifican.
    public class ConfiguracionEmailService : IConfiguracionEmailService
    {
        private const string PERMISO = "CONFIGURAR_CUENTA_EMAIL";

        private readonly MapperConfiguracionEmail _mapper;
        private readonly BitacoraService _bitacora;

        public ConfiguracionEmailService()
        {
            _mapper = new MapperConfiguracionEmail();
            _bitacora = new BitacoraService();
        }

        public ConfiguracionEmail Obtener() => _mapper.Obtener();

        public void Actualizar(ConfiguracionEmail configuracion)
        {
            RequerirAdminOPermiso(PERMISO, "ActualizarConfiguracionEmail");
            Validar(configuracion);
            _mapper.Actualizar(configuracion);
            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.ConfiguracionEmail,
                $"Cuenta de email actualizada: {configuracion.Host}:{configuracion.Puerto} "
                + $"(SSL {(configuracion.Ssl ? "sí" : "no")}), remitente {configuracion.Remitente}.");
        }

        public void EnviarPrueba(ConfiguracionEmail configuracion, string destinatario)
        {
            RequerirAdminOPermiso(PERMISO, "EnviarPruebaEmail");
            Validar(configuracion);
            if (!EsEmailValido(destinatario))
                throw new BLLException("ERR_EMAIL_DESTINO_INVALIDO", "Ingresá un email de destino válido para la prueba.");

            try
            {
                using (SmtpClient smtp = CrearClienteSmtp(configuracion))
                using (var mail = new MailMessage(new MailAddress(configuracion.Remitente, "Avanti Auto"), new MailAddress(destinatario.Trim())))
                {
                    mail.Subject = "Avanti Auto — correo de prueba";
                    mail.Body = "Si recibiste este correo, la cuenta de email de Avanti Auto está bien configurada.";
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                string detalle = ex.InnerException?.Message ?? ex.Message;
                _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error,
                    $"Falló el correo de prueba a {destinatario}: {detalle}");
                throw new BLLException("ERR_EMAIL_PRUEBA", "No se pudo enviar el correo de prueba: {0}", detalle);
            }
            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.ConfiguracionEmail,
                $"Correo de prueba enviado a {destinatario} desde {configuracion.Host}:{configuracion.Puerto}.");
        }

        // Usado también por NotificacionService para los avisos.
        internal static SmtpClient CrearClienteSmtp(ConfiguracionEmail c)
        {
            var smtp = new SmtpClient(c.Host, c.Puerto)
            {
                EnableSsl = c.Ssl,
                Timeout = c.TimeoutMs,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            if (!string.IsNullOrWhiteSpace(c.Usuario))
                smtp.Credentials = new NetworkCredential(c.Usuario, c.Password);
            return smtp;
        }

        private static void Validar(ConfiguracionEmail c)
        {
            if (c == null || string.IsNullOrWhiteSpace(c.Host))
                throw new BLLException("ERR_EMAIL_HOST_REQUERIDO", "Ingresá el servidor SMTP.");
            if (c.Puerto < 1 || c.Puerto > 65535)
                throw new BLLException("ERR_EMAIL_PUERTO_INVALIDO", "El puerto debe estar entre 1 y 65535.");
            if (c.TimeoutMs < 1000 || c.TimeoutMs > 120000)
                throw new BLLException("ERR_EMAIL_TIMEOUT_INVALIDO", "El tiempo de espera debe estar entre 1000 y 120000 ms.");
            if (!EsEmailValido(c.Remitente))
                throw new BLLException("ERR_EMAIL_REMITENTE_INVALIDO", "Ingresá un email remitente válido.");

            c.Host = c.Host.Trim();
            c.Remitente = c.Remitente.Trim();
            c.Usuario = string.IsNullOrWhiteSpace(c.Usuario) ? null : c.Usuario.Trim();
        }

        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try { return new MailAddress(email.Trim()).Address == email.Trim(); }
            catch (FormatException) { return false; }
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;
            if (new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;
            _bitacora.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
