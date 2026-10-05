using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IMarcaService
    {
        List<Marca> Listar();
        int Registrar(string nombre);
        void Editar(int id, string nuevoNombre);
        void Eliminar(int id);
    }
}
