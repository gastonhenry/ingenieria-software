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
                db.CrearParametro("@IdModelo",           u.Modelo.Id),
                db.CrearParametro("@Anio",               u.Anio),
                db.CrearParametro("@Kilometraje",        u.Kilometraje),
                db.CrearParametro("@PrecioCompra",       u.PrecioCompra),
                db.CrearParametro("@Descripcion",        u.Descripcion),
                db.CrearParametro("@IdPersona",          u.Vendedor.Id),
                db.CrearParametro("@IdCompradorUsuario", u.Comprador.Id)
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

        // Arma la unidad con su Modelo (y Marca) completos. Del vendedor y del comprador el SP trae
        // sólo los datos de identificación; la ficha completa del vendedor la carga
        // UnidadService.ObtenerTrazabilidad.
        private static Unidad MapearFila(DataRow row)
        {
            return new Unidad
            {
                Id           = (int)row["Id"],
                Dominio      = (string)row["Dominio"],
                Modelo       = new Modelo
                {
                    Id             = (int)row["IdModelo"],
                    Nombre         = (string)row["ModeloNombre"],
                    TipoCarroceria = (TipoCarroceria)(int)row["TipoCarroceria"],
                    Marca          = new Marca { Id = (int)row["IdMarca"], Nombre = (string)row["MarcaNombre"] }
                },
                Anio         = (int)row["Anio"],
                Kilometraje  = (int)row["Kilometraje"],
                PrecioCompra = (decimal)row["PrecioCompra"],
                Descripcion  = row.IsNull("Descripcion") ? null : (string)row["Descripcion"],
                EstadoActual = (EstadoUnidad)Convert.ToInt32(row["EstadoActual"]),
                Vendedor     = new Persona
                {
                    Id          = (int)row["IdPersona"],
                    Nombre      = (string)row["VendedorNombre"],
                    Documento   = (string)row["VendedorDocumento"],
                    TipoPersona = Convert.ToChar(row["VendedorTipo"])
                },
                Comprador    = new Usuario
                {
                    Id       = (int)row["IdCompradorUsuario"],
                    Username = (string)row["CompradorUsername"]
                },
                FechaIngreso = (DateTime)row["FechaIngreso"],
                DVH          = row.IsNull("DVH") ? null : (string)row["DVH"]
            };
        }
    }
}
