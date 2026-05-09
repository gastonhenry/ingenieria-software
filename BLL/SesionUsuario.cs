using BE;

namespace BLL
{
    public class SesionUsuario
    {
        private static SesionUsuario _instancia = null;

        private SesionUsuario() { }

        public static SesionUsuario GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new SesionUsuario();
            }
            return _instancia;
        }

        public Usuario Usuario { get; set; }
    }
}