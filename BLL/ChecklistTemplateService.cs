using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class ChecklistTemplateService : IChecklistTemplateService
    {
        private readonly MapperChecklistItemTemplate _mapperTemplate;
        private readonly PermisoService _permisoService;
        private readonly BitacoraService _bitacoraService;

        public ChecklistTemplateService()
        {
            _mapperTemplate = new MapperChecklistItemTemplate();
            _permisoService = new PermisoService();
            _bitacoraService = new BitacoraService();
        }

        private void RequerirPermiso()
        {
            var usuarioActual = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuarioActual != null && usuarioActual.Username.ToLower() == "admin";
            if (esAdmin) return;
            if (usuarioActual != null && _permisoService.UsuarioTienePermiso(usuarioActual, "GESTIONAR_TEMPLATE_CHECKLIST")) return;

            _bitacoraService.Insertar(usuarioActual, TipoBitacora.Error,
                "Acceso denegado a gestión de template de checklist.");
            throw new BLLException("ERR_SIN_PERMISO_TEMPLATE_CHECKLIST",
                "No tenés permiso para gestionar el template del checklist.");
        }

        public List<ChecklistItemTemplate> Listar() => _mapperTemplate.Listar();

        public List<ChecklistItemTemplate> ListarActivos() => _mapperTemplate.ListarActivos();

        public ChecklistItemTemplate Obtener(int templateId) => _mapperTemplate.Obtener(templateId);

        public int AltaItem(string nombre, string descripcion)
        {
            RequerirPermiso();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BLLException("ERR_TEMPLATE_NOMBRE_VACIO", "El nombre del item del template es obligatorio.");

            var nuevo = new ChecklistItemTemplate
            {
                Nombre = nombre.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                Activo = true
            };
            int id = _mapperTemplate.Insertar(nuevo);
            new VerificadorChecklistItemTemplate().RecalcularDVs();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.GestionTemplateChecklist,
                $"Alta item template: '{nuevo.Nombre}' (Id {id}).");
            return id;
        }

        public void EditarDescripcion(int templateId, string descripcion)
        {
            RequerirPermiso();
            var existente = _mapperTemplate.Obtener(templateId);
            if (existente == null)
                throw new BLLException("ERR_TEMPLATE_NO_ENCONTRADO", "No se encontró el item del template.");

            string nueva = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
            _mapperTemplate.EditarDescripcion(templateId, nueva);
            new VerificadorChecklistItemTemplate().RecalcularDVs();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.GestionTemplateChecklist,
                $"Edición descripción template Id {templateId}.");
        }

        public void BajaLogica(int templateId)
        {
            RequerirPermiso();
            var existente = _mapperTemplate.Obtener(templateId);
            if (existente == null)
                throw new BLLException("ERR_TEMPLATE_NO_ENCONTRADO", "No se encontró el item del template.");

            _mapperTemplate.BajaLogica(templateId);
            new VerificadorChecklistItemTemplate().RecalcularDVs();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.GestionTemplateChecklist,
                $"Baja lógica template '{existente.Nombre}' (Id {templateId}).");
        }

        public void Reactivar(int templateId)
        {
            RequerirPermiso();
            var existente = _mapperTemplate.Obtener(templateId);
            if (existente == null)
                throw new BLLException("ERR_TEMPLATE_NO_ENCONTRADO", "No se encontró el item del template.");

            _mapperTemplate.Reactivar(templateId);
            new VerificadorChecklistItemTemplate().RecalcularDVs();
            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.GestionTemplateChecklist,
                $"Reactivación template '{existente.Nombre}' (Id {templateId}).");
        }
    }
}
