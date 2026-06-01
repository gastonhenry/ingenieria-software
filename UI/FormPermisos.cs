using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPermisos : Form
    {
        private readonly IPermisoService _permisoService;
        private readonly IUsuarioService _usuarioService;

        public FormPermisos()
        {
            InitializeComponent();
            _permisoService = new PermisoService();
            _usuarioService = new UsuarioService();

            if (!_usuarioService.EsAdmin())
            {
                MessageBox.Show("Solo el administrador puede gestionar permisos.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarFamilias();
        }

        private void CargarFamilias()
        {
            int seleccionIdx = lstContenedores.SelectedIndex;

            lstContenedores.Items.Clear();
            lstContenido.Items.Clear();
            lstDisponibles.Items.Clear();
            lblCol2.Text = "Contenido";

            try
            {
                foreach (Permiso p in _permisoService.ListarPlano())
                    if (p is FamiliaPermiso f)
                        lstContenedores.Items.Add(f);

                if (seleccionIdx >= 0 && seleccionIdx < lstContenedores.Items.Count)
                    lstContenedores.SelectedIndex = seleccionIdx;
                else
                    ActualizarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar familias:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstContenedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarContenido();
            ActualizarDisponibles();
        }

        private void ActualizarContenido()
        {
            lstContenido.Items.Clear();
            var familia = lstContenedores.SelectedItem as FamiliaPermiso;
            if (familia == null) { lblCol2.Text = "Contenido"; return; }

            lblCol2.Text = "Contenido de: " + familia.Codigo;

            FamiliaPermiso familiaConHijos = BuscarFamiliaEnArbol(familia.Id);
            if (familiaConHijos != null)
                foreach (Permiso h in familiaConHijos.Hijos)
                    lstContenido.Items.Add(h);
        }

        private FamiliaPermiso BuscarFamiliaEnArbol(int id)
        {
            return BuscarRec(_permisoService.ListarArbol(), id);
        }

        private FamiliaPermiso BuscarRec(List<Permiso> nodos, int id)
        {
            foreach (Permiso n in nodos)
            {
                FamiliaPermiso f = n as FamiliaPermiso;
                if (f == null) continue;
                if (f.Id == id) return f;
                var sub = BuscarRec(f.Hijos, id);
                if (sub != null) return sub;
            }
            return null;
        }

        private void ActualizarDisponibles()
        {
            lstDisponibles.Items.Clear();
            var familia = lstContenedores.SelectedItem as FamiliaPermiso;

            int? excluirId = null;
            var yaContenidos = new HashSet<int>();
            if (familia != null)
            {
                excluirId = familia.Id;
                foreach (Permiso p in lstContenido.Items)
                    yaContenidos.Add(p.Id);
            }

            foreach (Permiso p in _permisoService.ListarPlano())
            {
                if (excluirId.HasValue && p.Id == excluirId.Value) continue;
                if (yaContenidos.Contains(p.Id)) continue;
                lstDisponibles.Items.Add(p);
            }
        }

        private void btnNuevaFamilia_Click(object sender, EventArgs e)
        {
            string codigo = Prompt("Código de la familia (ej: FAM_GESTION_VENTAS):");
            if (string.IsNullOrWhiteSpace(codigo)) return;

            string descripcion = Prompt("Descripción (opcional):");
            try
            {
                _permisoService.CrearFamiliaPermiso(codigo, descripcion);
                CargarFamilias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            var familia = lstContenedores.SelectedItem as FamiliaPermiso;
            if (familia == null)
            {
                MessageBox.Show("Seleccioná una familia a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Borrar la familia '{familia.Codigo}'?\n\n" +
                "Esta acción es irreversible. La familia se desasignará automáticamente " +
                "de todos los roles y usuarios que la tengan asignada. " +
                "Los permisos hijos (patentes/subfamilias) NO se borran, sólo quedan sin padre.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _permisoService.EliminarFamilia(familia.Id);
                CargarFamilias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var familia = lstContenedores.SelectedItem as FamiliaPermiso;
            if (familia == null)
            {
                MessageBox.Show("Seleccioná una familia a la izquierda.",
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
                    redundanciasTotal += _permisoService.AgregarHijo(familia.Id, item.Id);
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
                    $"Se agregaron {procesados}. Se quitaron {redundanciasTotal} asignación(es) directa(s) de roles cubiertas por esta familia.",
                    "Limpieza automática", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var familia = lstContenedores.SelectedItem as FamiliaPermiso;
            if (familia == null)
            {
                MessageBox.Show("Seleccioná una familia a la izquierda.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstContenido.SelectedItems.Count == 0)
            {
                MessageBox.Show("Seleccioná uno o más hijos a quitar (Ctrl+click o Shift+click para varios).",
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
                    _permisoService.QuitarHijo(item.Id);
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
