using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRoles : Form
    {
        private readonly IRolService _rolService;
        private readonly IPermisoService _permisoService;
        private readonly IUsuarioService _usuarioService;

        public FormRoles()
        {
            InitializeComponent();
            _rolService = new RolService();
            _permisoService = new PermisoService();
            _usuarioService = new UsuarioService();

            if (!_usuarioService.EsAdmin())
            {
                MessageBox.Show("Solo el administrador puede gestionar roles.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarRoles();
        }

        private void CargarRoles()
        {
            int seleccionIdx = lstRoles.SelectedIndex;

            lstRoles.Items.Clear();
            lstContenido.Items.Clear();
            lstDisponibles.Items.Clear();
            lblCol2.Text = "Permisos del rol";

            try
            {
                foreach (Rol r in _rolService.Listar())
                    lstRoles.Items.Add(r);

                if (seleccionIdx >= 0 && seleccionIdx < lstRoles.Items.Count)
                    lstRoles.SelectedIndex = seleccionIdx;
                else
                    ActualizarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarContenido();
            ActualizarDisponibles();
        }

        private void ActualizarContenido()
        {
            lstContenido.Items.Clear();
            var rol = lstRoles.SelectedItem as Rol;
            if (rol == null) { lblCol2.Text = "Permisos del rol"; return; }

            lblCol2.Text = "Permisos del rol: " + rol.Nombre;

            foreach (Permiso p in _rolService.ListarPermisosDeRol(rol.Id))
                lstContenido.Items.Add(p);
        }

        private void ActualizarDisponibles()
        {
            lstDisponibles.Items.Clear();
            var rol = lstRoles.SelectedItem as Rol;

            var yaContenidos = new HashSet<int>();
            if (rol != null)
                foreach (Permiso p in lstContenido.Items)
                    yaContenidos.Add(p.Id);

            foreach (Permiso p in _permisoService.ListarPlano())
            {
                if (yaContenidos.Contains(p.Id)) continue;
                lstDisponibles.Items.Add(p);
            }
        }

        private void btnNuevoRol_Click(object sender, EventArgs e)
        {
            string nombre = Prompt("Nombre del rol (ej: Supervisor):");
            if (string.IsNullOrWhiteSpace(nombre)) return;

            string descripcion = Prompt("Descripción (opcional):");
            try
            {
                _rolService.Crear(nombre, descripcion);
                CargarRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarRol_Click(object sender, EventArgs e)
        {
            var rol = lstRoles.SelectedItem as Rol;
            if (rol == null)
            {
                MessageBox.Show("Seleccioná un rol a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Eliminar el rol '{rol.Nombre}'?\n\n" +
                "Esta acción es irreversible. El rol se desasignará automáticamente " +
                "de todos los usuarios que lo tengan asignado.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _rolService.Eliminar(rol.Id);
                CargarRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var rol = lstRoles.SelectedItem as Rol;
            if (rol == null)
            {
                MessageBox.Show("Seleccioná un rol a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstDisponibles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<Permiso>();
            foreach (Permiso p in lstDisponibles.SelectedItems) items.Add(p);

            int procesados = 0;
            int redundanciasTotal = 0;
            Permiso falla = null;
            string motivo = null;

            foreach (Permiso item in items)
            {
                try
                {
                    redundanciasTotal += _rolService.AsignarPermiso(rol.Id, item.Id);
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = ex.Message;
                    break;
                }
            }

            ActualizarContenido();
            ActualizarDisponibles();

            if (falla != null)
            {
                MessageBox.Show(
                    $"Se procesaron {procesados} de {items.Count}. Falló en '{falla.Codigo}': {motivo}",
                    "Operación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (redundanciasTotal > 0)
            {
                MessageBox.Show(
                    $"Se agregaron {procesados}. Se quitaron {redundanciasTotal} asignación(es) directa(s) cubiertas por una familia.",
                    "Limpieza automática", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var rol = lstRoles.SelectedItem as Rol;
            if (rol == null)
            {
                MessageBox.Show("Seleccioná un rol a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstContenido.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccioná uno o más permisos a quitar (Ctrl+click o Shift+click para varios).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<Permiso>();
            foreach (Permiso p in lstContenido.SelectedItems) items.Add(p);

            int procesados = 0;
            Permiso falla = null;
            string motivo = null;

            foreach (Permiso item in items)
            {
                try
                {
                    _rolService.QuitarPermiso(rol.Id, item.Id);
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = ex.Message;
                    break;
                }
            }

            ActualizarContenido();
            ActualizarDisponibles();

            if (falla != null)
                MessageBox.Show(
                    $"Se procesaron {procesados} de {items.Count}. Falló en '{falla.Codigo}': {motivo}",
                    "Operación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static string Prompt(string mensaje)
        {
            using (var form = new Form())
            using (var lbl = new Label())
            using (var txt = new TextBox())
            using (var ok = new Button())
            using (var cancel = new Button())
            {
                form.Text = "Ingresar dato";
                form.ClientSize = new Size(380, 130);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false; form.MaximizeBox = false;

                lbl.Text = mensaje; lbl.SetBounds(15, 15, 350, 20);
                txt.SetBounds(15, 40, 350, 25); txt.Font = new Font("Segoe UI", 9.5F);

                ok.Text = "Aceptar";   ok.SetBounds(195, 80, 80, 30); ok.DialogResult = DialogResult.OK;
                cancel.Text = "Cancelar"; cancel.SetBounds(285, 80, 80, 30); cancel.DialogResult = DialogResult.Cancel;

                form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
                form.AcceptButton = ok; form.CancelButton = cancel;

                return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
            }
        }
    }
}
