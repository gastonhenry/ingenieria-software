using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    // N03 — parámetros, suspensión de avisos, notificaciones y datos del tablero de captación.
    public class MapperCaptacion
    {
        // ------------------------------------------------------------
        // Parámetros
        // ------------------------------------------------------------

        public ParametrosCaptacion ObtenerParametros()
        {
            DataTable tabla = AccesoDB.GetInstancia().Leer("ObtenerParametrosCaptacion");
            DataRow row = tabla.Rows[0];
            return new ParametrosCaptacion
            {
                NotificacionesActivas    = (bool)row["NotificacionesActivas"],
                DiasVigenciaBusqueda     = (int)row["DiasVigenciaBusqueda"],
                DiasAlertaSinInteresados = (int)row["DiasAlertaSinInteresados"]
            };
        }

        public void ActualizarParametros(ParametrosCaptacion p)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ActualizarParametrosCaptacion", new List<SqlParameter>
            {
                db.CrearParametro("@NotificacionesActivas",    p.NotificacionesActivas),
                db.CrearParametro("@DiasVigenciaBusqueda",     p.DiasVigenciaBusqueda),
                db.CrearParametro("@DiasAlertaSinInteresados", p.DiasAlertaSinInteresados)
            });
        }

        // ------------------------------------------------------------
        // Suspensión de avisos
        // ------------------------------------------------------------

        public void SuspenderAvisos(int idPersona, int idUsuario, string motivo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("SuspenderAvisosCliente", new List<SqlParameter>
            {
                db.CrearParametro("@IdPersona", idPersona),
                db.CrearParametro("@IdUsuario", idUsuario),
                MapperBusquedaCliente.Opcional("@Motivo", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(motivo) ? null : motivo)
            });
        }

        public void ReanudarAvisos(int idPersona)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ReanudarAvisosCliente",
                new List<SqlParameter> { db.CrearParametro("@IdPersona", idPersona) });
        }

        public SuspensionAvisos ObtenerSuspension(int idPersona)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ObtenerSuspensionAvisos",
                new List<SqlParameter> { db.CrearParametro("@IdPersona", idPersona) });
            if (tabla.Rows.Count == 0) return null;
            DataRow row = tabla.Rows[0];
            return new SuspensionAvisos
            {
                IdPersona       = (int)row["IdPersona"],
                FechaSuspension = (DateTime)row["FechaSuspension"],
                Usuario         = new Usuario { Id = (int)row["IdUsuario"], Username = (string)row["Username"] },
                Motivo          = row.IsNull("Motivo") ? null : (string)row["Motivo"]
            };
        }

        // ------------------------------------------------------------
        // Notificaciones
        // ------------------------------------------------------------

        public int InsertarNotificacion(NotificacionEnviada n)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("InsertarNotificacionEnviada", new List<SqlParameter>
            {
                db.CrearParametro("@IdBusqueda", n.IdBusqueda),
                db.CrearParametro("@IdPersona",  n.IdPersona),
                db.CrearParametro("@IdUnidad",   n.IdUnidad),
                db.CrearParametro("@Exitoso",    n.Exitoso),
                MapperBusquedaCliente.Opcional("@Error", SqlDbType.NVarChar, Recortar(n.Error, 1000))
            });
        }

        public bool ExisteNotificacionExitosa(int idPersona, int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("ExisteNotificacionExitosa", new List<SqlParameter>
            {
                db.CrearParametro("@IdPersona", idPersona),
                db.CrearParametro("@IdUnidad",  idUnidad)
            }) == 1;
        }

        public List<NotificacionEnviada> ListarNotificaciones(DateTime? desde, DateTime? hasta, int? idPersona)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarNotificaciones", new List<SqlParameter>
            {
                MapperBusquedaCliente.Opcional("@Desde",     SqlDbType.DateTime, desde),
                MapperBusquedaCliente.Opcional("@Hasta",     SqlDbType.DateTime, hasta),
                MapperBusquedaCliente.Opcional("@IdPersona", SqlDbType.Int,      idPersona)
            });
            var lista = new List<NotificacionEnviada>();
            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new NotificacionEnviada
                {
                    Id            = (int)row["Id"],
                    IdBusqueda    = (int)row["IdBusqueda"],
                    IdPersona     = (int)row["IdPersona"],
                    IdUnidad      = (int)row["IdUnidad"],
                    FechaEnvio    = (DateTime)row["FechaEnvio"],
                    Exitoso       = (bool)row["Exitoso"],
                    Error         = row.IsNull("Error")        ? null : (string)row["Error"],
                    ClienteNombre = (string)row["ClienteNombre"],
                    ClienteEmail  = row.IsNull("ClienteEmail") ? null : (string)row["ClienteEmail"],
                    ClienteDocumento = (string)row["ClienteDocumento"],
                    Unidad        = (string)row["Unidad"]
                });
            }
            return lista;
        }

        // ------------------------------------------------------------
        // Tablero
        // ------------------------------------------------------------

        public List<UnidadEnVenta> ListarUnidadesEnVenta()
        {
            DataTable tabla = AccesoDB.GetInstancia().Leer("ListarUnidadesEnVentaCaptacion");
            var lista = new List<UnidadEnVenta>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearUnidad(row));
            return lista;
        }

        public UnidadEnVenta ObtenerUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarUnidadesEnVentaCaptacion",
                new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) });
            return tabla.Rows.Count == 0 ? null : MapearUnidad(tabla.Rows[0]);
        }

        public List<ConversionTablero> ListarConversiones(DateTime desde, DateTime hasta)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarConversionesCaptacion", new List<SqlParameter>
            {
                db.CrearParametro("@Desde", desde),
                db.CrearParametro("@Hasta", hasta)
            });
            var lista = new List<ConversionTablero>();
            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new ConversionTablero
                {
                    IdPersona      = (int)row["IdPersona"],
                    ClienteNombre  = (string)row["ClienteNombre"],
                    UnidadComprada = (string)row["UnidadComprada"],
                    FechaAviso     = (DateTime)row["FechaAviso"],
                    FechaVenta     = (DateTime)row["FechaVenta"],
                    Precio         = (decimal)row["Precio"]
                });
            }
            return lista;
        }

        public int ContarClientesAvisados(DateTime desde, DateTime hasta)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("ContarClientesAvisados", new List<SqlParameter>
            {
                db.CrearParametro("@Desde", desde),
                db.CrearParametro("@Hasta", hasta)
            });
        }

        private static UnidadEnVenta MapearUnidad(DataRow row)
        {
            return new UnidadEnVenta
            {
                IdUnidad          = (int)row["IdUnidad"],
                Dominio           = (string)row["Dominio"],
                IdMarca           = (int)row["IdMarca"],
                Marca             = (string)row["Marca"],
                IdModelo          = (int)row["IdModelo"],
                Modelo            = (string)row["Modelo"],
                TipoCarroceria    = (TipoCarroceria)Convert.ToInt32(row["TipoCarroceria"]),
                Anio              = (int)row["Anio"],
                Kilometraje       = (int)row["Kilometraje"],
                PrecioPublicacion = row.IsNull("PrecioPublicacion") ? (decimal?)null : (decimal)row["PrecioPublicacion"],
                FechaEnVenta      = (DateTime)row["FechaEnVenta"]
            };
        }

        private static string Recortar(string texto, int max) =>
            texto == null || texto.Length <= max ? texto : texto.Substring(0, max);
    }
}
