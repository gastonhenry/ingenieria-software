using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperRol : Mapper<Rol>
    {
        public override int Insertar(Rol obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Nombre",      obj.Nombre),
                db.CrearParametro("@Descripcion", obj.Descripcion)
            };
            return db.LeerEscalar("InsertarRol", parametros);
        }

        public override int Eliminar(Rol obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.Escribir("EliminarRol", new List<SqlParameter>
            {
                db.CrearParametro("@RolId", obj.Id)
            });
        }

        public override List<Rol> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarRoles", new List<SqlParameter>());

            var lista = new List<Rol>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        public List<Rol> ListarDeUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) };
            DataTable tabla = db.Leer("ListarRolesDeUsuario", parametros);

            var lista = new List<Rol>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        public void AsignarPermiso(int rolId, int permisoId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("AsignarPermisoARol", new List<SqlParameter>
            {
                db.CrearParametro("@RolId",     rolId),
                db.CrearParametro("@PermisoId", permisoId)
            });
        }

        public void QuitarPermiso(int rolId, int permisoId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("QuitarPermisoDeRol", new List<SqlParameter>
            {
                db.CrearParametro("@RolId",     rolId),
                db.CrearParametro("@PermisoId", permisoId)
            });
        }

        public void AsignarAUsuario(int usuarioId, int rolId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("AsignarRolAUsuario", new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId),
                db.CrearParametro("@RolId",     rolId)
            });
        }

        public void QuitarDeUsuario(int usuarioId, int rolId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("QuitarRolDeUsuario", new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId),
                db.CrearParametro("@RolId",     rolId)
            });
        }

        public void ActualizarDVH(int rolId, string dvh)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ActualizarDVHRol", new List<SqlParameter>
            {
                db.CrearParametro("@RolId", rolId),
                db.CrearParametro("@DVH",   dvh)
            });
        }

        private static Rol MapearFila(DataRow row)
        {
            return new Rol
            {
                Id          = (int)row["Id"],
                Nombre      = (string)row["Nombre"],
                Descripcion = row.IsNull("Descripcion") ? null : (string)row["Descripcion"],
                DVH         = row.IsNull("DVH") ? null : (string)row["DVH"]
            };
        }
    }
}
