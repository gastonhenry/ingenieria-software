using BE;

namespace BLL
{
    public interface IVerificadorEntidad
    {
        string NombreTabla { get; }
        ResultadoIntegridad Verificar();
        void RecalcularDVs();
    }
}
