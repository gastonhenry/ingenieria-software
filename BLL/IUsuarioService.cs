using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IUsuarioService
    {
        Usuario Obtener(string username);
        bool Login(string username, string password);
        void Logout();
        bool Registro(Usuario usuario);
        List<Usuario> Listar();
        void Bloquear(int usuarioId, string username);
        void Desbloquear(int usuarioId, string username);
        bool EsAdmin();
    }
}
