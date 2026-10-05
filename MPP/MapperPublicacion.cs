using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperPublicacion
    {
        public int CrearSiNoExiste(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("CrearPublicacion",
                new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) });
        }

        public PublicacionUnidad ObtenerPorUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ObtenerPublicacionPorUnidad",
                new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) });
            if (tabla.Rows.Count == 0) return null;
            DataRow row = tabla.Rows[0];
            return new PublicacionUnidad
            {
                Id                      = (int)row["Id"],
                IdUnidad                = (int)row["IdUnidad"],
                PrecioPublicacion       = row.IsNull("PrecioPublicacion")      ? (decimal?)null : (decimal)row["PrecioPublicacion"],
                DescripcionPublicacion  = row.IsNull("DescripcionPublicacion") ? null           : (string)row["DescripcionPublicacion"],
                FechaCreacion           = (DateTime)row["FechaCreacion"],
                FechaUltimaEdicion      = row.IsNull("FechaUltimaEdicion")     ? (DateTime?)null : (DateTime)row["FechaUltimaEdicion"]
            };
        }

        // Publicaciones ya iniciadas (con fila en PublicacionUnidad) de unidades En venta / Pendiente de venta.
        public List<PublicacionListado> ListarPublicaciones()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarPublicaciones");
            var lista = new List<PublicacionListado>();
            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new PublicacionListado
                {
                    IdUnidad               = (int)row["IdUnidad"],
                    Dominio                = (string)row["Dominio"],
                    Marca                  = (string)row["Marca"],
                    Modelo                 = (string)row["Modelo"],
                    Anio                   = (int)row["Anio"],
                    Kilometraje            = (int)row["Kilometraje"],
                    EstadoActual           = (BE.Enums.EstadoUnidad)Convert.ToInt32(row["EstadoActual"]),
                    PrecioPublicacion      = row.IsNull("PrecioPublicacion")      ? (decimal?)null  : (decimal)row["PrecioPublicacion"],
                    DescripcionPublicacion = row.IsNull("DescripcionPublicacion") ? null            : (string)row["DescripcionPublicacion"],
                    FechaCreacion          = (DateTime)row["FechaCreacion"],
                    FechaUltimaEdicion     = row.IsNull("FechaUltimaEdicion")     ? (DateTime?)null : (DateTime)row["FechaUltimaEdicion"],
                    CantidadImagenes       = Convert.ToInt32(row["CantidadImagenes"])
                });
            }
            return lista;
        }

        public void Actualizar(int idUnidad, decimal? precio, string descripcion)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdUnidad", idUnidad)
            };
            var pPrecio = new SqlParameter("@PrecioPublicacion", System.Data.SqlDbType.Decimal)
            {
                Value = (object)precio ?? DBNull.Value
            };
            var pDesc = new SqlParameter("@DescripcionPublicacion", System.Data.SqlDbType.NVarChar, -1)
            {
                Value = string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion
            };
            parametros.Add(pPrecio);
            parametros.Add(pDesc);
            db.Escribir("ActualizarPublicacion", parametros);
        }
    }
}
