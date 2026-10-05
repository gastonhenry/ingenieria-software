using BE;
using BLL;
using HELPERS;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRegistrarMarca : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormRegistrarMarca";

        private readonly IMarcaService _marcaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormRegistrarMarca()
        {
            InitializeComponent();
            _marcaService = new MarcaService();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "GESTIONAR_MARCAS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para dar de alta marcas."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarMarcas();

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
            this.Text         = Tr("title",        "Registrar Marca");
            lblTitulo.Text    = Tr("lblTitulo",    "Registrar Marca");
            lblNombre.Text    = Tr("lblNombre",    "Nombre:");
            btnDarDeAlta.Text = Tr("btnDarDeAlta", "Dar de alta");
            lblExistentes.Text = Tr("lblExistentes", "Marcas existentes:");
            btnEditar.Text    = Tr("btnEditar",    "Editar");
            btnEliminar.Text  = Tr("btnEliminar",  "Eliminar");
        }

        private void lstMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var marca = lstMarcas.SelectedItem as Marca;
            if (marca != null) txtNombre.Text = marca.Nombre;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                var marca = lstMarcas.SelectedItem as Marca;
                if (marca == null)
                {
                    MessageBox.Show(Tr("msgSeleccionarMarca", "Seleccioná una marca de la lista."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nuevoNombre = txtNombre.Text.Trim();
                if (string.IsNullOrEmpty(nuevoNombre))
                {
                    MessageBox.Show(Tr("msgNombreVacio", "Ingresá un nombre de marca."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _marcaService.Editar(marca.Id, nuevoNombre);
                MessageBox.Show(Tr("msgMarcaEditada", "Marca editada correctamente."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                CargarMarcas();
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
                var marca = lstMarcas.SelectedItem as Marca;
                if (marca == null)
                {
                    MessageBox.Show(Tr("msgSeleccionarMarca", "Seleccioná una marca de la lista."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    string.Format(Tr("msgConfirmarEliminar", "¿Eliminar la marca '{0}'?"), marca.Nombre),
                    Tr("msgConfirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _marcaService.Eliminar(marca.Id);
                MessageBox.Show(Tr("msgMarcaEliminada", "Marca eliminada."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                CargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarMarcas()
        {
            try
            {
                lstMarcas.Items.Clear();
                foreach (Marca m in _marcaService.Listar())
                    lstMarcas.Items.Add(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDarDeAlta_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show(Tr("msgNombreVacio", "Ingresá un nombre de marca."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _marcaService.Registrar(nombre);
                MessageBox.Show(string.Format(Tr("msgMarcaRegistrada", "Marca '{0}' registrada."), nombre),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                txtNombre.Focus();
                CargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
