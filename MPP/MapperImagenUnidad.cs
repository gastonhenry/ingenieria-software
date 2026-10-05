using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperImagenUnidad
    {
        public int Agregar(int idUnidad, string rutaArchivo, int orden)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdUnidad",    idUnidad),
                db.CrearParametro("@RutaArchivo", rutaArchivo),
                db.CrearParametro("@Orden",       orden)
            };
            return db.LeerEscalar("AgregarImagenUnidad", parametros);
        }

        public void Eliminar(int id)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("EliminarImagenUnidad",
                new List<SqlParameter> { db.CrearParametro("@Id", id) });
        }

        public ImagenUnidad ObtenerPorId(int id)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ObtenerImagenUnidadPorId",
                new List<SqlParameter> { db.CrearParametro("@Id", id) });
            return tabla.Rows.Count == 0 ? null : MapearFila(tabla.Rows[0]);
        }

        public List<ImagenUnidad> ListarPorUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarImagenesPorUnidad",
                new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) });
            var lista = new List<ImagenUnidad>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        private static ImagenUnidad MapearFila(DataRow row)
        {
            return new ImagenUnidad
            {
                Id          = (int)row["Id"],
                RutaArchivo = (string)row["RutaArchivo"],
                Orden       = (int)row["Orden"],
                FechaCarga  = (DateTime)row["FechaCarga"]
            };
        }
    }
}
