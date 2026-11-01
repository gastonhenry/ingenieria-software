using BE;
using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    // Confirmación de una acción con motivo opcional (ej. suspender avisos, cerrar búsqueda).
    // El título, el mensaje y el texto del botón los pasa quien la abre, ya traducidos.
    public partial class FormConfirmarMotivo : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormConfirmarMotivo";

        private readonly IIdiomaService _idiomaService;
        private readonly string _textoAceptar;
        private readonly string _textoCancelar;
        private bool _suscrito;

        public string Motivo { get; private set; }

        public FormConfirmarMotivo(string titulo, string mensaje, string textoAceptar, string textoCancelar = null)
        {
            InitializeComponent();
            _idiomaService = new IdiomaService();
            _textoAceptar = textoAceptar;
            _textoCancelar = textoCancelar;
            this.Text = titulo;
            lblMensaje.Text = mensaje;

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());
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

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            lblMotivo.Text   = Tr("lblMotivo",   "Motivo (opcional):");
            btnAceptar.Text  = string.IsNullOrEmpty(_textoAceptar) ? Tr("btnAceptar", "Aceptar") : _textoAceptar;
            btnCancelar.Text = string.IsNullOrEmpty(_textoCancelar) ? Tr("btnCancelar", "Cancelar") : _textoCancelar;
            AjustarLayout();
        }

        // El alto del mensaje y el ancho de los botones dependen del texto (y del idioma).
        private void AjustarLayout()
        {
            int margen = lblMensaje.Left;
            int ancho = ClientSize.Width - margen * 2;
            Size medida = TextRenderer.MeasureText(lblMensaje.Text ?? "", lblMensaje.Font,
                new Size(ancho, int.MaxValue), TextFormatFlags.WordBreak);
            lblMensaje.SetBounds(margen, lblMensaje.Top, ancho, medida.Height + 4);

            lblMotivo.Top = lblMensaje.Bottom + 12;
            txtMotivo.SetBounds(margen, lblMotivo.Bottom + 6, ancho, txtMotivo.Height);

            foreach (var b in new[] { btnAceptar, btnCancelar })
                b.Width = Math.Max(b.Width, TextRenderer.MeasureText(b.Text, b.Font).Width + 30);
            int yBotones = txtMotivo.Bottom + 16;
            btnCancelar.Location = new Point(ClientSize.Width - margen - btnCancelar.Width, yBotones);
            btnAceptar.Location = new Point(btnCancelar.Left - 10 - btnAceptar.Width, yBotones);
            ClientSize = new Size(ClientSize.Width, btnCancelar.Bottom + 16);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Motivo = string.IsNullOrWhiteSpace(txtMotivo.Text) ? null : txtMotivo.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
