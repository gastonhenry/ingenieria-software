using BE;
using BLL;
using HELPERS;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPrincipal : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormPrincipal";

        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private string _username;
        private bool _loggingOut = false;

        public FormPrincipal(string username)
        {
            _username = username;
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new NavBarColorTable());
            this.MaximizedBounds = Screen.FromControl(this).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            bool esAdmin = _usuarioService.EsAdmin();

            bool puedeRegistrarUsuario  = esAdmin || TienePermiso("REGISTRAR_USUARIO");
            bool puedeGestionarUsuarios = esAdmin || TienePermiso("GESTIONAR_USUARIOS");
            menuInsertarUsuario.Visible = puedeRegistrarUsuario;
            menuVerUsuarios.Visible     = puedeGestionarUsuarios;
            menuUsuarios.Visible        = puedeRegistrarUsuario || puedeGestionarUsuarios;

            bool puedeAsignarPermisos = esAdmin || TienePermiso("ASIGNAR_PERMISOS");
            bool puedeGestionarPermisos = esAdmin || TienePermiso("GESTIONAR_PERMISOS");
            menuGestionPermisos.Visible    = puedeGestionarPermisos;
            menuAsignacionPermisos.Visible = puedeAsignarPermisos;
            menuPermisos.Visible           = puedeGestionarPermisos || puedeAsignarPermisos;

            menuIdiomas.Visible  = esAdmin || TienePermiso("GESTIONAR_IDIOMAS");
            menuBitacora.Visible = esAdmin || TienePermiso("VER_BITACORA");
            menuMantenimiento.Visible = esAdmin;

            CargarSelectorIdiomas();
            menuSeleccionIdioma.DropDownOpening += (s, e) => CargarSelectorIdiomas();
            try
            {
                _idiomaService.Suscribir(this);
                ActualizarIdioma(_idiomaService.IdiomaActual());
            }
            catch (Exception)
            {
                ActualizarIdioma(null);
            }

            this.Shown += (s, e) => Navegar(new FormHome());
        }

        private void CargarSelectorIdiomas()
        {
            menuSeleccionIdioma.DropDownItems.Clear();
            try
            {
                Idioma actual = _idiomaService.IdiomaActual();

                foreach (Idioma idioma in _idiomaService.Listar())
                {
                    var item = new ToolStripMenuItem(idioma.ToString()) { Tag = idioma };
                    if (!idioma.Completo)
                    {
                        item.Enabled = false;
                        item.ToolTipText = Tr("tooltipIdiomaEnProceso", "Idioma en proceso de creación: faltan traducciones.");
                    }
                    else
                    {
                        item.Click += IdiomaItem_Click;
                        item.Checked = actual != null && actual.Id == idioma.Id;
                    }
                    menuSeleccionIdioma.DropDownItems.Add(item);
                }
            }
            catch (Exception)
            {
                var item = new ToolStripMenuItem(Tr("itemSinIdiomas", "(sin idiomas disponibles)")) { Enabled = false };
                menuSeleccionIdioma.DropDownItems.Add(item);
            }
        }

        private void IdiomaItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            var idioma = item?.Tag as Idioma;
            if (idioma == null) return;

            try { _idiomaService.CambiarIdiomaActual(idioma.Id); }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorCambiarIdioma", "Error al cambiar idioma:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            lblSesion.Text              = Tr("lblSesion", "Sesión") + ": " + _username;
            lblEstadoSesion.ToolTipText = Tr("tooltipSesionActiva", "Sesión activa");

            menuInicio.Text              = Tr("menuInicio",              "Inicio");
            menuUsuarios.Text            = Tr("menuUsuarios",            "Usuarios");
            menuInsertarUsuario.Text     = Tr("menuInsertarUsuario",     "Registrar Usuario");
            menuVerUsuarios.Text         = Tr("menuVerUsuarios",         "Gestión de Usuarios");
            menuPermisos.Text            = Tr("menuPermisos",            "Permisos");
            menuGestionPermisos.Text     = Tr("menuGestionPermisos",     "Gestión de Permisos");
            menuAsignacionPermisos.Text  = Tr("menuAsignacionPermisos",  "Asignación de Permisos a Usuarios");
            menuBitacora.Text            = Tr("menuBitacora",            "Bitácora");
            menuVerBitacora.Text         = Tr("menuVerBitacora",         "Ver Bitácora");
            menuIdiomas.Text             = Tr("menuIdiomas",             "Idiomas");
            menuGestionIdiomas.Text      = Tr("menuGestionIdiomas",      "Gestión de Idiomas");
            menuSeleccionIdioma.Text     = Tr("menuSeleccionIdioma",     "Idioma ▾");
            menuMantenimiento.Text       = Tr("menuMantenimiento",       "Mantenimiento");
            menuLogout.Text              = Tr("menuLogout",              "Logout");

            foreach (ToolStripMenuItem item in menuSeleccionIdioma.DropDownItems)
            {
                var idi = item.Tag as Idioma;
                item.Checked = idi != null && nuevoIdioma != null && idi.Id == nuevoIdioma.Id;
            }
        }

        private bool TienePermiso(string codigo)
        {
            return _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, codigo);
        }

        private class NavBarColorTable : ProfessionalColorTable
        {
            static readonly Color Base     = Color.FromArgb(30, 90, 200);
            static readonly Color Hover    = Color.FromArgb(55, 120, 230);
            static readonly Color Pressed  = Color.FromArgb(20, 65, 160);

            public override Color MenuStripGradientBegin              => Base;
            public override Color MenuStripGradientEnd                => Base;
            public override Color MenuItemSelectedGradientBegin       => Hover;
            public override Color MenuItemSelectedGradientEnd         => Hover;
            public override Color MenuItemPressedGradientBegin        => Pressed;
            public override Color MenuItemPressedGradientEnd          => Pressed;
            public override Color MenuItemBorder                      => Color.Transparent;
            public override Color MenuItemSelected                    => Hover;
            public override Color MenuBorder                          => Color.FromArgb(210, 210, 210);
            public override Color ToolStripDropDownBackground         => Color.White;
            public override Color ImageMarginGradientBegin            => Color.White;
            public override Color ImageMarginGradientMiddle           => Color.White;
            public override Color ImageMarginGradientEnd              => Color.White;
        }

        private void menuInicio_Click(object sender, EventArgs e)
        {
            Navegar(new FormHome());
        }

        private void Navegar(Form destino)
        {
            foreach (Form child in this.MdiChildren.ToList())
                child.Close();

            destino.ControlBox = false;
            destino.MdiParent = this;
            destino.WindowState = FormWindowState.Maximized;
            destino.Show();
        }

        private void menuInsertarUsuario_Click(object sender, EventArgs e) =>
            Navegar(new FormInsertarUsuario());

        private void menuVerUsuarios_Click(object sender, EventArgs e) =>
            Navegar(new FormUsuarios());

        private void menuAsignacionPermisos_Click(object sender, EventArgs e) =>
            Navegar(new FormAsignacionPermisos());

        private void menuVerBitacora_Click(object sender, EventArgs e) =>
            Navegar(new FormBitacora());

        private void menuGestionPermisos_Click(object sender, EventArgs e) =>
            Navegar(new FormPermisos());

        private void menuGestionIdiomas_Click(object sender, EventArgs e) =>
            Navegar(new FormIdiomas());

        private void menuMantenimiento_Click(object sender, EventArgs e)
        {
            var mantSrv = new MantenimientoService();
            ResultadoIntegridadGlobal resultado;
            try { resultado = mantSrv.VerificarTodo(); }
            catch (Exception)
            {
                MessageBox.Show(
                    Tr("msgMantenimientoErrorVerificar", "No se pudo verificar la integridad antes de abrir mantenimiento."),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool restauracionEjecutada;
            using (var formMant = new FormMantenimiento(resultado))
            {
                formMant.ShowDialog(this);
                restauracionEjecutada = formMant.RestauracionEjecutada;
            }

            if (restauracionEjecutada)
            {
                MessageBox.Show(
                    Tr("msgMantenimientoLogoutForzado",
                       "Se realizó una restauración de la base de datos. Por seguridad la sesión se cerrará y volverás al login."),
                    Tr("msgMantenimientoLogoutForzadoTitulo", "Sesión cerrada"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CerrarSesionYVolverAlLogin();
            }
        }

        private void CerrarSesionYVolverAlLogin()
        {
            try { _usuarioService.Logout(); } catch { /* la sesión puede haber quedado inválida tras el restore */ }
            SesionUsuario.GetInstancia().Usuario = null;
            _loggingOut = true;
            var login = new FormLogin();
            login.Show();
            this.Close();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                Tr("msgConfirmarLogout", "¿Querés cerrar sesión?"),
                Tr("msgConfirmarLogoutTitulo", "Confirmar cierre de sesión"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            _usuarioService.Logout();
            _loggingOut = true;
            var login = new FormLogin();
            login.Show();
            this.Close();
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_loggingOut) return;
            if (e.CloseReason != CloseReason.UserClosing) return;

            var confirm = MessageBox.Show(
                Tr("msgConfirmarLogout", "¿Querés cerrar sesión?"),
                Tr("msgConfirmarLogoutTitulo", "Confirmar cierre de sesión"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            _usuarioService.Logout();
            _loggingOut = true;
            var login = new FormLogin();
            login.Show();
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            _idiomaService.Desuscribir(this);
            if (!_loggingOut)
            {
                _usuarioService.Logout();
                Application.Exit();
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_RESTORE   = 0xF120;
            const int SC_SIZE      = 0xF000;
            const int SC_MOVE      = 0xF010;

            if (m.Msg == WM_SYSCOMMAND)
            {
                int cmd = m.WParam.ToInt32() & 0xFFF0;
                if (cmd == SC_RESTORE && this.WindowState != FormWindowState.Minimized)
                    return;
                if (cmd == SC_SIZE || cmd == SC_MOVE)
                    return;
            }
            base.WndProc(ref m);
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
