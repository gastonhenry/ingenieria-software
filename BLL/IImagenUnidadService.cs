using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IImagenUnidadService
    {
        List<ImagenUnidad> Listar(int idUnidad);
        string ObtenerRutaCompleta(ImagenUnidad imagen);
        int Agregar(int idUnidad, string rutaArchivoOrigen);
        void Eliminar(int idImagen);
    }
}
