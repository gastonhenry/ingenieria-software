using BE;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class VerificadorPermiso : IVerificadorEntidad
    {
        public const string TablaPermiso = "Permiso";

        private readonly MapperPermiso _mapperPermiso;
        private readonly DigitoVerificadorService _digitoVerificadorService;

        public VerificadorPermiso()
        {
            _mapperPermiso = new MapperPermiso();
            _digitoVerificadorService = new DigitoVerificadorService();
        }

        public string NombreTabla => TablaPermiso;

        public ResultadoIntegridad Verificar()
        {
            var filas = _mapperPermiso.Listar();
            return _digitoVerificadorService.Verificar(TablaPermiso, filas, ObtenerCamposParaDVH);
        }

        public void RecalcularDVs()
        {
            var filas = _mapperPermiso.Listar();
            _digitoVerificadorService.Recalcular(TablaPermiso, filas, ObtenerCamposParaDVH, _mapperPermiso.ActualizarDVH);
        }

        internal static IList<KeyValuePair<string, string>> ObtenerCamposParaDVH(Permiso p)
        {
            string tipo = p is Rol ? "R" : "I";
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Codigo",      p.Codigo      ?? string.Empty),
                new KeyValuePair<string, string>("Descripcion", p.Descripcion ?? string.Empty),
                new KeyValuePair<string, string>("Tipo",        tipo),
            };
        }
    }
}
