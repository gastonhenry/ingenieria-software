using BE;
using BE.Enums;
using System.Collections.Generic;

namespace BLL
{
    public interface IUsuarioHistorialService
    {
        void RegistrarSnapshot(Usuario usuario, AccionUsuarioHistorial accion, int? modificadoPorUsuarioId, int? restauracionId = null);
        List<UsuarioHistorial> ListarPorUsuario(int usuarioId);
        UsuarioHistorial ObtenerPorIdHistorial(int historialId);
        void Restaurar(int historialId);
    }
}
