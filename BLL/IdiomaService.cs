using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class IdiomaService : IIdiomaService
    {
        private readonly MapperIdioma _mapperIdioma;
        private readonly MapperControl _mapperControl;
        private readonly MapperTraduccion _mapperTraduccion;
        private readonly MapperUsuario _mapperUsuario;
        private readonly BitacoraService _bitacoraService;
        private readonly GestorIdiomas _gestor;

        public IdiomaService()
        {
            _mapperIdioma = new MapperIdioma();
            _mapperControl = new MapperControl();
            _mapperTraduccion = new MapperTraduccion();
            _mapperUsuario = new MapperUsuario();
            _bitacoraService = new BitacoraService();
            _gestor = GestorIdiomas.GetInstancia();
        }

        public List<Idioma> Listar()
        {
            var idiomas = _mapperIdioma.Listar();
            int totalControles = _mapperControl.Listar().Count;
            foreach (Idioma i in idiomas)
                i.Completo = EsCompleto(i.Id, totalControles);
            return idiomas;
        }

        public bool EstaCompleto(int idiomaId)
        {
            int totalControles = _mapperControl.Listar().Count;
            return EsCompleto(idiomaId, totalControles);
        }

        private bool EsCompleto(int idiomaId, int totalControles)
        {
            if (totalControles == 0) return true;
            int completas = 0;
            foreach (Traduccion t in _mapperTraduccion.ListarPorIdioma(idiomaId))
                if (!string.IsNullOrWhiteSpace(t.Texto)) completas++;
            return completas >= totalControles;
        }

        public int Crear(string nombre)
        {
            RequerirAdminOPermiso("GESTIONAR_IDIOMAS", "CrearIdioma");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BLLException("ERR_IDIOMA_NOMBRE_OBLIGATORIO", "El nombre del idioma es obligatorio.");

            nombre = nombre.Trim();

            if (ExisteNombre(nombre))
                throw new BLLException("ERR_IDIOMA_NOMBRE_DUPLICADO",
                    "Ya existe un idioma con el nombre '{0}'. Los nombres deben ser únicos.", nombre);

            var idioma = new Idioma { Nombre = nombre };
            int id = _mapperIdioma.Insertar(idioma);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaIdioma,
                $"Idioma creado: {nombre}");
            return id;
        }

        private const string IDIOMA_PROTEGIDO = "Español";

        public void Eliminar(int idiomaId)
        {
            RequerirAdminOPermiso("GESTIONAR_IDIOMAS", "EliminarIdioma");
            Idioma idioma = ObtenerPorId(idiomaId);
            if (idioma == null)
                throw new BLLException("ERR_IDIOMA_NO_EXISTE", "El idioma no existe.");

            if (string.Equals(idioma.Nombre, IDIOMA_PROTEGIDO, StringComparison.OrdinalIgnoreCase))
                throw new BLLException("ERR_IDIOMA_PROTEGIDO_BORRAR",
                    "El idioma '{0}' no puede eliminarse: es el idioma base del sistema.", IDIOMA_PROTEGIDO);

            _mapperIdioma.Eliminar(idioma);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaIdioma,
                $"Idioma eliminado: {idioma.Nombre}");
        }

        public void CambiarIdiomaActual(int idiomaId)
        {
            Idioma idioma = ObtenerPorId(idiomaId);
            if (idioma == null)
                throw new BLLException("ERR_IDIOMA_NO_EXISTE", "El idioma no existe.");

            if (!EstaCompleto(idiomaId))
                throw new BLLException("ERR_IDIOMA_INCOMPLETO",
                    "El idioma '{0}' está en proceso de creación. Completá todas las traducciones antes de activarlo.", idioma.Nombre);

            idioma.Completo = true;
            _gestor.CambiarIdioma(idioma);

            var usuario = SesionUsuario.GetInstancia().Usuario;
            if (usuario != null)
            {
                try { _mapperUsuario.ActualizarIdioma(usuario.Id, idiomaId); }
                catch (Exception) { }
                usuario.IdIdioma = idiomaId;
            }
        }

        public Idioma IdiomaActual() => _gestor.IdiomaActual;

        public void Suscribir(IObservadorIdioma observador) => _gestor.Suscribir(observador);

        public void Desuscribir(IObservadorIdioma observador) => _gestor.Desuscribir(observador);

        public string Traducir(string formCodigo, string controlCodigo) =>
            _gestor.Traducir(formCodigo, controlCodigo);

        public List<Traduccion> ListarTraduccionesParaEdicion(int idiomaId)
        {
            var controles = _mapperControl.Listar();
            var existentes = _mapperTraduccion.ListarPorIdioma(idiomaId);

            var indice = new Dictionary<int, Traduccion>();
            foreach (Traduccion t in existentes)
                indice[t.IdControl] = t;

            var resultado = new List<Traduccion>();
            foreach (Control c in controles)
            {
                if (indice.TryGetValue(c.Id, out Traduccion existente))
                {
                    existente.Control = c;
                    resultado.Add(existente);
                }
                else
                {
                    resultado.Add(new Traduccion
                    {
                        Id = 0,
                        IdIdioma = idiomaId,
                        IdControl = c.Id,
                        Texto = string.Empty,
                        Control = c
                    });
                }
            }
            return resultado;
        }

        public void GuardarTraduccion(int idiomaId, int controlId, string texto)
        {
            RequerirAdminOPermiso("GESTIONAR_IDIOMAS", "GuardarTraduccion");
            if (texto == null) texto = string.Empty;

            Idioma idioma = ObtenerPorId(idiomaId);
            if (idioma == null)
                throw new BLLException("ERR_IDIOMA_NO_EXISTE", "El idioma no existe.");

            bool esProtegido = string.Equals(idioma.Nombre, IDIOMA_PROTEGIDO, StringComparison.OrdinalIgnoreCase);
            if (esProtegido && string.IsNullOrWhiteSpace(texto))
                throw new BLLException("ERR_IDIOMA_PROTEGIDO_VACIO",
                    "El idioma '{0}' no admite traducciones vacías: es el idioma base del sistema y debe estar siempre completo.", IDIOMA_PROTEGIDO);

            var traduccion = new Traduccion
            {
                IdIdioma = idiomaId,
                IdControl = controlId,
                Texto = texto
            };
            _mapperTraduccion.Insertar(traduccion);
            _gestor.RecargarSiIdiomaActivo(idiomaId);
        }

        private Idioma ObtenerPorId(int id)
        {
            foreach (Idioma i in _mapperIdioma.Listar())
                if (i.Id == id) return i;
            return null;
        }

        private bool ExisteNombre(string nombre)
        {
            foreach (Idioma i in _mapperIdioma.Listar())
                if (string.Equals(i.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null
                && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;

            if (new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;

            _bitacoraService.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
