using BE;
using BE.Enums;
using System.Collections.Generic;

namespace BLL
{
    public interface IModeloService
    {
        List<Modelo> Listar();
        List<Modelo> ListarPorMarca(int idMarca);
        int Registrar(string nombre, Marca marca, TipoCarroceria tipoCarroceria);
        void Editar(int id, string nuevoNombre, Marca marca, TipoCarroceria tipoCarroceria);
        void Eliminar(int id);
    }
}
