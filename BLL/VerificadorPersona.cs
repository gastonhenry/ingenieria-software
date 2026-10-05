using BE;
using MPP;
using System.Collections.Generic;
using System.Globalization;

namespace BLL
{
    public class VerificadorPersona : IVerificadorEntidad
    {
        public const string TablaPersona = "Persona";

        private readonly MapperPersona _mapperPersona;
        private readonly DigitoVerificadorService _digitoVerificadorService;

        public VerificadorPersona()
        {
            _mapperPersona = new MapperPersona();
            _digitoVerificadorService = new DigitoVerificadorService();
        }

        public string NombreTabla => TablaPersona;

        public ResultadoIntegridad Verificar()
        {
            var filas = _mapperPersona.ListarParaVerificacion();
            return _digitoVerificadorService.Verificar(TablaPersona, filas, ObtenerCamposParaDVH);
        }

        public void RecalcularDVs()
        {
            var filas = _mapperPersona.ListarParaVerificacion();
            _digitoVerificadorService.Recalcular(TablaPersona, filas, ObtenerCamposParaDVH, _mapperPersona.ActualizarDVH);
        }

        internal static IList<KeyValuePair<string, string>> ObtenerCamposParaDVH(Persona p)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("TipoPersona",     p.TipoPersona.ToString()),
                new KeyValuePair<string, string>("Documento",       p.Documento       ?? string.Empty),
                new KeyValuePair<string, string>("Nombre",          p.Nombre          ?? string.Empty),
                new KeyValuePair<string, string>("Domicilio",       p.Domicilio       ?? string.Empty),
                new KeyValuePair<string, string>("Telefono",        p.Telefono        ?? string.Empty),
                new KeyValuePair<string, string>("Email",           p.Email           ?? string.Empty),
                new KeyValuePair<string, string>("EstadoCivil",     ((int)p.EstadoCivil).ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("FechaNacimiento", p.FechaNacimiento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            };
        }
    }
}
