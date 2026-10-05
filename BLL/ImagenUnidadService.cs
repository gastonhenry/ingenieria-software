using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;
using System.IO;

namespace BLL
{
    public class ImagenUnidadService : IImagenUnidadService
    {
        // Carpeta raíz de las imágenes físicas: %ProgramData%\Avanti Auto\Imagenes\{IdUnidad}\.
        // ProgramData es escribible por usuarios comunes (Program Files no) y no depende de la unidad C:.
        public static readonly string CarpetaRaiz = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Avanti Auto", "Imagenes");

        private readonly MapperImagenUnidad _mapper;
        private readonly BitacoraService _bitacora;

        public ImagenUnidadService()
        {
            _mapper = new MapperImagenUnidad();
            _bitacora = new BitacoraService();
        }

        public List<ImagenUnidad> Listar(int idUnidad) => _mapper.ListarPorUnidad(idUnidad);

        // En la base se guarda la ruta relativa a CarpetaRaiz ("{IdUnidad}\archivo.ext").
        // Si la fila tiene una ruta absoluta (datos viejos), Path.Combine la devuelve tal cual.
        public string ObtenerRutaCompleta(ImagenUnidad imagen)
        {
            if (imagen == null || string.IsNullOrWhiteSpace(imagen.RutaArchivo)) return null;
            return Path.Combine(CarpetaRaiz, imagen.RutaArchivo);
        }

        public int Agregar(int idUnidad, string rutaArchivoOrigen)
        {
            RequerirAdminOPermiso("GESTIONAR_PUBLICACION", "AgregarImagen");

            if (string.IsNullOrWhiteSpace(rutaArchivoOrigen) || !File.Exists(rutaArchivoOrigen))
                throw new BLLException("ERR_IMAGEN_ARCHIVO_NO_EXISTE",
                    "El archivo '{0}' no existe.", rutaArchivoOrigen);

            string carpetaUnidad = Path.Combine(CarpetaRaiz, idUnidad.ToString());
            Directory.CreateDirectory(carpetaUnidad);

            // Nombre destino: timestamp + extensión original para evitar colisiones.
            string ext = Path.GetExtension(rutaArchivoOrigen);
            string nombreDestino = $"{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
            string rutaRelativa = Path.Combine(idUnidad.ToString(), nombreDestino);

            File.Copy(rutaArchivoOrigen, Path.Combine(CarpetaRaiz, rutaRelativa), overwrite: false);

            int siguienteOrden = _mapper.ListarPorUnidad(idUnidad).Count;
            int id = _mapper.Agregar(idUnidad, rutaRelativa, siguienteOrden);

            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.TransicionUnidad,
                $"Unidad Id {idUnidad}: imagen agregada ({nombreDestino}).");
            return id;
        }

        public void Eliminar(int idImagen)
        {
            RequerirAdminOPermiso("GESTIONAR_PUBLICACION", "EliminarImagen");

            ImagenUnidad imagen = _mapper.ObtenerPorId(idImagen);
            _mapper.Eliminar(idImagen);

            // El archivo se borra después de la fila: si falla el disco, la base ya quedó consistente
            // y sólo queda un archivo huérfano (se deja registrado en bitácora).
            string rutaCompleta = ObtenerRutaCompleta(imagen);
            if (rutaCompleta != null && File.Exists(rutaCompleta))
            {
                try { File.Delete(rutaCompleta); }
                catch (Exception ex)
                {
                    _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Error,
                        $"Imagen Id {idImagen}: no se pudo borrar el archivo '{rutaCompleta}': {ex.Message}");
                }
            }

            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.TransicionUnidad,
                $"Imagen Id {idImagen} eliminada.");
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
