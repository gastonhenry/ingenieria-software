using BE;

namespace BLL
{
    public interface IUsuarioService
    {
        Usuario Obtener(string username);
        bool Login(string username, string password);
        void Logout();
        bool Registro(Usuario usuario);
    }
}
