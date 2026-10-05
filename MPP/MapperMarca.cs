using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperMarca : Mapper<Marca>
    {
        public override int Insertar(Marca m)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Nombre", m.Nombre)
            };
            return db.LeerEscalar("InsertarMarca", parametros);
        }

        public override int Editar(Marca m)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Id",     m.Id),
                db.CrearParametro("@Nombre", m.Nombre)
            };
            return db.Escribir("EditarMarca", parametros);
        }

        public override int Eliminar(Marca m)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Id", m.Id)
            };
            return db.Escribir("EliminarMarca", parametros);
        }

        public int ContarUnidadesConMarca(int idMarca)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdMarca", idMarca)
            };
            return db.LeerEscalar("ContarUnidadesConMarca", parametros);
        }

        public override List<Marca> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarMarcas");
            var lista = new List<Marca>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public Marca ObtenerPorNombre(string nombre)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@Nombre", nombre) };
            DataTable tabla = db.Leer("ObtenerMarcaPorNombre", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        private static Marca MapearFila(DataRow row)
        {
            return new Marca
            {
                Id     = (int)row["Id"],
                Nombre = (string)row["Nombre"]
            };
        }
    }
}
