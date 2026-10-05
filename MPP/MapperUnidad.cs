using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperUnidad : Mapper<Unidad>
    {
        public override int Insertar(Unidad u)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Dominio",            u.Dominio),
                db.CrearParametro("@Marca",              u.Marca),
                db.CrearParametro("@Modelo",             u.Modelo),
                db.CrearParametro("@Anio",               u.Anio),
                db.CrearParametro("@Kilometraje",        u.Kilometraje),
                db.CrearParametro("@PrecioCompra",       u.PrecioCompra),
                db.CrearParametro("@Descripcion",        u.Descripcion),
                db.CrearParametro("@IdPersona",          u.IdPersona),
                db.CrearParametro("@IdCompradorUsuario", u.IdCompradorUsuario)
            };
            return db.LeerEscalar("InsertarUnidad", parametros);
        }

        public override Unidad Obtener(int id)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@UnidadId", id) };
            DataTable tabla = db.Leer("ObtenerUnidadPorId", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public Unidad ObtenerPorDominio(string dominio)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@Dominio", dominio) };
            DataTable tabla = db.Leer("ObtenerUnidadPorDominio", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public override List<Unidad> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarUnidades");
            var lista = new List<Unidad>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public List<Unidad> ListarPorEstado(EstadoUnidad estado)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@Estado", (int)estado) };
            DataTable tabla = db.Leer("ListarUnidadesPorEstado", parametros);
            var lista = new List<Unidad>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public List<Unidad> ListarParaVerificacion()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarUnidadesParaVerificacion");
            var lista = new List<Unidad>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        // Actualiza el estado y persiste la entrada de historial en una única transacción de SP.
        public void TransicionarEstado(int unidadId, EstadoUnidad origen, EstadoUnidad destino, int idUsuario, string motivo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UnidadId",      unidadId),
                db.CrearParametro("@EstadoOrigen",  (int)origen),
                db.CrearParametro("@EstadoDestino", (int)destino),
                db.CrearParametro("@IdUsuario",     idUsuario),
                db.CrearParametro("@Motivo",        motivo)
            };
            db.EscribirConTransaccion("TransicionarEstadoUnidad", parametros);
        }

        public void ActualizarDVH(int unidadId, string dvh)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UnidadId", unidadId),
                db.CrearParametro("@DVH",      dvh)
            };
            db.Escribir("ActualizarDVHUnidad", parametros);
        }

        private static Unidad MapearFila(DataRow row)
        {
            return new Unidad
            {
                Id                 = (int)row["Id"],
                Dominio            = (string)row["Dominio"],
                Marca              = (string)row["Marca"],
                Modelo             = (string)row["Modelo"],
                Anio               = (int)row["Anio"],
                Kilometraje        = (int)row["Kilometraje"],
                PrecioCompra       = (decimal)row["PrecioCompra"],
                Descripcion        = row.IsNull("Descripcion") ? null : (string)row["Descripcion"],
                EstadoActual       = (EstadoUnidad)Convert.ToInt32(row["EstadoActual"]),
                IdPersona          = (int)row["IdPersona"],
                IdCompradorUsuario = (int)row["IdCompradorUsuario"],
                FechaIngreso       = (DateTime)row["FechaIngreso"],
                DVH                = row.IsNull("DVH") ? null : (string)row["DVH"]
            };
        }
    }
}
