using BE;

namespace BLL
{
    // A02 — Serialización: exportar / importar el perfil de una persona en XML (pantalla Registrar Persona).
    public interface IPerfilClienteService
    {
        void Exportar(int idPersona, string rutaDestino);
        ResultadoImportacionPerfil Importar(string rutaOrigen);
        string NombreArchivoSugerido(int idPersona);
    }
}
