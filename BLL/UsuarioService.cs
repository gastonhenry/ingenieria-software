using System;
using DAL;
using BE;

namespace BLL
{
    public class UsuarioService : IUsuarioService
    {
        private readonly MapperUsuario _mapperUsuario;
        private readonly MapperBitacora _mapperBitacora;

        public UsuarioService()
        {
            _mapperUsuario = new MapperUsuario();
            _mapperBitacora = new MapperBitacora();
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
                SesionUsuario.GetInstancia().Bitacora = new Bitacora(usuario);
                SesionUsuario.GetInstancia().Bitacora.Id = _mapperBitacora.Insertar(SesionUsuario.GetInstancia().Bitacora);
                return true;
            }

            return false;
        }

        public void Logout()
        {
            if (SesionUsuario.GetInstancia().EstaAutenticado())
            {
                SesionUsuario.GetInstancia().Bitacora.FechaHoraFin = DateTime.Now;
                _mapperBitacora.Editar(SesionUsuario.GetInstancia().Bitacora);
                SesionUsuario.GetInstancia().Logout();
            }
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
