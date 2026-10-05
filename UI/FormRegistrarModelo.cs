using BE;
using BE.Enums;
using BLL;
using HELPERS;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRegistrarModelo : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormRegistrarModelo";

        private readonly IModeloService _modeloService;
        private readonly IMarcaService _marcaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        private const int FILTRO_TODAS = -1;

        public FormRegistrarModelo()
        {
            InitializeComponent();
            _modeloService = new ModeloService();
            _marcaService = new MarcaService();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(SesionUsuario.GetInstancia().Usuario, "GESTIONAR_MODELOS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para dar de alta modelos."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarMarcasEnCombos();
            CargarTipoCarroceria();
            CargarModelos();

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
            this.Text              = Tr("title",              "Registrar Modelo");
            lblTitulo.Text         = Tr("lblTitulo",          "Registrar Modelo");
            lblMarca.Text          = Tr("lblMarca",           "Marca:");
            lblNombre.Text         = Tr("lblNombre",          "Nombre:");
            lblTipoCarroceria.Text = Tr("lblTipoCarroceria",  "Tipo carrocería:");
            btnDarDeAlta.Text      = Tr("btnDarDeAlta",       "Dar de alta");
            btnEditar.Text         = Tr("btnEditar",          "Editar");
            btnEliminar.Text       = Tr("btnEliminar",        "Eliminar");
            lblExistentes.Text     = Tr("lblExistentes",      "Modelos existentes (filtra por marca):");
            RecargarItemTodasLasMarcas();
            RecargarItemsCarroceria();
        }

        private void RecargarItemTodasLasMarcas()
        {
            // Primer item del combo de filtro = "(todas las marcas)". Mantengo selección.
            var sel = cboMarcaFiltro.SelectedItem;
            cboMarcaFiltro.Items.Clear();
            cboMarcaFiltro.Items.Add(new MarcaFiltroItem { Id = FILTRO_TODAS, Nombre = Tr("itemTodasLasMarcas", "(todas las marcas)") });
            foreach (Marca m in _marcaService.Listar())
                cboMarcaFiltro.Items.Add(new MarcaFiltroItem { Id = m.Id, Nombre = m.Nombre });
            if (sel is MarcaFiltroItem prev)
            {
                for (int i = 0; i < cboMarcaFiltro.Items.Count; i++)
                    if (((MarcaFiltroItem)cboMarcaFiltro.Items[i]).Id == prev.Id) { cboMarcaFiltro.SelectedIndex = i; return; }
            }
            cboMarcaFiltro.SelectedIndex = 0;
        }

        private void RecargarItemsCarroceria()
        {
            var sel = cboTipoCarroceria.SelectedItem as CarroceriaItem;
            cboTipoCarroceria.Items.Clear();
            foreach (TipoCarroceria t in Enum.GetValues(typeof(TipoCarroceria)))
                cboTipoCarroceria.Items.Add(new CarroceriaItem { Valor = t, Texto = t.GetDescripcionTraducida() });
            if (sel != null)
            {
                for (int i = 0; i < cboTipoCarroceria.Items.Count; i++)
                    if (((CarroceriaItem)cboTipoCarroceria.Items[i]).Valor == sel.Valor) { cboTipoCarroceria.SelectedIndex = i; return; }
            }
            if (cboTipoCarroceria.Items.Count > 0) cboTipoCarroceria.SelectedIndex = 0;
        }

        private void CargarMarcasEnCombos()
        {
            try
            {
                cboMarca.Items.Clear();
                foreach (Marca m in _marcaService.Listar()) cboMarca.Items.Add(m);
                RecargarItemTodasLasMarcas();
            }
            catch (Exception) { /* no-op */ }
        }

        private void CargarTipoCarroceria() => RecargarItemsCarroceria();

        private void CargarModelos()
        {
            try
            {
                lstModelos.Items.Clear();
                var filtro = cboMarcaFiltro.SelectedItem as MarcaFiltroItem;
                System.Collections.Generic.List<Modelo> modelos;
                if (filtro == null || filtro.Id == FILTRO_TODAS)
                    modelos = _modeloService.Listar();
                else
                    modelos = _modeloService.ListarPorMarca(filtro.Id);

                foreach (Modelo m in modelos) lstModelos.Items.Add(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboMarcaFiltro_SelectedIndexChanged(object sender, EventArgs e) => CargarModelos();

        private void lstModelos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var modelo = lstModelos.SelectedItem as Modelo;
            if (modelo == null) return;
            txtNombre.Text = modelo.Nombre;
            for (int i = 0; i < cboMarca.Items.Count; i++)
                if (((Marca)cboMarca.Items[i]).Id == modelo.Marca.Id) { cboMarca.SelectedIndex = i; break; }
            for (int i = 0; i < cboTipoCarroceria.Items.Count; i++)
                if (((CarroceriaItem)cboTipoCarroceria.Items[i]).Valor == modelo.TipoCarroceria) { cboTipoCarroceria.SelectedIndex = i; break; }
        }

        private bool ValidarCamposAlta(out string nombre, out Marca marca, out TipoCarroceria tc)
        {
            nombre = txtNombre.Text.Trim();
            marca = cboMarca.SelectedItem as Marca;
            tc = TipoCarroceria.Sedan;

            if (marca == null)
            {
                MessageBox.Show(Tr("msgMarcaVacia", "Seleccioná una marca."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show(Tr("msgNombreVacio", "Ingresá un nombre de modelo."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var ci = cboTipoCarroceria.SelectedItem as CarroceriaItem;
            if (ci == null) return false;
            tc = ci.Valor;
            return true;
        }

        private void btnDarDeAlta_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCamposAlta(out string nombre, out Marca marca, out TipoCarroceria tc)) return;

                _modeloService.Registrar(nombre, marca, tc);
                MessageBox.Show(string.Format(Tr("msgModeloRegistrado", "Modelo '{0}' registrado."), nombre),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                CargarModelos();
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
                var modelo = lstModelos.SelectedItem as Modelo;
                if (modelo == null)
                {
                    MessageBox.Show(Tr("msgSeleccionarModelo", "Seleccioná un modelo de la lista."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCamposAlta(out string nombre, out Marca marca, out TipoCarroceria tc)) return;

                _modeloService.Editar(modelo.Id, nombre, marca, tc);
                MessageBox.Show(Tr("msgModeloEditado", "Modelo editado correctamente."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                CargarModelos();
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
                var modelo = lstModelos.SelectedItem as Modelo;
                if (modelo == null)
                {
                    MessageBox.Show(Tr("msgSeleccionarModelo", "Seleccioná un modelo de la lista."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    string.Format(Tr("msgConfirmarEliminar", "¿Eliminar el modelo '{0}'?"), modelo.Nombre),
                    Tr("msgConfirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _modeloService.Eliminar(modelo.Id);
                MessageBox.Show(Tr("msgModeloEliminado", "Modelo eliminado."),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                CargarModelos();
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

        private class MarcaFiltroItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public override string ToString() => Nombre;
        }

        private class CarroceriaItem
        {
            public TipoCarroceria Valor { get; set; }
            public string Texto { get; set; }
            public override string ToString() => Texto;
        }
    }
}
