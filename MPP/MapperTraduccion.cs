using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperTraduccion : Mapper<Traduccion>
    {
        public override int Insertar(Traduccion obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdIdioma",  obj.IdIdioma),
                db.CrearParametro("@IdControl", obj.IdControl),
                db.CrearParametro("@Texto",     obj.Texto)
            };
            return db.Escribir("InsertarTraduccion", parametros);
        }

        public List<Traduccion> ListarPorIdioma(int idiomaId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdiomaId", idiomaId)
            };
            DataTable tabla = db.Leer("ListarTraducciones", parametros);

            var lista = new List<Traduccion>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));
            return lista;
        }

        public override List<Traduccion> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarTraducciones", new List<SqlParameter>());

            var lista = new List<Traduccion>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));
            return lista;
        }

        private static Traduccion MapearFila(DataRow row)
        {
            var control = new Control
            {
                Id     = (int)row["IdControl"],
                Codigo = (string)row["Codigo"],
                Form   = (string)row["Form"]
            };

            return new Traduccion
            {
                Id        = (int)row["Id"],
                IdIdioma  = (int)row["IdIdioma"],
                IdControl = (int)row["IdControl"],
                Texto     = (string)row["Texto"],
                Control   = control
            };
        }
    }
}
