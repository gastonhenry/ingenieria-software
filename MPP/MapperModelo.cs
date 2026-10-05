using BE;
using BE.Enums;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperModelo : Mapper<Modelo>
    {
        public override int Insertar(Modelo m)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Nombre",         m.Nombre),
                db.CrearParametro("@IdMarca",        m.Marca.Id),
                db.CrearParametro("@TipoCarroceria", (int)m.TipoCarroceria)
            };
            return db.LeerEscalar("InsertarModelo", parametros);
        }

        public override int Editar(Modelo m)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Id",             m.Id),
                db.CrearParametro("@Nombre",         m.Nombre),
                db.CrearParametro("@IdMarca",        m.Marca.Id),
                db.CrearParametro("@TipoCarroceria", (int)m.TipoCarroceria)
            };
            return db.Escribir("EditarModelo", parametros);
        }

        public override int Eliminar(Modelo m)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Id", m.Id)
            };
            return db.Escribir("EliminarModelo", parametros);
        }

        public override List<Modelo> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarModelos");
            var lista = new List<Modelo>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public List<Modelo> ListarPorMarca(int idMarca)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdMarca", idMarca)
            };
            DataTable tabla = db.Leer("ListarModelosPorMarca", parametros);
            var lista = new List<Modelo>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public Modelo ObtenerPorNombreYMarca(string nombre, int idMarca)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Nombre",  nombre),
                db.CrearParametro("@IdMarca", idMarca)
            };
            DataTable tabla = db.Leer("ObtenerModeloPorNombreYMarca", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public int ContarUnidadesConModelo(int idModelo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdModelo", idModelo)
            };
            return db.LeerEscalar("ContarUnidadesConModelo", parametros);
        }

        private static Modelo MapearFila(DataRow row)
        {
            return new Modelo
            {
                Id             = (int)row["Id"],
                Nombre         = (string)row["Nombre"],
                Marca          = new Marca { Id = (int)row["IdMarca"], Nombre = (string)row["MarcaNombre"] },
                TipoCarroceria = (TipoCarroceria)(int)row["TipoCarroceria"]
            };
        }
    }
}
