namespace BE
{
    public class SesionUsuario
    {
        private static SesionUsuario _instancia = null;

        private Bitacora bitacora;
        public Bitacora Bitacora
        {
            get { return bitacora; }
            set { bitacora = value; }
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
            return _instancia != null && _instancia.Bitacora.Id > 0;
        }

        public void Logout()
        {
            _instancia = null;
        }
    }
}