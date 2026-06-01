using BE;
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
                    var padre = indice[idPadre.Value] as FamiliaPermiso;
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

        public int CrearFamiliaPermiso(string codigo, string descripcion)
        {
            RequerirAdmin("CrearFamiliaPermiso");
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código es obligatorio.");

            codigo = codigo.Trim().ToUpper();

            if (ExisteCodigo(codigo))
                throw new InvalidOperationException(
                    $"Ya existe un permiso con el código '{codigo}'. Los códigos deben ser únicos.");

            var familia = new FamiliaPermiso
            {
                Codigo = codigo,
                Descripcion = descripcion
            };

            int id = _mapperPermiso.Insertar(familia);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaPermiso,
                $"Familia de permisos creada: {codigo}");
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
            RequerirAdmin("AgregarHijo");

            if (idPadre == idHijo)
                throw new InvalidOperationException("Un permiso no puede contenerse a sí mismo.");

            if (GeneraCiclo(idPadre, idHijo))
                throw new InvalidOperationException(
                    "La asignación generaría una referencia circular entre permisos.");

            string codHijo  = CodigoDePermiso(idHijo);
            string codPadre = CodigoDePermiso(idPadre);

            _mapperPermiso.AgregarHijo(idPadre, idHijo);

            var rolService = new RolService();
            int quitados = rolService.LimpiarRedundanciasDeTodosLosRoles();

            string detalle = $"Permiso {codHijo} agregado como hijo de {codPadre}";
            if (quitados > 0)
                detalle += $". Limpieza global: {quitados} asignación(es) directa(s) redundante(s) quitadas de roles.";

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso, detalle);
            return quitados;
        }

        public void QuitarHijo(int idHijo)
        {
            RequerirAdmin("QuitarHijo");
            string codHijo = CodigoDePermiso(idHijo);
            _mapperPermiso.QuitarHijo(idHijo);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso,
                $"Permiso {codHijo} desvinculado de su familia padre");
        }

        public void EliminarFamilia(int familiaId)
        {
            RequerirAdmin("EliminarFamiliaPermiso");

            var familia = _mapperPermiso.ListarConPadre()
                .Find(par => par.Key.Id == familiaId).Key as FamiliaPermiso;

            if (familia == null)
                throw new InvalidOperationException("La familia no existe o no es una familia de permisos.");

            _mapperPermiso.Eliminar(familia);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaPermiso,
                $"Familia {familia.Codigo} eliminada (se desasignó de roles y usuarios que la tenían)");
        }

        public void AsignarAUsuario(int usuarioId, int permisoId)
        {
            RequerirAdmin("AsignarPermisoAUsuario");
            string codPermiso = CodigoDePermiso(permisoId);
            string username   = UsernameDeUsuario(usuarioId);
            _mapperPermiso.AsignarAUsuario(usuarioId, permisoId);
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso,
                $"Permiso {codPermiso} asignado al usuario '{username}'");
        }

        public void QuitarDeUsuario(int usuarioId, int permisoId)
        {
            RequerirAdmin("QuitarPermisoDeUsuario");
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
            var mapperRol = new MapperRol();
            var roles = mapperRol.ListarDeUsuario(usuarioId);

            var arbol = ListarArbol();
            var indice = new Dictionary<int, Permiso>();
            IndexarArbol(arbol, indice);

            var resultado = new Dictionary<int, Permiso>();
            foreach (var p in directos)
                AcumularConHijos(p.Id, indice, resultado);

            foreach (var rol in roles)
            {
                var permisosRol = _mapperPermiso.ListarPorRol(rol.Id);
                foreach (var p in permisosRol)
                    AcumularConHijos(p.Id, indice, resultado);
            }

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
                var familia = n as FamiliaPermiso;
                if (familia != null)
                    IndexarArbol(familia.Hijos, indice);
            }
        }

        private void AcumularConHijos(int id, Dictionary<int, Permiso> indice, Dictionary<int, Permiso> resultado)
        {
            if (!indice.TryGetValue(id, out Permiso p)) return;
            if (!resultado.ContainsKey(p.Id))
                resultado[p.Id] = p;

            var familia = p as FamiliaPermiso;
            if (familia != null)
                foreach (var h in familia.Hijos)
                    AcumularConHijos(h.Id, indice, resultado);
        }

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
                    "Sólo el usuario administrador puede gestionar permisos.");
            }
        }
    }
}
