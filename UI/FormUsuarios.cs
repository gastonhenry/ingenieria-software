using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormUsuarios : Form
    {
        private readonly IUsuarioService _usuarioService;
        private List<Usuario> _allData = new List<Usuario>();

        public FormUsuarios()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();

            if (!_usuarioService.EsAdmin())
            {
                MessageBox.Show("Solo el administrador puede acceder a esta sección.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            InicializarGrilla();
            InicializarFiltros();
            dgvUsuarios.SelectionChanged += (s, e) => ActualizarBotones();
            CargarDatos();
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
            try
            {
                _allData = _usuarioService.Listar();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                resultado = resultado.Where(u => u.Username.ToLower().Contains(filtro));

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

            dgvUsuarios.Columns.Add("Id",               "ID");
            dgvUsuarios.Columns.Add("Username",         "Usuario");
            dgvUsuarios.Columns.Add("Nombre",           "Nombre");
            dgvUsuarios.Columns.Add("Apellido",         "Apellido");
            dgvUsuarios.Columns.Add("Estado",           "Estado");
            dgvUsuarios.Columns.Add("IntentosFallidos", "Int. Fallidos");
            dgvUsuarios.Columns.Add("UltimoLogin",      "Último Login");

            dgvUsuarios.Columns["Id"].Visible                             = false;
            dgvUsuarios.Columns["Username"].Width                         = 160;
            dgvUsuarios.Columns["Nombre"].Width                           = 130;
            dgvUsuarios.Columns["Apellido"].Width                         = 130;
            dgvUsuarios.Columns["Estado"].Width                           = 90;
            dgvUsuarios.Columns["IntentosFallidos"].Width                 = 100;
            dgvUsuarios.Columns["IntentosFallidos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsuarios.Columns["UltimoLogin"].AutoSizeMode               = DataGridViewAutoSizeColumnMode.Fill;
            dgvUsuarios.Columns["UltimoLogin"].MinimumWidth               = 130;

            foreach (Usuario u in lista)
            {
                int idx = dgvUsuarios.Rows.Add(
                    u.Id, u.Username, u.Nombre, u.Apellido,
                    u.Bloqueado ? "Bloqueado" : "Activo",
                    u.IntentosFallidos,
                    u.UltimoLogin.HasValue ? u.UltimoLogin.Value.ToString("dd/MM/yyyy HH:mm") : "-");

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
            lblTotal.Text          = $"Total: {lista.Count}";
            lblBloqueados.Visible  = mostrarTodos;
            lblActivos.Visible     = mostrarTodos;
            if (mostrarTodos)
            {
                lblBloqueados.Text = $"Bloqueados: {lista.Count(u => u.Bloqueado)}";
                lblActivos.Text    = $"Activos: {lista.Count(u => !u.Bloqueado)}";
            }
        }

        private void ActualizarBotones()
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                btnBloquear.Enabled = btnDesbloquear.Enabled = false;
                lblHint.Visible = true;
                return;
            }
            lblHint.Visible = false;
            bool esBloqueado = dgvUsuarios.SelectedRows[0].Cells["Estado"].Value?.ToString() == "Bloqueado";
            btnBloquear.Enabled    = !esBloqueado;
            btnDesbloquear.Enabled =  esBloqueado;
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

            if (MessageBox.Show($"¿Bloquear al usuario '{username}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _usuarioService.Bloquear(id.Value, username);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null) return;

            string username = dgvUsuarios.SelectedRows[0].Cells["Username"].Value?.ToString();
            if (MessageBox.Show($"¿Desbloquear al usuario '{username}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                _usuarioService.Desbloquear(id.Value, username);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbEstado.SelectedIndex = 0;
            txtUsuarioFiltro.Text = string.Empty;
            AplicarFiltros();
        }
        private void btnActualizar_Click(object sender, EventArgs e) => CargarDatos();
    }
}
