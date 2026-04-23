using BLL;
using System;
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
        }

        private void menuInsertarUsuario_Click(object sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is FormInsertarUsuario)
                {
                    child.Activate();
                    return;
                }
            }

            var form = new FormInsertarUsuario();
            form.MdiParent = this;
            form.Show();
        }

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
                Application.Exit();
        }
    }
}
