using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IUsuarioService
    {
        Usuario Obtener(string username);
        LoginResultado Login(string username, string password);
        void Logout();
        bool Registro(Usuario usuario);
        List<Usuario> Listar();
        void Bloquear(int usuarioId, string username);
        void Desbloquear(int usuarioId, string username);
        bool EstaAutenticado();
        bool EsAdmin();
        ResultadoIntegridad VerificarIntegridad();

        int AsignarRol(int usuarioId, int rolId);
        int AsignarPermiso(int usuarioId, int permisoId);
        void QuitarRol(int usuarioId, int rolId);
        void QuitarPermiso(int usuarioId, int permisoId);
        List<Rol> ListarRolesDeUsuario(int usuarioId);
        List<Permiso> ListarPermisosDirectosDeUsuario(int usuarioId);
    }
}
