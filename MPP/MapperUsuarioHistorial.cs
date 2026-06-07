using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperUsuarioHistorial : Mapper<UsuarioHistorial>
    {
        public override int Insertar(UsuarioHistorial h)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", h.UsuarioId),
                db.CrearParametro("@Accion",    h.Accion.ToString()),
                new SqlParameter("@ModificadoPorUsuarioId", SqlDbType.Int)
                {
                    Value = h.ModificadoPorUsuarioId.HasValue
                        ? (object)h.ModificadoPorUsuarioId.Value
                        : DBNull.Value
                },
                new SqlParameter("@RestauracionId", SqlDbType.Int)
                {
                    Value = h.RestauracionId.HasValue
                        ? (object)h.RestauracionId.Value
                        : DBNull.Value
                },
                db.CrearParametro("@Nombre",    h.Nombre),
                db.CrearParametro("@Apellido",  h.Apellido),
                db.CrearParametro("@Email",     h.Email),
                db.CrearParametro("@Telefono",  h.Telefono),
                db.CrearParametro("@Documento", h.Documento),
                db.CrearParametro("@Domicilio", h.Domicilio),
                db.CrearParametro("@Bloqueado", h.Bloqueado)
            };
            return db.LeerEscalar("InsertarUsuarioHistorial", parametros);
        }

        public List<UsuarioHistorial> ListarPorUsuario(int usuarioId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", usuarioId)
            };
            DataTable tabla = db.Leer("ListarUsuarioHistorial", parametros);

            var lista = new List<UsuarioHistorial>();
            foreach (DataRow row in tabla.Rows)
                lista.Add(MapearFila(row));
            return lista;
        }

        public UsuarioHistorial ObtenerPorIdHistorial(int historialId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Id", historialId)
            };
            DataTable tabla = db.Leer("ObtenerUsuarioHistorialPorId", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        private static UsuarioHistorial MapearFila(DataRow row)
        {
            AccionUsuarioHistorial accion;
            if (!Enum.TryParse((string)row["Accion"], out accion))
                accion = AccionUsuarioHistorial.Edicion;

            return new UsuarioHistorial
            {
                Id                     = (int)row["Id"],
                UsuarioId              = (int)row["UsuarioId"],
                FechaHora              = (DateTime)row["FechaHora"],
                Accion                 = accion,
                ModificadoPorUsuarioId = row.IsNull("ModificadoPorUsuarioId") ? (int?)null : (int)row["ModificadoPorUsuarioId"],
                ModificadoPorUsername  = row.IsNull("ModificadoPorUsername")  ? null       : (string)row["ModificadoPorUsername"],
                RestauracionId         = row.IsNull("RestauracionId")         ? (int?)null : (int)row["RestauracionId"],
                RestauracionFechaHora  = row.IsNull("RestauracionFechaHora")  ? (DateTime?)null : (DateTime)row["RestauracionFechaHora"],
                Nombre                 = (string)row["Nombre"],
                Apellido               = (string)row["Apellido"],
                Email                  = (string)row["Email"],
                Telefono               = (string)row["Telefono"],
                Documento              = (string)row["Documento"],
                Domicilio              = (string)row["Domicilio"],
                Bloqueado              = (bool)row["Bloqueado"]
            };
        }
    }
}
