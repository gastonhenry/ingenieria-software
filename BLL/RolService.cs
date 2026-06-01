using BE;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class RolService : IRolService
    {
        private readonly MapperRol _mapperRol;
        private readonly MapperPermiso _mapperPermiso;
        private readonly BitacoraService _bitacoraService;

        public RolService()
        {
            _mapperRol = new MapperRol();
            _mapperPermiso = new MapperPermiso();
            _bitacoraService = new BitacoraService();
        }

        public List<Rol> Listar() => _mapperRol.Listar();

        public int Crear(string nombre, string descripcion)
        {
            RequerirAdmin("CrearRol");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre es obligatorio.");

            nombre = nombre.Trim().ToUpper();

            if (ExisteNombre(nombre))
                throw new InvalidOperationException(
                    $"Ya existe un rol con el nombre '{nombre}'. Los nombres deben ser únicos.");

            var rol = new Rol { Nombre = nombre, Descripcion = descripcion };
            int id = _mapperRol.Insertar(rol);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaRol,
                $"Rol creado: {nombre}");
            return id;
        }

        private bool ExisteNombre(string nombre)
        {
            foreach (Rol r in _mapperRol.Listar())
                if (string.Equals(r.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        public int AsignarPermiso(int rolId, int permisoId)
        {
            RequerirAdmin("AsignarPermisoARol");
            string nombreRol  = NombreDeRol(rolId);
            string codPermiso = CodigoDePermiso(permisoId);

            _mapperRol.AsignarPermiso(rolId, permisoId);

            int quitados = LimpiarRedundancias(rolId);

            string detalle = $"Permiso {codPermiso} asignado al rol {nombreRol}";
            if (quitados > 0)
                detalle += $". Limpieza: {quitados} asignación(es) directa(s) redundante(s) quitadas.";

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso, detalle);
            return quitados;
        }

        public void Eliminar(int rolId)
        {
            RequerirAdmin("EliminarRol");
            var rol = ObtenerRolPorId(rolId);
            if (rol == null)
                throw new InvalidOperationException("El rol no existe.");

            _mapperRol.Eliminar(rol);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaRol,
                $"Rol {rol.Nombre} eliminado (se desasignó automáticamente de todos los usuarios)");
        }

        public int LimpiarRedundanciasDeTodosLosRoles()
        {
            int total = 0;
            foreach (Rol rol in _mapperRol.Listar())
                total += LimpiarRedundancias(rol.Id);
            return total;
        }

        private int LimpiarRedundancias(int rolId)
        {
            var asignados = _mapperPermiso.ListarPorRol(rolId);

            var familiaIds = new List<int>();
            foreach (var p in asignados)
                if (p is FamiliaPermiso) familiaIds.Add(p.Id);

            if (familiaIds.Count == 0) return 0;

            var hijosPorPadre = ConstruirIndiceHijos();
            var cubiertos = new HashSet<int>();
            foreach (int fid in familiaIds)
                RecolectarDescendientes(fid, hijosPorPadre, cubiertos);

            int quitados = 0;
            foreach (var p in asignados)
            {
                if (cubiertos.Contains(p.Id))
                {
                    _mapperRol.QuitarPermiso(rolId, p.Id);
                    quitados++;
                }
            }
            return quitados;
        }

        private Dictionary<int, List<int>> ConstruirIndiceHijos()
        {
            var conPadre = _mapperPermiso.ListarConPadre();
            var hijosPorPadre = new Dictionary<int, List<int>>();
            foreach (var par in conPadre)
            {
                if (!par.Value.HasValue) continue;
                int padre = par.Value.Value;
                if (!hijosPorPadre.ContainsKey(padre))
                    hijosPorPadre[padre] = new List<int>();
                hijosPorPadre[padre].Add(par.Key.Id);
            }
            return hijosPorPadre;
        }

        private void RecolectarDescendientes(int id, Dictionary<int, List<int>> hijosPorPadre, HashSet<int> acumulador)
        {
            if (!hijosPorPadre.TryGetValue(id, out List<int> hijos)) return;
            foreach (int h in hijos)
                if (acumulador.Add(h))
                    RecolectarDescendientes(h, hijosPorPadre, acumulador);
        }

        public void QuitarPermiso(int rolId, int permisoId)
        {
            RequerirAdmin("QuitarPermisoDeRol");
            string nombreRol  = NombreDeRol(rolId);
            string codPermiso = CodigoDePermiso(permisoId);
            _mapperRol.QuitarPermiso(rolId, permisoId);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso,
                $"Permiso {codPermiso} quitado del rol {nombreRol}");
        }

        public void AsignarAUsuario(int usuarioId, int rolId)
        {
            RequerirAdmin("AsignarRolAUsuario");
            string nombreRol = NombreDeRol(rolId);
            string username  = UsernameDeUsuario(usuarioId);
            _mapperRol.AsignarAUsuario(usuarioId, rolId);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionRol,
                $"Rol {nombreRol} asignado al usuario '{username}'");
        }

        public void QuitarDeUsuario(int usuarioId, int rolId)
        {
            RequerirAdmin("QuitarRolDeUsuario");
            string nombreRol = NombreDeRol(rolId);
            string username  = UsernameDeUsuario(usuarioId);
            _mapperRol.QuitarDeUsuario(usuarioId, rolId);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionRol,
                $"Rol {nombreRol} quitado al usuario '{username}'");
        }

        public List<Rol> ListarDeUsuario(int usuarioId) => _mapperRol.ListarDeUsuario(usuarioId);

        private Rol ObtenerRolPorId(int rolId)
        {
            foreach (Rol r in _mapperRol.Listar())
                if (r.Id == rolId) return r;
            return null;
        }

        private string NombreDeRol(int rolId)
        {
            var r = ObtenerRolPorId(rolId);
            return r != null ? r.Nombre : "id=" + rolId;
        }

        private string CodigoDePermiso(int permisoId)
        {
            foreach (var par in _mapperPermiso.ListarConPadre())
                if (par.Key.Id == permisoId) return par.Key.Codigo;
            return "id=" + permisoId;
        }

        private string UsernameDeUsuario(int usuarioId)
        {
            var mapperUsuario = new MapperUsuario();
            foreach (Usuario u in mapperUsuario.Listar())
                if (u.Id == usuarioId) return u.Username;
            return "id=" + usuarioId;
        }

        public List<Permiso> ListarPermisosDeRol(int rolId) => _mapperPermiso.ListarPorRol(rolId);

        private void RequerirAdmin(string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null
                && usuario.Username.ToLower() == "admin";

            if (!esAdmin)
            {
                _bitacoraService.Insertar(usuario, TipoBitacora.Error,
                    $"Acceso denegado a '{accion}': se requiere usuario admin.");
                throw new UnauthorizedAccessException(
                    "Sólo el usuario administrador puede gestionar roles.");
            }
        }
    }
}
