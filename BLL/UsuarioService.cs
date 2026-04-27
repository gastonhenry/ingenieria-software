using System;
using System.Collections.Generic;
using MPP;
using BE;

namespace BLL
{
    public class UsuarioService : IUsuarioService
    {
        private readonly MapperUsuario _mapperUsuario;
        private readonly BitacoraService _bitacoraService;

        public UsuarioService()
        {
            _mapperUsuario = new MapperUsuario();
            _bitacoraService = new BitacoraService();
        }

        public Usuario Obtener(string username)
        {
            Usuario usuario = _mapperUsuario.Obtener(username.ToLower());
            return usuario;
        }

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            Usuario usuario = _mapperUsuario.Login(username, password);
            if (usuario != null)
            {
                SesionUsuario.GetInstancia().Login(usuario);
                _bitacoraService.Insertar(usuario, BitacoraEnum.Login);
                return true;
            }

            return false;
        }

        public void Logout()
        {
            if (SesionUsuario.GetInstancia().EstaAutenticado())
            {
                var usuario = SesionUsuario.GetInstancia().Usuario;

                string detalle = null;
                if (usuario.UltimoLogin.HasValue)
                {
                    var duracion = DateTime.Now - usuario.UltimoLogin.Value;
                    detalle = $"Tiempo conectado: {(int)duracion.TotalMinutes} min. {duracion.Seconds} seg.";
                }

                _bitacoraService.Insertar(usuario, BitacoraEnum.Logout, detalle);
                SesionUsuario.GetInstancia().Logout();
            }
        }

        public List<Usuario> Listar() => _mapperUsuario.Listar();

        public void Desbloquear(int usuarioId, string username)
        {
            if (EsAdmin())
            {
                _mapperUsuario.DesbloquearUsuario(usuarioId);
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, BitacoraEnum.DesbloqueoUsuario, $"Admin desbloquea usuario: '{username}'");
            }
            else
            {
                throw new Exception("No tiene permisos para desbloquear usuarios");
            }
        }

        public void Bloquear(int usuarioId, string username)
        {
            if (EsAdmin())
            {
                _mapperUsuario.BloquearUsuario(usuarioId);
                _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, BitacoraEnum.BloqueoUsuario, $"Admin bloquea usuario: '{username}'");
            }
            else
            {
                throw new Exception("No tiene permisos para bloquear usuarios");
            }
        }

        public bool EsAdmin()
        {
            var sesion = SesionUsuario.GetInstancia();
            return sesion.EstaAutenticado() &&
                   sesion.Usuario.Username.ToLower() == "admin";
        }

        public bool Registro(Usuario user)
        {
            bool result = false;
            var usuarioExistente = _mapperUsuario.Obtener(user.Username);
            if (usuarioExistente == null)
            {
                if (user != null && !string.IsNullOrWhiteSpace(user.Username)
                    || !string.IsNullOrWhiteSpace(user.Nombre) || !string.IsNullOrWhiteSpace(user.Apellido))
                {
                    result = _mapperUsuario.Insertar(user) > 0;

                    _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, BitacoraEnum.RegistroUsuario, $"Usuario creado: {user.Username}");
                }
            }
            else
            {
                throw new Exception("Ya existe el username");
            }

            return result;
        }
    }
}
