using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperIdioma : Mapper<Idioma>
    {
        public override int Insertar(Idioma obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Nombre", obj.Nombre)
            };
            return db.LeerEscalar("InsertarIdioma", parametros);
        }

        public override int Eliminar(Idioma obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.Escribir("EliminarIdioma", new List<SqlParameter>
            {
                db.CrearParametro("@IdiomaId", obj.Id)
            });
        }

        public override List<Idioma> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarIdiomas", new List<SqlParameter>());

            var lista = new List<Idioma>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        private static Idioma MapearFila(DataRow row)
        {
            return new Idioma
            {
                Id     = (int)row["Id"],
                Nombre = (string)row["Nombre"]
            };
        }
    }
}
