using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    // Concentra la persistencia del Checklist de una unidad y sus items.
    public class MapperChecklist
    {
        public int CrearParaUnidad(int idUnidad, int idCreadorUsuario)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdUnidad",         idUnidad),
                db.CrearParametro("@IdCreadorUsuario", idCreadorUsuario)
            };
            return db.LeerEscalar("CrearChecklistDeUnidad", parametros);
        }

        public ChecklistPreparacion ObtenerPorUnidad(int idUnidad)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@IdUnidad", idUnidad) };
            DataTable tabla = db.Leer("ObtenerChecklistPorUnidad", parametros);
            if (tabla.Rows.Count == 0) return null;

            DataRow row = tabla.Rows[0];
            var checklist = new ChecklistPreparacion
            {
                Id               = (int)row["Id"],
                IdUnidad         = (int)row["IdUnidad"],
                FechaCreacion    = (DateTime)row["FechaCreacion"],
                IdCreadorUsuario = (int)row["IdCreadorUsuario"]
            };
            checklist.Items = ListarItems(checklist.Id);
            return checklist;
        }

        public List<ChecklistItem> ListarItems(int idChecklist)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@IdChecklist", idChecklist) };
            DataTable tabla = db.Leer("ListarItemsDeChecklist", parametros);

            var lista = new List<ChecklistItem>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearItem(row));
            return lista;
        }

        public int AgregarItemExtra(int idChecklist, string nombre, string descripcion, decimal? costoEstimado)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdChecklist",   idChecklist),
                db.CrearParametro("@Nombre",        nombre),
                db.CrearParametro("@Descripcion",   descripcion),
            };
            var pCosto = new SqlParameter("@CostoEstimado", System.Data.SqlDbType.Decimal)
            {
                Value = (object)costoEstimado ?? DBNull.Value
            };
            parametros.Add(pCosto);
            return db.LeerEscalar("AgregarItemExtraAChecklist", parametros);
        }

        public void AprobarItemsExtraPropuestos(int idChecklist)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("AprobarItemsExtraPropuestos",
                new List<SqlParameter> { db.CrearParametro("@IdChecklist", idChecklist) });
        }

        public void RechazarItemsExtraPropuestos(int idChecklist)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("RechazarItemsExtraPropuestos",
                new List<SqlParameter> { db.CrearParametro("@IdChecklist", idChecklist) });
        }

        public int ContarItemsExtraPropuestos(int idChecklist)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            return db.LeerEscalar("ContarItemsExtraPropuestos",
                new List<SqlParameter> { db.CrearParametro("@IdChecklist", idChecklist) });
        }

        public void EliminarItemExtra(int idItem)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("EliminarItemExtraDeChecklist",
                new List<SqlParameter> { db.CrearParametro("@IdItem", idItem) });
        }

        public void MarcarItemRevisado(int idItem, ResultadoRevision resultado, string comentario, int idUsuarioRevisor)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@IdItem",            idItem),
                db.CrearParametro("@ResultadoRevision", (int)resultado),
                db.CrearParametro("@IdUsuarioRevisor",  idUsuarioRevisor)
            };
            var pCom = new SqlParameter("@ComentarioRevision", System.Data.SqlDbType.NVarChar, 500)
            {
                Value = string.IsNullOrWhiteSpace(comentario) ? (object)DBNull.Value : comentario.Trim()
            };
            parametros.Add(pCom);
            db.Escribir("MarcarItemChecklistRevisado", parametros);
        }

        private static ChecklistItem MapearItem(DataRow row)
        {
            return new ChecklistItem
            {
                Id                  = (int)row["Id"],
                IdChecklist         = (int)row["IdChecklist"],
                Nombre              = (string)row["Nombre"],
                Descripcion         = row.IsNull("Descripcion")        ? null            : (string)row["Descripcion"],
                EsDelTemplate       = (bool)row["EsDelTemplate"],
                IdTemplateOrigen    = row.IsNull("IdTemplateOrigen")   ? (int?)null      : (int)row["IdTemplateOrigen"],
                ResultadoRevision   = (ResultadoRevision)Convert.ToInt32(row["ResultadoRevision"]),
                ComentarioRevision  = row.IsNull("ComentarioRevision") ? null            : (string)row["ComentarioRevision"],
                FechaRevision       = row.IsNull("FechaRevision")      ? (DateTime?)null : (DateTime)row["FechaRevision"],
                IdUsuarioRevisor    = row.IsNull("IdUsuarioRevisor")   ? (int?)null      : (int)row["IdUsuarioRevisor"],
                CostoEstimado       = row.IsNull("CostoEstimado")      ? (decimal?)null  : (decimal)row["CostoEstimado"],
                EstadoAprobacion    = (EstadoAprobacionItem)Convert.ToInt32(row["EstadoAprobacion"])
            };
        }
    }
}
