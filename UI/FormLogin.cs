using BE;
using BE.Enums;
using BLL;
using HELPERS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class FormLogin : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMantenimientoService _mantenimientoService;
        private ResultadoIntegridadGlobal _resultadoIntegridad;
        private bool _idiomaEnIngles;

        private static readonly Color ColorTextoActivo   = Color.White;
        private static readonly Color ColorTextoInactivo = Color.FromArgb(180, 210, 255);

        private static readonly Dictionary<string, string> _textosEs = new Dictionary<string, string>
        {
            { "title",                     "" },
            { "lblAppName",                "Bienvenidos" },
            { "lblTitle",                  "Iniciar Sesión" },
            { "lblUsuario",                "Usuario" },
            { "lblPassword",               "Contraseña" },
            { "btnLogin",                  "Ingresar" },
            { "msgLoginDeshabilitado",     "Login deshabilitado por inconsistencias en la base de datos. Sólo está habilitado para usuarios administradores." },
            { "msgAccesoDenegado",         "Acceso denegado" },
            { "msgCamposVacios",           "Ingresá usuario y contraseña." },
            { "msgAdvertencia",            "Advertencia" },
            { "msgCredenciales",           "Usuario o contraseña incorrectos." },
            { "msgError",                  "Error" },
            { "msgUsuarioBloqueado",       "El usuario se encuentra bloqueado. Contacte al administrador." },
            { "msgBloqueado",              "Su cuenta fue bloqueada por superar la cantidad máxima de intentos. Contacte al administrador." },
            { "msgErrorLogin",             "Error al iniciar sesión. Por favor, reintente más tarde." },
            { "msgCredencialesMantenimiento", "Credenciales de mantenimiento inválidas.\n\nRecordá que en modo mantenimiento es la definida en App.config." },
        };

        private static readonly Dictionary<string, string> _textosEn = new Dictionary<string, string>
        {
            { "title",                     "" },
            { "lblAppName",                "Welcome" },
            { "lblTitle",                  "Sign In" },
            { "lblUsuario",                "Username" },
            { "lblPassword",               "Password" },
            { "btnLogin",                  "Sign in" },
            { "msgLoginDeshabilitado",     "Login disabled due to database integrity issues. Only admin users can access." },
            { "msgAccesoDenegado",         "Access denied" },
            { "msgCamposVacios",           "Please enter your username and password." },
            { "msgAdvertencia",            "Warning" },
            { "msgCredenciales",           "Invalid username or password." },
            { "msgError",                  "Error" },
            { "msgUsuarioBloqueado",       "User is locked. Please contact the administrator." },
            { "msgBloqueado",              "Your account has been locked after too many failed attempts. Please contact the administrator." },
            { "msgErrorLogin",             "Login error. Please try again later." },
            { "msgCredencialesMantenimiento", "Invalid maintenance credentials.\n\nRemember that in maintenance mode the password is defined in App.config." },
        };

        private string Tr(string clave) => (_idiomaEnIngles ? _textosEn : _textosEs)[clave];

        public FormLogin() : this(null) { }

        public FormLogin(ResultadoIntegridadGlobal resultadoIntegridad)
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            _mantenimientoService = new MantenimientoService();
            _resultadoIntegridad = resultadoIntegridad ?? VerificarIntegridadSegura();

            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = new Size(screen.Width / 2, screen.Height / 2);
            CentrarCard();

            lblToggleEs.Click += (s, e) => CambiarIdiomaLogin(false);
            lblToggleEn.Click += (s, e) => CambiarIdiomaLogin(true);
            AplicarTextos();
        }

        private void CambiarIdiomaLogin(bool ingles)
        {
            if (_idiomaEnIngles == ingles) return;
            _idiomaEnIngles = ingles;
            AplicarTextos();
        }

        private void AplicarTextos()
        {
            this.Text       = Tr("title");
            lblAppName.Text = Tr("lblAppName");
            lblTitle.Text   = Tr("lblTitle");
            lblUsuario.Text = Tr("lblUsuario");
            lblPassword.Text = Tr("lblPassword");
            btnLogin.Text   = Tr("btnLogin");

            lblToggleEs.ForeColor = _idiomaEnIngles ? ColorTextoInactivo : ColorTextoActivo;
            lblToggleEs.Font      = new Font("Segoe UI", 9F, _idiomaEnIngles ? FontStyle.Regular : FontStyle.Bold);
            lblToggleEn.ForeColor = _idiomaEnIngles ? ColorTextoActivo   : ColorTextoInactivo;
            lblToggleEn.Font      = new Font("Segoe UI", 9F, _idiomaEnIngles ? FontStyle.Bold    : FontStyle.Regular);
        }

        private ResultadoIntegridadGlobal VerificarIntegridadSegura()
        {
            try { return _mantenimientoService.VerificarTodo(); }
            catch
            {
                var fallback = new ResultadoIntegridadGlobal { Ok = false };
                fallback.PorTabla["__startup__"] = new ResultadoIntegridad { Ok = false, DVVInvalido = true };
                return fallback;
            }
        }

        private void CentrarCard()
        {
            pnlCard.Location = new Point(
                (ClientSize.Width  - pnlCard.Width)  / 2,
                (ClientSize.Height - pnlCard.Height) / 2);
        }

        private void FormLogin_Resize(object sender, EventArgs e) => CentrarCard();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(Tr("msgCamposVacios"), Tr("msgAdvertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Reverificar integridad por si la BD fue tampered entre el arranque y este intento de login.
            Cursor cursorPrevio = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                _resultadoIntegridad = VerificarIntegridadSegura();
            }
            finally
            {
                this.Cursor = cursorPrevio;
            }

            if (_resultadoIntegridad != null && !_resultadoIntegridad.Ok)
            {
                IngresarEnModoMantenimiento(username, password);
                return;
            }

            IngresarEnModoNormal(username, password);
        }

        private void IngresarEnModoMantenimiento(string username, string password)
        {
            bool esAdmin = string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase);
            if (!esAdmin)
            {
                MessageBox.Show(Tr("msgLoginDeshabilitado"),
                    Tr("msgAccesoDenegado"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool ok = _mantenimientoService.AutenticarMantenimiento(username, password);
            if (!ok)
            {
                MessageBox.Show(Tr("msgCredencialesMantenimiento"),
                    Tr("msgError"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var formMant = new FormMantenimiento(_resultadoIntegridad))
            {
                this.Hide();
                formMant.ShowDialog();
            }

            // Al cerrar mantenimiento, reverificamos por si quedó OK y el admin quiere ingresar normal.
            try { _resultadoIntegridad = _mantenimientoService.VerificarTodo(); }
            catch { /* dejamos el resultado anterior */ }

            txtPassword.Text = string.Empty;
            this.Show();
        }

        private void IngresarEnModoNormal(string username, string password)
        {
            try
            {
                LoginResultado resultado = _usuarioService.Login(username, password);
                switch (resultado)
                {
                    case LoginResultado.Exitoso:
                        AplicarIdiomaPreferido();
                        var principal = new FormPrincipal(username);
                        principal.Show();
                        this.Hide();
                        break;

                    case LoginResultado.UsuarioInexistente:
                    case LoginResultado.CredencialesInvalidas:
                        MessageBox.Show(Tr("msgCredenciales"),
                            Tr("msgError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case LoginResultado.UsuarioBloqueado:
                        MessageBox.Show(Tr("msgUsuarioBloqueado"),
                            Tr("msgAccesoDenegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case LoginResultado.Bloqueado:
                        MessageBox.Show(Tr("msgBloqueado"),
                            Tr("msgAccesoDenegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Tr("msgErrorLogin"),
                    Tr("msgError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarIdiomaPreferido()
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            if (usuario == null || !usuario.IdIdioma.HasValue) return;

            try { new IdiomaService().CambiarIdiomaActual(usuario.IdIdioma.Value); }
            catch (Exception) { }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnLogin_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                txtUsername.Text = "admin";
                txtPassword.Text = "123456789";
            }
        }
    }
}
