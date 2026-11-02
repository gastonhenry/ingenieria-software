using BE;
using BLL;
using HELPERS;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    // N03 — CU-CAP-02: Tablero de captación. Cruza búsquedas, unidades en venta y ventas;
    // los cálculos los hace CaptacionService y acá solo se muestran.
    // A la izquierda las unidades en venta; a la derecha, los interesados en la unidad elegida.
    public partial class FormTableroCaptacion : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormTableroCaptacion";
        private const int DIAS_RESUMEN = 30;

        private readonly ICaptacionService _captacionService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;
        private TableroCaptacion _tablero;

        public FormTableroCaptacion()
        {
            InitializeComponent();
            _captacionService = new CaptacionService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            cardEnVenta.MostrarVer = false;
            cardPorSalir.MostrarVer = false;
            btnGestionarAvisos.Visible = Tiene("GESTIONAR_AVISOS_CLIENTE");

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => Recargar();
        }

        private bool Tiene(string permiso)
        {
            var u = SesionUsuario.GetInstancia().Usuario;
            if (u != null && u.Username != null && u.Username.ToLower() == "admin") return true;
            return u != null && _permisoService.UsuarioTienePermiso(u, permiso);
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
            this.Text                      = Tr("title",                   "Tablero de captación");
            lblTitulo.Text                 = Tr("lblTitulo",               "Tablero de captación");
            cardEnVenta.Titulo             = Tr("cardEnVenta",             "Vehículos en venta");
            cardPorSalir.Titulo            = Tr("cardPorSalir",            "Por salir a la venta");
            cardPosiblesCompradores.Titulo = Tr("cardPosiblesCompradores", "Posibles compradores");
            cardSinInteresados.Titulo      = Tr("cardSinInteresados",      "Unidades sin interesados");
            cardPosiblesCompradores.TextoVer = Tr("lnkVer", "Ver ▸");
            cardSinInteresados.TextoVer      = Tr("lnkVer", "Ver ▸");
            cardInteresadosSinUnidades.Titulo   = Tr("cardInteresadosSinUnidades", "Interesados sin unidades");
            cardInteresadosSinUnidades.TextoVer = Tr("lnkVer", "Ver ▸");
            cardConversion.Titulo   = Tr("cardConversion", "Conversión de avisos");
            cardConversion.TextoVer = Tr("lnkVer", "Ver ▸");
            lblUnidades.Text               = Tr("lblUnidades",             "Unidades en venta");
            colUnidad.HeaderText           = Tr("colUnidad",               "Unidad");
            colPrecio.HeaderText           = Tr("colPrecio",               "Precio");
            colDias.HeaderText             = Tr("colDias",                 "Días publicada");
            colInteresados.HeaderText      = Tr("colInteresados",          "Interesados");
            btnExportarPdf.Text            = Tr("btnExportarPdf",          "Exportar PDF");
            colCliente.HeaderText          = Tr("colCliente",              "Cliente");
            colTelefono.HeaderText         = Tr("colTelefono",             "Teléfono");
            colBusca.HeaderText            = Tr("colBusca",                "Busca");
            colEntrevista.HeaderText       = Tr("colEntrevista",           "Entrevista");
            colAvisado.HeaderText          = Tr("colAvisado",              "Avisado");
            btnGestionarAvisos.Text        = Tr("btnGestionarAvisos",      "Avisos del cliente...");
            if (_tablero != null) Mostrar(UnidadSeleccionada()?.Unidad.IdUnidad);
            else MostrarInteresados();
        }

        // ------------------------------------------------------------
        // Carga
        // ------------------------------------------------------------

        // Lo usa FormPrincipal al guardar los parámetros de captación con el tablero abierto.
        public void Refrescar() => Recargar();

        private void Recargar()
        {
            int? idSeleccionada = UnidadSeleccionada()?.Unidad.IdUnidad;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _tablero = _captacionService.ObtenerTablero(DateTime.Today.AddDays(-DIAS_RESUMEN), DateTime.Today);
                var parametros = _captacionService.ObtenerParametros();
                lblEstadoAvisos.Text = parametros.NotificacionesActivas
                    ? Tr("lblAvisosActivos", "● Envío automático de avisos: activado")
                    : Tr("lblAvisosInactivos", "● Envío automático de avisos: desactivado");
                lblEstadoAvisos.ForeColor = parametros.NotificacionesActivas ? Color.FromArgb(30, 150, 70) : Color.FromArgb(200, 60, 40);
                Mostrar(idSeleccionada);
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void Mostrar(int? idUnidadASeleccionar)
        {
            var t = _tablero;

            // Indicadores
            cardEnVenta.Valor = t.VehiculosEnVenta.ToString();
            cardEnVenta.Detalle = "";
            cardPorSalir.Valor = t.UnidadesPorSalirAVenta.ToString();
            cardPorSalir.Detalle = Tr("detallePorSalir", "pendientes de autorización");
            cardPosiblesCompradores.Valor = t.PosiblesCompradores.ToString();
            cardPosiblesCompradores.Detalle = Tr("detalleBusquedasActivas", "con búsqueda activa");
            cardSinInteresados.Valor = t.UnidadesSinInteresados.ToString();
            int alertas = t.Unidades.Count(u => u.Alerta);
            cardSinInteresados.Detalle = alertas > 0 ? string.Format(Tr("detalleAlertas", "{0} en alerta"), alertas) : "";
            cardInteresadosSinUnidades.Valor = t.InteresadosSinUnidades.Count.ToString();
            cardInteresadosSinUnidades.Detalle = Tr("detalleEsperandoStock", "esperando stock");

            // Unidades en venta
            dgvUnidades.SelectionChanged -= dgvUnidades_SelectionChanged;
            dgvUnidades.Rows.Clear();
            int filaASeleccionar = 0;
            foreach (var f in t.Unidades)
            {
                int i = dgvUnidades.Rows.Add(f.Unidad.ToString(), TextosCaptacion.Precio(f.Unidad.PrecioPublicacion, Tr),
                    f.DiasPublicada, f.CantidadInteresados);
                dgvUnidades.Rows[i].Tag = f;
                if (f.Unidad.IdUnidad == idUnidadASeleccionar) filaASeleccionar = i;
                if (f.Alerta)
                {
                    dgvUnidades.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 220);
                    dgvUnidades.Rows[i].Cells[colInteresados.Index].ToolTipText =
                        Tr("tooltipAlerta", "Sin interesados y publicada hace muchos días: evaluar promoción o precio.");
                }
            }
            if (dgvUnidades.Rows.Count > 0)
                dgvUnidades.CurrentCell = dgvUnidades.Rows[filaASeleccionar].Cells[0];
            dgvUnidades.SelectionChanged += dgvUnidades_SelectionChanged;
            MostrarInteresados();
            btnExportarPdf.Enabled = dgvUnidades.Rows.Count > 0;

            // Resumen de los últimos 30 días
            int enviados = t.AvisosEnviados;
            int fallidos = t.Avisos.Count(a => !a.Exitoso);
            lblResumen.Text = string.Format(Tr("lblResumenAvisos", "Últimos {0} días: {1} aviso(s) enviado(s){2}."),
                DIAS_RESUMEN, enviados,
                fallidos > 0 ? string.Format(Tr("resumenFallidos", " ({0} falló/fallaron)"), fallidos) : "");

            // Conversión: de los clientes avisados en el período, cuántos compraron después del aviso.
            cardConversion.Valor = t.ClientesAvisados > 0
                ? string.Format(CultureInfo.CurrentCulture, "{0:0} %", t.PorcentajeConversion)
                : "—";
            cardConversion.Detalle = string.Format(Tr("detalleConversion", "{0} de {1} avisados compraron"),
                t.ClientesConvertidos, t.ClientesAvisados);
        }

        private FilaUnidadTablero UnidadSeleccionada() => dgvUnidades.CurrentRow?.Tag as FilaUnidadTablero;

        // Panel derecho: clientes con búsqueda activa que coincide con la unidad elegida.
        private void MostrarInteresados()
        {
            dgvInteresados.Rows.Clear();
            var fila = UnidadSeleccionada();
            if (_tablero == null || fila == null)
            {
                lblInteresados.Text = Tr("lblSeleccioneUnidad", "Elegí una unidad para ver sus interesados");
                btnGestionarAvisos.Enabled = false;
                return;
            }

            lblInteresados.Text = string.Format(Tr("lblInteresadosEn", "Interesados en: {0}"), fila.Unidad);
            foreach (var it in _tablero.Interesados.Where(i => i.Coincidencias.Any(u => u.IdUnidad == fila.Unidad.IdUnidad)))
            {
                string busca = string.Join(" | ", it.Busquedas.Select(b => TextosCaptacion.DescribirBusqueda(b, Tr)));
                string cliente = it.AvisosSuspendidos ? it.Cliente.Nombre + " " + Tr("marcaSuspendido", "(avisos suspendidos)") : it.Cliente.Nombre;
                var avisado = EstadoAviso(it, fila.Unidad.IdUnidad);
                int i = dgvInteresados.Rows.Add(cliente, it.Cliente.Telefono, busca,
                    it.FechaUltimaEntrevista.ToString("d", CultureInfo.CurrentCulture), avisado.Texto);
                dgvInteresados.Rows[i].Tag = it;
                if (it.AvisosSuspendidos) dgvInteresados.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(130, 130, 140);
                dgvInteresados.Rows[i].Cells[colAvisado.Index].Style.ForeColor = avisado.Color;
                if (avisado.Detalle != null) dgvInteresados.Rows[i].Cells[colAvisado.Index].ToolTipText = avisado.Detalle;
            }
            if (dgvInteresados.Rows.Count == 0)
                lblInteresados.Text += "  —  " + Tr("lblSinInteresadosUnidad", "ninguna búsqueda activa coincide");
            btnGestionarAvisos.Enabled = dgvInteresados.Rows.Count > 0;
        }

        // Si el interesado ya recibió el aviso de esta unidad: "Sí · fecha", "Falló" o "No (motivo)".
        private (string Texto, Color Color, string Detalle) EstadoAviso(InteresadoTablero it, int idUnidad)
        {
            var avisos = _tablero.AvisosUnidadesEnVenta
                .Where(a => a.IdPersona == it.Cliente.Id && a.IdUnidad == idUnidad).ToList();
            var exitoso = avisos.Where(a => a.Exitoso).OrderBy(a => a.FechaEnvio).FirstOrDefault();
            if (exitoso != null)
                return (string.Format(Tr("avisadoSi", "Sí · {0:d}"), exitoso.FechaEnvio), Color.FromArgb(30, 150, 70), null);
            var fallido = avisos.OrderByDescending(a => a.FechaEnvio).FirstOrDefault();
            if (fallido != null)
                return (Tr("avisadoFallo", "Falló"), Color.FromArgb(190, 50, 40), fallido.Error);
            if (it.AvisosSuspendidos)
                return (Tr("avisadoSuspendido", "No (avisos suspendidos)"), Color.FromArgb(130, 130, 140), null);
            if (!it.Busquedas.Any(b => b.AceptaAvisos))
                return (Tr("avisadoNoAcepta", "No (no acepta avisos)"), Color.FromArgb(130, 130, 140), null);
            return (Tr("avisadoNo", "No"), Color.FromArgb(200, 120, 20),
                    Tr("avisadoNoDetalle", "No recibió aviso de esta unidad (por ejemplo, la entrevista fue posterior a la publicación). Conviene contactarlo."));
        }

        private void dgvUnidades_SelectionChanged(object sender, EventArgs e) => MostrarInteresados();

        // ------------------------------------------------------------
        // Detalle (modales) y acciones
        // ------------------------------------------------------------

        private void AbrirDetalle(FormDetalleCaptacion.Modo modo)
        {
            if (_tablero == null) return;
            int? idUnidadElegida;
            using (var f = new FormDetalleCaptacion(_tablero, modo))
            {
                f.ShowDialog(this);
                idUnidadElegida = f.IdUnidadElegida;
            }
            if (modo == FormDetalleCaptacion.Modo.PosiblesCompradores || modo == FormDetalleCaptacion.Modo.InteresadosSinUnidades)
                RecargarManteniendoPosicion();
            if (idUnidadElegida.HasValue) SeleccionarUnidad(idUnidadElegida.Value);
        }

        // Posiciona la grilla de unidades en venta en la unidad indicada (ej. doble clic en "sin interesados").
        private void SeleccionarUnidad(int idUnidad)
        {
            foreach (DataGridViewRow row in dgvUnidades.Rows)
            {
                if (!(row.Tag is FilaUnidadTablero f) || f.Unidad.IdUnidad != idUnidad) continue;
                dgvUnidades.CurrentCell = row.Cells[0];
                if (!row.Displayed) dgvUnidades.FirstDisplayedScrollingRowIndex = row.Index;
                dgvUnidades.Focus();
                return;
            }
        }

        private void cardPosiblesCompradores_VerClick(object sender, EventArgs e) => AbrirDetalle(FormDetalleCaptacion.Modo.PosiblesCompradores);
        private void cardSinInteresados_VerClick(object sender, EventArgs e) => AbrirDetalle(FormDetalleCaptacion.Modo.UnidadesSinInteresados);
        private void cardInteresadosSinUnidades_VerClick(object sender, EventArgs e) => AbrirDetalle(FormDetalleCaptacion.Modo.InteresadosSinUnidades);
        private void cardConversion_VerClick(object sender, EventArgs e) => AbrirDetalle(FormDetalleCaptacion.Modo.Conversiones);

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            var f = UnidadSeleccionada();
            if (f == null) return;
            ExportadorPdfPublicacion.Exportar(this, f.Unidad.IdUnidad);
        }

        private void btnGestionarAvisos_Click(object sender, EventArgs e)
        {
            var it = dgvInteresados.CurrentRow?.Tag as InteresadoTablero;
            if (it == null) return;
            using (var f = new FormAvisosCliente(it.Cliente))
                f.ShowDialog(this);
            RecargarManteniendoPosicion();
        }

        // Recarga al volver de un modal sin perder la posición: misma unidad (ya la conserva Recargar),
        // mismo interesado si sigue en la lista (puede haber cerrado su búsqueda) y el scroll de ambas grillas.
        private void RecargarManteniendoPosicion()
        {
            int scrollUnidades = dgvUnidades.FirstDisplayedScrollingRowIndex;
            int scrollInteresados = dgvInteresados.FirstDisplayedScrollingRowIndex;
            int? idCliente = (dgvInteresados.CurrentRow?.Tag as InteresadoTablero)?.Cliente.Id;
            Recargar();

            if (scrollUnidades >= 0 && scrollUnidades < dgvUnidades.RowCount)
                dgvUnidades.FirstDisplayedScrollingRowIndex = scrollUnidades;
            if (dgvUnidades.CurrentRow != null && !dgvUnidades.CurrentRow.Displayed)
                dgvUnidades.FirstDisplayedScrollingRowIndex = dgvUnidades.CurrentRow.Index;
            if (idCliente == null) return;
            foreach (DataGridViewRow row in dgvInteresados.Rows)
            {
                if (!(row.Tag is InteresadoTablero x) || x.Cliente.Id != idCliente) continue;
                dgvInteresados.CurrentCell = row.Cells[0];
                if (scrollInteresados >= 0 && scrollInteresados < dgvInteresados.RowCount)
                    dgvInteresados.FirstDisplayedScrollingRowIndex = scrollInteresados;
                if (!row.Displayed) dgvInteresados.FirstDisplayedScrollingRowIndex = row.Index;
                break;
            }
        }

        private void dgvInteresados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnGestionarAvisos.Visible) btnGestionarAvisos_Click(sender, EventArgs.Empty);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
