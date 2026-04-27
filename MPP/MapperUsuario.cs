using BE;
using DAL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperUsuario : Mapper<Usuario>
    {
        const int maxIntentosFallidos = 3;

        public Usuario Login(string username, string password)
        {
            Usuario usuario = Obtener(username);
            if (usuario == null)
            {
                throw new InvalidOperationException("El usuario no existe.");
            }

            if (usuario.Bloqueado)
            {
                IncrementarIntentosFallidos(usuario.Id);
                throw new InvalidOperationException("El usuario se encuentra bloqueado." +
                    " Contacte al administrador para desbloquearlo.");
            }

            string hashIngresado = PasswordHasher.HashPassword(password, usuario.Salt);
            if (!hashIngresado.Equals(usuario.Hash, StringComparison.OrdinalIgnoreCase))
            {
                if (usuario.Username.ToLower() != "admin")
                {
                    int contador = IncrementarIntentosFallidos(usuario.Id);
                    if (contador >= maxIntentosFallidos)
                    {
                        BloquearUsuario(usuario.Id);
                        throw new InvalidOperationException("El usuario ha sido bloqueado debido a múltiples intentos fallidos de inicio de sesión." +
                            " Contacte al administrador para desbloquearlo.");
                    }
                }

                throw new InvalidOperationException("Usuario o contraseña incorrectos.");
            }

            ActualizarUltimoLogin(usuario.Id);

            return new Usuario
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Password = string.Empty,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                UltimoLogin = DateTime.Now
            };
        }

        private void ActualizarUltimoLogin(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            try
            {
                db.Abrir();
                db.Escribir("ActualizarUltimoLogin",
                    new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
            }
            finally
            {
                db.Cerrar();
            }
        }

        private int IncrementarIntentosFallidos(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            int intentos = 0;
            try
            {
                db.Abrir();
                intentos = db.LeerEscalar("IncrementarIntentosFallidos",
                    new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
            }
            finally
            {
                db.Cerrar();
            }
            return intentos;
        }

        public void BloquearUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            try
            {
                db.Abrir();
                db.Escribir("BloquearUsuario",
                    new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
            }
            finally
            {
                db.Cerrar();
            }
        }

        public void DesbloquearUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            try
            {
                db.Abrir();
                db.Escribir("DesbloquearUsuario",
                    new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
            }
            finally
            {
                db.Cerrar();
            }
        }

        public override int Insertar(Usuario obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
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
            AccesoDB db = AccesoDB.GetInstancia();

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
                        int ulIdx = reader.GetOrdinal("UltimoLogin");
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Password = string.Empty,
                            Hash = reader.GetString(reader.GetOrdinal("Hash")),
                            Salt = reader.GetString(reader.GetOrdinal("Salt")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            Bloqueado = reader.GetBoolean(reader.GetOrdinal("Bloqueado")),
                            UltimoLogin = reader.IsDBNull(ulIdx) ? (DateTime?)null : reader.GetDateTime(ulIdx)
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
            var lista = new List<Usuario>();
            AccesoDB db = AccesoDB.GetInstancia();
            try
            {
                db.Abrir();
                using (SqlDataReader reader = db.Leer("ListarUsuarios", new List<SqlParameter>()))
                {
                    while (reader.Read())
                    {
                        int ulIdx = reader.GetOrdinal("UltimoLogin");
                        lista.Add(new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            Bloqueado = reader.GetBoolean(reader.GetOrdinal("Bloqueado")),
                            IntentosFallidos = reader.GetInt32(reader.GetOrdinal("IntentosFallidos")),
                            UltimoLogin = reader.IsDBNull(ulIdx) ? (DateTime?)null : reader.GetDateTime(ulIdx)
                        });
                    }
                }
            }
            finally
            {
                db.Cerrar();
            }
            return lista;
        }
    }
}
