using BE;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class VerificadorUnidad : IVerificadorEntidad
    {
        public const string TablaUnidad = "Unidad";

        private readonly MapperUnidad _mapperUnidad;
        private readonly DigitoVerificadorService _digitoVerificadorService;

        public VerificadorUnidad()
        {
            _mapperUnidad = new MapperUnidad();
            _digitoVerificadorService = new DigitoVerificadorService();
        }

        public string NombreTabla => TablaUnidad;

        public ResultadoIntegridad Verificar()
        {
            var filas = _mapperUnidad.ListarParaVerificacion();
            return _digitoVerificadorService.Verificar(TablaUnidad, filas, ObtenerCamposParaDVH);
        }

        public void RecalcularDVs()
        {
            var filas = _mapperUnidad.ListarParaVerificacion();
            _digitoVerificadorService.Recalcular(TablaUnidad, filas, ObtenerCamposParaDVH, _mapperUnidad.ActualizarDVH);
        }

        internal static IList<KeyValuePair<string, string>> ObtenerCamposParaDVH(Unidad u)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Dominio",            u.Dominio     ?? string.Empty),
                new KeyValuePair<string, string>("Marca",              u.Marca       ?? string.Empty),
                new KeyValuePair<string, string>("Modelo",             u.Modelo      ?? string.Empty),
                new KeyValuePair<string, string>("Anio",               u.Anio.ToString()),
                new KeyValuePair<string, string>("Kilometraje",        u.Kilometraje.ToString()),
                new KeyValuePair<string, string>("PrecioCompra",       u.PrecioCompra.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("Descripcion",        u.Descripcion ?? string.Empty),
                new KeyValuePair<string, string>("EstadoActual",       ((int)u.EstadoActual).ToString()),
                new KeyValuePair<string, string>("IdPersona",          u.IdPersona.ToString()),
                new KeyValuePair<string, string>("IdCompradorUsuario", u.IdCompradorUsuario.ToString()),
                new KeyValuePair<string, string>("FechaIngreso",       u.FechaIngreso.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
            };
        }
    }
}
