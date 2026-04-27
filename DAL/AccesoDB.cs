using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AccesoDB
    {
        private const string CONNECTION_STRING =
            "Data Source=localhost\\SQLEXPRESS; Initial Catalog=ingenieria; Integrated Security=SSPI;";

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
            const string sql = @"
                ALTER DATABASE [ingenieria] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [ingenieria] FROM DISK = @ruta WITH REPLACE;
                ALTER DATABASE [ingenieria] SET MULTI_USER;";

            string conexionMaster = CONNECTION_STRING.Replace("ingenieria", "master");

            using (var conn = new SqlConnection(conexionMaster))
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