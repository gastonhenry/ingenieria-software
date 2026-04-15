using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        private readonly IUsuarioService _usuarioService;

        public Form1()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Ingresá usuario y contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool ok = _usuarioService.Login(username, password);
                if (ok)
                {
                    var principal = new FormPrincipal();
                    principal.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
