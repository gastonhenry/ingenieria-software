using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class FormAsignacionPermisos : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IPermisoService _permisoService;

        public FormAsignacionPermisos()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            _rolService = new RolService();
            _permisoService = new PermisoService();

            if (!_usuarioService.EsAdmin())
            {
                MessageBox.Show("Solo el administrador puede asignar permisos a usuarios.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            int seleccionIdx = lstUsuarios.SelectedIndex;

            lstUsuarios.Items.Clear();
            lstAsignado.Items.Clear();
            lstDisponible.Items.Clear();
            lblCol2.Text = "Asignaciones del usuario";

            try
            {
                foreach (Usuario u in _usuarioService.Listar())
                {
                    if (u.Username != null && u.Username.ToLower() == "admin") continue;
                    lstUsuarios.Items.Add(u);
                }

                if (seleccionIdx >= 0 && seleccionIdx < lstUsuarios.Items.Count)
                    lstUsuarios.SelectedIndex = seleccionIdx;
                else
                    ActualizarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarAsignados();
            ActualizarDisponibles();
        }

        private void ActualizarAsignados()
        {
            lstAsignado.Items.Clear();
            var usuario = lstUsuarios.SelectedItem as Usuario;
            if (usuario == null) { lblCol2.Text = "Asignaciones del usuario"; return; }

            lblCol2.Text = "Asignaciones de: " + usuario.Username;

            foreach (Rol r in _usuarioService.ListarRolesDeUsuario(usuario.Id))
                lstAsignado.Items.Add(r);

            foreach (Permiso p in _usuarioService.ListarPermisosDirectosDeUsuario(usuario.Id))
                lstAsignado.Items.Add(p);
        }

        private void ActualizarDisponibles()
        {
            lstDisponible.Items.Clear();
            var usuario = lstUsuarios.SelectedItem as Usuario;

            var rolesAsignados = new HashSet<int>();
            var permisosAsignados = new HashSet<int>();
            if (usuario != null)
            {
                foreach (Rol r in _usuarioService.ListarRolesDeUsuario(usuario.Id))
                    rolesAsignados.Add(r.Id);
                foreach (Permiso p in _usuarioService.ListarPermisosDirectosDeUsuario(usuario.Id))
                    permisosAsignados.Add(p.Id);
            }

            foreach (Rol r in _rolService.Listar())
            {
                if (rolesAsignados.Contains(r.Id)) continue;
                lstDisponible.Items.Add(r);
            }

            foreach (Permiso p in _permisoService.ListarPlano())
            {
                if (permisosAsignados.Contains(p.Id)) continue;
                lstDisponible.Items.Add(p);
            }
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            var usuario = lstUsuarios.SelectedItem as Usuario;
            if (usuario == null)
            {
                MessageBox.Show("Seleccioná un usuario a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstDisponible.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccioná uno o más roles/permisos de la lista de la derecha (Ctrl+click o Shift+click para varios).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<object>();
            foreach (object o in lstDisponible.SelectedItems) items.Add(o);

            int procesados = 0;
            int redundanciasTotal = 0;
            object falla = null;
            string motivo = null;

            foreach (object item in items)
            {
                try
                {
                    int red;
                    if (item is Rol r)
                        red = _usuarioService.AsignarRol(usuario.Id, r.Id);
                    else
                        red = _usuarioService.AsignarPermiso(usuario.Id, ((Permiso)item).Id);

                    redundanciasTotal += red;
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = ex.Message;
                    break;
                }
            }

            ActualizarAsignados();
            ActualizarDisponibles();

            if (falla != null)
            {
                MessageBox.Show(
                    $"Se procesaron {procesados} de {items.Count}. Falló en '{falla}': {motivo}",
                    "Operación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (redundanciasTotal > 0)
            {
                MessageBox.Show(
                    $"Se asignaron {procesados}. Se quitaron {redundanciasTotal} asignación(es) directa(s) redundante(s) porque quedaron cubiertas.",
                    "Limpieza automática", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var usuario = lstUsuarios.SelectedItem as Usuario;
            if (usuario == null)
            {
                MessageBox.Show("Seleccioná un usuario a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstAsignado.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccioná una o más asignaciones a quitar (Ctrl+click o Shift+click para varias).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<object>();
            foreach (object o in lstAsignado.SelectedItems) items.Add(o);

            int procesados = 0;
            object falla = null;
            string motivo = null;

            foreach (object item in items)
            {
                try
                {
                    if (item is Rol r)
                        _usuarioService.QuitarRol(usuario.Id, r.Id);
                    else
                        _usuarioService.QuitarPermiso(usuario.Id, ((Permiso)item).Id);
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = ex.Message;
                    break;
                }
            }

            ActualizarAsignados();
            ActualizarDisponibles();

            if (falla != null)
                MessageBox.Show(
                    $"Se procesaron {procesados} de {items.Count}. Falló en '{falla}': {motivo}",
                    "Operación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
