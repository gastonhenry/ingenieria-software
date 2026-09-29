using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AccesoDB
    {
        private static readonly string CONNECTION_STRING = ObtenerConnectionString();

        private static string ObtenerConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["Avanti"];
            if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
                throw new ConfigurationErrorsException(
                    "Falta el connection string 'Avanti' en la configuración (UI.exe.config).");
            return cs.ConnectionString;
        }

        private static readonly AccesoDB _instancia = new AccesoDB();
        public static AccesoDB GetInstancia() => _instancia;
        private AccesoDB() { }

        public int Escribir(string sql, List<SqlParameter> parametros = null)
        {
            using (var conexion = new SqlConnection(CONNECTION_STRING))
            using (var cmd = CrearComando(sql, parametros, conexion))
            {
                conexion.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int EscribirConTransaccion(string sql, List<SqlParameter> parametros = null)
        {
            using (var conexion = new SqlConnection(CONNECTION_STRING))
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                using (var cmd = CrearComando(sql, parametros, conexion, transaccion))
                {
                    try
                    {
                        int filas = cmd.ExecuteNonQuery();
                        transaccion.Commit();
                        return filas;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable Leer(string sql, List<SqlParameter> parametros = null)
        {
            using (var conexion = new SqlConnection(CONNECTION_STRING))
            using (var cmd = CrearComando(sql, parametros, conexion))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var tabla = new DataTable();
                    adapter.Fill(tabla);
                    return tabla;
                }
            }
        }

        public int LeerEscalar(string sql, List<SqlParameter> parametros = null)
        {
            using (var conexion = new SqlConnection(CONNECTION_STRING))
            using (var cmd = CrearComando(sql, parametros, conexion))
            {
                conexion.Open();
                object resultado = cmd.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                    return 0;
                return Convert.ToInt32(resultado);
            }
        }

        public void Backup(string sql, List<SqlParameter> parametros = null)
        {
            using (var conexion = new SqlConnection(CONNECTION_STRING))
            using (var cmd = CrearComando(sql, parametros, conexion))
            {
                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Restore(string rutaArchivo)
        {
            var builder = new SqlConnectionStringBuilder(CONNECTION_STRING);
            string dbName = builder.InitialCatalog;
            builder.InitialCatalog = "master";

            string sql = string.Format(@"
                ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [{0}] FROM DISK = @ruta WITH REPLACE;
                ALTER DATABASE [{0}] SET MULTI_USER;", dbName);

            using (var conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 300;
                    cmd.Parameters.Add(new SqlParameter("@ruta", SqlDbType.NVarChar) { Value = rutaArchivo });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private SqlCommand CrearComando(string sql, List<SqlParameter> parametros, SqlConnection conexion, SqlTransaction transaccion = null)
        {
            var cmd = new SqlCommand(sql, conexion)
            {
                CommandType = CommandType.StoredProcedure,
                Transaction = transaccion
            };

            if (parametros != null && parametros.Count > 0)
                cmd.Parameters.AddRange(parametros.ToArray());

            return cmd;
        }

        public SqlParameter CrearParametro(string nombre, string valor) =>
            new SqlParameter(nombre, DbType.String) { Value = valor ?? (object)DBNull.Value };

        public SqlParameter CrearParametro(string nombre, int valor) =>
            new SqlParameter(nombre, DbType.Int32) { Value = valor };

        public SqlParameter CrearParametro(string nombre, float valor) =>
            new SqlParameter(nombre, DbType.Single) { Value = valor };

        public SqlParameter CrearParametro(string nombre, decimal valor) =>
            new SqlParameter(nombre, DbType.Decimal) { Value = valor };

        public SqlParameter CrearParametro(string nombre, DateTime valor) =>
            new SqlParameter(nombre, DbType.DateTime) { Value = valor };

        public SqlParameter CrearParametro(string nombre, bool valor) =>
            new SqlParameter(nombre, DbType.Boolean) { Value = valor };
    }
}