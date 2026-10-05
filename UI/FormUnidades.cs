using BE;
using BE.Enums;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace UI
{
    public partial class FormUnidades : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormUnidades";
        private const string CODIGO_FORM_ESTADO = "EstadoUnidad";

        private readonly IUnidadService _unidadService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        // Dataset base (todas las unidades), para calcular opciones de combos y filtrar en memoria.
        private List<Unidad> _todas = new List<Unidad>();
        private List<Unidad> _datos = new List<Unidad>();
        private string _sortProp = null;
        private bool _sortAsc = true;
        private bool _cargandoFiltros;

        public FormUnidades()
        {
            InitializeComponent();
            _unidadService = new UnidadService();
            _idiomaService = new IdiomaService();

            // Cada columna se ajusta a su contenido (header incluido); la última ocupa el espacio sobrante.
            dgvUnidades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            colIngreso.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colIngreso.MinimumWidth = 120;
            dgvUnidades.BackgroundColor = Color.White;
            dgvUnidades.AllowUserToResizeColumns = false;
            dgvUnidades.AllowUserToResizeRows = false;

            txtBuscarDominio.KeyDown += txtBuscarDominio_KeyDown;

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => { AcomodarLayout(); Recargar(); };
            this.Resize += (s, e) => AcomodarLayout();
        }

        // La grilla ocupa todo el alto disponible y deja abajo una franja fija para "Total" y "Ver detalle",
        // así se ven en cualquier resolución/escalado (monitor grande o notebook).
        private void AcomodarLayout()
        {
            const int margen = 12;
            int altoPie = Math.Max(btnVerDetalle.Height, lblTotal.Height) + margen * 2;

            dgvUnidades.Width  = Math.Max(200, ClientSize.Width - dgvUnidades.Left - margen * 2);
            dgvUnidades.Height = Math.Max(100, ClientSize.Height - altoPie - dgvUnidades.Top);

            btnVerDetalle.Top  = dgvUnidades.Bottom + margen;
            btnVerDetalle.Left = dgvUnidades.Right - btnVerDetalle.Width;
            lblTotal.Left      = dgvUnidades.Left;
            lblTotal.Top       = btnVerDetalle.Top + (btnVerDetalle.Height - lblTotal.Height) / 2;
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

        private string TrEstado(EstadoUnidad e)
        {
            try
            {
                string t = _idiomaService?.Traducir(CODIGO_FORM_ESTADO, e.ToString());
                if (!string.IsNullOrEmpty(t)) return t;
            }
            catch { }
            var field = typeof(EstadoUnidad).GetField(e.ToString());
            var attrs = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attrs != null && attrs.Length > 0)
                ? ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description
                : e.ToString();
        }

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            this.Text                    = Tr("title",            "Unidades");
            lblTitulo.Text               = Tr("lblTitulo",        "Unidades");
            lblBuscarDominio.Text        = Tr("lblBuscarDominio", "Dominio:");
            lblFiltroMarca.Text          = Tr("lblFiltroMarca",   "Marca:");
            lblFiltroModelo.Text         = Tr("lblFiltroModelo",  "Modelo:");
            lblFiltroAnio.Text           = Tr("lblFiltroAnio",    "Año:");
            lblFiltroEstado.Text         = Tr("lblFiltroEstado",  "Estados:");
            btnBuscar.Text               = Tr("btnBuscar",        "Buscar");
            btnActualizar.Text           = Tr("btnActualizar",    "Actualizar");
            btnVerDetalle.Text           = Tr("btnVerDetalle",    "Ver detalle");
            btnAyudaEstados.Text         = Tr("btnAyudaEstados",  "Flujo de estados");
            btnMarcarTodos.Text          = Tr("btnMarcarTodos",   "Marcar todos");
            btnDesmarcarTodos.Text       = Tr("btnDesmarcarTodos","Desmarcar todos");

            colDominio.HeaderText = Tr("colDominio", "Dominio");
            colMarca.HeaderText   = Tr("colMarca",   "Marca");
            colModelo.HeaderText  = Tr("colModelo",  "Modelo");
            colAnio.HeaderText    = Tr("colAnio",    "Año");
            colKm.HeaderText      = Tr("colKm",      "Km");
            colEstado.HeaderText  = Tr("colEstado",  "Estado");
            colIngreso.HeaderText = Tr("colIngreso", "Ingreso");

            // Re-poblar los combos/checklist para que las etiquetas se re-traduzcan.
            if (_todas.Count > 0) PoblarFiltros(preservarSeleccion: true);
            dgvUnidades.Refresh();
            RefrescarLblTotal();
        }

        // ------------------------------------------------------------
        // Poblar los filtros a partir del dataset base
        // ------------------------------------------------------------

        private void PoblarFiltros(bool preservarSeleccion)
        {
            _cargandoFiltros = true;
            try
            {
                string marcaSel  = preservarSeleccion ? ValorCombo(cmbMarca)  : null;
                string modeloSel = preservarSeleccion ? ValorCombo(cmbModelo) : null;
                string anioSel   = preservarSeleccion ? ValorCombo(cmbAnio)   : null;
                var estadosSel   = preservarSeleccion ? EstadosTildados()     : null;

                string todos = Tr("itemTodos", "(Todos)");

                // Marcas: todas las de _todas, ordenadas alfabéticamente.
                var marcas = _todas.Select(u => u.Marca).Where(m => !string.IsNullOrEmpty(m))
                                   .Distinct().OrderBy(m => m).ToList();
                cmbMarca.Items.Clear();
                cmbMarca.Items.Add(todos);
                foreach (var m in marcas) cmbMarca.Items.Add(m);
                cmbMarca.SelectedIndex = Math.Max(0, cmbMarca.Items.IndexOf(marcaSel ?? todos));

                // Modelos: dependen de la marca seleccionada.
                RepoblarModelos(modeloSel);

                // Años: dependen de la marca y el modelo seleccionados.
                RepoblarAnios(anioSel);

                // Estados: todos los del enum, todos tildados por default.
                chkEstados.Items.Clear();
                foreach (EstadoUnidad e in Enum.GetValues(typeof(EstadoUnidad)))
                {
                    bool tildado = estadosSel == null || estadosSel.Contains(e);
                    chkEstados.Items.Add(new EstadoItem(e, TrEstado(e)), tildado);
                }
            }
            finally { _cargandoFiltros = false; }
        }

        private void RepoblarModelos(string modeloPrev)
        {
            // Se restaura el valor previo: si lo llama PoblarFiltros, el flag tiene que seguir en true.
            bool previo = _cargandoFiltros;
            _cargandoFiltros = true;
            try
            {
                string todos = Tr("itemTodos", "(Todos)");
                string marcaSel = ValorCombo(cmbMarca);
                var query = _todas.AsEnumerable();
                if (!string.IsNullOrEmpty(marcaSel) && marcaSel != todos)
                    query = query.Where(u => u.Marca == marcaSel);
                var modelos = query.Select(u => u.Modelo).Where(m => !string.IsNullOrEmpty(m))
                                   .Distinct().OrderBy(m => m).ToList();
                cmbModelo.Items.Clear();
                cmbModelo.Items.Add(todos);
                foreach (var m in modelos) cmbModelo.Items.Add(m);
                int idx = cmbModelo.Items.IndexOf(modeloPrev ?? todos);
                cmbModelo.SelectedIndex = idx >= 0 ? idx : 0;
            }
            finally { _cargandoFiltros = previo; }
        }

        // Conserva el año previo si sigue existiendo para la marca/modelo elegidos; si no, vuelve a "(Todos)".
        private void RepoblarAnios(string anioPrev)
        {
            // Se restaura el valor previo: si lo llama PoblarFiltros, el flag tiene que seguir en true.
            bool previo = _cargandoFiltros;
            _cargandoFiltros = true;
            try
            {
                string todos = Tr("itemTodos", "(Todos)");
                string marcaSel  = ValorCombo(cmbMarca);
                string modeloSel = ValorCombo(cmbModelo);
                var query = _todas.AsEnumerable();
                if (!string.IsNullOrEmpty(marcaSel) && marcaSel != todos)
                    query = query.Where(u => u.Marca == marcaSel);
                if (!string.IsNullOrEmpty(modeloSel) && modeloSel != todos)
                    query = query.Where(u => u.Modelo == modeloSel);
                var anios = query.Select(u => u.Anio).Distinct().OrderByDescending(a => a).ToList();
                cmbAnio.Items.Clear();
                cmbAnio.Items.Add(todos);
                foreach (var a in anios) cmbAnio.Items.Add(a.ToString());
                int idx = cmbAnio.Items.IndexOf(anioPrev ?? todos);
                cmbAnio.SelectedIndex = idx >= 0 ? idx : 0;
            }
            finally { _cargandoFiltros = previo; }
        }

        private static string ValorCombo(ComboBox cmb) =>
            cmb.SelectedItem?.ToString();

        private HashSet<EstadoUnidad> EstadosTildados()
        {
            var set = new HashSet<EstadoUnidad>();
            foreach (var it in chkEstados.CheckedItems)
                if (it is EstadoItem ei) set.Add(ei.Estado);
            return set;
        }

        // ------------------------------------------------------------
        // Carga y filtrado
        // ------------------------------------------------------------

        private void Recargar()
        {
            try
            {
                _todas = _unidadService.Listar() ?? new List<Unidad>();
                PoblarFiltros(preservarSeleccion: _todas.Count > 0 && chkEstados.Items.Count > 0);
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltros()
        {
            string todos = Tr("itemTodos", "(Todos)");

            IEnumerable<Unidad> q = _todas;

            string dom = txtBuscarDominio.Text?.Trim().ToUpper();
            if (!string.IsNullOrEmpty(dom))
                q = q.Where(u => u.Dominio != null && u.Dominio.ToUpper().Contains(dom));

            string marca = ValorCombo(cmbMarca);
            if (!string.IsNullOrEmpty(marca) && marca != todos)
                q = q.Where(u => u.Marca == marca);

            string modelo = ValorCombo(cmbModelo);
            if (!string.IsNullOrEmpty(modelo) && modelo != todos)
                q = q.Where(u => u.Modelo == modelo);

            string anio = ValorCombo(cmbAnio);
            if (!string.IsNullOrEmpty(anio) && anio != todos && int.TryParse(anio, out int a))
                q = q.Where(u => u.Anio == a);

            var estados = EstadosTildados();
            if (estados.Count > 0 && estados.Count < Enum.GetValues(typeof(EstadoUnidad)).Length)
                q = q.Where(u => estados.Contains(u.EstadoActual));
            else if (estados.Count == 0)
                q = Enumerable.Empty<Unidad>(); // ningún estado tildado → ninguna fila

            _datos = q.ToList();
            if (!string.IsNullOrEmpty(_sortProp)) OrdenarDatos();
            RebindDatos();
        }

        private void RebindDatos()
        {
            dgvUnidades.AutoGenerateColumns = false;
            dgvUnidades.DataSource = null;
            dgvUnidades.DataSource = _datos;
            lblTotal.Text = string.Format(Tr("lblTotal", "Total: {0}"), _datos.Count);
            MostrarIndicadorOrden();
        }

        private void OrdenarDatos()
        {
            if (string.IsNullOrEmpty(_sortProp)) return;
            PropertyInfo prop = typeof(Unidad).GetProperty(_sortProp);
            if (prop == null) return;
            _datos = _sortAsc
                ? _datos.OrderBy(u => prop.GetValue(u, null)).ToList()
                : _datos.OrderByDescending(u => prop.GetValue(u, null)).ToList();
        }

        private void MostrarIndicadorOrden()
        {
            foreach (DataGridViewColumn col in dgvUnidades.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            if (string.IsNullOrEmpty(_sortProp)) return;
            foreach (DataGridViewColumn col in dgvUnidades.Columns)
            {
                if (col.DataPropertyName == _sortProp)
                {
                    col.HeaderCell.SortGlyphDirection = _sortAsc ? SortOrder.Ascending : SortOrder.Descending;
                    break;
                }
            }
        }

        private void RefrescarLblTotal()
        {
            int count = dgvUnidades.RowCount;
            lblTotal.Text = string.Format(Tr("lblTotal", "Total: {0}"), count);
        }

        private Unidad UnidadSeleccionada() => dgvUnidades.CurrentRow?.DataBoundItem as Unidad;

        // ------------------------------------------------------------
        // Handlers
        // ------------------------------------------------------------

        private void cmbMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoFiltros) return;
            RepoblarModelos(null);
            RepoblarAnios(ValorCombo(cmbAnio));
            AplicarFiltros();
        }

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoFiltros) return;
            if (sender == cmbModelo)
                RepoblarAnios(ValorCombo(cmbAnio));
            AplicarFiltros();
        }

        private void chkEstados_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_cargandoFiltros) return;
            // El evento se dispara antes de que CheckedItems refleje el cambio.
            this.BeginInvoke((Action)AplicarFiltros);
        }

        private void btnMarcarTodos_Click(object sender, EventArgs e)
        {
            _cargandoFiltros = true;
            try
            {
                for (int i = 0; i < chkEstados.Items.Count; i++)
                    chkEstados.SetItemChecked(i, true);
            }
            finally { _cargandoFiltros = false; }
            AplicarFiltros();
        }

        private void btnDesmarcarTodos_Click(object sender, EventArgs e)
        {
            _cargandoFiltros = true;
            try
            {
                for (int i = 0; i < chkEstados.Items.Count; i++)
                    chkEstados.SetItemChecked(i, false);
            }
            finally { _cargandoFiltros = false; }
            AplicarFiltros();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => AplicarFiltros();

        private void txtBuscarDominio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true; // evita el "ding" de Windows
            AplicarFiltros();
        }
        private void btnActualizar_Click(object sender, EventArgs e) => Recargar();

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            var u = UnidadSeleccionada();
            if (u == null)
            {
                MessageBox.Show(Tr("msgSeleccionar", "Seleccioná una unidad."), Tr("msgInformacion", "Info"));
                return;
            }
            int scroll = dgvUnidades.FirstDisplayedScrollingRowIndex;
            using (var form = new FormDetalleUnidad(u.Id))
                form.ShowDialog(this);
            Recargar();
            SeleccionarUnidad(u.Id, scroll);
        }

        // Tras recargar, vuelve a posicionar la grilla en la unidad que estaba seleccionada
        // (si sigue visible con los filtros actuales) y restaura el scroll.
        private void SeleccionarUnidad(int idUnidad, int scrollPrevio)
        {
            foreach (DataGridViewRow row in dgvUnidades.Rows)
            {
                if (row.DataBoundItem is Unidad x && x.Id == idUnidad)
                {
                    if (scrollPrevio >= 0 && scrollPrevio < dgvUnidades.RowCount)
                        dgvUnidades.FirstDisplayedScrollingRowIndex = scrollPrevio;
                    dgvUnidades.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private void btnAyudaEstados_Click(object sender, EventArgs e)
        {
            using (var form = new FormAyudaFlujoEstados())
                form.ShowDialog(this);
        }

        private void dgvUnidades_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnVerDetalle_Click(sender, EventArgs.Empty);
        }

        private void dgvUnidades_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvUnidades.Columns[e.ColumnIndex].DataPropertyName == "EstadoActual" && e.Value is EstadoUnidad est)
            {
                e.Value = TrEstado(est);
                e.FormattingApplied = true;

                var color = ColoresEstadoUnidad.Obtener(est);
                e.CellStyle.ForeColor = color;
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.Font = new Font(dgvUnidades.Font, FontStyle.Bold);
            }
        }

        private void dgvUnidades_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;
            string prop = dgvUnidades.Columns[e.ColumnIndex].DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            if (_sortProp == prop) _sortAsc = !_sortAsc;
            else { _sortProp = prop; _sortAsc = true; }

            OrdenarDatos();
            RebindDatos();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }

        // Wrapper para que cada item del CheckedListBox conozca su enum pero muestre el texto traducido.
        private class EstadoItem
        {
            public EstadoUnidad Estado { get; }
            private readonly string _texto;
            public EstadoItem(EstadoUnidad e, string texto) { Estado = e; _texto = texto; }
            public override string ToString() => _texto;
        }
    }
}
