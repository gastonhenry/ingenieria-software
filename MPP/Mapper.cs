using DAL;
using System.Collections.Generic;

namespace MPP
{
    public abstract class Mapper<T>
    {
        internal AccesoDB Acceso { get; set; }
        public abstract int Insertar(T obj);
        public abstract int Editar(T obj);
        public abstract int Eliminar(T obj);
        public abstract List<T> Listar();
        public abstract T Obtener(int id);
    }
}
