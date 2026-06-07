using BE;
using BLL;
using HELPERS;
using System;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormEditarUsuario : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormEditarUsuario";

        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private readonly int _usuarioId;
        private Usuario _usuario;
        private bool _suscrito;
        private bool _guardado;

        public bool Guardado => _guardado;

        public FormEditarUsuario(int usuarioId)
        {
            InitializeComponent();
            _usuarioId = usuarioId;
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "EDITAR_USUARIO");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para editar usuarios."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            _usuario = _usuarioService.ObtenerPorId(_usuarioId);
            if (_usuario == null)
            {
                MessageBox.Show(Tr("msgUsuarioNoEncontrado", "No se encontró el usuario."),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Load += (s, e) => this.Close();
                return;
            }

            PoblarCampos();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());
        }

        private void PoblarCampos()
        {
            lblUsernameValor.Text = _usuario.Username  ?? string.Empty;
            txtNombre.Text        = _usuario.Nombre    ?? string.Empty;
            txtApellido.Text      = _usuario.Apellido  ?? string.Empty;
            txtEmail.Text         = _usuario.Email     ?? string.Empty;
            txtTelefono.Text      = _usuario.Telefono  ?? string.Empty;
            txtDocumento.Text     = _usuario.Documento ?? string.Empty;
            txtDomicilio.Text     = _usuario.Domicilio ?? string.Empty;

            lblIdiomaValor.Text = NombreIdiomaPreferido();
        }

        private string NombreIdiomaPreferido()
        {
            if (!_usuario.IdIdioma.HasValue) return Tr("idiomaSinSeleccion", "— (sin seleccionar)");
            try
            {
                var idioma = _idiomaService.Listar().FirstOrDefault(i => i.Id == _usuario.IdIdioma.Value);
                return idioma != null ? idioma.Nombre : Tr("idiomaSinSeleccion", "— (sin seleccionar)");
            }
            catch { return Tr("idiomaSinSeleccion", "— (sin seleccionar)"); }
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
            lblTitulo.Text    = Tr("lblTitulo",    "Editar Usuario");
            lblUsername.Text  = Tr("lblUsername",  "Usuario:");
            lblNombre.Text    = Tr("lblNombre",    "Nombre:");
            lblApellido.Text  = Tr("lblApellido",  "Apellido:");
            lblEmail.Text     = Tr("lblEmail",     "Email:");
            lblTelefono.Text  = Tr("lblTelefono",  "Teléfono:");
            lblDocumento.Text = Tr("lblDocumento", "Documento:");
            lblDomicilio.Text = Tr("lblDomicilio", "Domicilio:");
            lblIdioma.Text    = Tr("lblIdioma",    "Idioma preferido:");
            btnGuardar.Text   = Tr("btnGuardar",   "Guardar cambios");
            this.Text         = Tr("title",        "Editar Usuario");

            // Refrescar el "sin seleccionar" si aplica
            if (_usuario != null && !_usuario.IdIdioma.HasValue)
                lblIdiomaValor.Text = Tr("idiomaSinSeleccion", "— (sin seleccionar)");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_usuario == null) return;

            string nombre    = txtNombre.Text.Trim();
            string apellido  = txtApellido.Text.Trim();
            string email     = txtEmail.Text.Trim();
            string telefono  = txtTelefono.Text.Trim();
            string documento = txtDocumento.Text.Trim();
            string domicilio = txtDomicilio.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido)
                || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telefono)
                || string.IsNullOrWhiteSpace(documento) || string.IsNullOrWhiteSpace(domicilio))
            {
                MessageBox.Show(Tr("msgCamposVacios", "Completá todos los campos."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _usuario.Nombre    = nombre;
                _usuario.Apellido  = apellido;
                _usuario.Email     = email;
                _usuario.Telefono  = telefono;
                _usuario.Documento = documento;
                _usuario.Domicilio = domicilio;

                bool cambio = _usuarioService.Editar(_usuario);
                if (!cambio)
                {
                    MessageBox.Show(Tr("msgSinCambios", "No hay cambios para guardar."),
                        Tr("msgInformacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _guardado = true;
                MessageBox.Show(Tr("msgUsuarioEditado", "Usuario editado correctamente."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
