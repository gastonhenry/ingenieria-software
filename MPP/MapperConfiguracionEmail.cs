using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    // Cuenta de email saliente (registro único).
    public class MapperConfiguracionEmail
    {
        public ConfiguracionEmail Obtener()
        {
            DataTable tabla = AccesoDB.GetInstancia().Leer("ObtenerConfiguracionEmail");
            DataRow row = tabla.Rows[0];
            return new ConfiguracionEmail
            {
                Host      = row["Host"] as string,
                Puerto    = (int)row["Puerto"],
                Ssl       = (bool)row["Ssl"],
                Usuario   = row["Usuario"] as string,
                Password  = row["Password"] as string,
                TimeoutMs = (int)row["TimeoutMs"],
                Remitente = row["Remitente"] as string
            };
        }

        public void Actualizar(ConfiguracionEmail c)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ActualizarConfiguracionEmail", new List<SqlParameter>
            {
                db.CrearParametro("@Host",            c.Host),
                db.CrearParametro("@Puerto",          c.Puerto),
                db.CrearParametro("@Ssl",             c.Ssl),
                db.CrearParametro("@Usuario",         c.Usuario),
                db.CrearParametro("@Password",        c.Password),
                db.CrearParametro("@TimeoutMs",       c.TimeoutMs),
                db.CrearParametro("@Remitente",       c.Remitente)
            });
        }
    }
}
