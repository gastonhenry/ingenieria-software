using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IRolService
    {
        List<Rol> Listar();
        int Crear(string nombre, string descripcion);
        void Eliminar(int rolId);
        int AsignarPermiso(int rolId, int permisoId);
        void QuitarPermiso(int rolId, int permisoId);
        void AsignarAUsuario(int usuarioId, int rolId);
        void QuitarDeUsuario(int usuarioId, int rolId);
        List<Rol> ListarDeUsuario(int usuarioId);
        List<Permiso> ListarPermisosDeRol(int rolId);
        int LimpiarRedundanciasDeTodosLosRoles();
    }
}
