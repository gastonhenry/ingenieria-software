using BE;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class GestorIdiomas
    {
        private static readonly GestorIdiomas _instancia = new GestorIdiomas();
        public static GestorIdiomas GetInstancia() => _instancia;

        private readonly List<IObservadorIdioma> _observadores = new List<IObservadorIdioma>();
        private readonly MapperIdioma _mapperIdioma = new MapperIdioma();
        private readonly MapperTraduccion _mapperTraduccion = new MapperTraduccion();

        private Idioma _idiomaActual;
        private Dictionary<string, string> _cacheTraducciones = new Dictionary<string, string>();

        private GestorIdiomas() { }

        public Idioma IdiomaActual
        {
            get { return _idiomaActual; }
        }

        public void Suscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            if (!_observadores.Contains(observador))
                _observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            _observadores.Remove(observador);
        }

        public void CambiarIdioma(Idioma idioma)
        {
            if (idioma == null) return;
            _idiomaActual = idioma;
            RecargarCache();
            Notificar();
        }

        public string Traducir(string formCodigo, string controlCodigo)
        {
            if (_idiomaActual == null) return null;
            string clave = ConstruirClave(formCodigo, controlCodigo);
            string texto;
            return _cacheTraducciones.TryGetValue(clave, out texto) ? texto : null;
        }

        public List<Idioma> ListarIdiomas() => _mapperIdioma.Listar();

        public void RecargarSiIdiomaActivo(int idiomaId)
        {
            if (_idiomaActual == null || _idiomaActual.Id != idiomaId) return;
            RecargarCache();
            Notificar();
        }

        private void Notificar()
        {
            foreach (IObservadorIdioma obs in _observadores.ToArray())
                obs.ActualizarIdioma(_idiomaActual);
        }

        private void RecargarCache()
        {
            var nuevoCache = new Dictionary<string, string>();
            if (_idiomaActual != null)
            {
                foreach (Traduccion t in _mapperTraduccion.ListarPorIdioma(_idiomaActual.Id))
                {
                    if (t.Control == null) continue;
                    string clave = ConstruirClave(t.Control.Form, t.Control.Codigo);
                    nuevoCache[clave] = t.Texto;
                }
            }
            _cacheTraducciones = nuevoCache;
        }

        private static string ConstruirClave(string form, string codigo)
        {
            return (form ?? "") + "::" + (codigo ?? "");
        }
    }
}
