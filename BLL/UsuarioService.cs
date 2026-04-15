using System;
using DAL;
using BE;

namespace BLL
{
    public class UsuarioService : IUsuarioService
    {
        private readonly MapperUsuario _mapperUsuario;
        //private readonly MapperBitacoraSesion _mapperBitacoraSesion;

        public UsuarioService()
        {
            _mapperUsuario = new MapperUsuario();
            //_mapperBitacoraSesion = new MapperBitacoraSesion();
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
                //Sesion.Instancia.BitacoraSesion = new BitacoraSesion(usuario);
                //Sesion.Instancia.BitacoraSesion.Id = _mapperBitacoraSesion.Insertar(Sesion.Instancia.BitacoraSesion);
                return true;
            }

            return false;
        }

        public void Logout()
        {
            //if (Sesion.Instancia.EstaAutenticado())
            //{
            //    Sesion.Instancia.BitacoraSesion.FechaHoraFin = DateTime.Now;
            //    _mapperBitacoraSesion.Editar(Sesion.Instancia.BitacoraSesion);
            //    Sesion.Instancia.Logout();
            //}
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
