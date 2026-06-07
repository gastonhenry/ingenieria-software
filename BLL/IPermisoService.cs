using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IPermisoService
    {
        List<Permiso> ListarArbol();
        List<Permiso> ListarPlano();
        int CrearRol(string codigo, string descripcion);
        void EliminarRol(int rolId);
        int AgregarHijo(int idPadre, int idHijo);
        void QuitarHijo(int idHijo);
        void AsignarAUsuario(int usuarioId, int permisoId);
        void QuitarDeUsuario(int usuarioId, int permisoId);
        List<Permiso> ListarPermisosDeUsuario(int usuarioId);
        bool UsuarioTienePermiso(Usuario usuario, string codigoPermiso);
    }
}
