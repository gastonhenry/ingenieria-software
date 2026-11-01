using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MPP
{
    // N03 — búsquedas de clientes potenciales.
    public class MapperBusquedaCliente
    {
        public int Insertar(BusquedaCliente b)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdPersona",           b.Cliente.Id),
                db.CrearParametro("@IdUsuarioEntrevista", b.Entrevistador.Id),
                db.CrearParametro("@FechaVencimiento",    b.FechaVencimiento),
                db.CrearParametro("@AceptaAvisos",        b.AceptaAvisos),
                // Multiselección: listas separadas por coma ("3,7,12"); el SP las separa.
                Opcional("@Marcas",         SqlDbType.NVarChar, ListaCsv(b.Marcas.Select(m => m.Id))),
                Opcional("@Modelos",        SqlDbType.NVarChar, ListaCsv(b.Modelos.Select(m => m.Id))),
                Opcional("@Carrocerias",    SqlDbType.NVarChar, ListaCsv(b.Carrocerias.Select(c => (int)c))),
                Opcional("@AnioDesde",      SqlDbType.Int,      b.AnioDesde),
                Opcional("@AnioHasta",      SqlDbType.Int,      b.AnioHasta),
                Opcional("@PrecioMaximo",   SqlDbType.Decimal,  b.PrecioMaximo),
                Opcional("@Observaciones",  SqlDbType.NVarChar, string.IsNullOrWhiteSpace(b.Observaciones) ? null : b.Observaciones)
            };
            return db.LeerEscalar("InsertarBusquedaCliente", parametros);
        }

        public List<BusquedaCliente> Listar(EstadoBusqueda? estado, int? idPersona)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                Opcional("@Estado",    SqlDbType.Int, estado.HasValue ? (int?)estado.Value : null),
                Opcional("@IdPersona", SqlDbType.Int, idPersona)
            };
            DataTable tabla = db.Leer("ListarBusquedasCliente", parametros);
            var lista = new List<BusquedaCliente>();
            var porId = new Dictionary<int, BusquedaCliente>();
            foreach (DataRow row in tabla.Rows)
            {
                var b = MapearFila(row);
                lista.Add(b);
                porId[b.Id] = b;
            }
            if (lista.Count == 0) return lista;

            // Criterios multiselección (marcas, modelos, carrocerías) con los mismos filtros.
            DataTable criterios = db.Leer("ListarCriteriosBusquedaCliente", new List<SqlParameter>
            {
                Opcional("@Estado",    SqlDbType.Int, estado.HasValue ? (int?)estado.Value : null),
                Opcional("@IdPersona", SqlDbType.Int, idPersona)
            });
            foreach (DataRow row in criterios.Rows)
            {
                if (!porId.TryGetValue((int)row["IdBusqueda"], out var b)) continue;
                int valor = (int)row["Valor"];
                switch ((string)row["Criterio"])
                {
                    case "MA":
                        b.Marcas.Add(new Marca { Id = valor, Nombre = (string)row["Nombre"] });
                        break;
                    case "MO":
                        b.Modelos.Add(new Modelo
                        {
                            Id = valor,
                            Nombre = (string)row["Nombre"],
                            Marca = new Marca { Id = (int)row["IdMarca"], Nombre = (string)row["MarcaNombre"] }
                        });
                        break;
                    case "CA":
                        b.Carrocerias.Add((TipoCarroceria)valor);
                        break;
                }
            }
            return lista;
        }

        public List<Persona> BuscarPersonasConBusquedas(string texto)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("BuscarPersonasConBusquedas",
                new List<SqlParameter> { db.CrearParametro("@Texto", texto) });
            var lista = new List<Persona>();
            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new Persona
                {
                    Id          = (int)row["Id"],
                    TipoPersona = Convert.ToChar(row["TipoPersona"]),
                    Documento   = (string)row["Documento"],
                    Nombre      = (string)row["Nombre"],
                    Telefono    = row.IsNull("Telefono") ? null : (string)row["Telefono"],
                    Email       = row.IsNull("Email")    ? null : (string)row["Email"]
                });
            }
            return lista;
        }

        public void Cerrar(int idBusqueda, string motivo)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("CerrarBusquedaCliente", new List<SqlParameter>
            {
                db.CrearParametro("@Id", idBusqueda),
                Opcional("@Motivo", SqlDbType.NVarChar, string.IsNullOrWhiteSpace(motivo) ? null : motivo)
            });
        }

        public int VencerBusquedas()
        {
            return AccesoDB.GetInstancia().LeerEscalar("VencerBusquedasCliente");
        }

        private static BusquedaCliente MapearFila(DataRow row)
        {
            var b = new BusquedaCliente
            {
                Id               = (int)row["Id"],
                Cliente          = new Persona
                {
                    Id          = (int)row["IdPersona"],
                    Nombre      = (string)row["ClienteNombre"],
                    Documento   = (string)row["ClienteDocumento"],
                    TipoPersona = Convert.ToChar(row["ClienteTipo"]),
                    Telefono    = row.IsNull("ClienteTelefono") ? null : (string)row["ClienteTelefono"],
                    Email       = row.IsNull("ClienteEmail")    ? null : (string)row["ClienteEmail"]
                },
                Entrevistador    = new Usuario
                {
                    Id       = (int)row["IdUsuarioEntrevista"],
                    Username = (string)row["EntrevistadorUsername"]
                },
                FechaRegistro     = (DateTime)row["FechaRegistro"],
                FechaVencimiento  = (DateTime)row["FechaVencimiento"],
                AnioDesde         = row.IsNull("AnioDesde")      ? (int?)null     : (int)row["AnioDesde"],
                AnioHasta         = row.IsNull("AnioHasta")      ? (int?)null     : (int)row["AnioHasta"],
                PrecioMaximo      = row.IsNull("PrecioMaximo")   ? (decimal?)null : (decimal)row["PrecioMaximo"],
                Observaciones     = row.IsNull("Observaciones")  ? null           : (string)row["Observaciones"],
                AceptaAvisos      = (bool)row["AceptaAvisos"],
                Estado            = (EstadoBusqueda)Convert.ToInt32(row["Estado"]),
                FechaCierre       = row.IsNull("FechaCierre")    ? (DateTime?)null : (DateTime)row["FechaCierre"],
                MotivoCierre      = row.IsNull("MotivoCierre")   ? null            : (string)row["MotivoCierre"],
                AvisosSuspendidos = (bool)row["AvisosSuspendidos"]
            };
            return b;
        }

        // "3,7,12" o null si la lista está vacía.
        private static string ListaCsv(IEnumerable<int> ids)
        {
            string csv = string.Join(",", ids.Distinct());
            return csv.Length == 0 ? null : csv;
        }

        internal static SqlParameter Opcional(string nombre, SqlDbType tipo, object valor)
        {
            var p = new SqlParameter(nombre, tipo) { Value = valor ?? DBNull.Value };
            if (tipo == SqlDbType.NVarChar) p.Size = -1;
            return p;
        }
    }
}
