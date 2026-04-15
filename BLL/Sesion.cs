using BE;

namespace BLL
{
    public class Sesion
    {
        private static Sesion _instancia;

        public static Sesion Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new Sesion();
                return _instancia;
            }
        }

        private Sesion() { }

        public Usuario UsuarioActual { get; set; }

        public bool EstaAutenticado()
        {
            return UsuarioActual != null;
        }

        public void Logout()
        {
            UsuarioActual = null;
        }
    }
}
