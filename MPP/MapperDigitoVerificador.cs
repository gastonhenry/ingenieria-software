using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperDigitoVerificador
    {
        public DigitoVerificador Obtener(string nombreTabla)
        {
            AccesoDB db = AccesoDB.GetInstancia();

            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@NombreTabla", nombreTabla)
            };

            DataTable tabla = db.Leer("ObtenerDVV", parametros);

            if (tabla.Rows.Count == 0)
                return null;

            DataRow row = tabla.Rows[0];
            return new DigitoVerificador
            {
                NombreTabla = (string)row["NombreTabla"],
                DVV = (string)row["DVV"]
            };
        }

        public void Guardar(string nombreTabla, string dvv)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("UpsertDVV", new List<SqlParameter>
            {
                db.CrearParametro("@NombreTabla", nombreTabla),
                db.CrearParametro("@DVV", dvv)
            });
        }
    }
}
