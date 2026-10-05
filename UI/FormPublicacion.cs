using BE;
using BLL;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace UI
{
    // Modal para que el Gerente/Vendedor edite la publicación de una unidad (precio, descripción, imágenes).
    public partial class FormPublicacion : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormPublicacion";

        private readonly int _idUnidad;
        private readonly IPublicacionService _pubService;
        private readonly IImagenUnidadService _imgService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormPublicacion(int idUnidad)
        {
            InitializeComponent();
            _idUnidad = idUnidad;
            _pubService = new PublicacionService();
            _imgService = new ImagenUnidadService();
            _idiomaService = new IdiomaService();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => Cargar();
        }

        private string Tr(string codigo, string fallback)
        {
            try
            {
                string t = _idiomaService?.Traducir(CODIGO_FORM, codigo);
                return string.IsNullOrEmpty(t) ? fallback : t;
            }
            catch { return fallback; }
        }

        private string TrError(Exception ex) => TraductorErrores.TraducirError(ex, _idiomaService);

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            this.Text             = Tr("title",            "Publicación");
            lblTitulo.Text        = Tr("lblTitulo",        "Publicación de la unidad");
            lblPrecio.Text        = Tr("lblPrecio",        "Precio ($):");
            lblDescripcion.Text   = Tr("lblDescripcion",   "Descripción:");
            btnGuardar.Text       = Tr("btnGuardar",       "Guardar");
            lblImagenes.Text      = Tr("lblImagenes",      "Imágenes:");
            btnAgregarImagen.Text = Tr("btnAgregarImagen", "Agregar imagen");
            btnQuitarImagen.Text  = Tr("btnQuitarImagen",  "Quitar imagen");
        }

        private void Cargar()
        {
            try
            {
                _pubService.CrearSiNoExiste(_idUnidad);
                var pub = _pubService.ObtenerPorUnidad(_idUnidad);
                if (pub != null)
                {
                    txtPrecio.Text = pub.PrecioPublicacion?.ToString("F2", CultureInfo.CurrentCulture) ?? "";
                    txtDescripcion.Text = pub.DescripcionPublicacion ?? "";
                }
                RecargarImagenes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RecargarImagenes()
        {
            lstImagenes.Items.Clear();
            foreach (ImagenUnidad img in _imgService.Listar(_idUnidad))
                lstImagenes.Items.Add(new ImagenItem(img));
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                decimal? precio = null;
                string txt = txtPrecio.Text.Trim();
                if (!string.IsNullOrEmpty(txt))
                {
                    if (!decimal.TryParse(txt, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal p) || p < 0)
                    {
                        MessageBox.Show(Tr("msgPrecioInvalido", "Precio inválido."), Tr("msgAdvertencia", "Advertencia"),
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    precio = p;
                }

                _pubService.Actualizar(_idUnidad, precio,
                    string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text);

                MessageBox.Show(Tr("msgPublicacionGuardada", "Publicación guardada."), Tr("msgExito", "Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = Tr("filtroImagenes", "Imágenes") + "|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dlg.Multiselect = false;
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    _imgService.Agregar(_idUnidad, dlg.FileName);
                    RecargarImagenes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnQuitarImagen_Click(object sender, EventArgs e)
        {
            var ii = lstImagenes.SelectedItem as ImagenItem;
            if (ii == null) return;
            try
            {
                _imgService.Eliminar(ii.Imagen.Id);
                RecargarImagenes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }

        private class ImagenItem
        {
            public ImagenUnidad Imagen { get; }
            public ImagenItem(ImagenUnidad i) { Imagen = i; }
            public override string ToString() => $"[{Imagen.Orden}] {Path.GetFileName(Imagen.RutaArchivo)}";
        }
    }
}
