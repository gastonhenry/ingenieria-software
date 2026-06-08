using DAL;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperMantenimiento
    {
        public void Backup(string rutaArchivo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Backup("BackupBaseDeDatos", new List<SqlParameter>
            {
                db.CrearParametro("@RutaArchivo", rutaArchivo)
            });
        }

        public void Restore(string rutaArchivo)
        {
            AccesoDB.GetInstancia().Restore(rutaArchivo);
        }
    }
}
