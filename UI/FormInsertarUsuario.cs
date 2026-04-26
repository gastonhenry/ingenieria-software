using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormInsertarUsuario : Form
    {
        private readonly IUsuarioService _usuarioService;

        public FormInsertarUsuario()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();

            if (!_usuarioService.EsAdmin())
            {
                MessageBox.Show("Solo el administrador puede acceder a esta sección.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var usuario = new Usuario
                {
                    Username = username,
                    Password = password,
                    Nombre = nombre,
                    Apellido = apellido
                };

                bool ok = _usuarioService.Registro(usuario);
                if (ok)
                {
                    MessageBox.Show("Usuario registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtUsername.Focus();
        }
    }
}
