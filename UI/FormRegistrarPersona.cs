using BE;
using BE.Enums;
using BLL;
using HELPERS;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRegistrarPersona : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormRegistrarPersona";

        private readonly IPersonaService _personaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormRegistrarPersona()
        {
            InitializeComponent();
            _personaService = new PersonaService();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "GESTIONAR_PERSONAS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para dar de alta personas."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            rbFisica.Checked = true;
            CargarEstadoCivil();
            dtpFechaNacimiento.Value = new DateTime(1990, 1, 1);
            CargarPersonas();

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
            this.Text              = Tr("title",              "Registrar Persona");
            lblTitulo.Text         = Tr("lblTitulo",          "Registrar Persona");
            lblTipoPersona.Text    = Tr("lblTipoPersona",     "Tipo:");
            rbFisica.Text          = Tr("rbFisica",           "Física");
            rbJuridica.Text        = Tr("rbJuridica",         "Jurídica");
            lblNombre.Text         = Tr("lblNombre",          "Nombre / R.S.:");
            lblDocumento.Text      = Tr("lblDocumento",       "DNI / CUIT:");
            lblDomicilio.Text      = Tr("lblDomicilio",       "Domicilio:");
            lblTelefono.Text       = Tr("lblTelefono",        "Teléfono:");
            lblEmail.Text          = Tr("lblEmail",           "Email:");
            lblEstadoCivil.Text    = Tr("lblEstadoCivil",     "Estado civil:");
            lblFechaNacimiento.Text= Tr("lblFechaNacimiento", "Fecha de nacimiento:");
            btnDarDeAlta.Text      = Tr("btnDarDeAlta",       "Dar de alta");
            btnEditar.Text         = Tr("btnEditar",          "Editar");
            btnEliminar.Text       = Tr("btnEliminar",        "Eliminar");
            lblExistentes.Text     = Tr("lblExistentes",      "Personas registradas:");
            RecargarItemsEstadoCivil();
        }

        private void CargarEstadoCivil() => RecargarItemsEstadoCivil();

        private void RecargarItemsEstadoCivil()
        {
            var sel = cboEstadoCivil.SelectedItem as EstadoCivilItem;
            cboEstadoCivil.Items.Clear();
            foreach (EstadoCivil ec in Enum.GetValues(typeof(EstadoCivil)))
                cboEstadoCivil.Items.Add(new EstadoCivilItem { Valor = ec, Texto = ec.GetDescripcionTraducida() });
            if (sel != null)
            {
                for (int i = 0; i < cboEstadoCivil.Items.Count; i++)
                    if (((EstadoCivilItem)cboEstadoCivil.Items[i]).Valor == sel.Valor) { cboEstadoCivil.SelectedIndex = i; return; }
            }
            if (cboEstadoCivil.Items.Count > 0) cboEstadoCivil.SelectedIndex = 0;
        }

        private void CargarPersonas()
        {
            try
            {
                lstPersonas.Items.Clear();
                foreach (Persona p in _personaService.Listar())
                    lstPersonas.Items.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var p = lstPersonas.SelectedItem as Persona;
            if (p == null) return;
            rbFisica.Checked = p.TipoPersona == 'F';
            rbJuridica.Checked = p.TipoPersona == 'J';
            txtNombre.Text = p.Nombre;
            txtDocumento.Text = p.Documento;
            txtDomicilio.Text = p.Domicilio ?? "";
            txtTelefono.Text = p.Telefono ?? "";
            txtEmail.Text = p.Email ?? "";
            for (int i = 0; i < cboEstadoCivil.Items.Count; i++)
                if (((EstadoCivilItem)cboEstadoCivil.Items[i]).Valor == p.EstadoCivil) { cboEstadoCivil.SelectedIndex = i; break; }
            dtpFechaNacimiento.Value = p.FechaNacimiento == default(DateTime) ? new DateTime(1990, 1, 1) : p.FechaNacimiento;
        }

        private bool ValidarCampos(out char tipo, out string documento, out string nombre, out string domicilio,
                                   out string telefono, out string email, out EstadoCivil ec, out DateTime fecha)
        {
            tipo = rbJuridica.Checked ? 'J' : 'F';
            documento = txtDocumento.Text.Trim();
            nombre = txtNombre.Text.Trim();
            domicilio = txtDomicilio.Text.Trim();
            telefono = txtTelefono.Text.Trim();
            email = txtEmail.Text.Trim();
            ec = EstadoCivil.Soltero;
            fecha = dtpFechaNacimiento.Value.Date;

            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(domicilio))
            {
                MessageBox.Show(Tr("msgCamposVacios", "Completá nombre, documento y domicilio."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var eci = cboEstadoCivil.SelectedItem as EstadoCivilItem;
            if (eci == null) return false;
            ec = eci.Valor;
            return true;
        }

        private void btnDarDeAlta_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos(out char tipo, out string doc, out string nombre, out string domicilio,
                                   out string tel, out string email, out EstadoCivil ec, out DateTime fecha)) return;

                _personaService.Registrar(tipo, doc, nombre, domicilio, tel, email, ec, fecha);
                MessageBox.Show(Tr("msgPersonaRegistrada", "Persona registrada."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPersonas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                var persona = lstPersonas.SelectedItem as Persona;
                if (persona == null)
                {
                    MessageBox.Show(Tr("msgSeleccionarPersona", "Seleccioná una persona de la lista."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCampos(out char tipo, out string doc, out string nombre, out string domicilio,
                                   out string tel, out string email, out EstadoCivil ec, out DateTime fecha)) return;

                _personaService.Editar(persona.Id, tipo, doc, nombre, domicilio, tel, email, ec, fecha);
                MessageBox.Show(Tr("msgPersonaEditada", "Persona editada correctamente."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarPersonas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var persona = lstPersonas.SelectedItem as Persona;
                if (persona == null)
                {
                    MessageBox.Show(Tr("msgSeleccionarPersona", "Seleccioná una persona de la lista."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    string.Format(Tr("msgConfirmarEliminar", "¿Eliminar la persona '{0}'?"), persona.Nombre),
                    Tr("msgConfirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _personaService.Eliminar(persona.Id);
                MessageBox.Show(Tr("msgPersonaEliminada", "Persona eliminada."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPersonas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDocumento.Clear();
            txtDomicilio.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            rbFisica.Checked = true;
            if (cboEstadoCivil.Items.Count > 0) cboEstadoCivil.SelectedIndex = 0;
            dtpFechaNacimiento.Value = new DateTime(1990, 1, 1);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }

        private class EstadoCivilItem
        {
            public EstadoCivil Valor { get; set; }
            public string Texto { get; set; }
            public override string ToString() => Texto;
        }
    }
}
