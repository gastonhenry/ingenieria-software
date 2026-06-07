using BE;
using BE.Enums;
using System.Collections.Generic;

namespace BLL
{
    public interface IUsuarioService
    {
        Usuario Obtener(string username);
        Usuario ObtenerPorId(int usuarioId);
        LoginResultado Login(string username, string password);
        void Logout();
        bool Registro(Usuario usuario);
        bool Editar(Usuario usuario);
        List<Usuario> Listar();
        void Bloquear(int usuarioId, string username);
        void Desbloquear(int usuarioId, string username);
        bool EstaAutenticado();
        bool EsAdmin();
        ResultadoIntegridad VerificarIntegridad();

        int AsignarPermiso(int usuarioId, int permisoId);
        void QuitarPermiso(int usuarioId, int permisoId);
        List<Permiso> ListarPermisosDirectosDeUsuario(int usuarioId);
    }
}
