using System;
using System.Collections.Generic;
using MPP;
using BE;
using HELPERS;
using BE.Enums;

namespace BLL
{
    public class UsuarioService : IUsuarioService
    {
        private const int MaxIntentosFallidos = 3;

        private readonly MapperUsuario _mapperUsuario;
        private readonly MapperPermiso _mapperPermiso;
        private readonly BitacoraService _bitacoraService;
        private readonly PermisoService _permisoService;
        private readonly UsuarioHistorialService _historialService;

        public UsuarioService()
        {
            _mapperUsuario = new MapperUsuario();
            _mapperPermiso = new MapperPermiso();
            _bitacoraService = new BitacoraService();
            _permisoService = new PermisoService();
            _historialService = new UsuarioHistorialService();
        }

        private static int? ActuanteId()
        {
            return SesionUsuario.GetInstancia().Usuario?.Id;
        }

        private bool PuedeGestionarUsuarios()
        {
            return EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "GESTIONAR_USUARIOS");
        }

        public Usuario Obtener(string username)
        {
            Usuario usuario = _mapperUsuario.Obtener(username.ToLower());
            return usuario;
        }

        public LoginResultado Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return LoginResultado.CredencialesInvalidas;

                Usuario usuario = _mapperUsuario.Obtener(username.ToLower());
                if (usuario == null)
                    return LoginResultado.UsuarioInexistente;

                if (usuario.Bloqueado)
                {
                    _bitacoraService.Insertar(usuario, TipoBitacora.IntentoAccesoBloqueado);
                    return LoginResultado.UsuarioBloqueado;
                }

                if (!PasswordCorrecta(usuario, password))
                    return RegistrarFalloLogin(usuario);

