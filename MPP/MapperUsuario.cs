using BE;
using DAL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperUsuario : Mapper<Usuario>
    {
        public void ActualizarUltimoLogin(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ActualizarUltimoLogin",
                new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public int IncrementarIntentosFallidos(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("IncrementarIntentosFallidos",
                new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public void BloquearUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("BloquearUsuario",
                new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public void DesbloquearUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("DesbloquearUsuario",
                new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) });
        }

        public override int Insertar(Usuario obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();

            string salt = PasswordHasher.GenerateSalt();
            string hashedPassword = PasswordHasher.HashPassword(obj.Password, salt);

            var parametros = new List<SqlParameter>
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

            var parametros = new List<SqlParameter>
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

            DataTable tabla = db.Leer("ListarUsuarios", new List<SqlParameter>());

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