namespace BE
{
    public class SesionUsuario
    {
        private static SesionUsuario _instancia = null;
        private Usuario usuario;
        public Usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        private SesionUsuario() { }
        public static SesionUsuario GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new SesionUsuario();
            }
            return _instancia;
        }

        public bool EstaAutenticado()
        {
            return _instancia != null && _instancia.Usuario != null;
        }

        public void Login(Usuario usuario)
        {
            _instancia = this;
            _instancia.usuario = usuario;
        }

        public void Logout()
        {
            _instancia = null;
        }
    }
}