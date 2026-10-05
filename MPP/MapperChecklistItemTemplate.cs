using BE;
using DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperChecklistItemTemplate : Mapper<ChecklistItemTemplate>
    {
        public override int Insertar(ChecklistItemTemplate t)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Nombre",      t.Nombre),
                db.CrearParametro("@Descripcion", t.Descripcion)
            };
            return db.LeerEscalar("InsertarChecklistItemTemplate", parametros);
        }

        // Solo se puede editar la descripción — el nombre queda inmutable para
        // preservar la trazabilidad de IdTemplateOrigen en los checklists ya generados.
        public void EditarDescripcion(int templateId, string descripcion)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@TemplateId",  templateId),
                db.CrearParametro("@Descripcion", descripcion)
            };
            db.Escribir("EditarDescripcionChecklistItemTemplate", parametros);
        }

        public void BajaLogica(int templateId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("BajaLogicaChecklistItemTemplate",
                new List<SqlParameter> { db.CrearParametro("@TemplateId", templateId) });
        }

        public void Reactivar(int templateId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Escribir("ReactivarChecklistItemTemplate",
                new List<SqlParameter> { db.CrearParametro("@TemplateId", templateId) });
        }

        public override List<ChecklistItemTemplate> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarChecklistItemsTemplate");
            var lista = new List<ChecklistItemTemplate>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public List<ChecklistItemTemplate> ListarActivos()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarChecklistItemsTemplateActivos");
            var lista = new List<ChecklistItemTemplate>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public void ActualizarDVH(int templateId, string dvh)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@TemplateId", templateId),
                db.CrearParametro("@DVH",        dvh)
            };
            db.Escribir("ActualizarDVHChecklistItemTemplate", parametros);
        }

        public override ChecklistItemTemplate Obtener(int id)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@TemplateId", id) };
            DataTable tabla = db.Leer("ObtenerChecklistItemTemplatePorId", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        private static ChecklistItemTemplate MapearFila(DataRow row)
        {
            return new ChecklistItemTemplate
            {
                Id          = (int)row["Id"],
                Nombre      = (string)row["Nombre"],
                Descripcion = row.IsNull("Descripcion") ? null : (string)row["Descripcion"],
                Activo      = (bool)row["Activo"],
                DVH         = row.IsNull("DVH") ? null : (string)row["DVH"]
            };
        }
    }
}
