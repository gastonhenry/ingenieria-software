using BLL;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace UI
{
    // N03 — CU-CAP-04: pide la ubicación, genera el PDF de la publicación y lo abre.
    // Se usa desde Publicaciones, el tablero de captación y la entrevista.
    internal static class ExportadorPdfPublicacion
    {
        private const string CODIGO_FORM = "ExportarPdf";

        public static void Exportar(IWin32Window owner, int idUnidad)
        {
            var idioma = new IdiomaService();
            Func<string, string, string> tr = (codigo, fallback) =>
            {
                try
                {
                    string t = idioma.Traducir(CODIGO_FORM, codigo);
                    return string.IsNullOrEmpty(t) ? fallback : t;
                }
                catch { return fallback; }
            };

            try
            {
                var pdfService = new PublicacionPdfService();
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Title = tr("title", "Exportar publicación a PDF");
                    dlg.Filter = tr("filtro", "Documento PDF") + " (*.pdf)|*.pdf";
                    dlg.FileName = pdfService.NombreArchivoSugerido(idUnidad);
                    dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (dlg.ShowDialog(owner) != DialogResult.OK) return;

                    Cursor.Current = Cursors.WaitCursor;
                    pdfService.Exportar(idUnidad, dlg.FileName);
                    Cursor.Current = Cursors.Default;

                    try { Process.Start(dlg.FileName); }
                    catch
                    {
                        MessageBox.Show(owner, string.Format(tr("msgGuardado", "Se guardó el PDF en:\n{0}"), dlg.FileName),
                            tr("msgInformacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(owner, TraductorErrores.TraducirError(ex, idioma), tr("msgError", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
