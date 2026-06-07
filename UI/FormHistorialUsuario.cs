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
    public partial class FormHistorialUsuario : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormHistorialUsuario";

        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioHistorialService _historialService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private readonly int _usuarioId;
        private Usuario _usuario;
        private List<UsuarioHistorial> _historiales = new List<UsuarioHistorial>();
        private bool _suscrito;
        private bool _puedeRestaurar;
        private bool _huboRestauracion;

        public bool HuboRestauracion => _huboRestauracion;

        public FormHistorialUsuario(int usuarioId)
        {
            InitializeComponent();
            _usuarioId = usuarioId;
            _usuarioService = new UsuarioService();
            _historialService = new UsuarioHistorialService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool puedeVer = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "VER_HISTORIAL_USUARIO");
            if (!puedeVer)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para ver el historial de usuarios."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            _puedeRestaurar = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "EDITAR_USUARIO");

            _usuario = _usuarioService.ObtenerPorId(_usuarioId);
            if (_usuario == null)
            {
                MessageBox.Show(Tr("msgUsuarioNoEncontrado", "No se encontró el usuario."),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Load += (s, e) => this.Close();
                return;
            }

            InicializarGrilla();
            CargarHistorial();

            dgvHistorial.SelectionChanged += (s, e) => ActualizarBotonRestaurar();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());
        }

        private void InicializarGrilla()
        {
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvHistorial.EnableHeadersVisualStyles = false;
            dgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 90, 200);
            dgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgvHistorial.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
            dgvHistorial.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvHistorial.GridColor = Color.FromArgb(210, 220, 240);
        }

        private void CargarHistorial()
        {
            try
            {
                _historiales = _historialService.ListarPorUsuario(_usuarioId);
                RefrescarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorCargar", "Error al cargar el historial:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefrescarGrilla()
        {
            dgvHistorial.Rows.Clear();
            dgvHistorial.Columns.Clear();

            dgvHistorial.Columns.Add("Id",              "Id");
            dgvHistorial.Columns.Add("FechaHora",       Tr("colFechaHora",       "Fecha y Hora"));
            dgvHistorial.Columns.Add("Accion",          Tr("colAccion",          "Acción"));
            dgvHistorial.Columns.Add("ModificadoPor",   Tr("colModificadoPor",   "Modificado por"));
            dgvHistorial.Columns.Add("Nombre",          Tr("colNombre",          "Nombre"));
            dgvHistorial.Columns.Add("Apellido",        Tr("colApellido",        "Apellido"));
            dgvHistorial.Columns.Add("Email",           Tr("colEmail",           "Email"));
            dgvHistorial.Columns.Add("Telefono",        Tr("colTelefono",        "Teléfono"));
            dgvHistorial.Columns.Add("Documento",       Tr("colDocumento",       "Documento"));
            dgvHistorial.Columns.Add("Domicilio",       Tr("colDomicilio",       "Domicilio"));
            dgvHistorial.Columns.Add("Bloqueado",       Tr("colBloqueado",       "Bloqueado"));
            dgvHistorial.Columns.Add("Restauracion",    Tr("colRestauracion",    "Restaurado desde"));

            dgvHistorial.Columns["Id"].Visible = false;
            dgvHistorial.Columns["Restauracion"].Width = 150;
            dgvHistorial.Columns["FechaHora"].Width    = 130;
            dgvHistorial.Columns["Accion"].Width       = 100;
            dgvHistorial.Columns["ModificadoPor"].Width = 130;
            dgvHistorial.Columns["Bloqueado"].Width    = 80;
            dgvHistorial.Columns["Domicilio"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHistorial.Columns["Domicilio"].MinimumWidth = 140;

            foreach (DataGridViewColumn col in dgvHistorial.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            string si       = Tr("valorSi",       "Sí");
            string no       = Tr("valorNo",       "No");
            string sistema  = Tr("valorSistema",  "(sistema)");

            foreach (UsuarioHistorial h in _historiales)
            {
                int idx = dgvHistorial.Rows.Add(
                    h.Id,
                    h.FechaHora.ToString("dd/MM/yyyy HH:mm:ss"),
                    h.Accion.GetDescripcionTraducida(),
                    string.IsNullOrEmpty(h.ModificadoPorUsername) ? sistema : h.ModificadoPorUsername,
                    h.Nombre,
                    h.Apellido,
                    h.Email,
                    h.Telefono,
                    h.Documento,
                    h.Domicilio,
                    h.Bloqueado ? si : no,
                    h.RestauracionFechaHora.HasValue ? h.RestauracionFechaHora.Value.ToString("dd/MM/yyyy HH:mm:ss") : "");
                dgvHistorial.Rows[idx].Cells["Accion"].Tag    = h.Accion;
                dgvHistorial.Rows[idx].Cells["Bloqueado"].Tag = h.Bloqueado;
            }

            MarcarVersionActual();
            ActualizarBotonRestaurar();
        }

        private void MarcarVersionActual()
        {
            if (dgvHistorial.Rows.Count == 0) return;
            var verde = Color.FromArgb(217, 240, 217);
            var verdeSel = Color.FromArgb(180, 220, 180);
            var fila = dgvHistorial.Rows[0];
            fila.DefaultCellStyle.BackColor = verde;
            fila.DefaultCellStyle.SelectionBackColor = verdeSel;
            fila.DefaultCellStyle.SelectionForeColor = Color.Black;
            fila.DefaultCellStyle.Font = new Font(dgvHistorial.DefaultCellStyle.Font ?? new Font("Segoe UI", 9f), FontStyle.Bold);
        }

        private bool FilaSeleccionadaEsActual()
        {
            return dgvHistorial.SelectedRows.Count > 0
                && dgvHistorial.SelectedRows[0].Index == 0;
        }

        private void ActualizarBotonRestaurar()
        {
            btnRestaurar.Enabled = _puedeRestaurar
                && dgvHistorial.SelectedRows.Count > 0
                && !FilaSeleccionadaEsActual();
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
            lblTitulo.Text       = string.Format(Tr("lblTituloDe", "Historial de: {0}"), _usuario?.Username ?? "");
            this.Text            = Tr("title", "Historial de Usuario");
            btnRestaurar.Text    = Tr("btnRestaurar", "Restaurar a esta versión");
            btnCerrar.Text       = Tr("btnCerrar",    "Cerrar");
            lblLeyendaActual.Text = Tr("leyendaActual", "● Versión actual");

            if (dgvHistorial.Columns.Count > 0)
            {
                dgvHistorial.Columns["FechaHora"].HeaderText     = Tr("colFechaHora",     "Fecha y Hora");
                dgvHistorial.Columns["Accion"].HeaderText        = Tr("colAccion",        "Acción");
                dgvHistorial.Columns["ModificadoPor"].HeaderText = Tr("colModificadoPor", "Modificado por");
                dgvHistorial.Columns["Nombre"].HeaderText        = Tr("colNombre",        "Nombre");
                dgvHistorial.Columns["Apellido"].HeaderText      = Tr("colApellido",      "Apellido");
                dgvHistorial.Columns["Email"].HeaderText         = Tr("colEmail",         "Email");
                dgvHistorial.Columns["Telefono"].HeaderText      = Tr("colTelefono",      "Teléfono");
                dgvHistorial.Columns["Documento"].HeaderText     = Tr("colDocumento",     "Documento");
                dgvHistorial.Columns["Domicilio"].HeaderText     = Tr("colDomicilio",     "Domicilio");
                dgvHistorial.Columns["Bloqueado"].HeaderText     = Tr("colBloqueado",     "Bloqueado");
                dgvHistorial.Columns["Restauracion"].HeaderText  = Tr("colRestauracion",  "Restaurado desde");

                string si = Tr("valorSi", "Sí"), no = Tr("valorNo", "No"), sistema = Tr("valorSistema", "(sistema)");
                foreach (DataGridViewRow row in dgvHistorial.Rows)
                {
                    if (row.Cells["Accion"].Tag is AccionUsuarioHistorial acc)
                        row.Cells["Accion"].Value = acc.GetDescripcionTraducida();
                    if (row.Cells["Bloqueado"].Tag is bool b)
                        row.Cells["Bloqueado"].Value = b ? si : no;

                    UsuarioHistorial source = _historiales.FirstOrDefault(h => h.Id == (int)row.Cells["Id"].Value);
                    if (source != null)
                        row.Cells["ModificadoPor"].Value = string.IsNullOrEmpty(source.ModificadoPorUsername) ? sistema : source.ModificadoPorUsername;
                }
            }
        }

        private int? IdHistorialSeleccionado()
        {
            if (dgvHistorial.SelectedRows.Count == 0) return null;
            return (int)dgvHistorial.SelectedRows[0].Cells["Id"].Value;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            int? historialId = IdHistorialSeleccionado();
            if (historialId == null)
            {
                MessageBox.Show(Tr("msgSelectVersion", "Seleccioná una versión a restaurar."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UsuarioHistorial snap = _historiales.FirstOrDefault(h => h.Id == historialId.Value);
            string fechaFmt = snap != null ? snap.FechaHora.ToString("dd/MM/yyyy HH:mm:ss") : "?";

            var confirm = MessageBox.Show(
                string.Format(Tr("msgConfirmarRestaurar",
                    "¿Restaurar el usuario al estado del {0}?\n\nSe sobrescribirán los datos actuales con los de esa versión. Quedará registrado como una nueva entrada en el historial."),
                    fechaFmt),
                Tr("msgConfirmarRestaurarTitulo", "Confirmar restauración"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _historialService.Restaurar(historialId.Value);
                _huboRestauracion = true;
                MessageBox.Show(Tr("msgRestauradoExito", "Usuario restaurado correctamente."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                _usuario = _usuarioService.ObtenerPorId(_usuarioId);
                CargarHistorial();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => this.Close();

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
