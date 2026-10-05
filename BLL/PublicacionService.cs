using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class PublicacionService : IPublicacionService
    {
        private readonly MapperPublicacion _mapper;
        private readonly BitacoraService _bitacora;

        public PublicacionService()
        {
            _mapper = new MapperPublicacion();
            _bitacora = new BitacoraService();
        }

        public List<PublicacionListado> ListarPublicaciones()
        {
            RequerirAdminOPermiso("VER_PUBLICACIONES", "ListarPublicaciones");
            return _mapper.ListarPublicaciones();
        }

        public PublicacionUnidad ObtenerPorUnidad(int idUnidad) => _mapper.ObtenerPorUnidad(idUnidad);

        public void CrearSiNoExiste(int idUnidad) => _mapper.CrearSiNoExiste(idUnidad);

        public void Actualizar(int idUnidad, decimal? precio, string descripcion)
        {
            RequerirAdminOPermiso("GESTIONAR_PUBLICACION", "ActualizarPublicacion");

            if (precio.HasValue && precio.Value < 0)
                throw new BLLException("ERR_PRECIO_INVALIDO", "El precio no puede ser negativo.");

            CrearSiNoExiste(idUnidad);
            _mapper.Actualizar(idUnidad, precio, descripcion);

            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.TransicionUnidad,
                $"Unidad Id {idUnidad}: publicación actualizada (precio={precio?.ToString("N2") ?? "N/A"}).");
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;
            if (new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;
            _bitacora.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
