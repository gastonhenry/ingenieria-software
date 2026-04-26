using BLL;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPrincipal : Form
    {
        private readonly IUsuarioService _usuarioService;
        private string _username;
        private bool _loggingOut = false;

        public FormPrincipal(string username)
        {
            _username = username;
            _usuarioService = new UsuarioService();
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new NavBarColorTable());
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = new Size(screen.Width / 2, screen.Height / 2);
            bool esAdmin = _usuarioService.EsAdmin();
            menuUsuarios.Visible = esAdmin;
            menuBitacora.Visible = esAdmin;
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
            foreach (Form child in this.MdiChildren.ToList())
                child.Close();
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

        private void menuVerBitacora_Click(object sender, EventArgs e) =>
            Navegar(new FormBitacora());

        private void menuLogout_Click(object sender, EventArgs e)
        {
            _usuarioService.Logout();
            _loggingOut = true;
            var login = new Form1();
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
    }
}
