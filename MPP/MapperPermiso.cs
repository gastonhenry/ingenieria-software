using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperPermiso : Mapper<Permiso>
    {
        public override int Insertar(Permiso obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            string tipo = obj is FamiliaPermiso ? "F" : "I";

            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Codigo",      obj.Codigo),
                db.CrearParametro("@Descripcion", obj.Descripcion),
                db.CrearParametro("@Tipo",        tipo)
            };

            return db.LeerEscalar("InsertarPermiso", parametros);
        }

        public override int Eliminar(Permiso obj)
        {
            if (!(obj is FamiliaPermiso))
                throw new InvalidOperationException("Sólo se pueden eliminar familias de permisos, no patentes.");

            AccesoDB db = AccesoDB.GetInstancia();
            return db.Escribir("EliminarFamiliaPermiso", new List<SqlParameter>
            {
                db.CrearParametro("@PermisoId", obj.Id)
            });
        }

        public override List<Permiso> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarPermisos", new List<SqlParameter>());

            var lista = new List<Permiso>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        public List<KeyValuePair<Permiso, int?>> ListarConPadre()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarPermisos", new List<SqlParameter>());

            var lista = new List<KeyValuePair<Permiso, int?>>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(new KeyValuePair<Permiso, int?>(MapearFila(row), ObtenerIdPadre(row)));

            return lista;
        }

        public List<Permiso> ListarPorRol(int rolId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@RolId", rolId) };
            DataTable tabla = db.Leer("ListarPermisosDeRol", parametros);

            var lista = new List<Permiso>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        public List<Permiso> ListarDirectosDeUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@UsuarioId", usuarioId) };
            DataTable tabla = db.Leer("ListarPermisosDirectosDeUsuario", parametros);

            var lista = new List<Permiso>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));

            return lista;
        }

        public void AgregarHijo(int idPadre, int idHijo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("AgregarHijoPermiso", new List<SqlParameter>
            {
                db.CrearParametro("@IdPadre", idPadre),
                db.CrearParametro("@IdHijo",  idHijo)
            });
        }

        public void QuitarHijo(int idHijo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("QuitarHijoPermiso", new List<SqlParameter>
            {
                db.CrearParametro("@IdHijo", idHijo)
            });
        }

        public void AsignarAUsuario(int usuarioId, int permisoId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("AsignarPermisoAUsuario", new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId),
                db.CrearParametro("@PermisoId", permisoId)
            });
        }

        public void QuitarDeUsuario(int usuarioId, int permisoId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("QuitarPermisoDeUsuario", new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId),
                db.CrearParametro("@PermisoId", permisoId)
            });
        }

        public void ActualizarDVH(int permisoId, string dvh)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ActualizarDVHPermiso", new List<SqlParameter>
            {
                db.CrearParametro("@PermisoId", permisoId),
                db.CrearParametro("@DVH",       dvh)
            });
        }

        private static Permiso MapearFila(DataRow row)
        {
            string tipo = (string)row["Tipo"];
            Permiso permiso = tipo == "F" ? (Permiso)new FamiliaPermiso() : new PermisoIndividual();

            permiso.Id          = (int)row["Id"];
            permiso.Codigo      = (string)row["Codigo"];
            permiso.Descripcion = row.IsNull("Descripcion") ? null : (string)row["Descripcion"];
            permiso.DVH         = row.IsNull("DVH") ? null : (string)row["DVH"];
            return permiso;
        }

        private static int? ObtenerIdPadre(DataRow row)
        {
            return row.IsNull("IdPadre") ? (int?)null : (int)row["IdPadre"];
        }
    }
}
