using BE;
using BE.Enums;
using System.Collections.Generic;

namespace BLL
{
    public interface IModeloService
    {
        List<Modelo> Listar();
        List<Modelo> ListarPorMarca(int idMarca);
        int Registrar(string nombre, int idMarca, TipoCarroceria tipoCarroceria);
        void Editar(int id, string nuevoNombre, int idMarca, TipoCarroceria tipoCarroceria);
        void Eliminar(int id);
    }
}
