using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PermisoService : IPermisoService
    {
        private readonly MapperPermiso _mapperPermiso;
        private readonly BitacoraService _bitacoraService;

        public PermisoService()
        {
            _mapperPermiso = new MapperPermiso();
            _bitacoraService = new BitacoraService();
        }

        public List<Permiso> ListarPlano()
        {
            var conPadre = _mapperPermiso.ListarConPadre();
            var lista = new List<Permiso>(conPadre.Count);
            foreach (var par in conPadre)
                lista.Add(par.Key);
            return lista;
        }

        public List<Permiso> ListarArbol()
        {
            var conPadre = _mapperPermiso.ListarConPadre();
            var indice = new Dictionary<int, Permiso>();
            var idPadrePorHijo = new Dictionary<int, int?>();

            foreach (var par in conPadre)
            {
                indice[par.Key.Id] = par.Key;
                idPadrePorHijo[par.Key.Id] = par.Value;
            }

            var raices = new List<Permiso>();
            foreach (var permiso in indice.Values)
            {
                int? idPadre = idPadrePorHijo[permiso.Id];
                if (idPadre.HasValue && indice.ContainsKey(idPadre.Value))
                {
                    var padre = indice[idPadre.Value] as Rol;
                    if (padre != null)
                        padre.Hijos.Add(permiso);
                    else
                        raices.Add(permiso);
                }
                else
                {
                    raices.Add(permiso);
                }
            }

            return raices;
        }

        public int CrearRol(string codigo, string descripcion)
        {
            RequerirAdminOPermiso("GESTIONAR_PERMISOS", "CrearRol");
            if (string.IsNullOrWhiteSpace(codigo))
                throw new BLLException("ERR_CODIGO_OBLIGATORIO", "Código es obligatorio.");

            codigo = codigo.Trim().ToUpper();

            if (ExisteCodigo(codigo))
                throw new BLLException("ERR_PERMISO_CODIGO_DUPLICADO",
                    "Ya existe un permiso con el código '{0}'. Los códigos deben ser únicos.", codigo);

            var rol = new Rol
            {
                Codigo = codigo,
                Descripcion = descripcion
            };

            int id = _mapperPermiso.Insertar(rol);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaRol,
                $"Rol creado: {codigo}");
            return id;
        }

        private bool ExisteCodigo(string codigo)
        {
            foreach (var par in _mapperPermiso.ListarConPadre())
                if (string.Equals(par.Key.Codigo, codigo, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        public int AgregarHijo(int idPadre, int idHijo)
        {
            RequerirAdminOPermiso("GESTIONAR_PERMISOS", "AgregarHijo");

            if (idPadre == idHijo)
                throw new BLLException("ERR_PERMISO_AUTOCONTENCION", "Un permiso no puede contenerse a sí mismo.");

            if (GeneraCiclo(idPadre, idHijo))
                throw new BLLException("ERR_PERMISO_CICLICO",
                    "La asignación generaría una referencia circular entre permisos.");

            string codHijo  = CodigoDePermiso(idHijo);
            string codPadre = CodigoDePermiso(idPadre);

            _mapperPermiso.AgregarHijo(idPadre, idHijo);

            string detalle = $"Permiso {codHijo} agregado como hijo de {codPadre}";
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso, detalle);
            return 0;
        }

        public void QuitarHijo(int idHijo)
        {
            RequerirAdminOPermiso("GESTIONAR_PERMISOS", "QuitarHijo");
            string codHijo = CodigoDePermiso(idHijo);
            _mapperPermiso.QuitarHijo(idHijo);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso,
                $"Permiso {codHijo} desvinculado de su rol padre");
        }

        public void EliminarRol(int rolId)
        {
            RequerirAdminOPermiso("GESTIONAR_PERMISOS", "EliminarRol");

            var rol = _mapperPermiso.ListarConPadre()
                .Find(par => par.Key.Id == rolId).Key as Rol;

            if (rol == null)
                throw new BLLException("ERR_ROL_NO_EXISTE", "El rol no existe.");

            _mapperPermiso.Eliminar(rol);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaRol,
                $"Rol {rol.Codigo} eliminado (se desasignó de los usuarios que lo tenían)");
        }

        public void AsignarAUsuario(int usuarioId, int permisoId)
        {
            RequerirAdminOPermiso("ASIGNAR_PERMISOS", "AsignarPermisoAUsuario");
            RequerirNoAutoasignacion(usuarioId, "AsignarPermisoAUsuario");
            string codPermiso = CodigoDePermiso(permisoId);
            string username   = UsernameDeUsuario(usuarioId);
            _mapperPermiso.AsignarAUsuario(usuarioId, permisoId);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso,
                $"Permiso {codPermiso} asignado al usuario '{username}'");
        }

        public void QuitarDeUsuario(int usuarioId, int permisoId)
        {
            RequerirAdminOPermiso("ASIGNAR_PERMISOS", "QuitarPermisoDeUsuario");
            RequerirNoAutoasignacion(usuarioId, "QuitarPermisoDeUsuario");
            string codPermiso = CodigoDePermiso(permisoId);
            string username   = UsernameDeUsuario(usuarioId);
            _mapperPermiso.QuitarDeUsuario(usuarioId, permisoId);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso,
                $"Permiso {codPermiso} quitado al usuario '{username}'");
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

        public List<Permiso> ListarPermisosDeUsuario(int usuarioId)
        {
            var directos = _mapperPermiso.ListarDirectosDeUsuario(usuarioId);

            var arbol = ListarArbol();
            var indice = new Dictionary<int, Permiso>();
            IndexarArbol(arbol, indice);

            var resultado = new Dictionary<int, Permiso>();
            foreach (var p in directos)
                AcumularConHijos(p.Id, indice, resultado);

            return new List<Permiso>(resultado.Values);
        }

        public bool UsuarioTienePermiso(Usuario usuario, string codigoPermiso)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(codigoPermiso))
                return false;

            var permisos = ListarPermisosDeUsuario(usuario.Id);
            foreach (var p in permisos)
                if (string.Equals(p.Codigo, codigoPermiso, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private bool GeneraCiclo(int idPadre, int idHijo)
        {
            var conPadre = _mapperPermiso.ListarConPadre();
            var padrePorId = new Dictionary<int, int?>();
            foreach (var par in conPadre)
                padrePorId[par.Key.Id] = par.Value;

            int? actual = idPadre;
            var visitados = new HashSet<int>();
            while (actual.HasValue)
            {
                if (actual.Value == idHijo) return true;
                if (!visitados.Add(actual.Value)) return true;
                if (!padrePorId.TryGetValue(actual.Value, out int? siguiente)) break;
                actual = siguiente;
            }

            var indice = new Dictionary<int, Permiso>();
            foreach (var par in conPadre)
                indice[par.Key.Id] = par.Key;

            var subarbol = new HashSet<int>();
            RecolectarDescendientes(idHijo, padrePorId, subarbol);
            return subarbol.Contains(idPadre);
        }

        private void RecolectarDescendientes(int id, Dictionary<int, int?> padrePorId, HashSet<int> acumulador)
        {
            foreach (var kv in padrePorId)
            {
                if (kv.Value == id && acumulador.Add(kv.Key))
                    RecolectarDescendientes(kv.Key, padrePorId, acumulador);
            }
        }

        private void IndexarArbol(List<Permiso> nodos, Dictionary<int, Permiso> indice)
        {
            foreach (var n in nodos)
            {
                indice[n.Id] = n;
                var rol = n as Rol;
                if (rol != null)
                    IndexarArbol(rol.Hijos, indice);
            }
        }

        private void AcumularConHijos(int id, Dictionary<int, Permiso> indice, Dictionary<int, Permiso> resultado)
        {
            if (!indice.TryGetValue(id, out Permiso p)) return;
            if (!resultado.ContainsKey(p.Id))
                resultado[p.Id] = p;

            var rol = p as Rol;
            if (rol != null)
                foreach (var h in rol.Hijos)
                    AcumularConHijos(h.Id, indice, resultado);
        }

        private void RequerirAdmin(string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            if (!EsAdmin(usuario))
            {
                _bitacoraService.Insertar(usuario, TipoBitacora.Error,
                    $"Acceso denegado a '{accion}': se requiere usuario admin.");
                throw new BLLException("ERR_SOLO_ADMIN_PERMISOS",
                    "Sólo el usuario administrador puede gestionar permisos.");
            }
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            if (EsAdmin(usuario)) return;
            if (UsuarioTienePermiso(usuario, codigoPermiso)) return;

            _bitacoraService.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }

        private static bool EsAdmin(Usuario usuario)
        {
            return usuario != null && usuario.Username != null
                && usuario.Username.ToLower() == "admin";
        }

        private void RequerirNoAutoasignacion(int usuarioIdDestino, string accion)
        {
            var usuarioActual = SesionUsuario.GetInstancia().Usuario;
            if (EsAdmin(usuarioActual)) return;
            if (usuarioActual == null || usuarioActual.Id != usuarioIdDestino) return;

            _bitacoraService.Insertar(usuarioActual, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': no podés modificar tus propios permisos.");
            throw new BLLException("ERR_AUTOASIGNACION_PERMISOS",
                "No podés asignarte o quitarte permisos a vos mismo. Sólo el administrador puede hacerlo.");
        }
    }
}
