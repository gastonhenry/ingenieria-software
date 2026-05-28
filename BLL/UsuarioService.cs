using System;
using System.Collections.Generic;
using MPP;
using BE;
using HELPERS;

namespace BLL
{
    public class UsuarioService : IUsuarioService
    {
        private const int MaxIntentosFallidos = 3;
        private const string NombreTabla = "Usuario";

        private readonly MapperUsuario _mapperUsuario;
        private readonly BitacoraService _bitacoraService;
        private readonly DigitoVerificadorService _digitoVerificadorService;

        public UsuarioService()
        {
            _mapperUsuario = new MapperUsuario();
            _bitacoraService = new BitacoraService();
            _digitoVerificadorService = new DigitoVerificadorService();
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
                Exception ex = new Exception("Ocurrió un error en Login.");
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
                    Exception ex = new Exception("Ocurrió un error en Logout.");
                    _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Logout: {ex.Message}");
                    throw ex;
                }
            }
        }

        public List<Usuario> Listar() => _mapperUsuario.Listar();

        public void Desbloquear(int usuarioId, string username)
        {
            if (EsAdmin())
            {
                _mapperUsuario.DesbloquearUsuario(usuarioId);
                RecalcularDV();
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.DesbloqueoUsuario, $"Admin desbloquea usuario: '{username}'");
            }
            else
            {
                Exception ex = new Exception("No tiene permisos para desbloquear usuarios");
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Desbloquear: {ex.Message}");
                throw ex;
            }
        }

        public void Bloquear(int usuarioId, string username)
        {
            if (EsAdmin())
            {
                if (username.ToLower() == "admin")
                {
                    Exception ex = new Exception("No podés bloquear a usuario administrador");
                    _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, $"Error en Bloquear: {ex.Message}");
                    throw ex;
                }
                _mapperUsuario.BloquearUsuario(usuarioId);
                RecalcularDV();
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.BloqueoUsuario, $"Admin bloquea usuario: '{username}'");
            }
            else
            {
                Exception ex = new Exception("No tiene permisos para bloquear usuarios");
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
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Nombre)
                || string.IsNullOrWhiteSpace(user.Apellido) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Faltan datos para registrar al usuario.");
            }

            var usuarioExistente = _mapperUsuario.Obtener(user.Username);
            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("Ya existe el username");
            }

            bool result = _mapperUsuario.Insertar(user) > 0;
            RecalcularDV();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.RegistroUsuario, $"Usuario creado: {user.Username}");

            return result;
        }

        public ResultadoIntegridad VerificarIntegridad() 
        {
            try
            {
                var filas = _mapperUsuario.ListarParaVerificacion();
                ResultadoIntegridad resultado = _digitoVerificadorService.Verificar(NombreTabla, filas, ObtenerCamposParaDVH);
                if (resultado.RequiereInicializacion)
                {
                    _digitoVerificadorService.Recalcular(NombreTabla, filas, ObtenerCamposParaDVH, _mapperUsuario.ActualizarDVH);
                    resultado.Ok = true;
                }

                if (resultado.Ok)
                    return resultado;

                string detalleError = $"Se detectaron inconsistencias en la tabla '{NombreTabla}' de la base de datos.";
                if (resultado.FilasInvalidas.Count > 0)
                    detalleError += $" Filas con DVH inválido: {resultado.FilasInvalidas.Count} (Ids: {string.Join(", ", resultado.FilasInvalidas)}).";
                if (resultado.DVVInvalido)
                    detalleError += " DVV inválido.";
                detalleError += " No se permitirán inicios de sesión hasta restaurar la integridad.";

                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error, detalleError);

                return resultado;
            }
            catch (Exception ex)
            {
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error,
                    $"Error al verificar la integridad de la tabla '{NombreTabla}': Error: {ex.Message}");
                throw ex;
            }
        }

        private void RecalcularDV()
        {
            var filas = _mapperUsuario.ListarParaVerificacion();
            _digitoVerificadorService.Recalcular(NombreTabla, filas, ObtenerCamposParaDVH, _mapperUsuario.ActualizarDVH);
        }

        private static string ObtenerCamposParaDVH(Usuario u)
        {
            string ultimoLogin = u.UltimoLogin?.ToString("yyyy-MM-ddTHH:mm:ss.fff") ?? string.Empty;
            return string.Join("|",
                u.Username, u.Hash, u.Salt, u.Nombre, u.Apellido, u.Email,
                u.Bloqueado.ToString(), u.IntentosFallidos.ToString(), ultimoLogin);
        }
    }
}
