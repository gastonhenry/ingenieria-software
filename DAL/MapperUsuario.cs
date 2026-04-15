using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class MapperUsuario : Mapper<Usuario>
    {
        public Usuario Login(string username, string password)
        {
            Usuario usuario = Obtener(username);
            if (usuario == null)
            {
                return null;
            }

            string hashIngresado = PasswordHasher.HashPassword(password, usuario.Salt);
            if (!hashIngresado.Equals(usuario.Hash, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return new Usuario
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Password = string.Empty,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };
        }

        public override int Insertar(Usuario obj)
        {
            AccesoDB db = new AccesoDB();
            int resultado = 0;

            try
            {
                db.Abrir();

                string salt = PasswordHasher.GenerateSalt();
                string hashedPassword = PasswordHasher.HashPassword(obj.Password, salt);

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    db.CrearParametro("@Username", obj.Username),
                    db.CrearParametro("@Hash", hashedPassword),
                    db.CrearParametro("@Salt", salt),
                    db.CrearParametro("@Nombre", obj.Nombre),
                    db.CrearParametro("@Apellido", obj.Apellido)
                };

                resultado = db.LeerEscalar("InsertarUsuario", parametros);
            }
            finally
            {
                db.Cerrar();
            }

            return resultado;
        }

        public Usuario Obtener(string username)
        {
            Usuario usuario = null;
            AccesoDB db = new AccesoDB();

            try
            {
                db.Abrir();

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    db.CrearParametro("@Username", username)
                };

                using (SqlDataReader reader = db.Leer("Login", parametros))
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Password = string.Empty,
                            Hash = reader.GetString(reader.GetOrdinal("Hash")),
                            Salt = reader.GetString(reader.GetOrdinal("Salt")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido"))
                        };
                    }
                }
            }
            finally
            {
                db.Cerrar();
            }

            return usuario;
        }

        public override Usuario Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public override int Editar(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public override int Eliminar(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public override List<Usuario> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