                return CompletarLoginExitoso(usuario);
            }
            catch (Exception)
            {
                var ex = new BLLException("ERR_LOGIN_GENERICO", "Ocurrió un error en Login.");
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Login: {ex.Message}");
                throw ex;
            }
        }

        private bool PasswordCorrecta(Usuario usuario, string password)
        {
            string hash = PasswordHasher.HashPassword(password, usuario.Salt);
            return hash.Equals(usuario.Hash, StringComparison.OrdinalIgnoreCase);
        }

        private LoginResultado RegistrarFalloLogin(Usuario usuario)
        {
            bool esAdmin = usuario.Username.ToLower() == "admin";
            if (esAdmin)
            {
                _bitacoraService.Insertar(usuario, TipoBitacora.LoginFallido);
                return LoginResultado.CredencialesInvalidas;
            }

            int intentos = _mapperUsuario.IncrementarIntentosFallidos(usuario.Id);
            if (intentos >= MaxIntentosFallidos)
            {
                _mapperUsuario.BloquearUsuario(usuario.Id);
                RecalcularDV();
                _bitacoraService.Insertar(usuario, TipoBitacora.BloqueoUsuario,
                    "Bloqueo automático por superar intentos fallidos.");

                var u = _mapperUsuario.ObtenerPorId(usuario.Id);
                if (u != null) _historialService.RegistrarSnapshot(u, AccionUsuarioHistorial.Bloqueo, null);

                return LoginResultado.Bloqueado;
            }

            RecalcularDV();
            _bitacoraService.Insertar(usuario, TipoBitacora.LoginFallido,
                $"Intento {intentos}/{MaxIntentosFallidos}");
            return LoginResultado.CredencialesInvalidas;
        }

        private LoginResultado CompletarLoginExitoso(Usuario usuario)
        {
            _mapperUsuario.ActualizarUltimoLogin(usuario.Id);
            RecalcularDV();
            usuario.UltimoLogin = DateTime.Now;
            usuario.Hash = null;
            usuario.Salt = null;
            SesionUsuario.GetInstancia().Usuario = usuario;
            _bitacoraService.Insertar(usuario, TipoBitacora.Login);
            return LoginResultado.Exitoso;
        }

        public void Logout()
        {
            if (EstaAutenticado())
            {
                try
                {
                    var usuario = SesionUsuario.GetInstancia().Usuario;

                    string detalle = null;
                    if (usuario.UltimoLogin.HasValue)
                    {
                        var duracion = DateTime.Now - usuario.UltimoLogin.Value;
                        detalle = $"Tiempo conectado: {(int)duracion.TotalMinutes} min. {duracion.Seconds} seg.";
                    }

                    _bitacoraService.Insertar(usuario, TipoBitacora.Logout, detalle);
                    SesionUsuario.GetInstancia().Usuario = null;
                }
                catch (Exception)
                {
                    var ex = new BLLException("ERR_LOGOUT_GENERICO", "Ocurrió un error en Logout.");
                    _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Logout: {ex.Message}");
                    throw ex;
                }
            }
        }

        public List<Usuario> Listar() => _mapperUsuario.Listar();

        public void Desbloquear(int usuarioId, string username)
        {
            if (PuedeGestionarUsuarios())
            {
                _mapperUsuario.DesbloquearUsuario(usuarioId);
                RecalcularDV();
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.DesbloqueoUsuario, $"Desbloqueo de usuario: '{username}'");

                var u = _mapperUsuario.ObtenerPorId(usuarioId);
                if (u != null) _historialService.RegistrarSnapshot(u, AccionUsuarioHistorial.Desbloqueo, ActuanteId());
            }
            else
            {
                var ex = new BLLException("ERR_SIN_PERMISO_DESBLOQUEAR", "No tiene permisos para desbloquear usuarios.");
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Desbloquear: {ex.Message}");
                throw ex;
            }
        }

        public void Bloquear(int usuarioId, string username)
        {
            if (PuedeGestionarUsuarios())
            {
                if (username.ToLower() == "admin")
                {
                    var ex = new BLLException("ERR_NO_BLOQUEAR_ADMIN", "No podés bloquear al usuario administrador.");
                    _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Bloquear: {ex.Message}");
                    throw ex;
                }
                _mapperUsuario.BloquearUsuario(usuarioId);
                RecalcularDV();
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.BloqueoUsuario, $"Bloqueo de usuario: '{username}'");

                var u = _mapperUsuario.ObtenerPorId(usuarioId);
                if (u != null) _historialService.RegistrarSnapshot(u, AccionUsuarioHistorial.Bloqueo, ActuanteId());
            }
            else
            {
                var ex = new BLLException("ERR_SIN_PERMISO_BLOQUEAR", "No tiene permisos para bloquear usuarios.");
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Bloquear: {ex.Message}");
                throw ex;
            }
        }

        public bool EstaAutenticado()
        {
            return SesionUsuario.GetInstancia().Usuario != null;
        }

        public bool EsAdmin()
        {
            return EstaAutenticado() &&
                   SesionUsuario.GetInstancia().Usuario.Username.ToLower() == "admin";
        }

        public bool Registro(Usuario user)
        {
            if (user == null
                || string.IsNullOrWhiteSpace(user.Username)
                || string.IsNullOrWhiteSpace(user.Nombre)
                || string.IsNullOrWhiteSpace(user.Apellido)
                || string.IsNullOrWhiteSpace(user.Password)
                || string.IsNullOrWhiteSpace(user.Email)
                || string.IsNullOrWhiteSpace(user.Telefono)
                || string.IsNullOrWhiteSpace(user.Documento)
                || string.IsNullOrWhiteSpace(user.Domicilio))
            {
                throw new BLLException("ERR_REGISTRO_DATOS_INCOMPLETOS", "Faltan datos para registrar al usuario.");
            }

            var usuarioExistente = _mapperUsuario.Obtener(user.Username);
            if (usuarioExistente != null)
            {
                throw new BLLException("ERR_USERNAME_EXISTE", "Ya existe el username.");
            }

            int nuevoId = _mapperUsuario.Insertar(user);
            bool result = nuevoId > 0;
            RecalcularDV();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.RegistroUsuario, $"Usuario creado: {user.Username}");

            if (result)
            {
                user.Id = nuevoId;
                _historialService.RegistrarSnapshot(user, AccionUsuarioHistorial.Alta, ActuanteId());
            }

            return result;
        }

        public Usuario ObtenerPorId(int usuarioId)
        {
            return _mapperUsuario.ObtenerPorId(usuarioId);
        }

        public bool Editar(Usuario user)
        {
            if (!(EsAdmin() || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "EDITAR_USUARIO")))
                throw new BLLException("ERR_SIN_PERMISO_EDITAR_USUARIO", "No tenés permiso para editar usuarios.");

            if (user == null
                || string.IsNullOrWhiteSpace(user.Username)
                || string.IsNullOrWhiteSpace(user.Nombre)
                || string.IsNullOrWhiteSpace(user.Apellido)
                || string.IsNullOrWhiteSpace(user.Email)
                || string.IsNullOrWhiteSpace(user.Telefono)
                || string.IsNullOrWhiteSpace(user.Documento)
                || string.IsNullOrWhiteSpace(user.Domicilio))
            {
                throw new BLLException("ERR_EDICION_DATOS_INCOMPLETOS", "Faltan datos para editar al usuario.");
            }

            var actual = _mapperUsuario.ObtenerPorId(user.Id);
            if (actual == null)
                throw new BLLException("ERR_USUARIO_NO_ENCONTRADO", "No se encontró el usuario.");

            bool hayCambios =
                !string.Equals(user.Username,  actual.Username,  StringComparison.Ordinal) ||
                !string.Equals(user.Nombre,    actual.Nombre,    StringComparison.Ordinal) ||
                !string.Equals(user.Apellido,  actual.Apellido,  StringComparison.Ordinal) ||
                !string.Equals(user.Email,     actual.Email,     StringComparison.Ordinal) ||
                !string.Equals(user.Telefono,  actual.Telefono,  StringComparison.Ordinal) ||
                !string.Equals(user.Documento, actual.Documento, StringComparison.Ordinal) ||
                !string.Equals(user.Domicilio, actual.Domicilio, StringComparison.Ordinal);

            if (!hayCambios) return false;

            var existente = _mapperUsuario.Obtener(user.Username);
            if (existente != null && existente.Id != user.Id)
                throw new BLLException("ERR_USERNAME_EN_USO", "El username '{0}' ya está en uso por otro usuario.", user.Username);

            _mapperUsuario.Editar(user);
            RecalcularDV();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.EdicionUsuario,
                $"Usuario editado: {user.Username} (Id {user.Id})");
            _historialService.RegistrarSnapshot(user, AccionUsuarioHistorial.Edicion, ActuanteId());
            return true;
        }

        private void RecalcularDV() => RecalcularDVUsuarios();

        public static void RecalcularDVUsuarios()
        {
            new VerificadorUsuario().RecalcularDVs();
        }

        internal static IList<KeyValuePair<string, string>> ObtenerCamposParaDVH(Usuario u)
        {
            string ultimoLogin = u.UltimoLogin?.ToString("yyyy-MM-ddTHH:mm:ss.fff") ?? string.Empty;
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Username",         u.Username         ?? string.Empty),
                new KeyValuePair<string, string>("Hash",             u.Hash             ?? string.Empty),
                new KeyValuePair<string, string>("Salt",             u.Salt             ?? string.Empty),
                new KeyValuePair<string, string>("Nombre",           u.Nombre           ?? string.Empty),
                new KeyValuePair<string, string>("Apellido",         u.Apellido         ?? string.Empty),
                new KeyValuePair<string, string>("Email",            u.Email            ?? string.Empty),
                new KeyValuePair<string, string>("Telefono",         u.Telefono         ?? string.Empty),
                new KeyValuePair<string, string>("Documento",        u.Documento        ?? string.Empty),
                new KeyValuePair<string, string>("Domicilio",        u.Domicilio        ?? string.Empty),
                new KeyValuePair<string, string>("Bloqueado",        u.Bloqueado.ToString()),
                new KeyValuePair<string, string>("IntentosFallidos", u.IntentosFallidos.ToString()),
                new KeyValuePair<string, string>("UltimoLogin",      ultimoLogin),
            };
        }

        public int AsignarPermiso(int usuarioId, int permisoId)
        {
            RequerirAdminOPermiso("ASIGNAR_PERMISOS", "AsignarPermisoAUsuario");
            RequerirNoAutoasignacion(usuarioId, "AsignarPermisoAUsuario");
            string codPermiso = CodigoDePermiso(permisoId);
            string username   = UsernameDeUsuario(usuarioId);

            _mapperPermiso.AsignarAUsuario(usuarioId, permisoId);
            int quitados = LimpiarRedundanciasUsuario(usuarioId);

            string detalle = $"Permiso {codPermiso} asignado al usuario '{username}'";
            if (quitados > 0)
                detalle += $". Limpieza: {quitados} asignación(es) directa(s) redundante(s) quitadas.";
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AsignacionPermiso, detalle);
            return quitados;
        }

        public void QuitarPermiso(int usuarioId, int permisoId)
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
            foreach (Usuario u in _mapperUsuario.Listar())
                if (u.Id == usuarioId) return u.Username;
            return "id=" + usuarioId;
        }

        public List<Permiso> ListarPermisosDirectosDeUsuario(int usuarioId) =>
            _mapperPermiso.ListarDirectosDeUsuario(usuarioId);

        private int LimpiarRedundanciasUsuario(int usuarioId)
        {
            var directos = _mapperPermiso.ListarDirectosDeUsuario(usuarioId);
            if (directos.Count == 0) return 0;

            var hijosPorPadre = ConstruirIndiceHijos();
            var cubiertos = new HashSet<int>();

            foreach (Permiso p in directos)
                if (p is Rol)
                    RecolectarDescendientes(p.Id, hijosPorPadre, cubiertos);

            int quitados = 0;
            foreach (Permiso p in directos)
            {
                if (cubiertos.Contains(p.Id))
                {
                    _mapperPermiso.QuitarDeUsuario(usuarioId, p.Id);
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

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            if (EsAdmin()) return;
            if (_permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, codigoPermiso)) return;

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }

        private void RequerirNoAutoasignacion(int usuarioIdDestino, string accion)
        {
            if (EsAdmin()) return;
            var usuarioActual = SesionUsuario.GetInstancia().Usuario;
            if (usuarioActual == null || usuarioActual.Id != usuarioIdDestino) return;

            _bitacoraService.Insertar(usuarioActual, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': no podés modificar tus propios permisos.");
            throw new BLLException("ERR_AUTOASIGNACION_PERMISOS",
                "No podés asignarte o quitarte permisos a vos mismo. Sólo el administrador puede hacerlo.");
        }
    }
}
