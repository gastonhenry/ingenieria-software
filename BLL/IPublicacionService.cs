using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IPublicacionService
    {
        List<PublicacionListado> ListarPublicaciones();
        PublicacionUnidad ObtenerPorUnidad(int idUnidad);
        void CrearSiNoExiste(int idUnidad);
        void Actualizar(int idUnidad, decimal? precio, string descripcion);
    }
}
