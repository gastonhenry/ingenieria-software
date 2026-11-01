using BE;
using BLL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    // N03 — CU-CAP-02 paso 5: detalle de un indicador del tablero (modal).
    // Reutiliza el TableroCaptacion ya calculado; no vuelve a consultar la base.
    public partial class FormDetalleCaptacion : Form, IObservadorIdioma
    {
        public enum Modo { PosiblesCompradores, UnidadesSinInteresados, InteresadosSinUnidades, Conversiones }

        private const string CODIGO_FORM = "FormDetalleCaptacion";

        private readonly TableroCaptacion _tablero;
        private readonly Modo _modo;
        private readonly FilaUnidadTablero _unidad;
        private readonly IIdiomaService _idiomaService;
        private readonly IPermisoService _permisoService;
        private bool _suscrito;

        public FormDetalleCaptacion(TableroCaptacion tablero, Modo modo, FilaUnidadTablero unidad = null)
        {
            InitializeComponent();
            _tablero = tablero;
            _modo = modo;
            _unidad = unidad;
            _idiomaService = new IdiomaService();
            _permisoService = new PermisoService();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());
        }

        private bool Tiene(string permiso)
        {
            var u = SesionUsuario.GetInstancia().Usuario;
            if (u != null && u.Username != null && u.Username.ToLower() == "admin") return true;
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

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            btnCerrar.Text        = Tr("btnCerrar",        "Cerrar");
            btnAvisosCliente.Text = Tr("btnAvisosCliente", "Avisos del cliente...");
            btnExportarPdf.Text   = Tr("btnExportarPdf",   "Exportar PDF");
            Cargar();
        }

        private void Cargar()
        {
            dgvDetalle.Rows.Clear();
            dgvDetalle.Columns.Clear();
            string periodo = string.Format(Tr("lblPeriodo", "Período: {0:d} al {1:d}"), _tablero.Desde, _tablero.Hasta);

            switch (_modo)
            {
                case Modo.PosiblesCompradores:
                    CargarPosiblesCompradores();
                    break;
                case Modo.InteresadosSinUnidades:
                    this.Text = lblTitulo.Text = Tr("tituloInteresadosSinUnidades", "Interesados sin unidades");
                    lblSubtitulo.Text = Tr("subInteresadosSinUnidades", "Clientes con búsqueda activa que hoy no coincide con ninguna unidad en venta. Se les avisa cuando ingrese una.");
                    Columnas(("colCliente", "Cliente", 20), ("colTelefono", "Teléfono", 14), ("colEmail", "Correo", 20),
                             ("colBusca", "Busca", 26), ("colEntrevista", "Entrevista", 10), ("colVence", "Vence", 10));
                    foreach (var it in _tablero.InteresadosSinUnidades)
                    {
                        string busca = string.Join(" | ", it.Busquedas.Select(b => TextosCaptacion.DescribirBusqueda(b, Tr)));
                        string email = it.Cliente.Email;
                        if (it.AvisosSuspendidos) email = (email ?? "") + " " + Tr("marcaSuspendido", "(avisos suspendidos)");
                        else if (!it.Busquedas.Any(b => b.AceptaAvisos)) email = (email ?? "") + " " + Tr("marcaSinAvisos", "(no acepta avisos)");
                        int i = dgvDetalle.Rows.Add(it.Cliente.Nombre, it.Cliente.Telefono, email, busca,
                            it.FechaUltimaEntrevista.ToString("d", CultureInfo.CurrentCulture),
                            it.Busquedas.Min(b => b.FechaVencimiento).ToString("d", CultureInfo.CurrentCulture));
                        dgvDetalle.Rows[i].Tag = it;
                        if (it.AvisosSuspendidos) dgvDetalle.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(130, 130, 140);
                    }
                    break;
                case Modo.UnidadesSinInteresados:
                    this.Text = lblTitulo.Text = Tr("tituloSinInteresados", "Unidades en venta sin interesados");
                    lblSubtitulo.Text = Tr("subSinInteresados", "Unidades que no coinciden con ninguna búsqueda activa. Las marcadas superan los días de alerta.");
                    Columnas(("colUnidad", "Unidad", 45), ("colPrecio", "Precio", 20), ("colDias", "Días publicada", 15), ("colAlerta", "Alerta", 20));
                    foreach (var f in _tablero.Unidades.Where(x => x.CantidadInteresados == 0))
                    {
                        int i = dgvDetalle.Rows.Add(f.Unidad.ToString(), TextosCaptacion.Precio(f.Unidad.PrecioPublicacion, Tr),
                            f.DiasPublicada, f.Alerta ? Tr("alertaPromocionar", "Evaluar promoción o precio") : "");
                        dgvDetalle.Rows[i].Tag = f.Unidad;
                        if (f.Alerta) dgvDetalle.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 220);
                    }
                    break;
                case Modo.Conversiones:
                    this.Text = lblTitulo.Text = Tr("tituloConversiones", "Clientes avisados que compraron");
                    lblSubtitulo.Text = periodo + "  ·  " + string.Format(Tr("subConversion", "{0} de {1} clientes avisados ({2}%)"),
                        _tablero.ClientesConvertidos, _tablero.ClientesAvisados, _tablero.PorcentajeConversion.ToString("0.#", CultureInfo.CurrentCulture));
                    Columnas(("colCliente", "Cliente", 22), ("colUnidadComprada", "Unidad comprada", 30), ("colFechaAviso", "Fecha de aviso", 16),
                             ("colFechaVenta", "Fecha de venta", 16), ("colPrecioVenta", "Precio", 16));
                    foreach (var c in _tablero.Conversiones)
                        dgvDetalle.Rows.Add(c.ClienteNombre, c.UnidadComprada, c.FechaAviso.ToString("d", CultureInfo.CurrentCulture),
                            c.FechaVenta.ToString("d", CultureInfo.CurrentCulture),
                            c.Precio.ToString("C0", CultureInfo.CurrentCulture));
                    break;
            }
            ActualizarBotones();
        }

        private void CargarPosiblesCompradores()
        {
            IEnumerable<InteresadoTablero> interesados = _tablero.Interesados;
            if (_unidad != null)
            {
                this.Text = lblTitulo.Text = string.Format(Tr("tituloInteresadosUnidad", "Posibles compradores — {0}"), _unidad.Unidad);
                lblSubtitulo.Text = string.Format(Tr("subInteresadosUnidad", "Clientes con búsqueda activa que coincide con esta unidad ({0})."),
                    TextosCaptacion.Precio(_unidad.Unidad.PrecioPublicacion, Tr));
                interesados = interesados.Where(i => i.Coincidencias.Any(u => u.IdUnidad == _unidad.Unidad.IdUnidad));
            }
            else
            {
                this.Text = lblTitulo.Text = Tr("tituloPosiblesCompradores", "Posibles compradores");
                lblSubtitulo.Text = Tr("subPosiblesCompradores", "Clientes con búsqueda activa que coincide con al menos una unidad en venta.");
            }

            Columnas(("colCliente", "Cliente", 18), ("colTelefono", "Teléfono", 13), ("colEmail", "Correo", 18),
                     ("colBusca", "Busca", 23), ("colEntrevista", "Entrevista", 10), ("colUnidades", "Unidades que coinciden", 18));
            foreach (var it in interesados)
            {
                string busca = string.Join(" | ", it.Busquedas.Select(b => TextosCaptacion.DescribirBusqueda(b, Tr)));
                string email = it.Cliente.Email;
                if (it.AvisosSuspendidos) email = (email ?? "") + " " + Tr("marcaSuspendido", "(avisos suspendidos)");
                else if (!it.Busquedas.Any(b => b.AceptaAvisos)) email = (email ?? "") + " " + Tr("marcaSinAvisos", "(no acepta avisos)");
                int i = dgvDetalle.Rows.Add(it.Cliente.Nombre, it.Cliente.Telefono, email, busca,
                    it.FechaUltimaEntrevista.ToString("d", CultureInfo.CurrentCulture),
                    string.Join(", ", it.Coincidencias.Select(u => $"{u.Marca} {u.Modelo} {u.Anio}")));
                dgvDetalle.Rows[i].Tag = it;
                if (it.AvisosSuspendidos) dgvDetalle.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(130, 130, 140);
            }
        }

        private void Columnas(params (string codigo, string texto, float peso)[] cols)
        {
            foreach (var c in cols)
                dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn { Name = c.codigo, HeaderText = Tr(c.codigo, c.texto), FillWeight = c.peso });
        }

        private void ActualizarBotones()
        {
            object sel = dgvDetalle.CurrentRow?.Tag;
            btnAvisosCliente.Visible = (_modo == Modo.PosiblesCompradores || _modo == Modo.InteresadosSinUnidades) && Tiene("GESTIONAR_AVISOS_CLIENTE");
            btnAvisosCliente.Enabled = sel is InteresadoTablero;
            // Exportar PDF: unidad seleccionada en "sin interesados", o la unidad filtrada en posibles compradores.
            btnExportarPdf.Visible = _modo == Modo.UnidadesSinInteresados || (_modo == Modo.PosiblesCompradores && _unidad != null);
            btnExportarPdf.Enabled = _unidad != null || sel is UnidadEnVenta;
        }

        private void dgvDetalle_SelectionChanged(object sender, EventArgs e) => ActualizarBotones();

        // Unidad elegida con doble clic (en "sin interesados"): el tablero la selecciona al cerrar.
        public int? IdUnidadElegida { get; private set; }

        private void dgvDetalle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !(dgvDetalle.Rows[e.RowIndex].Tag is UnidadEnVenta u)) return;
            IdUnidadElegida = u.IdUnidad;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAvisosCliente_Click(object sender, EventArgs e)
        {
            var it = dgvDetalle.CurrentRow?.Tag as InteresadoTablero;
            if (it == null) return;
            using (var f = new FormAvisosCliente(it.Cliente))
                f.ShowDialog(this);
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            int? idUnidad = _unidad?.Unidad.IdUnidad ?? (dgvDetalle.CurrentRow?.Tag as UnidadEnVenta)?.IdUnidad;
            if (idUnidad.HasValue) ExportadorPdfPublicacion.Exportar(this, idUnidad.Value);
        }

        private void btnCerrar_Click(object sender, EventArgs e) => this.Close();

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
