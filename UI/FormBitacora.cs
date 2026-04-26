using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormBitacora : Form
    {
        private readonly IBitacoraService _bitacoraService;
        private List<Bitacora> _bitacoras = new List<Bitacora>();

        public FormBitacora()
        {
            InitializeComponent();
            _bitacoraService = new BitacoraService();

            if (!new UsuarioService().EsAdmin())
            {
                MessageBox.Show("Solo el administrador puede acceder a esta sección.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            InicializarGrilla();
            InicializarFiltros();
            CargarDatos();
        }

        private void InicializarGrilla()
        {
            dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvBitacora.EnableHeadersVisualStyles = false;
            dgvBitacora.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 90, 200);
            dgvBitacora.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBitacora.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgvBitacora.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
            dgvBitacora.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvBitacora.GridColor = Color.FromArgb(210, 220, 240);
        }

        private void InicializarFiltros()
        {
            cmbTipo.FormattingEnabled = true;
            cmbTipo.Format += (s, e) =>
            {
                if (e.Value is BitacoraEnum t)
                    e.Value = t.GetDescripcion();
            };

            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("Todos");
            foreach (BitacoraEnum valor in Enum.GetValues(typeof(BitacoraEnum)))
                cmbTipo.Items.Add(valor);
            cmbTipo.SelectedIndex = 0;
            cmbTipo.SelectedIndexChanged    += (s, e) => AplicarFiltros();
            txtUsuarioFiltro.TextChanged    += (s, e) => AplicarFiltros();
        }

        public void CargarDatos()
        {
            try
            {
                _bitacoras = _bitacoraService.Listar();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la bitácora:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros()
        {
            IEnumerable<Bitacora> resultado = _bitacoras;

            if (cmbTipo.SelectedItem is BitacoraEnum tipoFiltro)
                resultado = resultado.Where(b => b.Tipo == tipoFiltro);

            string usuarioFiltro = txtUsuarioFiltro.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(usuarioFiltro))
                resultado = resultado.Where(b => b.Usuario.Username.ToLower().Contains(usuarioFiltro));

            var lista = resultado.ToList();
            RefrescarGrilla(lista);
            ActualizarResumen(lista);
        }

        private void RefrescarGrilla(List<Bitacora> lista)
        {
            dgvBitacora.Rows.Clear();
            dgvBitacora.Columns.Clear();

            dgvBitacora.Columns.Add("Id",          "ID");
            dgvBitacora.Columns.Add("Usuario",     "Usuario");
            dgvBitacora.Columns.Add("Tipo",        "Tipo");
            dgvBitacora.Columns.Add("FechaHora",   "Fecha y Hora");
            dgvBitacora.Columns.Add("Detalle",     "Detalle");

            dgvBitacora.Columns["Id"].Visible      = false;
            dgvBitacora.Columns["Usuario"].Width   = 240;
            dgvBitacora.Columns["Tipo"].Width      = 180;
            dgvBitacora.Columns["FechaHora"].Width = 145;
            dgvBitacora.Columns["Detalle"].AutoSizeMode    = DataGridViewAutoSizeColumnMode.Fill;
            dgvBitacora.Columns["Detalle"].MinimumWidth    = 200;

            foreach (Bitacora item in lista)
            {
                dgvBitacora.Rows.Add(
                    item.Id,
                    item.Usuario.Username,
                    item.Tipo.GetDescripcion(),
                    item.FechaHora.ToString("dd/MM/yyyy HH:mm:ss"),
                    item.Detalle
                );
            }
        }

        private void ActualizarResumen(List<Bitacora> lista)
        {
            lblTotal.Text = $"Total: {lista.Count}";
        }

private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbTipo.SelectedIndex = 0;
            txtUsuarioFiltro.Text = string.Empty;
            AplicarFiltros();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}
