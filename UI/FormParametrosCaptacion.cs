using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    // N03 — configuración de la captación (Gerente): interruptor global de avisos, vigencia y alerta.
    public partial class FormParametrosCaptacion : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormParametrosCaptacion";

        private readonly ICaptacionService _captacionService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormParametrosCaptacion()
        {
            InitializeComponent();
            _captacionService = new CaptacionService();
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
            this.Text                     = Tr("title",                   "Parámetros de captación");
            lblTitulo.Text                = Tr("lblTitulo",               "Parámetros de captación");
            chkNotificacionesActivas.Text = Tr("chkNotificacionesActivas", "Envío automático de avisos por correo activado");
            lblAyudaNotificaciones.Text   = Tr("lblAyudaNotificaciones",  "Si está desactivado, al publicar una unidad no se envía ningún aviso. El tablero sigue mostrando a los interesados.");
            lblVigencia.Text              = Tr("lblVigencia",             "Vigencia de una búsqueda (días):");
            lblAlerta.Text                = Tr("lblAlerta",               "Alerta de unidad sin interesados (días):");
            btnGuardar.Text               = Tr("btnGuardar",              "Guardar");
            btnCancelar.Text              = Tr("btnCancelar",             "Cancelar");
        }

        private void Cargar()
        {
            try
            {
                var p = _captacionService.ObtenerParametros();
                chkNotificacionesActivas.Checked = p.NotificacionesActivas;
                numVigencia.Value = Math.Max(numVigencia.Minimum, Math.Min(numVigencia.Maximum, p.DiasVigenciaBusqueda));
                numAlerta.Value = Math.Max(numAlerta.Minimum, Math.Min(numAlerta.Maximum, p.DiasAlertaSinInteresados));
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                _captacionService.ActualizarParametros(chkNotificacionesActivas.Checked, (int)numVigencia.Value, (int)numAlerta.Value);
                MessageBox.Show(Tr("msgGuardado", "Parámetros guardados."), Tr("msgExito", "Éxito"),
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
