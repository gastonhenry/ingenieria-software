using BE;
using BE.Enums;
using HELPERS;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MPP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BLL
{
    // Genera la ficha de la publicación con formato de aviso usando PDFsharp + MigraDoc (librería de terceros).
    public class PublicacionPdfService : IPublicacionPdfService
    {
        private const int MAX_FOTOS = 4;
        private static readonly Color AzulMarca = new Color(30, 90, 200);
        private static readonly CultureInfo CulturaAr = new CultureInfo("es-AR");

        private readonly MapperUnidad _mapperUnidad;
        private readonly MapperPublicacion _mapperPublicacion;
        private readonly ImagenUnidadService _imagenService;
        private readonly BitacoraService _bitacora;

        public PublicacionPdfService()
        {
            _mapperUnidad = new MapperUnidad();
            _mapperPublicacion = new MapperPublicacion();
            _imagenService = new ImagenUnidadService();
            _bitacora = new BitacoraService();
        }

        public void Exportar(int idUnidad, string rutaDestino)
        {
            RequerirAdminOPermiso("VER_PUBLICACIONES", "ExportarPublicacionPdf");
            if (string.IsNullOrWhiteSpace(rutaDestino))
                throw new BLLException("ERR_RUTA_PDF_INVALIDA", "Elegí dónde guardar el archivo.");

            try
            {
                Generar(idUnidad, rutaDestino);
            }
            catch (BLLException) { throw; }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new BLLException("ERR_GUARDAR_PDF", "No se pudo guardar el archivo PDF: {0}", ex.Message);
            }
        }

        public string NombreArchivoSugerido(int idUnidad)
        {
            var unidad = _mapperUnidad.Obtener(idUnidad);
            if (unidad == null) return "Publicacion.pdf";
            // "AA123BX - Jeep Renegade 2017.pdf"
            string nombre = $"{unidad.Dominio} - {unidad.Modelo.Marca.Nombre} {unidad.Modelo.Nombre} {unidad.Anio}.pdf";
            foreach (char c in Path.GetInvalidFileNameChars()) nombre = nombre.Replace(c, '_');
            return nombre;
        }

        // Lo usa NotificacionService para adjuntar la publicación al mail (sin chequeo de permisos:
        // se ejecuta dentro de CU-COM-06). Devuelve null si la unidad no tiene publicación.
        internal string GenerarTemporal(int idUnidad)
        {
            if (_mapperPublicacion.ObtenerPorUnidad(idUnidad) == null) return null;
            string carpeta = Path.Combine(Path.GetTempPath(), "AvantiAuto");
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta, $"{Guid.NewGuid():N}_{NombreArchivoSugerido(idUnidad)}");
            Generar(idUnidad, ruta);
            return ruta;
        }

        private void Generar(int idUnidad, string rutaDestino)
        {
            var unidad = _mapperUnidad.Obtener(idUnidad);
            if (unidad == null)
                throw new BLLException("ERR_UNIDAD_NO_ENCONTRADA", "No se encontró la unidad.");
            var publicacion = _mapperPublicacion.ObtenerPorUnidad(idUnidad);
            // RN-N03-12
            if (publicacion == null)
                throw new BLLException("ERR_UNIDAD_SIN_PUBLICACION", "La unidad no tiene una publicación creada.");

            Document doc = CrearDocumento(unidad, publicacion, idUnidad);
            var renderer = new PdfDocumentRenderer(true) { Document = doc };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(rutaDestino);
        }

        private Document CrearDocumento(Unidad unidad, PublicacionUnidad publicacion, int idUnidad)
        {
            var doc = new Document();
            doc.Info.Title = $"{unidad.Modelo.Marca.Nombre} {unidad.Modelo.Nombre} {unidad.Anio}";
            doc.Info.Author = "Avanti Auto";
            doc.Styles[StyleNames.Normal].Font.Name = "Segoe UI";
            doc.Styles[StyleNames.Normal].Font.Size = 10;

            Section seccion = doc.AddSection();
            seccion.PageSetup.PageFormat = PageFormat.A4;
            seccion.PageSetup.TopMargin = Unit.FromCentimeter(1.5);
            seccion.PageSetup.BottomMargin = Unit.FromCentimeter(1.5);
            seccion.PageSetup.LeftMargin = Unit.FromCentimeter(1.8);
            seccion.PageSetup.RightMargin = Unit.FromCentimeter(1.8);

            // Encabezado
            Paragraph marca = seccion.AddParagraph("AVANTI AUTO");
            marca.Format.Font.Size = 20;
            marca.Format.Font.Bold = true;
            marca.Format.Font.Color = AzulMarca;
            Paragraph sub = seccion.AddParagraph("Concesionaria de vehículos");
            sub.Format.Font.Color = Colors.Gray;
            sub.Format.SpaceAfter = Unit.FromCentimeter(0.4);
            sub.Format.Borders.Bottom.Width = 1;
            sub.Format.Borders.Bottom.Color = AzulMarca;

            // Título y precio
            Paragraph titulo = seccion.AddParagraph($"{unidad.Modelo.Marca.Nombre} {unidad.Modelo.Nombre} {unidad.Anio}");
            titulo.Format.Font.Size = 18;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceBefore = Unit.FromCentimeter(0.5);

            Paragraph precio = seccion.AddParagraph(publicacion.PrecioPublicacion.HasValue
                ? publicacion.PrecioPublicacion.Value.ToString("C0", CulturaAr)
                : "Precio a consultar");
            precio.Format.Font.Size = 16;
            precio.Format.Font.Color = AzulMarca;
            precio.Format.SpaceAfter = Unit.FromCentimeter(0.4);

            // Ficha técnica
            Table ficha = seccion.AddTable();
            ficha.Borders.Width = 0.5;
            ficha.Borders.Color = Colors.LightGray;
            ficha.AddColumn(Unit.FromCentimeter(5));
            ficha.AddColumn(Unit.FromCentimeter(12.4));
            AgregarFila(ficha, "Marca", unidad.Modelo.Marca.Nombre);
            AgregarFila(ficha, "Modelo", unidad.Modelo.Nombre);
            AgregarFila(ficha, "Año", unidad.Anio.ToString());
            AgregarFila(ficha, "Kilometraje", unidad.Kilometraje.ToString("N0", CulturaAr) + " km");
            AgregarFila(ficha, "Carrocería", unidad.Modelo.TipoCarroceria.GetDescripcion());

            // Descripción
            if (!string.IsNullOrWhiteSpace(publicacion.DescripcionPublicacion))
            {
                Paragraph tDesc = seccion.AddParagraph("Descripción");
                tDesc.Format.Font.Bold = true;
                tDesc.Format.Font.Size = 12;
                tDesc.Format.SpaceBefore = Unit.FromCentimeter(0.6);
                tDesc.Format.SpaceAfter = Unit.FromCentimeter(0.2);
                seccion.AddParagraph(publicacion.DescripcionPublicacion);
            }

            // Fotos (hasta 4, en grilla de 2 columnas)
            List<string> fotos = _imagenService.Listar(idUnidad)
                .Select(i => _imagenService.ObtenerRutaCompleta(i))
                .Where(File.Exists)
                .Take(MAX_FOTOS)
                .ToList();
            if (fotos.Count > 0)
            {
                Paragraph tFotos = seccion.AddParagraph("Fotos");
                tFotos.Format.Font.Bold = true;
                tFotos.Format.Font.Size = 12;
                tFotos.Format.SpaceBefore = Unit.FromCentimeter(0.6);
                tFotos.Format.SpaceAfter = Unit.FromCentimeter(0.2);

                Table grilla = seccion.AddTable();
                grilla.AddColumn(Unit.FromCentimeter(8.7));
                grilla.AddColumn(Unit.FromCentimeter(8.7));
                Row fila = null;
                for (int i = 0; i < fotos.Count; i++)
                {
                    if (i % 2 == 0) fila = grilla.AddRow();
                    var img = fila.Cells[i % 2].AddParagraph().AddImage(fotos[i]);
                    img.Width = Unit.FromCentimeter(8.3);
                    img.LockAspectRatio = true;
                    fila.Cells[i % 2].Format.SpaceAfter = Unit.FromCentimeter(0.3);
                }
            }

            // Pie
            Paragraph pie = seccion.Footers.Primary.AddParagraph(
                $"Emitido el {DateTime.Now.ToString("dd/MM/yyyy HH:mm", CulturaAr)} — Precio y disponibilidad sujetos a confirmación.");
            pie.Format.Font.Size = 8;
            pie.Format.Font.Color = Colors.Gray;
            pie.Format.Alignment = ParagraphAlignment.Center;
            return doc;
        }

        private static void AgregarFila(Table tabla, string etiqueta, string valor)
        {
            Row fila = tabla.AddRow();
            fila.TopPadding = Unit.FromPoint(3);
            fila.BottomPadding = Unit.FromPoint(3);
            fila.Cells[0].Shading.Color = new Color(240, 244, 252);
            Paragraph p = fila.Cells[0].AddParagraph(etiqueta);
            p.Format.Font.Bold = true;
            fila.Cells[1].AddParagraph(valor ?? "");
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
