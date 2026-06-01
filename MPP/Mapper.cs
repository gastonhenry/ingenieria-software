using System;
using System.Collections.Generic;

namespace MPP
{
    public abstract class Mapper<T>
    {
        public virtual int Insertar(T obj)   => throw new NotImplementedException();
        public virtual int Editar(T obj)     => throw new NotImplementedException();
        public virtual int Eliminar(T obj)   => throw new NotImplementedException();
        public virtual List<T> Listar()      => throw new NotImplementedException();
        public virtual T Obtener(int id)     => throw new NotImplementedException();
    }
}