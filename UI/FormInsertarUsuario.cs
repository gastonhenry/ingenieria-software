using BE;
using BLL;
using HELPERS;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormInsertarUsuario : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormInsertarUsuario";

        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormInsertarUsuario()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "REGISTRAR_USUARIO");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para acceder a esta sección."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

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
            lblTitulo.Text            = Tr("lblTitulo",            "Registrar Usuario");
            lblUsername.Text          = Tr("lblUsername",          "Usuario:");
            lblPassword.Text          = Tr("lblPassword",          "Contraseña:");
            lblConfirmarPassword.Text = Tr("lblConfirmarPassword", "Repetir contraseña:");
            lblNombre.Text            = Tr("lblNombre",            "Nombre:");
            lblApellido.Text          = Tr("lblApellido",          "Apellido:");
            lblEmail.Text             = Tr("lblEmail",             "Email:");
            lblTelefono.Text          = Tr("lblTelefono",          "Teléfono:");
            lblDocumento.Text         = Tr("lblDocumento",         "Documento:");
            lblDomicilio.Text         = Tr("lblDomicilio",         "Domicilio:");
            btnRegistrar.Text         = Tr("btnRegistrar",         "Registrar");
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmar = txtConfirmarPassword.Text;
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string documento = txtDocumento.Text.Trim();
            string domicilio = txtDomicilio.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmar)
                || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(documento) || string.IsNullOrWhiteSpace(domicilio))
            {
                MessageBox.Show(Tr("msgCamposVacios", "Completá todos los campos."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmar)
            {
                MessageBox.Show(Tr("msgPasswordsDistintos", "Las contraseñas no coinciden."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarPassword.Focus();
                txtConfirmarPassword.SelectAll();
                return;
            }

            try
            {
                var usuario = new Usuario
                {
                    Username  = username,
                    Password  = password,
                    Nombre    = nombre,
                    Apellido  = apellido,
                    Email     = email,
                    Telefono  = telefono,
                    Documento = documento,
                    Domicilio = domicilio
                };

                bool ok = _usuarioService.Registro(usuario);
                if (ok)
                {
                    MessageBox.Show(Tr("msgUsuarioRegistrado", "Usuario registrado correctamente."),
                        Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmarPassword.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtDocumento.Clear();
            txtDomicilio.Clear();
            txtUsername.Focus();
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
