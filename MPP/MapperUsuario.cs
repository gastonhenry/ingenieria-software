using BE;
using DAL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MapperUsuario : Mapper<Usuario>
    {
        const int maxIntentosFallidos = 3;

        public Usuario Login(string username, string password)
        {
            Usuario usuario = Obtener(username);
            if (usuario == null)
                throw new InvalidOperationException("El usuario no existe.");

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
                        throw new InvalidOperationException("El usuario ha sido bloqueado debido a múltiples intentos fallidos." +
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
            db.Escribir("ActualizarUltimoLogin",
                new List<System.Data.SqlClient.SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        private int IncrementarIntentosFallidos(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("IncrementarIntentosFallidos",
                new List<System.Data.SqlClient.SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public void BloquearUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("BloquearUsuario",
                new List<System.Data.SqlClient.SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public void DesbloquearUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("DesbloquearUsuario",
                new List<System.Data.SqlClient.SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public override int Insertar(Usuario obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();

            string salt = PasswordHasher.GenerateSalt();
            string hashedPassword = PasswordHasher.HashPassword(obj.Password, salt);

            var parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                db.CrearParametro("@Username", obj.Username),
                db.CrearParametro("@Hash",     hashedPassword),
                db.CrearParametro("@Salt",     salt),
                db.CrearParametro("@Nombre",   obj.Nombre),
                db.CrearParametro("@Apellido", obj.Apellido)
            };

            return db.LeerEscalar("InsertarUsuario", parametros);
        }

        public Usuario Obtener(string username)
        {
            AccesoDB db = AccesoDB.GetInstancia();

            var parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                db.CrearParametro("@Username", username)
            };

            DataTable tabla = db.Leer("Login", parametros);

            if (tabla.Rows.Count == 0)
                return null;

            DataRow row = tabla.Rows[0];

            return new Usuario
            {
                Id = (int)row["Id"],
                Username = (string)row["Username"],
                Password = string.Empty,
                Hash = (string)row["Hash"],
                Salt = (string)row["Salt"],
                Nombre = (string)row["Nombre"],
                Apellido = (string)row["Apellido"],
                Bloqueado = (bool)row["Bloqueado"],
                UltimoLogin = row.IsNull("UltimoLogin") ? (DateTime?)null : (DateTime)row["UltimoLogin"]
            };
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
            AccesoDB db = AccesoDB.GetInstancia();
            var lista = new List<Usuario>();

            DataTable tabla = db.Leer("ListarUsuarios", new List<System.Data.SqlClient.SqlParameter>());

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new Usuario
                {
                    Id = (int)row["Id"],
                    Username = (string)row["Username"],
                    Nombre = (string)row["Nombre"],
                    Apellido = (string)row["Apellido"],
                    Bloqueado = (bool)row["Bloqueado"],
                    IntentosFallidos = (int)row["IntentosFallidos"],
                    UltimoLogin = row.IsNull("UltimoLogin") ? (DateTime?)null : (DateTime)row["UltimoLogin"]
                });
            }

            return lista;
        }
    }
}