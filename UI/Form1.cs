using BE;
using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        private readonly IUsuarioService _usuarioService;
        private bool _integridadOk;

        public Form1()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = new Size(screen.Width / 2, screen.Height / 2);
            CentrarCard();
            VerificarIntegridad();
        }

        private void VerificarIntegridad()
        {
            try
            {
                ResultadoIntegridad resultado = _usuarioService.VerificarIntegridad();
                _integridadOk = resultado.Ok;
                if (_integridadOk) return;

                MessageBox.Show("No se permitirán inicios de sesión hasta restaurar la integridad. Contacte al administrador."
                    , "Error de integridad de la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                _integridadOk = false;
                MessageBox.Show("No se pudo verificar la integridad de la base de datos. Contacte al administrador.",
                    "Error de integridad de la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CentrarCard()
        {
            pnlCard.Location = new Point(
                (ClientSize.Width  - pnlCard.Width)  / 2,
                (ClientSize.Height - pnlCard.Height) / 2);
        }

        private void Form1_Resize(object sender, EventArgs e) => CentrarCard();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!_integridadOk)
            {
                MessageBox.Show("Login deshabilitado por inconsistencias en la base de datos.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Ingresá usuario y contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                LoginResultado resultado = _usuarioService.Login(username, password);
                switch (resultado)
                {
                    case LoginResultado.Exitoso:
                        var principal = new FormPrincipal(username);
                        principal.Show();
                        this.Hide();
                        break;

                    case LoginResultado.UsuarioInexistente:
                    case LoginResultado.CredencialesInvalidas:
                        MessageBox.Show("Usuario o contraseña incorrectos.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case LoginResultado.UsuarioBloqueado:
                        MessageBox.Show("El usuario se encuentra bloqueado. Contacte al administrador.",
                            "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case LoginResultado.Bloqueado:
                        MessageBox.Show("Su cuenta fue bloqueada por superar la cantidad máxima de intentos. Contacte al administrador.",
                            "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión. Por favor, reintente más tarde.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
