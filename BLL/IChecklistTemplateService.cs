using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IChecklistTemplateService
    {
        List<ChecklistItemTemplate> Listar();
        List<ChecklistItemTemplate> ListarActivos();
        ChecklistItemTemplate Obtener(int templateId);
        int AltaItem(string nombre, string descripcion);
        void EditarDescripcion(int templateId, string descripcion);
        void BajaLogica(int templateId);
        void Reactivar(int templateId);
    }
}
