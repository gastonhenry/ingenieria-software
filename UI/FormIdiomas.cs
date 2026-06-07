using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class FormIdiomas : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormIdiomas";

        private readonly IIdiomaService _idiomaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private List<Traduccion> _traduccionesActuales = new List<Traduccion>();
        private bool _suscrito;

        public FormIdiomas()
        {
            InitializeComponent();
            _idiomaService = new IdiomaService();
            _usuarioService = new UsuarioService();
            _permisoService = new PermisoService();

            bool permitido = _usuarioService.EsAdmin()
                || _permisoService.UsuarioTienePermiso(HELPERS.SesionUsuario.GetInstancia().Usuario, "GESTIONAR_IDIOMAS");
            if (!permitido)
            {
                MessageBox.Show(Tr("msgSinPermiso", "No tenés permiso para gestionar idiomas."),
                    Tr("msgAccesoDenegado", "Acceso denegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

            CargarIdiomas();

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
            lblTitulo.Text          = Tr("lblTitulo",          "Gestión de Idiomas");
            lblIdiomas.Text         = Tr("lblIdiomas",         "Idiomas disponibles");
            lblAlerta.Text          = Tr("lblAlerta",          "⚠  Idioma en proceso de creación — completá todas las traducciones para poder activarlo.");
            btnNuevoIdioma.Text     = Tr("btnNuevoIdioma",     "Nuevo Idioma");
            btnEliminarIdioma.Text  = Tr("btnEliminarIdioma",  "Eliminar Idioma");
            btnGuardar.Text         = Tr("btnGuardar",         "Guardar cambios");

            if (colForm != null)   colForm.HeaderText   = Tr("colForm",   "Form");
            if (colCodigo != null) colCodigo.HeaderText = Tr("colCodigo", "Código");
            if (colTexto != null)  colTexto.HeaderText  = Tr("colTexto",  "Texto");

            var idiomaSel = lstIdiomas.SelectedItem as Idioma;
            lblTraducciones.Text = idiomaSel == null
                ? Tr("lblTraducciones", "Traducciones")
                : string.Format(Tr("lblTraduccionesDe", "Traducciones — {0}"), idiomaSel.Nombre);
        }

        private void CargarIdiomas()
        {
            int seleccionIdx = lstIdiomas.SelectedIndex;

            lstIdiomas.Items.Clear();
            try
            {
                foreach (Idioma i in _idiomaService.Listar())
                    lstIdiomas.Items.Add(i);

                if (seleccionIdx >= 0 && seleccionIdx < lstIdiomas.Items.Count)
                    lstIdiomas.SelectedIndex = seleccionIdx;
                else if (lstIdiomas.Items.Count > 0)
                    lstIdiomas.SelectedIndex = 0;
                else
                    CargarTraducciones(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorCargarIdiomas", "Error al cargar idiomas:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTraducciones(lstIdiomas.SelectedItem as Idioma);
        }

        private void CargarTraducciones(Idioma idioma)
        {
            grdTraducciones.Rows.Clear();
            _traduccionesActuales.Clear();

            if (idioma == null)
            {
                lblTraducciones.Text = Tr("lblTraducciones", "Traducciones");
                btnGuardar.Enabled = false;
                pnlAlerta.Visible = false;
                return;
            }

            lblTraducciones.Text = string.Format(Tr("lblTraduccionesDe", "Traducciones — {0}"), idioma.Nombre);
            btnGuardar.Enabled = true;
            pnlAlerta.Visible = !idioma.Completo;

            try
            {
                _traduccionesActuales = _idiomaService.ListarTraduccionesParaEdicion(idioma.Id);
                foreach (Traduccion t in _traduccionesActuales)
                {
                    int idx = grdTraducciones.Rows.Add(
                        t.Control != null ? t.Control.Form   : "",
                        t.Control != null ? t.Control.Codigo : "",
                        t.Texto ?? "");
                    grdTraducciones.Rows[idx].Tag = t;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorCargarTraducciones", "Error al cargar traducciones:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoIdioma_Click(object sender, EventArgs e)
        {
            string nombre = Prompt(Tr("promptNombreIdioma", "Nombre del idioma:"));
            if (string.IsNullOrWhiteSpace(nombre)) return;

            try
            {
                _idiomaService.Crear(nombre);
                CargarIdiomas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Tr("msgErrorPrefix", "Error: {0}"), TrError(ex)),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarIdioma_Click(object sender, EventArgs e)
        {
            var idioma = lstIdiomas.SelectedItem as Idioma;
            if (idioma == null)
            {
                MessageBox.Show(Tr("msgSelectIdioma", "Seleccioná un idioma de la lista."),
                    Tr("msgValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                string.Format(Tr("msgConfirmarBorrarIdioma",
                    "¿Borrar el idioma '{0}'?\n\nEsta acción es irreversible y eliminará todas las traducciones asociadas."),
                    idioma.Nombre),
                Tr("msgConfirmarEliminacion", "Confirmar eliminación"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _idiomaService.Eliminar(idioma.Id);
                CargarIdiomas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Tr("msgErrorPrefix", "Error: {0}"), TrError(ex)),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var idioma = lstIdiomas.SelectedItem as Idioma;
            if (idioma == null) return;

            grdTraducciones.EndEdit();

            bool esEspañol = string.Equals(idioma.Nombre, "Español", StringComparison.OrdinalIgnoreCase);
            if (esEspañol)
            {
                foreach (DataGridViewRow fila in grdTraducciones.Rows)
                {
                    string texto = fila.Cells[colTexto.Index].Value?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(texto))
                    {
                        MessageBox.Show(
                            Tr("msgEspanolCompleto",
                                "El idioma 'Español' debe estar siempre completo: es el idioma base del sistema.\n\nCompletá todas las filas antes de guardar."),
                            Tr("msgNoSePuedeGuardar", "No se puede guardar"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            int guardadas = 0;
            try
            {
                foreach (DataGridViewRow fila in grdTraducciones.Rows)
                {
                    var traduccion = fila.Tag as Traduccion;
                    if (traduccion == null) continue;

                    string textoNuevo = fila.Cells[colTexto.Index].Value?.ToString() ?? "";
                    string textoActual = traduccion.Texto ?? "";

                    if (textoNuevo == textoActual) continue;

                    _idiomaService.GuardarTraduccion(idioma.Id, traduccion.IdControl, textoNuevo);
                    traduccion.Texto = textoNuevo;
                    guardadas++;
                }

                if (guardadas == 0)
                {
                    MessageBox.Show(Tr("msgSinCambios", "No hay cambios para guardar."),
                        Tr("msgInformacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(Tr("msgGuardadoExito", "Se guardaron {0} traducción(es)."), guardadas),
                        Tr("msgCambiosGuardados", "Cambios guardados"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefrescarEstadoIdioma(idioma.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgErrorGuardar", "Error al guardar:") + "\n" + TrError(ex),
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefrescarEstadoIdioma(int idiomaId)
        {
            bool completo = _idiomaService.EstaCompleto(idiomaId);
            for (int i = 0; i < lstIdiomas.Items.Count; i++)
            {
                var idi = lstIdiomas.Items[i] as Idioma;
                if (idi != null && idi.Id == idiomaId)
                {
                    idi.Completo = completo;
                    lstIdiomas.Items[i] = idi;
                    if (lstIdiomas.SelectedIndex == i)
                        pnlAlerta.Visible = !completo;
                    break;
                }
            }
        }

        private string Prompt(string mensaje)
        {
            using (var form = new Form())
            using (var lbl = new Label())
            using (var txt = new TextBox())
            using (var ok = new Button())
            using (var cancel = new Button())
            {
                form.Text = Tr("promptIngresarTitulo", "Ingresar");
                form.ClientSize = new Size(380, 130);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false; form.MaximizeBox = false;

                lbl.Text = mensaje; lbl.SetBounds(15, 15, 350, 20);
                txt.SetBounds(15, 40, 350, 25); txt.Font = new Font("Segoe UI", 9.5F);

                ok.Text     = Tr("btnAceptar",  "Aceptar");   ok.SetBounds(195, 80, 80, 30); ok.DialogResult     = DialogResult.OK;
                cancel.Text = Tr("btnCancelar", "Cancelar");  cancel.SetBounds(285, 80, 80, 30); cancel.DialogResult = DialogResult.Cancel;

                form.Controls.AddRange(new System.Windows.Forms.Control[] { lbl, txt, ok, cancel });
                form.AcceptButton = ok; form.CancelButton = cancel;

                return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
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
