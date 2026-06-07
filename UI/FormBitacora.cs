using BE;
using BE.Enums;
using BLL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormBitacora : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormBitacora";

        private readonly IBitacoraService _bitacoraService;
        private readonly IIdiomaService _idiomaService;
        private List<Bitacora> _bitacoras = new List<Bitacora>();
        private int _ultimoTotal;
        private bool _suscrito;

        public FormBitacora()
        {
            InitializeComponent();
            _bitacoraService = new BitacoraService();
            _idiomaService = new IdiomaService();

            var usuarioService = new UsuarioService();
            var permisoService = new PermisoService();

            bool permitido = usuarioService.EsAdmin() || permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "VER_BITACORA");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para acceder a esta sección."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            InicializarGrilla();
            InicializarFiltros();
            CargarDatos();

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
            lblTitulo.Text         = Tr("lblTitulo",         "Bitácora");
            grpFiltros.Text        = Tr("grpFiltros",        "Filtros");
            lblTipo.Text           = Tr("lblTipo",           "Tipo:");
            lblUsuarioFiltro.Text  = Tr("lblUsuarioFiltro",  "Usuario:");
            lblDesde.Text          = Tr("lblDesde",          "Desde:");
            lblHasta.Text          = Tr("lblHasta",          "Hasta:");
            btnLimpiar.Text        = Tr("btnLimpiar",        "Limpiar");
            btnActualizar.Text     = Tr("btnActualizar",     "Actualizar");
            lblTotal.Text          = string.Format(Tr("lblTotal", "Total: {0}"), _ultimoTotal);

            if (cmbTipo.Items.Count > 0)
                cmbTipo.Items[0] = Tr("cmbTipoTodos", "Todos");

            if (dgvBitacora.Columns.Count > 0)
            {
                dgvBitacora.Columns["Id"].HeaderText        = Tr("colId",        "ID");
                dgvBitacora.Columns["Usuario"].HeaderText   = Tr("colUsuario",   "Usuario");
                dgvBitacora.Columns["Tipo"].HeaderText      = Tr("colTipo",      "Tipo");
                dgvBitacora.Columns["FechaHora"].HeaderText = Tr("colFechaHora", "Fecha y Hora");
                dgvBitacora.Columns["Detalle"].HeaderText   = Tr("colDetalle",   "Detalle");

                foreach (DataGridViewRow row in dgvBitacora.Rows)
                {
                    if (row.Cells["Tipo"].Tag is TipoBitacora tipo)
                        row.Cells["Tipo"].Value = tipo.GetDescripcionTraducida();
                }
            }

            cmbTipo.Refresh();
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
                if (e.Value is TipoBitacora t)
                    e.Value = t.GetDescripcionTraducida();
            };

            cmbTipo.Items.Clear();
            cmbTipo.Items.Add(Tr("cmbTipoTodos", "Todos"));
            foreach (TipoBitacora valor in Enum.GetValues(typeof(TipoBitacora)))
                cmbTipo.Items.Add(valor);
            cmbTipo.SelectedIndex = 0;

            EstablecerRangoFechasPorDefecto();
            SincronizarRangoFechas();

            cmbTipo.SelectedIndexChanged    += (s, e) => AplicarFiltros();
            txtUsuarioFiltro.TextChanged    += (s, e) => AplicarFiltros();
            dtpDesde.ValueChanged           += (s, e) => { SincronizarRangoFechas(); AplicarFiltros(); };
            dtpHasta.ValueChanged           += (s, e) => { SincronizarRangoFechas(); AplicarFiltros(); };
        }

        private void EstablecerRangoFechasPorDefecto()
        {
            dtpDesde.Checked = false;
            dtpHasta.Checked = false;
            dtpDesde.Value = DateTime.Today.AddYears(-1);
            dtpHasta.Value = DateTime.Today;
            dtpDesde.Checked = true;
            dtpHasta.Checked = true;
        }

        private void SincronizarRangoFechas()
        {
            dtpHasta.MinDate = dtpDesde.Checked
                ? dtpDesde.Value.Date
                : DateTimePicker.MinimumDateTime;

            dtpDesde.MaxDate = dtpHasta.Checked
                ? dtpHasta.Value.Date
                : DateTimePicker.MaximumDateTime;
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
                MessageBox.Show(Tr("msgErrorCargar", "Error al cargar la bitácora:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros()
        {
            IEnumerable<Bitacora> resultado = _bitacoras;

            if (cmbTipo.SelectedItem is TipoBitacora tipoFiltro)
                resultado = resultado.Where(b => b.Tipo == tipoFiltro);

            string usuarioFiltro = txtUsuarioFiltro.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(usuarioFiltro))
                resultado = resultado.Where(b => b.Usuario.Username.ToLower().Contains(usuarioFiltro));

            if (dtpDesde.Checked)
                resultado = resultado.Where(b => b.FechaHora.Date >= dtpDesde.Value.Date);

            if (dtpHasta.Checked)
                resultado = resultado.Where(b => b.FechaHora.Date <= dtpHasta.Value.Date);

            var lista = resultado.ToList();
            RefrescarGrilla(lista);
            ActualizarResumen(lista);
        }

        private void RefrescarGrilla(List<Bitacora> lista)
        {
            dgvBitacora.Rows.Clear();
            dgvBitacora.Columns.Clear();

            dgvBitacora.Columns.Add("Id",        Tr("colId",        "ID"));
            dgvBitacora.Columns.Add("Usuario",   Tr("colUsuario",   "Usuario"));
            dgvBitacora.Columns.Add("Tipo",      Tr("colTipo",      "Tipo"));
            dgvBitacora.Columns.Add("FechaHora", Tr("colFechaHora", "Fecha y Hora"));
            dgvBitacora.Columns.Add("Detalle",   Tr("colDetalle",   "Detalle"));

            dgvBitacora.Columns["Id"].Visible      = false;
            dgvBitacora.Columns["Usuario"].Width   = 240;
            dgvBitacora.Columns["Tipo"].Width      = 180;
            dgvBitacora.Columns["FechaHora"].Width = 145;
            dgvBitacora.Columns["Detalle"].AutoSizeMode    = DataGridViewAutoSizeColumnMode.Fill;
            dgvBitacora.Columns["Detalle"].MinimumWidth    = 200;

            foreach (Bitacora item in lista)
            {
                int idx = dgvBitacora.Rows.Add(
                    item.Id,
                    item.Usuario.Username,
                    item.Tipo.GetDescripcionTraducida(),
                    item.FechaHora.ToString("dd/MM/yyyy HH:mm:ss"),
                    item.Detalle
                );
                dgvBitacora.Rows[idx].Cells["Tipo"].Tag = item.Tipo;
            }
        }

        private void ActualizarResumen(List<Bitacora> lista)
        {
            _ultimoTotal = lista.Count;
            lblTotal.Text = string.Format(Tr("lblTotal", "Total: {0}"), _ultimoTotal);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbTipo.SelectedIndex = 0;
            txtUsuarioFiltro.Text = string.Empty;
            EstablecerRangoFechasPorDefecto();
            AplicarFiltros();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito)
            {
                try { _idiomaService.Desuscribir(this); } catch { }
                _suscrito = false;
            }
            base.OnFormClosed(e);
        }
    }
}
