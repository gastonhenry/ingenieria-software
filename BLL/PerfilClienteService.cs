using BE;
using BE.Enums;
using HELPERS;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BLL
{
    public class PerfilClienteService : IPerfilClienteService
    {
        private static readonly XmlSerializer Serializador = new XmlSerializer(typeof(PerfilCliente));

        private readonly PersonaService _personaService;
        private readonly BitacoraService _bitacora;

        public PerfilClienteService()
        {
            _personaService = new PersonaService();
            _bitacora = new BitacoraService();
        }

        public void Exportar(int idPersona, string rutaDestino)
        {
            RequerirAdminOPermiso("GESTIONAR_PERSONAS", "ExportarPerfilCliente");
            var persona = idPersona > 0 ? _personaService.ObtenerPorId(idPersona) : null;
            if (persona == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná una persona.");

            var usuario = SesionUsuario.GetInstancia().Usuario;
            var perfil = new PerfilCliente
            {
                Version = PerfilCliente.VERSION_ACTUAL,
                FechaExportacion = DateTime.Now,
                ExportadoPor = usuario?.Username,
                Persona = new PersonaPerfil
                {
                    TipoPersona     = persona.TipoPersona.ToString(),
                    Documento       = persona.Documento,
                    Nombre          = persona.Nombre,
                    Domicilio       = persona.Domicilio,
                    Telefono        = persona.Telefono,
                    Email           = persona.Email,
                    EstadoCivil     = persona.EstadoCivil.ToString(),
                    FechaNacimiento = persona.FechaNacimiento
                }
            };

            try
            {
                var opciones = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(true) };
                using (var writer = XmlWriter.Create(rutaDestino, opciones))
                    Serializador.Serialize(writer, perfil);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new BLLException("ERR_ARCHIVO_PERFIL", "No se pudo acceder al archivo del perfil: {0}", ex.Message);
            }

            _bitacora.Insertar(usuario, TipoBitacora.PerfilCliente,
                $"Perfil exportado: {persona.Nombre} ({persona.Documento}).");
        }

        public string NombreArchivoSugerido(int idPersona)
        {
            var persona = _personaService.ObtenerPorId(idPersona);
            string nombre = $"Perfil_{persona?.Documento ?? idPersona.ToString()}.xml";
            foreach (char c in Path.GetInvalidFileNameChars()) nombre = nombre.Replace(c, '_');
            return nombre;
        }

        // Si no existe una persona con ese DNI/CUIT se da de alta. Si existe y el archivo trae otros
        // datos, se actualiza con los del archivo. Ambos casos usan las validaciones del alta/edición manual.
        public ResultadoImportacionPerfil Importar(string rutaOrigen)
        {
            RequerirAdminOPermiso("GESTIONAR_PERSONAS", "ImportarPerfilCliente");
            var p = Leer(rutaOrigen).Persona;
            var resultado = new ResultadoImportacionPerfil();

            char tipo = string.IsNullOrEmpty(p.TipoPersona) ? 'F' : char.ToUpper(p.TipoPersona[0]);
            if (!Enum.TryParse(p.EstadoCivil, out EstadoCivil estadoCivil)) estadoCivil = EstadoCivil.Soltero;
            string documento = p.Documento.Trim();

            Persona persona = _personaService.BuscarPorDocumento(documento);
            if (persona == null)
            {
                int id = _personaService.Registrar(tipo, documento, p.Nombre, p.Domicilio, p.Telefono, p.Email,
                                                   estadoCivil, p.FechaNacimiento);
                persona = _personaService.ObtenerPorId(id);
                resultado.PersonaCreada = true;
            }
            else if (TieneCambios(persona, tipo, p, estadoCivil))
            {
                _personaService.Editar(persona.Id, tipo, documento, p.Nombre, p.Domicilio, p.Telefono, p.Email,
                                       estadoCivil, p.FechaNacimiento);
                persona = _personaService.ObtenerPorId(persona.Id);
                resultado.PersonaActualizada = true;
            }
            resultado.Persona = persona;

            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.PerfilCliente,
                $"Perfil importado: {persona.Nombre} ({persona.Documento}), "
                + (resultado.PersonaCreada ? "persona dada de alta."
                   : resultado.PersonaActualizada ? "datos actualizados con los del archivo."
                   : "la persona ya existía con los mismos datos."));
            return resultado;
        }

        private static bool TieneCambios(Persona actual, char tipo, PersonaPerfil archivo, EstadoCivil estadoCivil)
        {
            return actual.TipoPersona != tipo
                || Distinto(actual.Nombre, archivo.Nombre)
                || Distinto(actual.Domicilio, archivo.Domicilio)
                || Distinto(actual.Telefono, archivo.Telefono)
                || Distinto(actual.Email, archivo.Email)
                || actual.EstadoCivil != estadoCivil
                || actual.FechaNacimiento.Date != archivo.FechaNacimiento.Date;
        }

        // Compara textos ignorando espacios de los extremos; null y "" cuentan como iguales.
        private static bool Distinto(string a, string b) =>
            !string.Equals((a ?? "").Trim(), (b ?? "").Trim(), StringComparison.Ordinal);

        private static PerfilCliente Leer(string ruta)
        {
            PerfilCliente perfil;
            try
            {
                using (var reader = XmlReader.Create(ruta))
                {
                    if (!Serializador.CanDeserialize(reader))
                        throw new BLLException("ERR_PERFIL_INVALIDO", "El archivo no es un perfil de cliente válido.");
                    perfil = (PerfilCliente)Serializador.Deserialize(reader);
                }
            }
            catch (BLLException) { throw; }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new BLLException("ERR_ARCHIVO_PERFIL", "No se pudo acceder al archivo del perfil: {0}", ex.Message);
            }
            catch (Exception ex) when (ex is XmlException || ex is InvalidOperationException)
            {
                throw new BLLException("ERR_PERFIL_INVALIDO", "El archivo no es un perfil de cliente válido.");
            }

            if (perfil.Version != PerfilCliente.VERSION_ACTUAL)
                throw new BLLException("ERR_PERFIL_VERSION",
                    "El perfil fue generado con otra versión del formato ({0}); se esperaba {1}.",
                    perfil.Version ?? "?", PerfilCliente.VERSION_ACTUAL);
            if (perfil.Persona == null || string.IsNullOrWhiteSpace(perfil.Persona.Documento) || string.IsNullOrWhiteSpace(perfil.Persona.Nombre))
                throw new BLLException("ERR_PERFIL_INVALIDO", "El archivo no es un perfil de cliente válido.");
            return perfil;
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;
            if (usuario != null && new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;
            _bitacora.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
