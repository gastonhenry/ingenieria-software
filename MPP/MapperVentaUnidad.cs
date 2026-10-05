using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperVentaUnidad
    {
        public int Insertar(VentaUnidad v)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdUnidad",           v.IdUnidad),
                db.CrearParametro("@Tipo",               (int)v.Tipo),
                db.CrearParametro("@IdPersonaComprador", v.IdPersonaComprador),
                db.CrearParametro("@IdVendedorUsuario",  v.IdVendedorUsuario),
                db.CrearParametro("@FechaOperacion",     v.FechaOperacion),
                db.CrearParametro("@PrecioAcordado",     v.PrecioAcordado)
            };
            parametros.Add(new SqlParameter("@MontoSena", System.Data.SqlDbType.Decimal)
            {
                Value = (object)v.MontoSena ?? DBNull.Value
            });
            parametros.Add(new SqlParameter("@FechaEstimadaFin", System.Data.SqlDbType.Date)
            {
                Value = (object)v.FechaEstimadaFin ?? DBNull.Value
            });
            parametros.Add(new SqlParameter("@Descripcion", System.Data.SqlDbType.NVarChar, -1)
            {
                Value = string.IsNullOrWhiteSpace(v.Descripcion) ? (object)DBNull.Value : v.Descripcion
            });
            return db.LeerEscalar("InsertarVentaUnidad", parametros);
        }

        public void CancelarVentaActiva(int idUnidad, string comentario)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdUnidad", idUnidad)
            };
            parametros.Add(new SqlParameter("@Comentario", System.Data.SqlDbType.NVarChar, 500)
            {
                Value = string.IsNullOrWhiteSpace(comentario) ? (object)DBNull.Value : comentario
            });
            db.Escribir("CancelarVentaActivaDeUnidad", parametros);
        }

        public VentaUnidad ObtenerActivaDeUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ObtenerVentaActivaDeUnidad",
                new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) });
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public List<VentaUnidad> ListarPorUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarVentasPorUnidad",
                new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) });
            var lista = new List<VentaUnidad>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        private static VentaUnidad MapearFila(DataRow row)
        {
            return new VentaUnidad
            {
                Id                    = (int)row["Id"],
                IdUnidad              = (int)row["IdUnidad"],
                Tipo                  = (TipoOperacionVenta)Convert.ToInt32(row["Tipo"]),
                Activa                = (bool)row["Activa"],
                IdPersonaComprador    = (int)row["IdPersonaComprador"],
                IdVendedorUsuario     = (int)row["IdVendedorUsuario"],
                FechaOperacion        = (DateTime)row["FechaOperacion"],
                PrecioAcordado        = (decimal)row["PrecioAcordado"],
                MontoSena             = row.IsNull("MontoSena")             ? (decimal?)null  : (decimal)row["MontoSena"],
                FechaEstimadaFin      = row.IsNull("FechaEstimadaFin")      ? (DateTime?)null : Convert.ToDateTime(row["FechaEstimadaFin"]),
                Descripcion           = row.IsNull("Descripcion")           ? null            : (string)row["Descripcion"],
                ComentarioCancelacion = row.IsNull("ComentarioCancelacion") ? null            : (string)row["ComentarioCancelacion"]
            };
        }
    }
}
