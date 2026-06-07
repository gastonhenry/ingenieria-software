using BE;
using BLL;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UI
{
    public partial class FormHome : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormHome";
        private const string EMAIL_SOPORTE = "soporte@ingenieria-software.com.ar";

        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int w, int h);

        public FormHome()
        {
            InitializeComponent();
            _idiomaService = new IdiomaService();

            AplicarBordesRedondeados(pnlNovedades, 18);
            AplicarBordesRedondeados(pnlAccesos,   18);

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

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            lblTitulo.Text          = Tr("lblTitulo",          "Inicio");
            lblNovedadesTitulo.Text = Tr("lblNovedadesTitulo", "Novedades");
            lblNovedadesBody.Text   = Tr("lblNovedadesBody",   "Bienvenido al sistema");
            lblAccesosTitulo.Text   = Tr("lblAccesosTitulo",   "Accesos rápidos");
            btnAcceso1.Text         = Tr("btnAyuda",           "Ayuda");
            btnAcceso2.Text         = Tr("btnContacto",        "Contactános");
        }

        private static void AplicarBordesRedondeados(System.Windows.Forms.Control control, int radio)
        {
            EventHandler refrescar = (s, e) =>
            {
                if (control.Width <= 0 || control.Height <= 0) return;
                control.Region = System.Drawing.Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, control.Width, control.Height, radio, radio));
            };
            control.Resize += refrescar;
            refrescar(control, EventArgs.Empty);
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Tr("msgProximamente", "Próximamente"),
                Tr("msgAyudaTitulo", "Ayuda"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnContacto_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("mailto:" + EMAIL_SOPORTE);
            }
            catch (Exception)
            {
                MessageBox.Show(Tr("msgErrorMail", "No se pudo abrir el cliente de correo.") + "\n" +
                                Tr("msgErrorMailEnvia", "Enviá un mail a") + " " + EMAIL_SOPORTE,
                    Tr("msgContactoTitulo", "Contacto"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
