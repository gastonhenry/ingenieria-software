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
                db.CrearParametro("@Username",  obj.Username),
                db.CrearParametro("@Hash",      hashedPassword),
                db.CrearParametro("@Salt",      salt),
                db.CrearParametro("@Nombre",    obj.Nombre),
                db.CrearParametro("@Apellido",  obj.Apellido),
                db.CrearParametro("@Email",     obj.Email),
                db.CrearParametro("@Telefono",  obj.Telefono),
                db.CrearParametro("@Documento", obj.Documento),
                db.CrearParametro("@Domicilio", obj.Domicilio)
            };

            return db.LeerEscalar("InsertarUsuario", parametros);
        }

        public override int Editar(Usuario obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", obj.Id),
                db.CrearParametro("@Username",  obj.Username),
                db.CrearParametro("@Nombre",    obj.Nombre),
                db.CrearParametro("@Apellido",  obj.Apellido),
                db.CrearParametro("@Email",     obj.Email),
                db.CrearParametro("@Telefono",  obj.Telefono),
                db.CrearParametro("@Documento", obj.Documento),
                db.CrearParametro("@Domicilio", obj.Domicilio)
            };
            return db.Escribir("EditarUsuario", parametros);
        }

        public Usuario ObtenerPorId(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId)
            };
            DataTable tabla = db.Leer("ObtenerUsuarioPorId", parametros);
            if (tabla.Rows.Count == 0) return null;

            DataRow row = tabla.Rows[0];
            return new Usuario
            {
                Id          = (int)row["Id"],
                Username    = (string)row["Username"],
                Password    = string.Empty,
                Hash        = (string)row["Hash"],
                Salt        = (string)row["Salt"],
                Nombre      = (string)row["Nombre"],
                Apellido    = (string)row["Apellido"],
                Email       = (string)row["Email"],
                Telefono    = (string)row["Telefono"],
                Documento   = (string)row["Documento"],
                Domicilio   = (string)row["Domicilio"],
                Bloqueado   = (bool)row["Bloqueado"],
                IntentosFallidos = (int)row["IntentosFallidos"],
                UltimoLogin = row.IsNull("UltimoLogin") ? (DateTime?)null : (DateTime)row["UltimoLogin"],
                IdIdioma    = row.IsNull("IdIdioma") ? (int?)null : (int)row["IdIdioma"],
                DVH         = row.IsNull("DVH") ? null : (string)row["DVH"]
            };
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
                Id        = (int)row["Id"],
                Username  = (string)row["Username"],
                Password  = string.Empty,
                Hash      = (string)row["Hash"],
                Salt      = (string)row["Salt"],
                Nombre    = (string)row["Nombre"],
                Apellido  = (string)row["Apellido"],
                Email     = (string)row["Email"],
                Telefono  = (string)row["Telefono"],
                Documento = (string)row["Documento"],
                Domicilio = (string)row["Domicilio"],
                Bloqueado = (bool)row["Bloqueado"],
                UltimoLogin = row.IsNull("UltimoLogin") ? (DateTime?)null : (DateTime)row["UltimoLogin"],
                IdIdioma  = row.IsNull("IdIdioma") ? (int?)null : (int)row["IdIdioma"]
            };
        }

        public void ActualizarIdioma(int usuarioId, int? idiomaId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId),
                new SqlParameter("@IdIdioma", DbType.Int32)
                {
                    Value = idiomaId.HasValue ? (object)idiomaId.Value : DBNull.Value
                }
            };
            db.Escribir("ActualizarIdiomaUsuario", parametros);
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
                    Id        = (int)row["Id"],
                    Username  = (string)row["Username"],
                    Nombre    = (string)row["Nombre"],
                    Apellido  = (string)row["Apellido"],
                    Email     = (string)row["Email"],
                    Telefono  = (string)row["Telefono"],
                    Documento = (string)row["Documento"],
                    Domicilio = (string)row["Domicilio"],
                    Bloqueado = (bool)row["Bloqueado"],
                    IntentosFallidos = (int)row["IntentosFallidos"],
                    UltimoLogin = row.IsNull("UltimoLogin") ? (DateTime?)null : (DateTime)row["UltimoLogin"],
                });
            }

            return lista;
        }

        public List<Usuario> ListarParaVerificacion()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var lista = new List<Usuario>();

            DataTable tabla = db.Leer("ListarUsuariosParaVerificacion", new List<SqlParameter>());

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new Usuario
                {
                    Id        = (int)row["Id"],
                    Username  = (string)row["Username"],
                    Hash      = (string)row["Hash"],
                    Salt      = (string)row["Salt"],
                    Nombre    = (string)row["Nombre"],
                    Apellido  = (string)row["Apellido"],
                    Email     = (string)row["Email"],
                    Telefono  = (string)row["Telefono"],
                    Documento = (string)row["Documento"],
                    Domicilio = (string)row["Domicilio"],
                    Bloqueado = (bool)row["Bloqueado"],
                    IntentosFallidos = (int)row["IntentosFallidos"],
                    UltimoLogin = row.IsNull("UltimoLogin") ? (DateTime?)null : (DateTime)row["UltimoLogin"],
                    DVH       = row.IsNull("DVH") ? null : (string)row["DVH"]
                });
            }

            return lista;
        }

        public void ActualizarDVH(int usuarioId, string dvh)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ActualizarDVHUsuario", new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId),
                db.CrearParametro("@DVH", dvh)
            });
        }
    }
}