using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class FormAsignacionPermisos : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormAsignacionPermisos";

        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormAsignacionPermisos()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(HELPERS.SesionUsuario.GetInstancia().Usuario, "ASIGNAR_PERMISOS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para asignar permisos a usuarios."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarUsuarios();

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
            lblTitulo.Text  = Tr("lblTitulo",  "Asignación de Permisos a Usuarios");
            lblCol1.Text    = Tr("lblCol1",    "Usuarios");
            lblCol3.Text    = Tr("lblCol3",    "Roles y Permisos disponibles");
            btnQuitar.Text  = Tr("btnQuitar",  "Quitar del usuario →");
            btnAsignar.Text = Tr("btnAsignar", "← Asignar al usuario");

            var usuario = lstUsuarios.SelectedItem as Usuario;
            lblCol2.Text = usuario == null
                ? Tr("lblCol2", "Asignaciones del usuario")
                : string.Format(Tr("lblCol2De", "Asignaciones de: {0}"), usuario.Username);
        }

        private void CargarUsuarios()
        {
            int seleccionIdx = lstUsuarios.SelectedIndex;

            lstUsuarios.Items.Clear();
            lstAsignado.Items.Clear();
            lstDisponible.Items.Clear();
            lblCol2.Text = Tr("lblCol2", "Asignaciones del usuario");

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
                MessageBox.Show(Tr("msgErrorCargar", "Error al cargar usuarios:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (usuario == null) { lblCol2.Text = Tr("lblCol2", "Asignaciones del usuario"); return; }

            lblCol2.Text = string.Format(Tr("lblCol2De", "Asignaciones de: {0}"), usuario.Username);

            foreach (Permiso p in _usuarioService.ListarPermisosDirectosDeUsuario(usuario.Id))
                lstAsignado.Items.Add(p);
        }

        private void ActualizarDisponibles()
        {
            lstDisponible.Items.Clear();
            var usuario = lstUsuarios.SelectedItem as Usuario;

            var permisosAsignados = new HashSet<int>();
            if (usuario != null)
                foreach (Permiso p in _usuarioService.ListarPermisosDirectosDeUsuario(usuario.Id))
                    permisosAsignados.Add(p.Id);

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
                MessageBox.Show(Tr("msgSelectUsuario", "Seleccioná un usuario a la izquierda."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstDisponible.SelectedItems.Count == 0)
            {
                MessageBox.Show(Tr("msgSelectPermisos", "Seleccioná uno o más permisos de la lista de la derecha (Ctrl+click o Shift+click para varios)."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<Permiso>();
            foreach (Permiso p in lstDisponible.SelectedItems) items.Add(p);

            int procesados = 0;
            int redundanciasTotal = 0;
            Permiso falla = null;
            string motivo = null;

            foreach (Permiso item in items)
            {
                try
                {
                    redundanciasTotal += _usuarioService.AsignarPermiso(usuario.Id, item.Id);
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = TrError(ex);
                    break;
                }
            }

            ActualizarAsignados();
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
                    string.Format(Tr("msgLimpiezaAuto", "Se asignaron {0}. Se quitaron {1} asignación(es) directa(s) redundante(s) porque quedaron cubiertas."),
                        procesados, redundanciasTotal),
                    Tr("msgLimpiezaAutoTitulo", "Limpieza automática"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var usuario = lstUsuarios.SelectedItem as Usuario;
            if (usuario == null)
            {
                MessageBox.Show(Tr("msgSelectUsuario", "Seleccioná un usuario a la izquierda."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstAsignado.SelectedItems.Count == 0)
            {
                MessageBox.Show(Tr("msgSelectAsignaciones", "Seleccioná una o más asignaciones a quitar (Ctrl+click o Shift+click para varias)."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<Permiso>();
            foreach (Permiso p in lstAsignado.SelectedItems) items.Add(p);

            int procesados = 0;
            Permiso falla = null;
            string motivo = null;

            foreach (Permiso item in items)
            {
                try
                {
                    _usuarioService.QuitarPermiso(usuario.Id, item.Id);
                    procesados++;
                }
                catch (Exception ex)
                {
                    falla = item;
                    motivo = TrError(ex);
                    break;
                }
            }

            ActualizarAsignados();
            ActualizarDisponibles();

            if (falla != null)
                MessageBox.Show(
                    string.Format(Tr("msgFallaProc", "Se procesaron {0} de {1}. Falló en '{2}': {3}"),
                        procesados, items.Count, falla.Codigo, motivo),
                    Tr("msgOperacionInterrumpida", "Operación interrumpida"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
