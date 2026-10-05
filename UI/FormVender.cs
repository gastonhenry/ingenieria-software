using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace UI
{
    // Modal para registrar una venta: busca comprador por doc, pide precio final, fecha y descripción.
    public partial class FormVender : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormVender";

        private readonly IPersonaService _personaService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public int IdPersonaComprador { get; private set; }
        public DateTime FechaOperacion { get; private set; }
        public decimal PrecioFinal { get; private set; }
        public string Descripcion { get; private set; }

        public FormVender()
        {
            InitializeComponent();
            _personaService = new PersonaService();
            _idiomaService = new IdiomaService();
            dtpFecha.Value = DateTime.Now;

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

        private string TrError(Exception ex) => TraductorErrores.TraducirError(ex, _idiomaService);

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            this.Text             = Tr("title",            "Vender");
            lblTitulo.Text        = Tr("lblTitulo",        "Registrar venta");
            lblDocBusqueda.Text   = Tr("lblDocBusqueda",   "Comprador (DNI/CUIT):");
            btnBuscar.Text        = Tr("btnBuscar",        "Buscar");
            lblCoincidencias.Text = Tr("lblCoincidencias", "Coincidencias:");
            lblFecha.Text         = Tr("lblFecha",         "Fecha:");
            lblPrecio.Text        = Tr("lblPrecio",        "Precio final ($):");
            lblDescripcion.Text   = Tr("lblDescripcion",   "Descripción:");
            btnAceptar.Text       = Tr("btnAceptar",       "Vender");
            btnCancelar.Text      = Tr("btnCancelar",      "Cancelar");
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string frag = txtDocBusqueda.Text.Trim();
                lstCoincidencias.Items.Clear();
                if (string.IsNullOrEmpty(frag)) return;
                List<Persona> rs = _personaService.BuscarPorDocumentoParcial(frag);
                if (rs.Count == 0)
                {
                    MessageBox.Show(Tr("msgSinCoincidencias", "No se encontraron personas con ese documento."),
                        Tr("msgInformacion", "Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (Persona p in rs) lstCoincidencias.Items.Add(new PersonaItem(p, Tr));
                if (lstCoincidencias.Items.Count > 0) lstCoincidencias.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var pi = lstCoincidencias.SelectedItem as PersonaItem;
            if (pi == null)
            {
                MessageBox.Show(Tr("msgSeleccionarComprador", "Buscá y seleccioná la persona compradora."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal precio)
                || precio <= 0)
            {
                MessageBox.Show(Tr("msgPrecioInvalido", "Precio final inválido."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IdPersonaComprador = pi.Persona.Id;
            FechaOperacion     = dtpFecha.Value;
            PrecioFinal        = precio;
            Descripcion        = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim();

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

        private class PersonaItem
        {
            private readonly Func<string, string, string> _tr;
            public Persona Persona { get; }
            public PersonaItem(Persona p, Func<string, string, string> tr) { Persona = p; _tr = tr; }
            public override string ToString() =>
                $"{Persona.Documento} — {Persona.Nombre} ({(Persona.TipoPersona == 'J' ? _tr("tipoJuridica", "Jurídica") : _tr("tipoFisica", "Física"))})";
        }
    }
}
