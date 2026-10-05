using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    // Modal simple para capturar el motivo de pausa.
    public partial class FormPausar : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormPausar";

        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public string Motivo { get; private set; }

        public FormPausar()
        {
            InitializeComponent();
            _idiomaService = new IdiomaService();
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
            this.Text        = Tr("title",       "Pausar unidad");
            lblTitulo.Text   = Tr("lblTitulo",   "Pausar unidad");
            lblMotivo.Text   = Tr("lblMotivo",   "Motivo (obligatorio):");
            btnAceptar.Text  = Tr("btnAceptar",  "Pausar");
            btnCancelar.Text = Tr("btnCancelar", "Cancelar");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string m = txtMotivo.Text.Trim();
            if (string.IsNullOrEmpty(m))
            {
                MessageBox.Show(Tr("msgMotivoObligatorio", "El motivo de la pausa es obligatorio."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Motivo = m;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
