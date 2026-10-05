using BE;
using BE.Enums;
using BLL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace UI
{
    // Listado de publicaciones iniciadas (unidades En venta / Pendiente de venta con PublicacionUnidad creada).
    // El "Ver detalle" no abre modal: arma la vista de la publicación en el panel derecho.
    public partial class FormPublicaciones : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormPublicaciones";
        private const string CODIGO_FORM_ESTADO = "EstadoUnidad";

        private static readonly Color ColorFiltroInvalido = Color.FromArgb(255, 225, 225);

        private readonly IPublicacionService _publicacionService;
        private readonly IImagenUnidadService _imagenService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private readonly bool _puedeEditar;
        private bool _suscrito;

        private List<PublicacionListado> _todas = new List<PublicacionListado>();
        private List<PublicacionListado> _datos = new List<PublicacionListado>();
        private string _sortProp = null;
        private bool _sortAsc = true;
        private bool _cargandoFiltros;

        // Publicación que se está mostrando en el panel derecho (null = panel vacío).
        private PublicacionListado _detalle;
        private readonly List<Image> _imagenesCargadas = new List<Image>();
        private PictureBox _miniaturaSeleccionada;
        private readonly ToolTip _toolTip;

        public FormPublicaciones()
        {
            InitializeComponent();
            _publicacionService = new PublicacionService();
            _imagenService = new ImagenUnidadService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            _puedeEditar = Tiene("GESTIONAR_PUBLICACION");

            // Cada columna se ajusta a su contenido (header incluido); la última ocupa el espacio sobrante.
            dgvPublicaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            colActualizacion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colActualizacion.MinimumWidth = 120;
            dgvPublicaciones.AllowUserToResizeColumns = false;
            dgvPublicaciones.SizeChanged += (s, e) => PosicionarBotonVerDetalle();
            pnlDetalleHeader.Resize += (s, e) =>
                btnEditarPublicacion.Left = pnlDetalleHeader.Width - btnEditarPublicacion.Width;

            // Foto grande: click → visor a pantalla completa.
            picPrincipal.Cursor = Cursors.Hand;
            picPrincipal.Click += (s, e) => AbrirVisor();
            _toolTip = new ToolTip();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => { PosicionarBotonVerDetalle(); Recargar(); };
        }

        private bool Tiene(string permiso)
        {
            var u = SesionUsuario.GetInstancia().Usuario;
            if (u != null && u.Username.ToLower() == "admin") return true;
            return u != null && _permisoService.UsuarioTienePermiso(u, permiso);
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
            this.Text                = Tr("title",             "Publicaciones");
            lblTitulo.Text           = Tr("lblTitulo",         "Publicaciones");
            lblBuscarDominio.Text    = Tr("lblBuscarDominio",  "Dominio:");
            lblFiltroMarca.Text      = Tr("lblFiltroMarca",    "Marca:");
            lblFiltroModelo.Text     = Tr("lblFiltroModelo",   "Modelo:");
            lblFiltroAnio.Text       = Tr("lblFiltroAnio",     "Año:");
            lblPrecioDesde.Text      = Tr("lblPrecioDesde",    "Precio desde:");
            lblPrecioHasta.Text      = Tr("lblPrecioHasta",    "Hasta:");
            lblKmHasta.Text          = Tr("lblKmHasta",        "Km hasta:");
            lblBuscarTexto.Text      = Tr("lblBuscarTexto",    "Descripción:");
            lblFiltroEstado.Text     = Tr("lblFiltroEstado",   "Estado:");
            chkEnVenta.Text          = TrEstado(EstadoUnidad.EnVenta);
            chkPendienteVenta.Text   = TrEstado(EstadoUnidad.PendienteVenta);
            chkSoloConPrecio.Text    = Tr("chkSoloConPrecio",  "Sólo con precio");
            btnLimpiar.Text          = Tr("btnLimpiar",        "Limpiar filtros");
            btnActualizar.Text       = Tr("btnActualizar",     "Actualizar");
            btnVerDetalle.Text       = Tr("btnVerDetalle",     "Ver detalle");
            btnEditarPublicacion.Text= Tr("btnEditarPublicacion", "Editar publicación");
            lblDetalleVacio.Text     = Tr("lblDetalleVacio",   "Seleccioná una publicación y presioná \"Ver detalle\".");
            lblDescripcionTitulo.Text= Tr("lblDescripcionTitulo", "Descripción");
            lblSinFotos.Text         = Tr("lblSinFotos",       "Esta unidad no tiene fotos cargadas.");
            _toolTip?.SetToolTip(picPrincipal, Tr("tooltipAmpliar", "Click para ampliar"));

            colDominio.HeaderText       = Tr("colDominio",       "Dominio");
            colMarca.HeaderText         = Tr("colMarca",         "Marca");
            colModelo.HeaderText        = Tr("colModelo",        "Modelo");
            colAnio.HeaderText          = Tr("colAnio",          "Año");
            colKm.HeaderText            = Tr("colKm",            "Km");
            colEstado.HeaderText        = Tr("colEstado",        "Estado");
            colPrecio.HeaderText        = Tr("colPrecio",        "Precio");
            colFotos.HeaderText         = Tr("colFotos",         "Fotos");
            colActualizacion.HeaderText = Tr("colActualizacion", "Última edición");

            if (_todas.Count > 0) PoblarFiltros(preservarSeleccion: true);
            dgvPublicaciones.Refresh();
            lblTotal.Text = string.Format(Tr("lblTotal", "Total: {0}"), _datos.Count);
            if (_detalle != null) MostrarDatosDetalle(_detalle);
        }

        // ------------------------------------------------------------
        // Filtros (mismo criterio que FormUnidades: combos dependientes marca → modelo → año)
        // ------------------------------------------------------------

        private void PoblarFiltros(bool preservarSeleccion)
        {
            _cargandoFiltros = true;
            try
            {
                string marcaSel  = preservarSeleccion ? ValorCombo(cmbMarca)  : null;
                string modeloSel = preservarSeleccion ? ValorCombo(cmbModelo) : null;
                string anioSel   = preservarSeleccion ? ValorCombo(cmbAnio)   : null;
                string todosPrev = cmbMarca.Items.Count > 0 ? cmbMarca.Items[0].ToString() : null;
                string todos = Tr("itemTodos", "(Todos)");
                // Si cambió el idioma, "(Todos)" viejo se traduce al nuevo.
                if (marcaSel == todosPrev)  marcaSel  = todos;
                if (modeloSel == todosPrev) modeloSel = todos;
                if (anioSel == todosPrev)   anioSel   = todos;

                var marcas = _todas.Select(p => p.Marca).Where(m => !string.IsNullOrEmpty(m))
                                   .Distinct().OrderBy(m => m).ToList();
                cmbMarca.Items.Clear();
                cmbMarca.Items.Add(todos);
                foreach (var m in marcas) cmbMarca.Items.Add(m);
                int idx = cmbMarca.Items.IndexOf(marcaSel ?? todos);
                cmbMarca.SelectedIndex = idx >= 0 ? idx : 0;

                RepoblarModelos(modeloSel);
                RepoblarAnios(anioSel);
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
                    query = query.Where(p => p.Marca == marcaSel);
                var modelos = query.Select(p => p.Modelo).Where(m => !string.IsNullOrEmpty(m))
                                   .Distinct().OrderBy(m => m).ToList();
                cmbModelo.Items.Clear();
                cmbModelo.Items.Add(todos);
                foreach (var m in modelos) cmbModelo.Items.Add(m);
                int idx = cmbModelo.Items.IndexOf(modeloPrev ?? todos);
                cmbModelo.SelectedIndex = idx >= 0 ? idx : 0;
            }
            finally { _cargandoFiltros = previo; }
        }

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
                    query = query.Where(p => p.Marca == marcaSel);
                if (!string.IsNullOrEmpty(modeloSel) && modeloSel != todos)
                    query = query.Where(p => p.Modelo == modeloSel);
                var anios = query.Select(p => p.Anio).Distinct().OrderByDescending(a => a).ToList();
                cmbAnio.Items.Clear();
                cmbAnio.Items.Add(todos);
                foreach (var a in anios) cmbAnio.Items.Add(a.ToString());
                int idx = cmbAnio.Items.IndexOf(anioPrev ?? todos);
                cmbAnio.SelectedIndex = idx >= 0 ? idx : 0;
            }
            finally { _cargandoFiltros = previo; }
        }

        private static string ValorCombo(ComboBox cmb) => cmb.SelectedItem?.ToString();

        // Devuelve null si el texto está vacío; marca el TextBox en rojo si no es un número válido.
        private static decimal? LeerDecimal(TextBox txt, out bool invalido)
        {
            invalido = false;
            string s = txt.Text?.Trim();
            if (string.IsNullOrEmpty(s)) { txt.BackColor = SystemColors.Window; return null; }
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal v) && v >= 0)
            {
                txt.BackColor = SystemColors.Window;
                return v;
            }
            invalido = true;
            txt.BackColor = ColorFiltroInvalido;
            return null;
        }

        // ------------------------------------------------------------
        // Carga y filtrado
        // ------------------------------------------------------------

        private void Recargar()
        {
            try
            {
                _todas = _publicacionService.ListarPublicaciones() ?? new List<PublicacionListado>();
                PoblarFiltros(preservarSeleccion: true);
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
            IEnumerable<PublicacionListado> q = _todas;

            string dom = txtBuscarDominio.Text?.Trim().ToUpper();
            if (!string.IsNullOrEmpty(dom))
                q = q.Where(p => p.Dominio != null && p.Dominio.ToUpper().Contains(dom));

            string marca = ValorCombo(cmbMarca);
            if (!string.IsNullOrEmpty(marca) && marca != todos)
                q = q.Where(p => p.Marca == marca);

            string modelo = ValorCombo(cmbModelo);
            if (!string.IsNullOrEmpty(modelo) && modelo != todos)
                q = q.Where(p => p.Modelo == modelo);

            string anio = ValorCombo(cmbAnio);
            if (!string.IsNullOrEmpty(anio) && anio != todos && int.TryParse(anio, out int a))
                q = q.Where(p => p.Anio == a);

            // Filtros numéricos inválidos se ignoran (el TextBox queda marcado en rojo).
            decimal? precioDesde = LeerDecimal(txtPrecioDesde, out _);
            decimal? precioHasta = LeerDecimal(txtPrecioHasta, out _);
            decimal? kmHasta     = LeerDecimal(txtKmHasta, out _);
            if (precioDesde.HasValue)
                q = q.Where(p => p.PrecioPublicacion.HasValue && p.PrecioPublicacion.Value >= precioDesde.Value);
            if (precioHasta.HasValue)
                q = q.Where(p => p.PrecioPublicacion.HasValue && p.PrecioPublicacion.Value <= precioHasta.Value);
            if (kmHasta.HasValue)
                q = q.Where(p => p.Kilometraje <= kmHasta.Value);

            string texto = txtBuscarTexto.Text?.Trim();
            if (!string.IsNullOrEmpty(texto))
                q = q.Where(p => p.DescripcionPublicacion != null
                              && p.DescripcionPublicacion.IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0);

            var estados = new HashSet<EstadoUnidad>();
            if (chkEnVenta.Checked)        estados.Add(EstadoUnidad.EnVenta);
            if (chkPendienteVenta.Checked) estados.Add(EstadoUnidad.PendienteVenta);
            q = q.Where(p => estados.Contains(p.EstadoActual));

            if (chkSoloConPrecio.Checked) q = q.Where(p => p.PrecioPublicacion.HasValue);

            _datos = q.ToList();
            if (!string.IsNullOrEmpty(_sortProp)) OrdenarDatos();
            RebindDatos();
        }

        private void RebindDatos()
        {
            dgvPublicaciones.AutoGenerateColumns = false;
            dgvPublicaciones.DataSource = null;
            dgvPublicaciones.DataSource = _datos;
            lblTotal.Text = string.Format(Tr("lblTotal", "Total: {0}"), _datos.Count);
            MostrarIndicadorOrden();
        }

        private void OrdenarDatos()
        {
            PropertyInfo prop = typeof(PublicacionListado).GetProperty(_sortProp);
            if (prop == null) return;
            _datos = _sortAsc
                ? _datos.OrderBy(p => prop.GetValue(p, null)).ToList()
                : _datos.OrderByDescending(p => prop.GetValue(p, null)).ToList();
        }

        private void MostrarIndicadorOrden()
        {
            foreach (DataGridViewColumn col in dgvPublicaciones.Columns)
                col.HeaderCell.SortGlyphDirection = col.DataPropertyName == _sortProp
                    ? (_sortAsc ? SortOrder.Ascending : SortOrder.Descending)
                    : SortOrder.None;
        }

        private PublicacionListado PublicacionSeleccionada() =>
            dgvPublicaciones.CurrentRow?.DataBoundItem as PublicacionListado;

        private void SeleccionarFila(int idUnidad)
        {
            foreach (DataGridViewRow row in dgvPublicaciones.Rows)
            {
                if (row.DataBoundItem is PublicacionListado p && p.IdUnidad == idUnidad)
                {
                    dgvPublicaciones.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private void PosicionarBotonVerDetalle()
        {
            // El botón queda alineado al borde derecho de la grilla (columna izquierda).
            btnVerDetalle.Left = Math.Max(lblTotal.Right + 10, dgvPublicaciones.Right - btnVerDetalle.Width);
        }

        // ------------------------------------------------------------
        // Panel de detalle (vista "publicación")
        // ------------------------------------------------------------

        private string FormatearPrecio(decimal? precio) =>
            precio.HasValue
                ? "$ " + precio.Value.ToString("N0", CultureInfo.CurrentCulture)
                : Tr("valorSinPrecio", "Precio a consultar");

        private void MostrarDetalle(PublicacionListado p)
        {
            _detalle = p;
            MostrarDatosDetalle(p);
            CargarFotos(p.IdUnidad);
            lblDetalleVacio.Visible = false;
            tlpDetalle.Visible = true;
        }

        // Textos del panel (se re-ejecuta al cambiar de idioma sin recargar las fotos).
        private void MostrarDatosDetalle(PublicacionListado p)
        {
            lblEstadoBadge.Text = TrEstado(p.EstadoActual).ToUpper();
            lblEstadoBadge.BackColor = ColoresEstadoUnidad.Obtener(p.EstadoActual);
            btnEditarPublicacion.Visible = _puedeEditar;

            lblTituloPub.Text = $"{p.Marca} {p.Modelo} {p.Anio}";
            lblSubtituloPub.Text = string.Format(Tr("lblSubtituloPub", "Dominio {0}  ·  {1} km"),
                p.Dominio, p.Kilometraje.ToString("N0", CultureInfo.CurrentCulture));
            lblPrecioPub.Text = FormatearPrecio(p.PrecioPublicacion);
            lblPrecioPub.ForeColor = p.PrecioPublicacion.HasValue
                ? Color.FromArgb(30, 90, 200)
                : Color.FromArgb(130, 130, 140);

            lblFechasPub.Text = p.FechaUltimaEdicion.HasValue
                ? string.Format(Tr("lblFechasPubEditada", "Publicación iniciada el {0:dd/MM/yyyy}  ·  Última edición {1:dd/MM/yyyy HH:mm}"),
                                p.FechaCreacion, p.FechaUltimaEdicion.Value)
                : string.Format(Tr("lblFechasPub", "Publicación iniciada el {0:dd/MM/yyyy}"), p.FechaCreacion);

            txtDescripcionPub.Text = string.IsNullOrWhiteSpace(p.DescripcionPublicacion)
                ? Tr("valorSinDescripcion", "(sin descripción)")
                : p.DescripcionPublicacion.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

            lblFotosTitulo.Text = string.Format(Tr("lblFotosTitulo", "Fotos ({0})"), _imagenesCargadas.Count);
        }

        private void CargarFotos(int idUnidad)
        {
            LiberarFotos();

            List<ImagenUnidad> imagenes;
            try { imagenes = _imagenService.Listar(idUnidad) ?? new List<ImagenUnidad>(); }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                imagenes = new List<ImagenUnidad>();
            }

            foreach (var img in imagenes)
            {
                Image bmp = CargarImagenSinBloquear(_imagenService.ObtenerRutaCompleta(img));
                if (bmp == null) continue; // archivo borrado o ilegible: se omite
                _imagenesCargadas.Add(bmp);

                var mini = new PictureBox
                {
                    Image = bmp,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(120, 86),
                    Margin = new Padding(0, 0, 8, 0),
                    Padding = new Padding(2),
                    BackColor = Color.FromArgb(230, 232, 238),
                    Cursor = Cursors.Hand
                };
                mini.Click += (s, e) => SeleccionarMiniatura((PictureBox)s);
                mini.DoubleClick += (s, e) => { SeleccionarMiniatura((PictureBox)s); AbrirVisor(); };
                flowMiniaturas.Controls.Add(mini);
            }

            bool hayFotos = _imagenesCargadas.Count > 0;
            // Filas: 8 = foto grande, 9 = "sin fotos", 10 = miniaturas.
            tlpDetalle.RowStyles[8].Height  = hayFotos ? 300F : 0F;
            tlpDetalle.RowStyles[9].Height  = hayFotos ? 0F : 30F;
            tlpDetalle.RowStyles[10].Height = hayFotos ? 104F : 0F;
            tlpDetalle.Height = (int)tlpDetalle.RowStyles.Cast<RowStyle>().Sum(r => r.Height);
            lblFotosTitulo.Text = string.Format(Tr("lblFotosTitulo", "Fotos ({0})"), _imagenesCargadas.Count);

            if (hayFotos) SeleccionarMiniatura((PictureBox)flowMiniaturas.Controls[0]);
        }

        private void SeleccionarMiniatura(PictureBox mini)
        {
            if (_miniaturaSeleccionada != null)
                _miniaturaSeleccionada.BackColor = Color.FromArgb(230, 232, 238);
            _miniaturaSeleccionada = mini;
            mini.BackColor = Color.FromArgb(30, 90, 200); // marco azul vía Padding
            picPrincipal.Image = mini.Image;
        }

        private void AbrirVisor()
        {
            if (picPrincipal.Image == null || _imagenesCargadas.Count == 0) return;
            int indice = Math.Max(0, _imagenesCargadas.IndexOf(picPrincipal.Image));
            using (var visor = new FormVisorImagen(_imagenesCargadas, indice,
                       Tr("lblAyudaVisor", "Esc para cerrar  ·  ← → para navegar")))
                visor.ShowDialog(this);
        }

        // Copia la imagen a memoria para no dejar el archivo bloqueado mientras el form está abierto.
        private static Image CargarImagenSinBloquear(string ruta)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta)) return null;
                using (var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var tmp = Image.FromStream(fs))
                    return new Bitmap(tmp);
            }
            catch { return null; }
        }

        private void LiberarFotos()
        {
            picPrincipal.Image = null;
            _miniaturaSeleccionada = null;
            foreach (var c in flowMiniaturas.Controls.Cast<System.Windows.Forms.Control>().ToList())
                c.Dispose();
            flowMiniaturas.Controls.Clear();
            foreach (var img in _imagenesCargadas) img.Dispose();
            _imagenesCargadas.Clear();
        }

        private void LimpiarDetalle()
        {
            _detalle = null;
            LiberarFotos();
            tlpDetalle.Visible = false;
            lblDetalleVacio.Visible = true;
        }

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

        private void filtroTexto_TextChanged(object sender, EventArgs e)
        {
            if (_cargandoFiltros) return;
            AplicarFiltros();
        }

        private void filtroCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (_cargandoFiltros) return;
            AplicarFiltros();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            _cargandoFiltros = true;
            try
            {
                txtBuscarDominio.Clear();
                txtBuscarTexto.Clear();
                txtPrecioDesde.Clear();
                txtPrecioHasta.Clear();
                txtKmHasta.Clear();
                chkEnVenta.Checked = true;
                chkPendienteVenta.Checked = true;
                chkSoloConPrecio.Checked = false;
            }
            finally { _cargandoFiltros = false; }
            PoblarFiltros(preservarSeleccion: false);
            AplicarFiltros();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            int? id = _detalle?.IdUnidad;
            Recargar();
            RefrescarDetalle(id);
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            var p = PublicacionSeleccionada();
            if (p == null)
            {
                MessageBox.Show(Tr("msgSeleccionar", "Seleccioná una publicación."), Tr("msgInformacion", "Info"));
                return;
            }
            MostrarDetalle(p);
        }

        private void dgvPublicaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnVerDetalle_Click(sender, EventArgs.Empty);
        }

        private void btnEditarPublicacion_Click(object sender, EventArgs e)
        {
            if (_detalle == null) return;
            int id = _detalle.IdUnidad;
            using (var f = new FormPublicacion(id))
                f.ShowDialog(this);
            Recargar();
            RefrescarDetalle(id);
        }

        // Tras recargar, vuelve a mostrar la publicación si sigue en el listado (puede haber cambiado de estado).
        private void RefrescarDetalle(int? idUnidad)
        {
            if (!idUnidad.HasValue) return;
            var p = _todas.FirstOrDefault(x => x.IdUnidad == idUnidad.Value);
            if (p == null) { LimpiarDetalle(); return; }
            SeleccionarFila(p.IdUnidad);
            MostrarDetalle(p);
        }

        private void dgvPublicaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string prop = dgvPublicaciones.Columns[e.ColumnIndex].DataPropertyName;

            if (prop == "EstadoActual" && e.Value is EstadoUnidad est)
            {
                e.Value = TrEstado(est);
                e.FormattingApplied = true;
                e.CellStyle.ForeColor = ColoresEstadoUnidad.Obtener(est);
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.Font = new Font(dgvPublicaciones.Font, FontStyle.Bold);
            }
            else if (prop == "PrecioPublicacion")
            {
                e.Value = e.Value is decimal d ? FormatearPrecio(d) : Tr("valorSinPrecioCorto", "—");
                e.FormattingApplied = true;
            }
            else if (prop == "Kilometraje" && e.Value is int km)
            {
                e.Value = km.ToString("N0", CultureInfo.CurrentCulture);
                e.FormattingApplied = true;
            }
            else if (prop == "FechaActualizacion" && e.Value is DateTime f)
            {
                e.Value = f.ToString("dd/MM/yyyy HH:mm");
                e.FormattingApplied = true;
            }
        }

        private void dgvPublicaciones_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;
            string prop = dgvPublicaciones.Columns[e.ColumnIndex].DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            if (_sortProp == prop) _sortAsc = !_sortAsc;
            else { _sortProp = prop; _sortAsc = true; }

            OrdenarDatos();
            RebindDatos();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            LiberarFotos();
            _toolTip?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
