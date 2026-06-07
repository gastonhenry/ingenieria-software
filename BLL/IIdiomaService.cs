using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IIdiomaService
    {
        List<Idioma> Listar();
        int Crear(string nombre);
        void Eliminar(int idiomaId);
        void CambiarIdiomaActual(int idiomaId);
        Idioma IdiomaActual();
        void Suscribir(IObservadorIdioma observador);
        void Desuscribir(IObservadorIdioma observador);
        string Traducir(string formCodigo, string controlCodigo);
        List<Traduccion> ListarTraduccionesParaEdicion(int idiomaId);
        void GuardarTraduccion(int idiomaId, int controlId, string texto);
        bool EstaCompleto(int idiomaId);
    }
}
