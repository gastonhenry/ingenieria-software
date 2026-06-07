using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPermisos : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormPermisos";

        private readonly IPermisoService _permisoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormPermisos()
        {
            InitializeComponent();
            _permisoService = new PermisoService();
            _usuarioService = new UsuarioService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(HELPERS.SesionUsuario.GetInstancia().Usuario, "GESTIONAR_PERMISOS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para gestionar permisos."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarRoles();

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
            lblTitulo.Text      = Tr("lblTitulo",      "Gestión de Permisos");
            lblCol1.Text        = Tr("lblCol1",        "Roles");
            lblCol3.Text        = Tr("lblCol3",        "Roles y Permisos disponibles");
            btnNuevoRol.Text    = Tr("btnNuevoRol",    "Nuevo Rol");
            btnEliminarRol.Text = Tr("btnEliminarRol", "Borrar Rol");
            btnQuitar.Text      = Tr("btnQuitar",      "Sacar de la lista →");
            btnAgregar.Text     = Tr("btnAgregar",     "← Agregar a la lista");

            var rol = lstContenedores.SelectedItem as Rol;
            lblCol2.Text = rol == null
                ? Tr("lblCol2", "Contenido")
                : string.Format(Tr("lblCol2De", "Contenido de: {0}"), rol.Codigo);
        }

        private void CargarRoles()
        {
            int seleccionIdx = lstContenedores.SelectedIndex;

            lstContenedores.Items.Clear();
            lstContenido.Items.Clear();
            lstDisponibles.Items.Clear();
            lblCol2.Text = Tr("lblCol2", "Contenido");

            try
            {
                foreach (Permiso p in _permisoService.ListarPlano())
                    if (p is Rol r)
                        lstContenedores.Items.Add(r);

                if (seleccionIdx >= 0 && seleccionIdx < lstContenedores.Items.Count)
                    lstContenedores.SelectedIndex = seleccionIdx;
                else
                    ActualizarDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorCargarRoles", "Error al cargar roles:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var rol = lstContenedores.SelectedItem as Rol;
            if (rol == null) { lblCol2.Text = Tr("lblCol2", "Contenido"); return; }

            lblCol2.Text = string.Format(Tr("lblCol2De", "Contenido de: {0}"), rol.Codigo);

            Rol rolConHijos = BuscarRolEnArbol(rol.Id);
            if (rolConHijos != null)
                foreach (Permiso h in rolConHijos.Hijos)
                    lstContenido.Items.Add(h);
        }

        private Rol BuscarRolEnArbol(int id)
        {
            return BuscarRec(_permisoService.ListarArbol(), id);
        }

        private Rol BuscarRec(List<Permiso> nodos, int id)
        {
            foreach (Permiso n in nodos)
            {
                Rol r = n as Rol;
                if (r == null) continue;
                if (r.Id == id) return r;
                var sub = BuscarRec(r.Hijos, id);
                if (sub != null) return sub;
            }
            return null;
        }

        private void ActualizarDisponibles()
        {
            lstDisponibles.Items.Clear();
            var rol = lstContenedores.SelectedItem as Rol;

            int? excluirId = null;
            var yaContenidos = new HashSet<int>();
            if (rol != null)
            {
                excluirId = rol.Id;
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

        private void btnNuevoRol_Click(object sender, EventArgs e)
        {
            string codigo = Prompt(Tr("promptCodigoRol", "Código del rol:"));
            if (string.IsNullOrWhiteSpace(codigo)) return;

            string descripcion = Prompt(Tr("promptDescripcionRol", "Descripción (opcional):"));
            try
            {
                _permisoService.CrearRol(codigo, descripcion);
                CargarRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Tr("msgErrorPrefix", "Error: {0}"), TrError(ex)),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarRol_Click(object sender, EventArgs e)
        {
            var rol = lstContenedores.SelectedItem as Rol;
            if (rol == null)
            {
                MessageBox.Show(Tr("msgSelectRol", "Seleccioná un rol a la izquierda."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                string.Format(Tr("msgConfirmarBorrarRol",
                    "¿Borrar el rol '{0}'?\n\nEsta acción es irreversible. El rol se desasignará automáticamente de todos los usuarios que lo tengan asignado. Los permisos hijos NO se borran, sólo quedan sin padre."),
                    rol.Codigo),
                Tr("msgConfirmarEliminacion", "Confirmar eliminación"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _permisoService.EliminarRol(rol.Id);
                CargarRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Tr("msgErrorPrefix", "Error: {0}"), TrError(ex)),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var rol = lstContenedores.SelectedItem as Rol;
            if (rol == null)
            {
                MessageBox.Show(Tr("msgSelectRol", "Seleccioná un rol a la izquierda."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstDisponibles.SelectedItems.Count == 0)
            {
                MessageBox.Show(Tr("msgSelectPermisos", "Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios)."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    redundanciasTotal += _permisoService.AgregarHijo(rol.Id, item.Id);
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = TrError(ex);
                    break;
                }
            }

            ActualizarContenido();
            ActualizarDisponibles();

            if (falla != null)
            {
                MessageBox.Show(
                    string.Format(Tr("msgFallaProc", "Se procesaron {0} de {1}. Falló en '{2}': {3}"),
                        procesados, items.Count, falla.Codigo, motivo),
                    Tr("msgOperacionInterrumpida", "Operación interrumpida"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (redundanciasTotal > 0)
            {
                MessageBox.Show(
                    string.Format(Tr("msgLimpiezaAuto", "Se agregaron {0}. Se quitaron {1} asignación(es) directa(s) cubiertas por este rol."),
                        procesados, redundanciasTotal),
                    Tr("msgLimpiezaAutoTitulo", "Limpieza automática"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var rol = lstContenedores.SelectedItem as Rol;
            if (rol == null)
            {
                MessageBox.Show(Tr("msgSelectRol", "Seleccioná un rol a la izquierda."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstContenido.SelectedItems.Count == 0)
            {
                MessageBox.Show(Tr("msgSelectHijos", "Seleccioná uno o más hijos a quitar (Ctrl+click o Shift+click para varios)."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    motivo = TrError(ex);
                    break;
                }
            }

            ActualizarContenido();
            ActualizarDisponibles();

            if (falla != null)
                MessageBox.Show(
                    string.Format(Tr("msgFallaProc", "Se procesaron {0} de {1}. Falló en '{2}': {3}"),
                        procesados, items.Count, falla.Codigo, motivo),
                    Tr("msgOperacionInterrumpida", "Operación interrumpida"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string Prompt(string mensaje)
        {
            using (var form = new Form())
            using (var lbl = new Label())
            using (var txt = new TextBox())
            using (var ok = new Button())
            using (var cancel = new Button())
            {
                form.Text = Tr("promptIngresarTitulo", "Ingresar");
                form.ClientSize = new Size(380, 130);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false; form.MaximizeBox = false;

                lbl.Text = mensaje; lbl.SetBounds(15, 15, 350, 20);
                txt.SetBounds(15, 40, 350, 25); txt.Font = new Font("Segoe UI", 9.5F);

                ok.Text     = Tr("btnAceptar",  "Aceptar");   ok.SetBounds(195, 80, 80, 30); ok.DialogResult     = DialogResult.OK;
                cancel.Text = Tr("btnCancelar", "Cancelar");  cancel.SetBounds(285, 80, 80, 30); cancel.DialogResult = DialogResult.Cancel;

                form.Controls.AddRange(new System.Windows.Forms.Control[] { lbl, txt, ok, cancel });
                form.AcceptButton = ok; form.CancelButton = cancel;

                return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
            }
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
