using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BLL
{
    // Envía los avisos por SMTP. La cuenta vive en la tabla ConfiguracionEmail (Mantenimiento → Configurar cuenta email).
    public class NotificacionService : INotificacionService
    {
        private static readonly CultureInfo CulturaAr = new CultureInfo("es-AR");

        private readonly MapperCaptacion _mapperCaptacion;
        private readonly MapperConfiguracionEmail _mapperEmail;
        private readonly CaptacionService _captacionService;
        private readonly PublicacionPdfService _pdfService;
        private readonly BitacoraService _bitacora;

        public NotificacionService()
        {
            _mapperCaptacion = new MapperCaptacion();
            _mapperEmail = new MapperConfiguracionEmail();
            _captacionService = new CaptacionService();
            _pdfService = new PublicacionPdfService();
            _bitacora = new BitacoraService();
        }

        public ResultadoNotificacion NotificarUnidad(int idUnidad)
        {
            var resultado = new ResultadoNotificacion();
            var usuario = SesionUsuario.GetInstancia().Usuario;

            // RN-N03-06: interruptor global.
            if (!_mapperCaptacion.ObtenerParametros().NotificacionesActivas)
            {
                resultado.EnvioDesactivado = true;
                _bitacora.Insertar(usuario, TipoBitacora.AvisoCliente,
                    $"Unidad Id {idUnidad} pasó a En venta: no se enviaron avisos (envío automático desactivado).");
                return resultado;
            }

            _captacionService.VencerBusquedas();
            var unidad = _mapperCaptacion.ObtenerUnidad(idUnidad);
            if (unidad == null) return resultado;

            // Un aviso por cliente: se toma la primera búsqueda coincidente que acepta avisos.
            var destinatarios = _captacionService.ListarBusquedasActivas()
                .Where(b => CaptacionService.Coincide(b, unidad))
                .Where(b => b.AceptaAvisos && !b.AvisosSuspendidos && !string.IsNullOrWhiteSpace(b.Cliente.Email))
                .GroupBy(b => b.Cliente.Id)
                .Select(g => g.OrderByDescending(b => b.FechaRegistro).First())
                .Where(b => !_mapperCaptacion.ExisteNotificacionExitosa(b.Cliente.Id, idUnidad))
                .ToList();
            if (destinatarios.Count == 0) return resultado;

            string pdf = null;
            try { pdf = _pdfService.GenerarTemporal(idUnidad); }
            catch (Exception ex)
            {
                _bitacora.Insertar(usuario, TipoBitacora.Error,
                    $"No se pudo generar el PDF de la unidad Id {idUnidad} para los avisos: {ex.Message}");
            }

            decimal? precio = unidad.PrecioPublicacion;
            ConfiguracionEmail cuenta = _mapperEmail.Obtener();
            try
            {
                using (SmtpClient smtp = ConfiguracionEmailService.CrearClienteSmtp(cuenta))
                {
                    foreach (var b in destinatarios)
                    {
                        var aviso = new NotificacionEnviada { IdBusqueda = b.Id, IdPersona = b.Cliente.Id, IdUnidad = idUnidad };
                        try
                        {
                            using (MailMessage mail = CrearMail(b, unidad, precio, pdf, cuenta.Remitente))
                                smtp.Send(mail);
                            aviso.Exitoso = true;
                            resultado.Enviados++;
                        }
                        catch (Exception ex)
                        {
                            // RN-N03-11: la falla queda registrada y no afecta la publicación.
                            aviso.Exitoso = false;
                            aviso.Error = ex.InnerException?.Message ?? ex.Message;
                            resultado.Fallidos++;
                            _bitacora.Insertar(usuario, TipoBitacora.Error,
                                $"Falló el aviso a {b.Cliente.Nombre} ({b.Cliente.Email}) por la unidad Id {idUnidad}: {aviso.Error}");
                        }
                        _mapperCaptacion.InsertarNotificacion(aviso);
                    }
                }
            }
            finally
            {
                if (pdf != null) { try { File.Delete(pdf); } catch { } }
            }

            _bitacora.Insertar(usuario, TipoBitacora.AvisoCliente,
                $"Unidad {unidad} pasó a En venta: {resultado.Enviados} aviso(s) enviado(s), {resultado.Fallidos} fallido(s).");
            return resultado;
        }

        private static MailMessage CrearMail(BusquedaCliente b, UnidadEnVenta u, decimal? precio, string rutaPdf, string remitente)
        {
            string vehiculo = $"{u.Marca} {u.Modelo} {u.Anio}";
            string precioTexto = precio.HasValue ? precio.Value.ToString("C0", CulturaAr) : "Precio a consultar";

            var cuerpo = new StringBuilder();
            cuerpo.Append("<div style='font-family:Segoe UI,Arial,sans-serif;font-size:14px;color:#222'>");
            cuerpo.Append($"<p>Hola {WebUtility.HtmlEncode(b.Cliente.Nombre)},</p>");
            cuerpo.Append("<p>Ingresó a <b>Avanti Auto</b> un vehículo que coincide con lo que estás buscando:</p>");
            cuerpo.Append("<table style='border-collapse:collapse'>");
            cuerpo.Append($"<tr><td style='padding:4px 12px 4px 0'><b>Vehículo</b></td><td>{WebUtility.HtmlEncode(vehiculo)}</td></tr>");
            cuerpo.Append($"<tr><td style='padding:4px 12px 4px 0'><b>Kilometraje</b></td><td>{u.Kilometraje.ToString("N0", CulturaAr)} km</td></tr>");
            cuerpo.Append($"<tr><td style='padding:4px 12px 4px 0'><b>Precio</b></td><td>{precioTexto}</td></tr>");
            cuerpo.Append("</table>");
            if (rutaPdf != null) cuerpo.Append("<p>Te adjuntamos la publicación completa con fotos.</p>");
            cuerpo.Append("<p>Si te interesa, respondé este correo o acercate a la concesionaria.</p>");
            cuerpo.Append("<p style='color:#888;font-size:12px'>Recibís este aviso porque lo aceptaste en tu entrevista. "
                        + "Si no querés recibir más avisos, avisanos y los suspendemos.</p>");
            cuerpo.Append("</div>");

            var mail = new MailMessage(remitente, b.Cliente.Email)
            {
                Subject = $"Avanti Auto: hay un {vehiculo} que coincide con tu búsqueda",
                Body = cuerpo.ToString(),
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };
            if (rutaPdf != null)
                mail.Attachments.Add(new Attachment(rutaPdf, "application/pdf")
                {
                    Name = Path.GetFileName(rutaPdf).Substring(33)     // quita el prefijo "{guid}_"
                });
            return mail;
        }
    }
}
