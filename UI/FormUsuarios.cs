using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormUsuarios : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormUsuarios";

        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private List<Usuario> _allData = new List<Usuario>();
        private int _ultimoTotal;
        private int _ultimoBloqueados;
        private int _ultimoActivos;
        private bool _suscrito;
        private bool _puedeEditar;
        private bool _puedeVerHistorial;

        public FormUsuarios()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(HELPERS.SesionUsuario.GetInstancia().Usuario, "GESTIONAR_USUARIOS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para acceder a esta sección."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            _puedeEditar = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(HELPERS.SesionUsuario.GetInstancia().Usuario, "EDITAR_USUARIO");
            btnEditar.Visible = _puedeEditar;

            _puedeVerHistorial = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(HELPERS.SesionUsuario.GetInstancia().Usuario, "VER_HISTORIAL_USUARIO");
            btnHistorial.Visible = _puedeVerHistorial;

            InicializarGrilla();
            InicializarFiltros();
            dgvUsuarios.SelectionChanged += (s, e) => ActualizarBotones();
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
            lblTitulo.Text         = Tr("lblTitulo",        "Gestión de Usuarios");
            grpFiltros.Text        = Tr("grpFiltros",       "Filtros");
            lblEstado.Text         = Tr("lblEstado",        "Estado:");
            lblUsuarioFiltro.Text  = Tr("lblUsuarioFiltro", "Buscar usuario:");
            btnLimpiar.Text        = Tr("btnLimpiar",       "Limpiar");
            lblHint.Text           = Tr("lblHint",          "← Seleccioná un usuario");
            btnEditar.Text         = Tr("btnEditar",        "Editar");
            btnHistorial.Text      = Tr("btnHistorial",     "Historial");
            btnBloquear.Text       = Tr("btnBloquear",      "Bloquear");
            btnDesbloquear.Text    = Tr("btnDesbloquear",   "Desbloquear");
            lblTotal.Text          = string.Format(Tr("lblTotal",      "Total: {0}"),      _ultimoTotal);
            lblBloqueados.Text     = string.Format(Tr("lblBloqueados", "Bloqueados: {0}"), _ultimoBloqueados);
            lblActivos.Text        = string.Format(Tr("lblActivos",    "Activos: {0}"),    _ultimoActivos);

            if (cmbEstado.Items.Count >= 3)
            {
                int idx = cmbEstado.SelectedIndex;
                cmbEstado.Items[0] = Tr("itemTodos",      "Todos");
                cmbEstado.Items[1] = Tr("itemActivos",    "Activos");
                cmbEstado.Items[2] = Tr("itemBloqueados", "Bloqueados");
                cmbEstado.SelectedIndex = idx;
            }

            if (dgvUsuarios.Columns.Count > 0)
            {
                dgvUsuarios.Columns["Id"].HeaderText               = Tr("colId",               "ID");
                dgvUsuarios.Columns["Username"].HeaderText         = Tr("colUsername",         "Usuario");
                dgvUsuarios.Columns["Nombre"].HeaderText           = Tr("colNombre",           "Nombre");
                dgvUsuarios.Columns["Apellido"].HeaderText         = Tr("colApellido",         "Apellido");
                dgvUsuarios.Columns["Email"].HeaderText            = Tr("colEmail",            "Email");
                dgvUsuarios.Columns["Estado"].HeaderText           = Tr("colEstado",           "Estado");
                dgvUsuarios.Columns["IntentosFallidos"].HeaderText = Tr("colIntentosFallidos", "Int. Fallidos");
                dgvUsuarios.Columns["UltimoLogin"].HeaderText      = Tr("colUltimoLogin",      "Último Login");
            }
            RefrescarCeldasEstado();
        }

        private void RefrescarCeldasEstado()
        {
            if (dgvUsuarios.Columns.Count == 0) return;
            string bloq = Tr("valorBloqueado", "Bloqueado");
            string act  = Tr("valorActivo",    "Activo");
            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                var cell = row.Cells["Estado"];
                if (cell?.Tag is bool esBloqueado)
                    cell.Value = esBloqueado ? bloq : act;
            }
        }

        private void InicializarGrilla()
        {
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 90, 200);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvUsuarios.GridColor = Color.FromArgb(210, 220, 240);
        }

        private void InicializarFiltros()
        {
            cmbEstado.Items.AddRange(new object[] { "Todos", "Activos", "Bloqueados" });
            cmbEstado.SelectedIndex = 0;
            cmbEstado.SelectedIndexChanged += (s, e) => AplicarFiltros();
            txtUsuarioFiltro.TextChanged    += (s, e) => AplicarFiltros();
        }

        private void CargarDatos()
        {
            int? idAMantener = GetSelectedId();
            try
            {
                _allData = _usuarioService.Listar();
                AplicarFiltros();
                if (idAMantener.HasValue) SeleccionarFilaPorId(idAMantener.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorCargar", "Error al cargar usuarios:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SeleccionarFilaPorId(int id)
        {
            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                if (row.Cells["Id"].Value is int rowId && rowId == id)
                {
                    dgvUsuarios.ClearSelection();
                    row.Selected = true;
                    dgvUsuarios.CurrentCell = row.Cells["Username"];
                    return;
                }
            }
        }

        private void AplicarFiltros()
        {
            IEnumerable<Usuario> resultado = _allData;

            switch (cmbEstado.SelectedIndex)
            {
                case 1: resultado = resultado.Where(u => !u.Bloqueado); break;
                case 2: resultado = resultado.Where(u =>  u.Bloqueado); break;
            }

            string filtro = txtUsuarioFiltro.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(filtro))
                resultado = resultado.Where(u =>
                    (u.Username != null && u.Username.ToLower().Contains(filtro)) ||
                    (u.Nombre   != null && u.Nombre.ToLower().Contains(filtro))   ||
                    (u.Apellido != null && u.Apellido.ToLower().Contains(filtro)) ||
                    (u.Email    != null && u.Email.ToLower().Contains(filtro)));

            var lista = resultado
                .OrderByDescending(u => u.Bloqueado)
                .ThenBy(u => u.Username)
                .ToList();

            RefrescarGrilla(lista);
            ActualizarResumen(lista);
        }

        private void RefrescarGrilla(List<Usuario> lista)
        {
            dgvUsuarios.Rows.Clear();
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add("Id",               Tr("colId",               "ID"));
            dgvUsuarios.Columns.Add("Username",         Tr("colUsername",         "Usuario"));
            dgvUsuarios.Columns.Add("Nombre",           Tr("colNombre",           "Nombre"));
            dgvUsuarios.Columns.Add("Apellido",         Tr("colApellido",         "Apellido"));
            dgvUsuarios.Columns.Add("Email",            Tr("colEmail",            "Email"));
            dgvUsuarios.Columns.Add("Estado",           Tr("colEstado",           "Estado"));
            dgvUsuarios.Columns.Add("IntentosFallidos", Tr("colIntentosFallidos", "Int. Fallidos"));
            dgvUsuarios.Columns.Add("UltimoLogin",      Tr("colUltimoLogin",      "Último Login"));

            dgvUsuarios.Columns["Id"].Visible                             = false;
            dgvUsuarios.Columns["Username"].Width                         = 140;
            dgvUsuarios.Columns["Nombre"].Width                           = 120;
            dgvUsuarios.Columns["Apellido"].Width                         = 120;
            dgvUsuarios.Columns["Email"].Width                            = 200;
            dgvUsuarios.Columns["Estado"].Width                           = 90;
            dgvUsuarios.Columns["IntentosFallidos"].Width                 = 100;
            dgvUsuarios.Columns["IntentosFallidos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsuarios.Columns["UltimoLogin"].AutoSizeMode               = DataGridViewAutoSizeColumnMode.Fill;
            dgvUsuarios.Columns["UltimoLogin"].MinimumWidth               = 130;

            string bloq = Tr("valorBloqueado", "Bloqueado");
            string act  = Tr("valorActivo",    "Activo");

            foreach (Usuario u in lista)
            {
                int idx = dgvUsuarios.Rows.Add(
                    u.Id, u.Username, u.Nombre, u.Apellido, u.Email,
                    u.Bloqueado ? bloq : act,
                    u.IntentosFallidos,
                    u.UltimoLogin.HasValue ? u.UltimoLogin.Value.ToString("dd/MM/yyyy HH:mm") : "-");

                dgvUsuarios.Rows[idx].Cells["Estado"].Tag = u.Bloqueado;

                if (u.Bloqueado)
                {
                    dgvUsuarios.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                    dgvUsuarios.Rows[idx].DefaultCellStyle.ForeColor = Color.FromArgb(160, 0, 0);
                }
            }

            ActualizarBotones();
        }

        private void ActualizarResumen(List<Usuario> lista)
        {
            bool mostrarTodos = cmbEstado.SelectedIndex == 0;
            _ultimoTotal = lista.Count;
            _ultimoBloqueados = lista.Count(u => u.Bloqueado);
            _ultimoActivos = lista.Count(u => !u.Bloqueado);

            lblTotal.Text         = string.Format(Tr("lblTotal", "Total: {0}"), _ultimoTotal);
            lblBloqueados.Visible = mostrarTodos;
            lblActivos.Visible    = mostrarTodos;
            if (mostrarTodos)
            {
                lblBloqueados.Text = string.Format(Tr("lblBloqueados", "Bloqueados: {0}"), _ultimoBloqueados);
                lblActivos.Text    = string.Format(Tr("lblActivos",    "Activos: {0}"),    _ultimoActivos);
            }
        }

        private void ActualizarBotones()
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                btnBloquear.Enabled = btnDesbloquear.Enabled = false;
                btnEditar.Enabled = false;
                btnHistorial.Enabled = false;
                lblHint.Visible = true;
                return;
            }
            lblHint.Visible = false;
            bool esBloqueado = false;
            var tag = dgvUsuarios.SelectedRows[0].Cells["Estado"].Tag;
            if (tag is bool b) esBloqueado = b;
            btnBloquear.Enabled    = !esBloqueado;
            btnDesbloquear.Enabled =  esBloqueado;
            btnEditar.Enabled      = _puedeEditar;
            btnHistorial.Enabled   = _puedeVerHistorial;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null) return;

            using (var form = new FormEditarUsuario(id.Value))
            {
                form.ShowDialog(this);
                if (form.Guardado) CargarDatos();
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null) return;

            using (var form = new FormHistorialUsuario(id.Value))
            {
                form.ShowDialog(this);
                if (form.HuboRestauracion) CargarDatos();
            }
        }

        private int? GetSelectedId()
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return null;
            return (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
        }

        private void btnBloquear_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null) return;

            string username = dgvUsuarios.SelectedRows[0].Cells["Username"].Value?.ToString();

            if (MessageBox.Show(string.Format(Tr("msgConfirmarBloqueo", "¿Bloquear al usuario '{0}'?"), username),
                Tr("msgConfirmar", "Confirmar"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _usuarioService.Bloquear(id.Value, username);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null) return;

            string username = dgvUsuarios.SelectedRows[0].Cells["Username"].Value?.ToString();
            if (MessageBox.Show(string.Format(Tr("msgConfirmarDesbloqueo", "¿Desbloquear al usuario '{0}'?"), username),
                Tr("msgConfirmar", "Confirmar"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _usuarioService.Desbloquear(id.Value, username);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbEstado.SelectedIndex = 0;
            txtUsuarioFiltro.Text = string.Empty;
            AplicarFiltros();
        }
        private void btnActualizar_Click(object sender, EventArgs e) => CargarDatos();

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
