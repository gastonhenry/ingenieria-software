using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AccesoDB
    {
        private static AccesoDB _instancia = null;

        private SqlConnection conexion;
        private SqlTransaction transaccion;

        public static AccesoDB GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new AccesoDB();
            }
            return _instancia;
        }

        private AccesoDB() { }

        public void Abrir()
        {
            conexion = new SqlConnection();
            conexion.ConnectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=ingenieria; Integrated Security=SSPI;";
            conexion.Open();
        }

        public void Cerrar()
        {
            conexion.Close();
            conexion = null;
            GC.Collect();
        }

        public void IniciarTx()
        {
            transaccion = conexion.BeginTransaction();
        }
        public void ConfirmarTx()
        {
            transaccion.Commit();
            transaccion = null;
        }

        public void DeshacerTx()
        {
            transaccion.Rollback();
            transaccion = null;
        }

        private SqlCommand CrearComando(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;

            if (parametros != null && parametros.Count > 0)
            {
                cmd.Parameters.AddRange(parametros.ToArray());
            }

            if (transaccion != null)
            {
                cmd.Transaction = transaccion;
            }

            return cmd;
        }

        public int Escribir(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = CrearComando(sql, parametros);
            int filas;
            try
            {
                filas = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                filas = -1;
            }

            return filas;
        }

        public void Backup(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = CrearComando(sql, parametros);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                throw ex;
            }
        }

        public void Restore(string rutaArchivo)
        {
            string conexionMaster = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=master; Integrated Security=SSPI;";

            using (SqlConnection conn = new SqlConnection(conexionMaster))
            {
                conn.Open();

                string sql = $@"ALTER DATABASE [BatallaNaval] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [BatallaNaval] FROM DISK = '{rutaArchivo}' WITH REPLACE;
                ALTER DATABASE [BatallaNaval] SET MULTI_USER;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public SqlDataReader Leer(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = CrearComando(sql, parametros);
            SqlDataReader lector = cmd.ExecuteReader();
            return lector;
        }

        public int LeerEscalar(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = CrearComando(sql, parametros);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            return new SqlParameter()
            {
                ParameterName = nombre,
                Value = valor,
                DbType = DbType.String,
            };
        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
            return new SqlParameter()
            {
                ParameterName = nombre,
                Value = valor,
                DbType = DbType.Int32,
            };
        }


        public SqlParameter CrearParametro(string nombre, float valor)
        {
            return new SqlParameter()
            {
                ParameterName = nombre,
                Value = valor,
                DbType = DbType.Single,
            };
        }

        public SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            return new SqlParameter()
            {
                ParameterName = nombre,
                Value = valor,
                DbType = DbType.DateTime,
            };
        }
    }
}
