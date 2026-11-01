namespace BLL
{
    // N03 — CU-CAP-04: exportar la publicación de una unidad a PDF (A02 — Informe y exportación a PDF).
    public interface IPublicacionPdfService
    {
        // Genera el PDF de la publicación y lo guarda en rutaDestino.
        void Exportar(int idUnidad, string rutaDestino);

        // Nombre de archivo sugerido: "AA123BX - Marca Modelo Año.pdf".
        string NombreArchivoSugerido(int idUnidad);
    }
}
