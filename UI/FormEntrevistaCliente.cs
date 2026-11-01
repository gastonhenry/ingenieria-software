using BE;
using BE.Enums;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    // N03 — CU-CAP-01: Registrar entrevista de cliente potencial.
    public partial class FormEntrevistaCliente : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormEntrevistaCliente";

        private readonly ICaptacionService _captacionService;
        private readonly IPersonaService _personaService;
        private readonly IMarcaService _marcaService;
        private readonly IModeloService _modeloService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;
        private List<UnidadEnVenta> _coincidentes = new List<UnidadEnVenta>();
        // Cliente de la entrevista recién registrada: el listado de abajo solo vale para él.
        private int? _idClienteCoincidentes;

        public FormEntrevistaCliente()
        {
            InitializeComponent();
            _captacionService = new CaptacionService();
            _personaService = new PersonaService();
            _marcaService = new MarcaService();
            _modeloService = new ModeloService();
            _idiomaService = new IdiomaService();

            txtDocBusqueda.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { btnBuscar_Click(s, e); e.SuppressKeyPress = true; } };

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => { CargarMarcas(); Limpiar(); AjustarLayout(); };
            this.Resize += (s, e) => AjustarLayout();
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
            this.Text                   = Tr("title",                  "Registrar entrevista");
            lblTitulo.Text              = Tr("lblTitulo",              "Registrar entrevista de cliente potencial");
            grpCliente.Text             = Tr("grpCliente",             "Cliente");
            lblDocBusqueda.Text         = Tr("lblDocBusqueda",         "Cliente (DNI/CUIT o nombre):");
            btnBuscar.Text              = Tr("btnBuscar",              "Buscar");
            btnPersonas.Text            = Tr("btnPersonas",            "Personas...");
            lblCoincidencias.Text       = Tr("lblCoincidencias",       "Coincidencias:");
            grpBusqueda.Text            = Tr("grpBusqueda",            "Qué busca");
            lblMarca.Text               = Tr("lblMarca",               "Marcas:");
            lblModelo.Text              = Tr("lblModelo",              "Modelos:");
            lblCarroceria.Text          = Tr("lblCarroceria",          "Carrocerías:");
            lblAyudaCriterios.Text      = Tr("lblAyudaCriterios",      "Sin tildar = cualquiera. Los modelos tildados limitan su marca a esos modelos.");
            lblAnioDesde.Text           = Tr("lblAnioDesde",           "Año desde:");
            lblAnioHasta.Text           = Tr("lblAnioHasta",           "Año hasta:");
            lblPrecioMax.Text           = Tr("lblPrecioMax",           "Precio máximo ($):");
            lblObservaciones.Text       = Tr("lblObservaciones",       "Observaciones:");
            chkAceptaAvisos.Text        = Tr("chkAceptaAvisos",        "El cliente acepta recibir avisos por correo electrónico");
            btnRegistrar.Text           = Tr("btnRegistrar",           "Registrar");
            btnLimpiar.Text             = Tr("btnLimpiar",             "Limpiar");
            lblUnidadesCoincidentes.Text= Tr("lblUnidadesCoincidentes","Unidades en venta que coinciden con la búsqueda");
            colUnidad.HeaderText        = Tr("colUnidad",              "Unidad");
            colCarroceria.HeaderText    = Tr("colCarroceria",          "Carrocería");
            colKm.HeaderText            = Tr("colKm",                  "Km");
            colPrecio.HeaderText        = Tr("colPrecio",              "Precio");
            btnExportarPdf.Text         = Tr("btnExportarPdf",         "Exportar PDF");

            RecargarCarrocerias();
            ActualizarEmailCliente();
            MostrarCoincidentes();
        }

        // ------------------------------------------------------------
        // Criterios multiselección (marcas, modelos, carrocerías)
        // ------------------------------------------------------------

        private List<Marca> _marcas = new List<Marca>();
        private readonly HashSet<int> _marcasTildadas = new HashSet<int>();
        private readonly HashSet<int> _modelosTildados = new HashSet<int>();
        private readonly Dictionary<int, List<Modelo>> _modelosPorMarca = new Dictionary<int, List<Modelo>>();
        private bool _cargandoListas;

        private void CargarMarcas()
        {
            try
            {
                _marcas = _marcaService.Listar();
                FiltrarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Muestra las marcas que contienen el texto buscado; las tildadas siguen tildadas aunque se oculten.
        private void FiltrarMarcas()
        {
            string filtro = txtBuscarMarca.Text.Trim();
            _cargandoListas = true;
            clbMarcas.BeginUpdate();
            clbMarcas.Items.Clear();
            foreach (var m in _marcas)
            {
                if (filtro.Length > 0 && m.Nombre.IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) < 0) continue;
                clbMarcas.Items.Add(new OpcionItem(m, m.Nombre), _marcasTildadas.Contains(m.Id));
            }
            clbMarcas.EndUpdate();
            _cargandoListas = false;
        }

        private void txtBuscarMarca_TextChanged(object sender, EventArgs e) => FiltrarMarcas();

        private void clbMarcas_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_cargandoListas) return;
            var marca = (Marca)((OpcionItem)clbMarcas.Items[e.Index]).Valor;
            if (e.NewValue == CheckState.Checked) _marcasTildadas.Add(marca.Id);
            else
            {
                _marcasTildadas.Remove(marca.Id);
                // Al destildar la marca se descartan sus modelos tildados.
                if (_modelosPorMarca.TryGetValue(marca.Id, out var mods))
                    foreach (var mo in mods) _modelosTildados.Remove(mo.Id);
            }
            RecargarModelos();
        }

        // Lista los modelos de las marcas tildadas ("Marca Modelo"), manteniendo los tildados.
        private void RecargarModelos()
        {
            _cargandoListas = true;
            clbModelos.BeginUpdate();
            clbModelos.Items.Clear();
            try
            {
                foreach (var marca in _marcas.Where(m => _marcasTildadas.Contains(m.Id)))
                {
                    if (!_modelosPorMarca.TryGetValue(marca.Id, out var modelos))
                    {
                        modelos = _modeloService.ListarPorMarca(marca.Id);
                        foreach (var mo in modelos) mo.Marca = marca;
                        _modelosPorMarca[marca.Id] = modelos;
                    }
                    foreach (var mo in modelos)
                        clbModelos.Items.Add(new OpcionItem(mo, $"{marca.Nombre} {mo.Nombre}"), _modelosTildados.Contains(mo.Id));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                clbModelos.EndUpdate();
                _cargandoListas = false;
            }
        }

        private void clbModelos_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_cargandoListas) return;
            var modelo = (Modelo)((OpcionItem)clbModelos.Items[e.Index]).Valor;
            if (e.NewValue == CheckState.Checked) _modelosTildados.Add(modelo.Id);
            else _modelosTildados.Remove(modelo.Id);
        }

        // Carrocerías traducidas; conserva las tildadas al cambiar de idioma.
        private void RecargarCarrocerias()
        {
            var tildadas = CarroceriasTildadas();
            clbCarrocerias.Items.Clear();
            foreach (TipoCarroceria t in Enum.GetValues(typeof(TipoCarroceria)))
                clbCarrocerias.Items.Add(new OpcionItem(t, t.GetDescripcionTraducida()), tildadas.Contains(t));
        }

        private List<TipoCarroceria> CarroceriasTildadas() =>
            clbCarrocerias.CheckedItems.Cast<OpcionItem>().Select(i => (TipoCarroceria)i.Valor).ToList();

        private void LimpiarCriterios()
        {
            _marcasTildadas.Clear();
            _modelosTildados.Clear();
            txtBuscarMarca.Clear();
            FiltrarMarcas();
            RecargarModelos();
            for (int i = 0; i < clbCarrocerias.Items.Count; i++) clbCarrocerias.SetItemChecked(i, false);
        }

        // ------------------------------------------------------------
        // Cliente
        // ------------------------------------------------------------

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string frag = txtDocBusqueda.Text.Trim();
                lstCoincidencias.Items.Clear();
                ActualizarEmailCliente();
                if (string.IsNullOrEmpty(frag)) return;
                List<Persona> rs = _personaService.BuscarPorDocumentoONombre(frag);
                if (rs.Count == 0)
                {
                    // 4.1.1: la persona no existe → se da de alta desde "Personas...".
                    MessageBox.Show(Tr("msgSinCoincidencias", "No se encontraron personas con ese DNI/CUIT o nombre. Podés darla de alta desde \"Personas...\"."),
                        Tr("msgInformacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (Persona p in rs) lstCoincidencias.Items.Add(new PersonaItem(p, Tr));
                lstCoincidencias.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPersonas_Click(object sender, EventArgs e)
        {
            using (var f = new FormRegistrarPersona())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            if (!string.IsNullOrWhiteSpace(txtDocBusqueda.Text)) btnBuscar_Click(sender, e);
        }

        private void lstCoincidencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarEmailCliente();
            // Si se elige otro cliente, las unidades coincidentes de la entrevista anterior ya no aplican.
            var p = (lstCoincidencias.SelectedItem as PersonaItem)?.Persona;
            if (_idClienteCoincidentes.HasValue && p?.Id != _idClienteCoincidentes)
            {
                _idClienteCoincidentes = null;
                _coincidentes = new List<UnidadEnVenta>();
                MostrarCoincidentes();
            }
        }

        private void ActualizarEmailCliente()
        {
            var p = (lstCoincidencias.SelectedItem as PersonaItem)?.Persona;
            if (p == null) { lblEmailCliente.Text = ""; return; }
            lblEmailCliente.Text = string.IsNullOrWhiteSpace(p.Email)
                ? Tr("lblSinEmail", "Sin correo electrónico registrado: no podrá recibir avisos.")
                : string.Format(Tr("lblEmail", "Correo: {0}"), p.Email);
            lblEmailCliente.ForeColor = string.IsNullOrWhiteSpace(p.Email)
                ? System.Drawing.Color.FromArgb(190, 90, 20)
                : System.Drawing.Color.FromArgb(90, 90, 100);
        }

        // ------------------------------------------------------------
        // Registrar
        // ------------------------------------------------------------

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            var persona = (lstCoincidencias.SelectedItem as PersonaItem)?.Persona;
            if (persona == null)
            {
                MessageBox.Show(Tr("msgSeleccionarCliente", "Buscá y seleccioná el cliente."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!LeerEnteroOpcional(txtAnioDesde, out int? anioDesde) || !LeerEnteroOpcional(txtAnioHasta, out int? anioHasta))
            {
                MessageBox.Show(Tr("msgAnioInvalido", "El año debe ser un número de 4 dígitos."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal? precioMax = null;
            if (!string.IsNullOrWhiteSpace(txtPrecioMax.Text))
            {
                if (!decimal.TryParse(txtPrecioMax.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal pm))
                {
                    MessageBox.Show(Tr("msgPrecioInvalido", "El precio máximo no es válido."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                precioMax = pm;
            }

            try
            {
                int idBusqueda = _captacionService.RegistrarEntrevista(persona.Id,
                    _marcasTildadas.ToList(), _modelosTildados.ToList(), CarroceriasTildadas(),
                    anioDesde, anioHasta, precioMax, txtObservaciones.Text, chkAceptaAvisos.Checked);

                _coincidentes = _captacionService.ListarUnidadesCoincidentes(idBusqueda);
                _idClienteCoincidentes = persona.Id;
                MostrarCoincidentes();

                string msg = Tr("msgEntrevistaRegistrada", "Entrevista registrada.");
                msg += "\n" + (_coincidentes.Count == 0
                    ? Tr("msgSinUnidadesCoincidentes", "Por el momento no hay unidades en venta que coincidan.")
                    : string.Format(Tr("msgUnidadesCoincidentes", "Hay {0} unidad(es) en venta que coinciden: se muestran abajo."), _coincidentes.Count));
                MessageBox.Show(msg, Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarCoincidentes()
        {
            dgvCoincidentes.Rows.Clear();
            foreach (var u in _coincidentes)
            {
                int i = dgvCoincidentes.Rows.Add(u.ToString(), u.TipoCarroceria.GetDescripcionTraducida(),
                    u.Kilometraje.ToString("N0", CultureInfo.CurrentCulture),
                    u.PrecioPublicacion.HasValue ? u.PrecioPublicacion.Value.ToString("C0", CultureInfo.CurrentCulture)
                                                 : Tr("precioAConsultar", "Precio a consultar"));
                dgvCoincidentes.Rows[i].Tag = u;
            }
            btnExportarPdf.Enabled = _coincidentes.Count > 0;
        }

        // Doble clic en una unidad coincidente: abre su publicación (encima, sin perder la entrevista).
        private void dgvCoincidentes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var u = dgvCoincidentes.Rows[e.RowIndex].Tag as UnidadEnVenta;
            if (u == null) return;
            using (var f = new FormPublicaciones(u.IdUnidad))
            {
                var area = Screen.FromControl(this).WorkingArea;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new System.Drawing.Size((int)(area.Width * 0.9), (int)(area.Height * 0.9));
                f.ShowDialog(this);
            }
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            var u = dgvCoincidentes.CurrentRow?.Tag as UnidadEnVenta;
            if (u == null) return;
            ExportadorPdfPublicacion.Exportar(this, u.IdUnidad);
        }

        // ------------------------------------------------------------
        // Layout: Cliente | Qué busca (mitad y mitad) y la grilla de coincidencias a todo el ancho.
        // Se calcula sobre los márgenes del diseño, así respeta la escala de pantalla.
        // ------------------------------------------------------------

        private bool _ajustando;

        private void AjustarLayout()
        {
            if (_ajustando || WindowState == FormWindowState.Minimized) return;
            _ajustando = true;
            SuspendLayout();
            try
            {
                int margen = grpCliente.Left;
                int separacion = margen;
                int anchoUtil = Math.Max(ClientSize.Width - margen * 2, 900);
                int anchoGrupo = (anchoUtil - separacion) / 2;

                // Grupos de arriba
                grpCliente.Width = anchoGrupo;
                grpBusqueda.Left = grpCliente.Right + separacion;
                grpBusqueda.Width = anchoGrupo;

                // Dentro de "Cliente"
                int interior = txtDocBusqueda.Left;
                lstCoincidencias.Width = grpCliente.Width - interior * 2;
                lblEmailCliente.Width = lstCoincidencias.Width;
                btnPersonas.Left = grpCliente.Width - interior - btnPersonas.Width;
                btnBuscar.Left = btnPersonas.Left - 10 - btnBuscar.Width;
                txtDocBusqueda.Width = btnBuscar.Left - 10 - txtDocBusqueda.Left;

                // Dentro de "Qué busca": las tres listas se reparten el ancho
                int x0 = clbMarcas.Left;
                int gap = 10;
                int anchoTotal = grpBusqueda.Width - x0 * 2;
                int anchoLista = (anchoTotal - gap * 2) / 3;
                txtBuscarMarca.Width = clbMarcas.Width = anchoLista;
                lblModelo.Left = clbModelos.Left = x0 + anchoLista + gap;
                clbModelos.Width = anchoLista;
                lblCarroceria.Left = clbCarrocerias.Left = clbModelos.Right + gap;
                clbCarrocerias.Width = grpBusqueda.Width - x0 - clbCarrocerias.Left;
                lblAyudaCriterios.Width = anchoTotal;

                // Años (ancho fijo) y precio hasta el borde
                lblPrecioMax.Left = txtPrecioMax.Left = txtAnioHasta.Right + 15;
                txtPrecioMax.Width = grpBusqueda.Width - x0 - txtPrecioMax.Left;
                txtObservaciones.Width = anchoTotal;
                chkAceptaAvisos.Width = anchoTotal;

                // Botones alineados al borde derecho de "Qué busca"
                btnLimpiar.Left = grpBusqueda.Right - btnLimpiar.Width;
                btnRegistrar.Left = btnLimpiar.Left - 10 - btnRegistrar.Width;

                // Grilla de coincidencias: todo el ancho y hasta abajo
                btnExportarPdf.Left = grpBusqueda.Right - btnExportarPdf.Width;
                dgvCoincidentes.Width = btnExportarPdf.Left - 10 - dgvCoincidentes.Left;
                dgvCoincidentes.Height = Math.Max(ClientSize.Height - dgvCoincidentes.Top - margen, 150);
            }
            finally
            {
                ResumeLayout();
                _ajustando = false;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => Limpiar();

        private void Limpiar()
        {
            txtDocBusqueda.Clear();
            lstCoincidencias.Items.Clear();
            LimpiarCriterios();
            txtAnioDesde.Clear();
            txtAnioHasta.Clear();
            txtPrecioMax.Clear();
            txtObservaciones.Clear();
            chkAceptaAvisos.Checked = true;
            _coincidentes = new List<UnidadEnVenta>();
            MostrarCoincidentes();
            ActualizarEmailCliente();
            txtDocBusqueda.Focus();
        }

        private static bool LeerEnteroOpcional(TextBox txt, out int? valor)
        {
            valor = null;
            if (string.IsNullOrWhiteSpace(txt.Text)) return true;
            if (!int.TryParse(txt.Text.Trim(), out int v)) return false;
            valor = v;
            return true;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }

        private class OpcionItem
        {
            public object Valor { get; }
            private readonly string _texto;
            public OpcionItem(object valor, string texto) { Valor = valor; _texto = texto; }
            public override string ToString() => _texto;
        }

        private class PersonaItem
        {
            private readonly Func<string, string, string> _tr;
            public Persona Persona { get; }
            public PersonaItem(Persona p, Func<string, string, string> tr) { Persona = p; _tr = tr; }
            public override string ToString() =>
                $"{Persona.Documento} — {Persona.Nombre} ({(Persona.TipoPersona == 'J' ? _tr("tipoJuridica", "Jurídica") : _tr("tipoFisica", "Física"))})";
        }
    }
}
