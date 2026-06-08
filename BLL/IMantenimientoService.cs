using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IMantenimientoService
    {
        ResultadoIntegridadGlobal VerificarTodo();
        void RecalcularTodo();
        string Backup();
        void Restore(string rutaArchivo);
        List<string> ListarBackupsDisponibles();
        bool AutenticarMantenimiento(string usuario, string password);
        string CarpetaBackups { get; }
    }
}
