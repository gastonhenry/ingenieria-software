using BE;
using BE.Enums;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace UI
{
    // N03 — CU-CAP-05: suspender / reanudar avisos a un cliente y cerrar búsquedas a su pedido.
    // Se abre desde el menú (buscando el cliente) o desde el tablero con el cliente ya elegido.
    public partial class FormAvisosCliente : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormAvisosCliente";

        private readonly ICaptacionService _captacionService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;
        private Persona _cliente;
        private SuspensionAvisos _suspension;

        public FormAvisosCliente() : this(null) { }

        public FormAvisosCliente(Persona cliente)
        {
            InitializeComponent();
            _captacionService = new CaptacionService();
            _idiomaService = new IdiomaService();
            _cliente = cliente;

            txtDocBusqueda.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { btnBuscar_Click(s, e); e.SuppressKeyPress = true; } };

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) =>
            {
                if (_cliente != null)
                {
                    // 1.1.1: viene del tablero con el cliente seleccionado.
                    txtDocBusqueda.Text = _cliente.Documento;
                    lstClientes.Items.Add(new ClienteItem(_cliente));
                    lstClientes.SelectedIndex = 0;
                }
                else MostrarCliente();
            };
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
            this.Text                  = Tr("title",               "Avisos a clientes");
            lblTitulo.Text             = Tr("lblTitulo",           "Avisos a clientes");
            lblDocBusqueda.Text        = Tr("lblDocBusqueda",      "Cliente (DNI/CUIT o nombre):");
            btnBuscar.Text             = Tr("btnBuscar",           "Buscar");
            btnSuspender.Text          = Tr("btnSuspender",        "Suspender avisos");
            btnReanudar.Text           = Tr("btnReanudar",         "Reanudar avisos");
            lblBusquedas.Text          = Tr("lblBusquedas",        "Búsquedas del cliente");
            colFecha.HeaderText        = Tr("colFecha",            "Entrevista");
            colBusca.HeaderText        = Tr("colBusca",            "Busca");
            colAceptaAvisos.HeaderText = Tr("colAceptaAvisos",     "Acepta avisos");
            colEstado.HeaderText       = Tr("colEstado",           "Estado");
            colVence.HeaderText        = Tr("colVence",            "Vence");
            btnCerrarBusqueda.Text     = Tr("btnCerrarBusqueda",   "Cerrar búsqueda seleccionada");
            lblAvisos.Text             = Tr("lblAvisos",           "Avisos enviados");
            colAvisoFecha.HeaderText   = Tr("colAvisoFecha",       "Fecha");
            colAvisoUnidad.HeaderText  = Tr("colAvisoUnidad",      "Unidad");
            colAvisoResultado.HeaderText = Tr("colAvisoResultado", "Resultado");
            if (IsHandleCreated) MostrarCliente();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                lstClientes.Items.Clear();
                _cliente = null;
                MostrarCliente();
                string frag = txtDocBusqueda.Text.Trim();
                if (string.IsNullOrEmpty(frag)) return;
                List<Persona> rs = _captacionService.BuscarClientes(frag);
                if (rs.Count == 0)
                {
                    MessageBox.Show(Tr("msgSinCoincidencias", "No hay clientes con búsquedas registradas que coincidan con ese DNI/CUIT o nombre."),
                        Tr("msgInformacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (var p in rs) lstClientes.Items.Add(new ClienteItem(p));
                lstClientes.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            _cliente = (lstClientes.SelectedItem as ClienteItem)?.Persona;
            MostrarCliente();
        }

        private void MostrarCliente()
        {
            dgvBusquedas.Rows.Clear();
            dgvAvisos.Rows.Clear();
            bool hay = _cliente != null;
            pnlCliente.Enabled = hay;
            if (!hay)
            {
                lblClienteNombre.Text = Tr("lblSinCliente", "Buscá un cliente por documento.");
                lblClienteContacto.Text = "";
                lblEstadoAvisos.Text = "";
                btnCerrarBusqueda.Enabled = false;
                return;
            }

            try
            {
                lblClienteNombre.Text = $"{_cliente.Nombre} ({_cliente.Documento})";
                lblClienteContacto.Text = string.Format(Tr("lblContacto", "Correo: {0}   ·   Teléfono: {1}"),
                    string.IsNullOrWhiteSpace(_cliente.Email) ? "—" : _cliente.Email,
                    string.IsNullOrWhiteSpace(_cliente.Telefono) ? "—" : _cliente.Telefono);

                _suspension = _captacionService.ObtenerSuspension(_cliente.Id);
                if (_suspension == null)
                {
                    lblEstadoAvisos.Text = Tr("lblAvisosActivos", "Avisos activos: recibe un correo cuando ingresa una unidad que coincide.");
                    lblEstadoAvisos.ForeColor = Color.FromArgb(30, 150, 70);
                }
                else
                {
                    lblEstadoAvisos.Text = string.Format(Tr("lblAvisosSuspendidos", "Avisos suspendidos desde el {0:d} por {1}.{2}"),
                        _suspension.FechaSuspension, _suspension.Usuario.Username,
                        string.IsNullOrWhiteSpace(_suspension.Motivo) ? "" : " " + string.Format(Tr("lblMotivoSuspension", "Motivo: {0}"), _suspension.Motivo));
                    lblEstadoAvisos.ForeColor = Color.FromArgb(200, 60, 40);
                }
                btnSuspender.Enabled = _suspension == null;
                btnReanudar.Enabled = _suspension != null;

                foreach (var b in _captacionService.ListarBusquedasDeCliente(_cliente.Id))
                {
                    int i = dgvBusquedas.Rows.Add(b.FechaRegistro.ToString("d", CultureInfo.CurrentCulture),
                        TextosCaptacion.DescribirBusqueda(b, Tr),
                        b.AceptaAvisos ? Tr("si", "Sí") : Tr("no", "No"),
                        b.Estado.GetDescripcionTraducida(),
                        b.FechaVencimiento.ToString("d", CultureInfo.CurrentCulture));
                    dgvBusquedas.Rows[i].Tag = b;
                    if (b.Estado != EstadoBusqueda.Activa) dgvBusquedas.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(130, 130, 140);
                }

                foreach (var a in _captacionService.ListarAvisosDeCliente(_cliente.Id))
                {
                    int i = dgvAvisos.Rows.Add(a.FechaEnvio.ToString("g", CultureInfo.CurrentCulture), a.Unidad,
                        a.Exitoso ? Tr("resultadoOk", "Enviado") : Tr("resultadoError", "Falló"));
                    if (!a.Exitoso)
                    {
                        dgvAvisos.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(190, 50, 40);
                        dgvAvisos.Rows[i].Cells[2].ToolTipText = a.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ActualizarBotonCerrarBusqueda();
        }

        private void dgvBusquedas_SelectionChanged(object sender, EventArgs e) => ActualizarBotonCerrarBusqueda();

        private void ActualizarBotonCerrarBusqueda()
        {
            var b = dgvBusquedas.CurrentRow?.Tag as BusquedaCliente;
            btnCerrarBusqueda.Enabled = b != null && b.Estado == EstadoBusqueda.Activa;
        }

        private void btnSuspender_Click(object sender, EventArgs e)
        {
            if (_cliente == null) return;
            string motivo;
            if (!ConfirmarConMotivo(string.Format(Tr("msgConfirmarSuspender", "¿Suspender los avisos por correo a {0}?\n\nSus búsquedas siguen activas, pero no recibe más correos hasta que se reanuden."), _cliente.Nombre),
                                    Tr("btnSuspender", "Suspender avisos"), out motivo)) return;
            Ejecutar(() => _captacionService.SuspenderAvisos(_cliente.Id, motivo),
                     Tr("msgAvisosSuspendidos", "Avisos suspendidos."));
        }

        private void btnReanudar_Click(object sender, EventArgs e)
        {
            if (_cliente == null) return;
            Ejecutar(() => _captacionService.ReanudarAvisos(_cliente.Id), Tr("msgAvisosReanudados", "Avisos reanudados."));
        }

        private void btnCerrarBusqueda_Click(object sender, EventArgs e)
        {
            var b = dgvBusquedas.CurrentRow?.Tag as BusquedaCliente;
            if (b == null) return;
            string motivo;
            if (!ConfirmarConMotivo(Tr("msgConfirmarCerrarBusqueda", "¿Cerrar la búsqueda seleccionada?\n\nEl cliente deja de figurar como interesado por esa búsqueda y no recibe más avisos por ella."),
                                    Tr("btnCerrarBusqueda", "Cerrar búsqueda seleccionada"), out motivo)) return;
            Ejecutar(() => _captacionService.CerrarBusqueda(b.Id, motivo), Tr("msgBusquedaCerrada", "Búsqueda cerrada."));
        }

        // Modal de confirmación con motivo opcional.
        private bool ConfirmarConMotivo(string mensaje, string textoAceptar, out string motivo)
        {
            motivo = null;
            using (var f = new FormConfirmarMotivo(Tr("msgConfirmar", "Confirmar"), mensaje, textoAceptar))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return false;
                motivo = f.Motivo;
                return true;
            }
        }

        private void Ejecutar(Action accion, string mensajeExito)
        {
            try
            {
                accion();
                MostrarCliente();
                MessageBox.Show(mensajeExito, Tr("msgExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private class ClienteItem
        {
            public Persona Persona { get; }
            public ClienteItem(Persona p) { Persona = p; }
            public override string ToString() => $"{Persona.Documento} — {Persona.Nombre}";
        }
    }
}
