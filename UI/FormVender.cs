using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace UI
{
    // Modal para registrar una venta: busca comprador por doc, pide precio final, fecha y descripción.
    public partial class FormVender : Form
    {
        private readonly IPersonaService _personaService;

        public int IdPersonaComprador { get; private set; }
        public DateTime FechaOperacion { get; private set; }
        public decimal PrecioFinal { get; private set; }
        public string Descripcion { get; private set; }

        public FormVender()
        {
            InitializeComponent();
            _personaService = new PersonaService();
            dtpFecha.Value = DateTime.Now;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string frag = txtDocBusqueda.Text.Trim();
            lstCoincidencias.Items.Clear();
            if (string.IsNullOrEmpty(frag)) return;
            List<Persona> rs = _personaService.BuscarPorDocumentoParcial(frag);
            if (rs.Count == 0)
            {
                MessageBox.Show("No se encontraron personas con ese documento.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (Persona p in rs) lstCoincidencias.Items.Add(new PersonaItem(p));
            if (lstCoincidencias.Items.Count > 0) lstCoincidencias.SelectedIndex = 0;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var pi = lstCoincidencias.SelectedItem as PersonaItem;
            if (pi == null)
            {
                MessageBox.Show("Buscá y seleccioná la persona compradora.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal precio)
                || precio <= 0)
            {
                MessageBox.Show("Precio final inválido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private class PersonaItem
        {
            public Persona Persona { get; }
            public PersonaItem(Persona p) { Persona = p; }
            public override string ToString() => $"{Persona.Documento} — {Persona.Nombre} ({(Persona.TipoPersona == 'J' ? "Jurídica" : "Física")})";
        }
    }
}
