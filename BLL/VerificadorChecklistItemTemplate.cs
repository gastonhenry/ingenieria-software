using BE;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class VerificadorChecklistItemTemplate : IVerificadorEntidad
    {
        public const string TablaChecklistItemTemplate = "ChecklistItemTemplate";

        private readonly MapperChecklistItemTemplate _mapperTemplate;
        private readonly DigitoVerificadorService _digitoVerificadorService;

        public VerificadorChecklistItemTemplate()
        {
            _mapperTemplate = new MapperChecklistItemTemplate();
            _digitoVerificadorService = new DigitoVerificadorService();
        }

        public string NombreTabla => TablaChecklistItemTemplate;

        public ResultadoIntegridad Verificar()
        {
            var filas = _mapperTemplate.Listar();
            return _digitoVerificadorService.Verificar(TablaChecklistItemTemplate, filas, ObtenerCamposParaDVH);
        }

        public void RecalcularDVs()
        {
            var filas = _mapperTemplate.Listar();
            _digitoVerificadorService.Recalcular(TablaChecklistItemTemplate, filas, ObtenerCamposParaDVH, _mapperTemplate.ActualizarDVH);
        }

        internal static IList<KeyValuePair<string, string>> ObtenerCamposParaDVH(ChecklistItemTemplate t)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Nombre",      t.Nombre      ?? string.Empty),
                new KeyValuePair<string, string>("Descripcion", t.Descripcion ?? string.Empty),
                new KeyValuePair<string, string>("Activo",      t.Activo.ToString()),
            };
        }
    }
}
