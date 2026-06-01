using BLL;
using HELPERS;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPrincipal : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private string _username;
        private bool _loggingOut = false;

        public FormPrincipal(string username)
        {
            _username = username;
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new NavBarColorTable());
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            bool esAdmin = _usuarioService.EsAdmin();
            menuUsuarios.Visible = esAdmin;
            menuPermisos.Visible = esAdmin;
            menuRoles.Visible = esAdmin;
            menuBitacora.Visible = esAdmin || TienePermiso("VER_BITACORA");
            lblSesion.Text = "Sesión: " + _username;
            this.Shown += (s, e) => Navegar(new FormHome());
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

        private void menuGestionRoles_Click(object sender, EventArgs e) =>
            Navegar(new FormRoles());

        private void menuLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "¿Querés cerrar sesión?",
                "Confirmar cierre de sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            _usuarioService.Logout();
            _loggingOut = true;
            var login = new FormLogin();
            login.Show();
            this.Close();
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
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
