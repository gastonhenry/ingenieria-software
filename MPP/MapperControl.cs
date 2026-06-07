using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperControl : Mapper<Control>
    {
        public override int Insertar(Control obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Codigo", obj.Codigo),
                db.CrearParametro("@Form",   obj.Form)
            };
            return db.LeerEscalar("InsertarControl", parametros);
        }

        public override List<Control> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarControles", new List<SqlParameter>());

            var lista = new List<Control>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        private static Control MapearFila(DataRow row)
        {
            return new Control
            {
                Id     = (int)row["Id"],
                Codigo = (string)row["Codigo"],
                Form   = (string)row["Form"]
            };
        }
    }
}
