using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperHistorialUnidad
    {
        public List<HistorialEstadoUnidad> ListarPorUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) };
            DataTable tabla = db.Leer("ListarHistorialEstadoUnidad", parametros);

            var lista = new List<HistorialEstadoUnidad>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        private static HistorialEstadoUnidad MapearFila(DataRow row)
        {
            return new HistorialEstadoUnidad
            {
                Id            = (int)row["Id"],
                EstadoOrigen  = row.IsNull("EstadoOrigen") ? (EstadoUnidad?)null : (EstadoUnidad)Convert.ToInt32(row["EstadoOrigen"]),
                EstadoDestino = (EstadoUnidad)Convert.ToInt32(row["EstadoDestino"]),
                Usuario       = new Usuario
                {
                    Id       = (int)row["IdUsuario"],
                    Username = (string)row["Username"]
                },
                FechaHora     = (DateTime)row["FechaHora"],
                Motivo        = row.IsNull("Motivo") ? null : (string)row["Motivo"]
            };
        }
    }
}
