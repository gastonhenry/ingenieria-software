using BE;
using MPP;

namespace BLL
{
    public class VerificadorUsuario : IVerificadorEntidad
    {
        public const string TablaUsuario = "Usuario";

        private readonly MapperUsuario _mapperUsuario;
        private readonly DigitoVerificadorService _digitoVerificadorService;

        public VerificadorUsuario()
        {
            _mapperUsuario = new MapperUsuario();
            _digitoVerificadorService = new DigitoVerificadorService();
        }

        public string NombreTabla => TablaUsuario;

        public ResultadoIntegridad Verificar()
        {
            var filas = _mapperUsuario.ListarParaVerificacion();
            return _digitoVerificadorService.Verificar(TablaUsuario, filas, UsuarioService.ObtenerCamposParaDVH);
        }

        public void RecalcularDVs()
        {
            var filas = _mapperUsuario.ListarParaVerificacion();
            _digitoVerificadorService.Recalcular(TablaUsuario, filas, UsuarioService.ObtenerCamposParaDVH, _mapperUsuario.ActualizarDVH);
        }
    }
}
