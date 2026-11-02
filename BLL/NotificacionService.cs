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

        // Envío manual desde la entrevista: la publicación (PDF) de una unidad a un cliente, en el momento.
        // No pasa por las búsquedas ni por el interruptor de avisos automáticos; queda en la bitácora.
        public void EnviarPublicacion(int idUnidad, int idPersona)
        {
            RequerirAdminOPermiso("VER_PUBLICACIONES", "EnviarPublicacionEmail");
            var usuario = SesionUsuario.GetInstancia().Usuario;

            Persona cliente = idPersona > 0 ? new PersonaService().ObtenerPorId(idPersona) : null;
            if (cliente == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná el cliente.");
            if (string.IsNullOrWhiteSpace(cliente.Email))
                throw new BLLException("ERR_CLIENTE_SIN_EMAIL_ENVIO", "El cliente no tiene un correo electrónico registrado.");

            var unidad = _mapperCaptacion.ObtenerUnidad(idUnidad);
            if (unidad == null)
                throw new BLLException("ERR_UNIDAD_NO_ENCONTRADA", "No se encontró la unidad.");

            string pdf = _pdfService.GenerarTemporal(idUnidad);
            if (pdf == null)
                throw new BLLException("ERR_UNIDAD_SIN_PUBLICACION", "La unidad no tiene una publicación creada.");

            try
            {
                ConfiguracionEmail cuenta = _mapperEmail.Obtener();
                using (SmtpClient smtp = ConfiguracionEmailService.CrearClienteSmtp(cuenta))
                using (MailMessage mail = CrearMail(cliente, unidad, unidad.PrecioPublicacion, pdf, cuenta.Remitente, false))
                    smtp.Send(mail);
            }
            catch (Exception ex)
            {
                string detalle = ex.InnerException?.Message ?? ex.Message;
                _bitacora.Insertar(usuario, TipoBitacora.Error,
                    $"Falló el envío de la publicación de {unidad} a {cliente.Nombre} ({cliente.Email}): {detalle}");
                throw new BLLException("ERR_ENVIO_PUBLICACION", "No se pudo enviar el correo: {0}", detalle);
            }
            finally
            {
                try { File.Delete(pdf); } catch { }
            }

            _bitacora.Insertar(usuario, TipoBitacora.AvisoCliente,
                $"Publicación de {unidad} enviada por email a {cliente.Nombre} ({cliente.Email}).");
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

        private static MailMessage CrearMail(BusquedaCliente b, UnidadEnVenta u, decimal? precio, string rutaPdf, string remitente) =>
            CrearMail(b.Cliente, u, precio, rutaPdf, remitente, true);

        // esAviso: aviso automático por búsqueda (true) o envío manual desde la entrevista (false).
        private static MailMessage CrearMail(Persona cliente, UnidadEnVenta u, decimal? precio, string rutaPdf, string remitente, bool esAviso)
        {
            string vehiculo = $"{u.Marca} {u.Modelo} {u.Anio}";
            string precioTexto = precio.HasValue ? precio.Value.ToString("C0", CulturaAr) : "Precio a consultar";

            var cuerpo = new StringBuilder();
            cuerpo.Append("<div style='font-family:Segoe UI,Arial,sans-serif;font-size:14px;color:#222'>");
            cuerpo.Append($"<p>Hola {WebUtility.HtmlEncode(cliente.Nombre)},</p>");
            cuerpo.Append(esAviso
                ? "<p>Ingresó a <b>Avanti Auto</b> un vehículo que coincide con lo que estás buscando:</p>"
                : "<p>Te enviamos desde <b>Avanti Auto</b> la publicación del vehículo que conversamos:</p>");
            cuerpo.Append("<table style='border-collapse:collapse'>");
            cuerpo.Append($"<tr><td style='padding:4px 12px 4px 0'><b>Vehículo</b></td><td>{WebUtility.HtmlEncode(vehiculo)}</td></tr>");
            cuerpo.Append($"<tr><td style='padding:4px 12px 4px 0'><b>Kilometraje</b></td><td>{u.Kilometraje.ToString("N0", CulturaAr)} km</td></tr>");
            cuerpo.Append($"<tr><td style='padding:4px 12px 4px 0'><b>Precio</b></td><td>{precioTexto}</td></tr>");
            cuerpo.Append("</table>");
            if (rutaPdf != null) cuerpo.Append("<p>Te adjuntamos la publicación completa con fotos.</p>");
            cuerpo.Append("<p>Si te interesa, respondé este correo o acercate a la concesionaria.</p>");
            if (esAviso)
                cuerpo.Append("<p style='color:#888;font-size:12px'>Recibís este aviso porque lo aceptaste en tu entrevista. "
                            + "Si no querés recibir más avisos, avisanos y los suspendemos.</p>");
            cuerpo.Append("</div>");

            var mail = new MailMessage(remitente, cliente.Email)
            {
                Subject = esAviso ? $"Avanti Auto: hay un {vehiculo} que coincide con tu búsqueda"
                                  : $"Avanti Auto: publicación del {vehiculo}",
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
