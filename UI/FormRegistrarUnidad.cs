using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRegistrarUnidad : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormRegistrarUnidad";

        private readonly IUnidadService _unidadService;
        private readonly IPersonaService _personaService;
        private readonly IMarcaService _marcaService;
        private readonly IModeloService _modeloService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormRegistrarUnidad()
        {
            InitializeComponent();
            _unidadService = new UnidadService();
            _personaService = new PersonaService();
            _marcaService = new MarcaService();
            _modeloService = new ModeloService();
            _idiomaService = new IdiomaService();

            CargarMarcas();
            cboMarca.SelectedIndexChanged += (s, e) => CargarModelosDeMarcaSeleccionada();
            txtDocBusqueda.TextChanged += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtDocBusqueda.Text))
                    lstCoincidencias.Items.Clear();
            };

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
            this.Text                    = Tr("title",               "Registrar Unidad");
            lblTitulo.Text               = Tr("lblTitulo",           "Registrar Unidad");
            grpUnidad.Text               = Tr("grpUnidad",           "Datos de la unidad");
            lblDominio.Text              = Tr("lblDominio",          "Dominio:");
            lblMarca.Text                = Tr("lblMarca",            "Marca:");
            lblModelo.Text               = Tr("lblModelo",           "Modelo:");
            lblAnio.Text                 = Tr("lblAnio",             "Año:");
            lblKilometraje.Text          = Tr("lblKilometraje",      "Kilometraje:");
            lblPrecioCompra.Text         = Tr("lblPrecioCompra",     "Precio compra:");
            lblDescripcion.Text          = Tr("lblDescripcion",      "Descripción:");
            grpPersona.Text              = Tr("grpPersona",          "Persona vendedora");
            lblDocBusqueda.Text          = Tr("lblDocBusqueda",      "DNI / CUIT:");
            btnBuscarPersona.Text        = Tr("btnBuscarPersona",    "Buscar");
            lblCoincidencias.Text        = Tr("lblCoincidencias",    "Coincidencias:");
            btnRegistrar.Text            = Tr("btnRegistrar",        "Registrar");
        }

        private void CargarMarcas()
        {
            try
            {
                cboMarca.Items.Clear();
                foreach (Marca m in _marcaService.Listar())
                    cboMarca.Items.Add(m);
            }
            catch (Exception) { /* no-op */ }
        }

        private void CargarModelosDeMarcaSeleccionada()
        {
            try
            {
                cboModelo.Items.Clear();
                var marca = cboMarca.SelectedItem as Marca;
                if (marca == null) return;
                foreach (Modelo mo in _modeloService.ListarPorMarca(marca.Id))
                    cboModelo.Items.Add(mo);
            }
            catch (Exception) { /* no-op */ }
        }

        private void btnBuscarPersona_Click(object sender, EventArgs e)
        {
            try
            {
                string frag = txtDocBusqueda.Text.Trim();
                if (string.IsNullOrEmpty(frag))
                {
                    MessageBox.Show(Tr("msgDocVacio", "Ingresá un DNI o CUIT (o parte) para buscar."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lstCoincidencias.Items.Clear();
                List<Persona> resultados = _personaService.BuscarPorDocumentoParcial(frag);
                if (resultados.Count == 0)
                {
                    MessageBox.Show(Tr("msgSinCoincidencias", "No se encontraron personas con ese documento."),
                        Tr("msgInformacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (Persona p in resultados)
                    lstCoincidencias.Items.Add(new PersonaItem(p));

                // Selecciono la primera para que quede listo para registrar si hay una sola.
                if (lstCoincidencias.Items.Count > 0) lstCoincidencias.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                var marcaSel = cboMarca.SelectedItem as Marca;
                if (marcaSel == null)
                { Msg(Tr("msgMarcaInvalida", "Seleccioná una marca."), MessageBoxIcon.Warning); return; }
                var modeloSel = cboModelo.SelectedItem as Modelo;
                if (modeloSel == null)
                { Msg(Tr("msgModeloInvalido", "Seleccioná un modelo."), MessageBoxIcon.Warning); return; }

                var personaItem = lstCoincidencias.SelectedItem as PersonaItem;
                if (personaItem == null)
                { Msg(Tr("msgPersonaInvalida", "Buscá y seleccioná una persona vendedora."), MessageBoxIcon.Warning); return; }

                if (!int.TryParse(txtAnio.Text.Trim(), out int anio) || anio <= 1900 || anio > DateTime.Now.Year + 1)
                { Msg(Tr("msgAnioInvalido", "Año inválido."), MessageBoxIcon.Warning); return; }
                if (!int.TryParse(txtKilometraje.Text.Trim(), out int km) || km < 0)
                { Msg(Tr("msgKmInvalido", "Kilometraje inválido."), MessageBoxIcon.Warning); return; }
                if (!decimal.TryParse(txtPrecioCompra.Text.Trim(), System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.CurrentCulture, out decimal precio) || precio <= 0)
                { Msg(Tr("msgPrecioInvalido", "Precio de compra inválido."), MessageBoxIcon.Warning); return; }

                var unidad = new Unidad
                {
                    Dominio = txtDominio.Text.Trim(),
                    Marca = marcaSel.Nombre,
                    Modelo = modeloSel.Nombre,
                    Anio = anio,
                    Kilometraje = km,
                    PrecioCompra = precio,
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim()
                };

                int id = _unidadService.RegistrarUnidad(unidad, personaItem.Persona.Id);
                MessageBox.Show(string.Format(Tr("msgUnidadRegistrada", "Unidad registrada con Id {0}."), id),
                    Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Msg(string texto, MessageBoxIcon icon)
        {
            MessageBox.Show(texto,
                icon == MessageBoxIcon.Warning ? Tr("msgAdvertencia", "Advertencia") : Tr("msgError", "Error"),
                MessageBoxButtons.OK, icon);
        }

        private void LimpiarCampos()
        {
            txtDominio.Clear();
            cboMarca.SelectedIndex = -1;
            cboModelo.Items.Clear();
            txtAnio.Clear();
            txtKilometraje.Clear();
            txtPrecioCompra.Clear();
            txtDescripcion.Clear();
            txtDocBusqueda.Clear();
            lstCoincidencias.Items.Clear();
            txtDominio.Focus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }

        private class PersonaItem
        {
            public Persona Persona { get; }
            public PersonaItem(Persona p) { Persona = p; }
            public override string ToString() =>
                $"{Persona.Documento} — {Persona.Nombre} ({(Persona.TipoPersona == 'J' ? "Jurídica" : "Física")})";
        }
    }
}
