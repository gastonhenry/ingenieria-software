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
        private const string EMAIL_SOPORTE = "gastonhenry@software.com";

        private readonly IIdiomaService _idiomaService;
        private readonly Action _abrirUnidades;
        private readonly Action _abrirPublicaciones;
        private readonly Action _abrirTableroCaptacion;
        private readonly Action _abrirEntrevista;
        private bool _suscrito;

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int w, int h);

        public FormHome() : this(null, null, null, null) { }

        public FormHome(Action abrirUnidades, Action abrirPublicaciones) : this(abrirUnidades, abrirPublicaciones, null, null) { }

        // Acción == null → el usuario no tiene permiso y el acceso rápido queda oculto.
        public FormHome(Action abrirUnidades, Action abrirPublicaciones, Action abrirTableroCaptacion, Action abrirEntrevista)
        {
            InitializeComponent();
            _idiomaService = new IdiomaService();
            _abrirUnidades = abrirUnidades;
            _abrirPublicaciones = abrirPublicaciones;
            _abrirTableroCaptacion = abrirTableroCaptacion;
            _abrirEntrevista = abrirEntrevista;
            btnVerUnidades.Visible = abrirUnidades != null;
            btnVerPublicaciones.Visible = abrirPublicaciones != null;
            btnTableroCaptacion.Visible = abrirTableroCaptacion != null;
            btnRegistrarEntrevista.Visible = abrirEntrevista != null;

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
            btnVerUnidades.Text     = Tr("btnVerUnidades",     "Ver Unidades");
            btnVerPublicaciones.Text= Tr("btnVerPublicaciones","Ver Publicaciones");
            btnAcceso1.Text         = Tr("btnAyuda",           "Ayuda");
            btnAcceso2.Text         = Tr("btnContacto",        "Contactános");
            btnTableroCaptacion.Text    = Tr("btnTableroCaptacion",    "Tablero de captación");
            btnRegistrarEntrevista.Text = Tr("btnRegistrarEntrevista", "Registrar entrevista");
        }

        private void btnTableroCaptacion_Click(object sender, EventArgs e) => _abrirTableroCaptacion?.Invoke();

        private void btnRegistrarEntrevista_Click(object sender, EventArgs e) => _abrirEntrevista?.Invoke();

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

        private void btnVerUnidades_Click(object sender, EventArgs e)
        {
            _abrirUnidades?.Invoke();
        }

        private void btnVerPublicaciones_Click(object sender, EventArgs e)
        {
            _abrirPublicaciones?.Invoke();
        }

        // D02 — Ayuda en línea: página HTML que viaja con la instalación (Ayuda\ayuda.html), se abre en el navegador.
        private void btnAyuda_Click(object sender, EventArgs e)
        {
            string ruta = System.IO.Path.Combine(Application.StartupPath, "Ayuda", "ayuda.html");
            try
            {
                if (!System.IO.File.Exists(ruta)) throw new System.IO.FileNotFoundException(ruta);
                Process.Start(ruta);
            }
            catch (Exception)
            {
                MessageBox.Show(Tr("msgAyudaNoEncontrada", "No se pudo abrir la ayuda.") + "\n" + ruta,
                    Tr("msgAyudaTitulo", "Ayuda"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
