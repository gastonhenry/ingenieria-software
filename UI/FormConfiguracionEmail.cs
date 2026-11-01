using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    // Mantenimiento → Configurar cuenta email (Admin y Gerente): cuenta SMTP desde la que salen los avisos.
    public partial class FormConfiguracionEmail : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormConfiguracionEmail";

        private readonly IConfiguracionEmailService _configuracionService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormConfiguracionEmail()
        {
            InitializeComponent();
            _configuracionService = new ConfiguracionEmailService();
            _idiomaService = new IdiomaService();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => Cargar();
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
            this.Text                = Tr("title",               "Configurar cuenta email");
            lblTitulo.Text           = Tr("lblTitulo",           "Configurar cuenta email");
            lblHost.Text             = Tr("lblHost",             "Servidor SMTP:");
            lblPuerto.Text           = Tr("lblPuerto",           "Puerto:");
            chkSsl.Text              = Tr("chkSsl",              "Usar conexión segura (SSL/TLS)");
            lblUsuario.Text          = Tr("lblUsuario",          "Usuario:");
            lblPassword.Text         = Tr("lblPassword",         "Contraseña:");
            chkMostrarPassword.Text  = Tr("chkMostrarPassword",  "Mostrar");
            lblRemitente.Text        = Tr("lblRemitente",        "Email remitente:");
            lblTimeout.Text          = Tr("lblTimeout",          "Tiempo de espera (ms):");
            lblAyuda.Text            = Tr("lblAyuda",            "Para Gmail: servidor smtp.gmail.com, puerto 587, conexión segura y una contraseña de aplicación.");
            lblDestinoPrueba.Text    = Tr("lblDestinoPrueba",    "Enviar prueba a:");
            btnEnviarPrueba.Text     = Tr("btnEnviarPrueba",     "Enviar correo de prueba");
            btnGuardar.Text          = Tr("btnGuardar",          "Guardar");
            btnCancelar.Text         = Tr("btnCancelar",         "Cancelar");
        }

        private void Cargar()
        {
            try
            {
                var c = _configuracionService.Obtener();
                txtHost.Text = c.Host;
                numPuerto.Value = Math.Max(numPuerto.Minimum, Math.Min(numPuerto.Maximum, c.Puerto));
                chkSsl.Checked = c.Ssl;
                txtUsuario.Text = c.Usuario;
                txtPassword.Text = c.Password;
                txtRemitente.Text = c.Remitente;
                numTimeout.Value = Math.Max(numTimeout.Minimum, Math.Min(numTimeout.Maximum, c.TimeoutMs));
                txtDestinoPrueba.Text = c.Remitente;
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ConfiguracionEmail LeerPantalla() => new ConfiguracionEmail
        {
            Host      = txtHost.Text,
            Puerto    = (int)numPuerto.Value,
            Ssl       = chkSsl.Checked,
            Usuario   = txtUsuario.Text,
            Password  = txtPassword.Text,
            TimeoutMs = (int)numTimeout.Value,
            Remitente = txtRemitente.Text
        };

        private void chkMostrarPassword_CheckedChanged(object sender, EventArgs e) =>
            txtPassword.UseSystemPasswordChar = !chkMostrarPassword.Checked;

        private void btnEnviarPrueba_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _configuracionService.EnviarPrueba(LeerPantalla(), txtDestinoPrueba.Text);
                Cursor = Cursors.Default;
                MessageBox.Show(string.Format(Tr("msgPruebaEnviada", "Correo de prueba enviado a {0}."), txtDestinoPrueba.Text.Trim()),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                _configuracionService.Actualizar(LeerPantalla());
                MessageBox.Show(Tr("msgGuardado", "Cuenta de email guardada."), Tr("msgExito", "Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
