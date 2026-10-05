using BE;
using BE.Enums;
using BLL;
using HELPERS;
using System;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    // Ventana modal con tabs (Datos / Checklist / Historial) y una barra de acciones
    // contextual al pie: solo se habilitan los botones que aplican al estado actual
    // de la unidad y al permiso del usuario en sesión.
    public partial class FormDetalleUnidad : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormDetalleUnidad";
        private const string CODIGO_FORM_ESTADO = "EstadoUnidad";

        private readonly int _unidadId;
        private readonly IUnidadService _unidadService;
        private readonly IVentaService _ventaService;
        private readonly IPermisoService _permisoService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;
        private Unidad _unidad;
        private int? _itemChecklistASeleccionar;

        public FormDetalleUnidad(int unidadId)
        {
            InitializeComponent();
            _unidadId = unidadId;

            // Historial: el ancho de cada columna se ajusta siempre al contenido (incluye headers traducidos).
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvHistorial.AllowUserToResizeColumns = false;
            _unidadService = new UnidadService();
            _ventaService = new VentaService();
            _permisoService = new PermisoService();
            _idiomaService = new IdiomaService();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => Recargar();
        }

        // ------------------------------------------------------------
        // i18n
        // ------------------------------------------------------------

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

        private string TrEstado(EstadoUnidad e)
        {
            try
            {
                string t = _idiomaService?.Traducir(CODIGO_FORM_ESTADO, e.ToString());
                if (!string.IsNullOrEmpty(t)) return t;
            }
            catch { }
            var field = typeof(EstadoUnidad).GetField(e.ToString());
            var attrs = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attrs != null && attrs.Length > 0)
                ? ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description
                : e.ToString();
        }

        private string TrRevision(ResultadoRevision r)
        {
            try { var t = _idiomaService?.Traducir("ResultadoRevision", r.ToString()); if (!string.IsNullOrEmpty(t)) return t; } catch { }
            return r.ToString();
        }

        private string TrAprobacion(EstadoAprobacionItem a)
        {
            try { var t = _idiomaService?.Traducir("EstadoAprobacionItem", a.ToString()); if (!string.IsNullOrEmpty(t)) return t; } catch { }
            return a.ToString();
        }

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            this.Text                       = Tr("title", "Detalle de unidad");
            tabDatos.Text                   = Tr("tabDatos",     "Datos");
            tabChecklist.Text               = Tr("tabChecklist", "Checklist");
            tabHistorial.Text               = Tr("tabHistorial", "Historial");
            btnAgregarItemExtra.Text        = Tr("btnAgregarItemExtra",     "Agregar item extra");
            btnQuitarItemExtra.Text         = Tr("btnQuitarItemExtra",      "Quitar item extra");
            btnMarcarItemEjecutado.Text     = Tr("btnMarcarOK",             "Marcar OK");
            btnMarcarObservado.Text         = Tr("btnMarcarObservado",      "Marcar Observado");
            lblResultado.Text               = Tr("lblComentarioRevision",   "Comentario (obligatorio al Observar):");
            btnTomarPreparacion.Text        = Tr("btnTomarPreparacion",     "Preparar Unidad");
            btnEnviarPresupuesto.Text       = Tr("btnEnviarPresupuesto",    "Enviar presupuesto");
            btnAprobarPresupuesto.Text      = Tr("btnAprobarPresupuesto",   "Aprobar presupuesto");
            btnRechazarPresupuesto.Text     = Tr("btnRechazarPresupuesto",  "Rechazar presupuesto");
            btnFinalizarPreparacion.Text    = Tr("btnFinalizarPreparacion", "Finalizar preparación");
            btnAutorizarPublicacion.Text    = Tr("btnAutorizarPublicacion", "Autorizar publicación");
            btnRechazarPublicacion.Text     = Tr("btnRechazarPublicacion",  "Rechazar publicación");
            btnPublicacion.Text             = Tr("btnPublicacion",          "Publicación");
            btnVender.Text                  = Tr("btnVender",               "Vender");
            btnReservar.Text                = Tr("btnReservar",             "Reservar");
            btnPausar.Text                  = Tr("btnPausar",               "Pausar");
            btnReanudar.Text                = Tr("btnReanudar",             "Reanudar");
            btnCancelarReserva.Text         = Tr("btnCancelarReserva",      "Cancelar reserva");

            foreach (System.Windows.Forms.DataGridViewColumn col in dgvChecklist.Columns)
            {
                switch (col.DataPropertyName)
                {
                    case "Nombre":             col.HeaderText = Tr("colChkNombre",      "Nombre"); break;
                    case "Descripcion":        col.HeaderText = Tr("colChkDescripcion", "Descripción"); break;
                    case "CostoEstimado":      col.HeaderText = Tr("colChkCosto",       "Costo"); break;
                    case "ResultadoRevision":  col.HeaderText = Tr("colChkRevision",    "Revisión"); break;
                    case "ComentarioRevision": col.HeaderText = Tr("colChkComentario",  "Comentario"); break;
                }
            }
            if (dgvHistorial.Columns.Count >= 5)
            {
                dgvHistorial.Columns[0].HeaderText = Tr("colHistFecha",   "Fecha/Hora");
                dgvHistorial.Columns[1].HeaderText = Tr("colHistOrigen",  "Origen");
                dgvHistorial.Columns[2].HeaderText = Tr("colHistDestino", "Destino");
                dgvHistorial.Columns[3].HeaderText = Tr("colHistUsuario", "Usuario");
                dgvHistorial.Columns[4].HeaderText = Tr("colHistMotivo",  "Motivo / obs.");
            }

            if (_unidad != null)
            {
                PintarDatos();
                PintarChecklist();
                dgvChecklist.Refresh();
                dgvHistorial.Refresh();
            }
        }

        // ------------------------------------------------------------
        // Recarga y pintado
        // ------------------------------------------------------------

        private void Recargar()
        {
            try
            {
                _unidad = _unidadService.ObtenerTrazabilidad(_unidadId);
                PintarDatos();
                PintarChecklist();
                PintarHistorial();
                ActualizarBotonesAccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void PintarDatos()
        {
            var u = _unidad;
            var p = _unidad.Vendedor;
            lblTitulo.Text = $"{u.Dominio} — {u.Modelo.Marca.Nombre} {u.Modelo.Nombre} ({u.Anio})";
            lblEstado.Text = Tr("lblEstadoPrefix", "Estado: ") + TrEstado(u.EstadoActual);
            lblEstado.ForeColor = ColoresEstadoUnidad.Obtener(u.EstadoActual);
            lblEstado.Font = new System.Drawing.Font(lblEstado.Font, System.Drawing.FontStyle.Bold);

            string tipoP = p == null ? "" :
                (p.TipoPersona == 'J' ? Tr("vendedorTipoJuridica", "Jurídica") : Tr("vendedorTipoFisica", "Física"));

            string datos =
                "Dominio: " + u.Dominio + Environment.NewLine +
                "Marca: " + u.Modelo.Marca.Nombre + Environment.NewLine +
                "Modelo: " + u.Modelo.Nombre + Environment.NewLine +
                "Año: " + u.Anio + Environment.NewLine +
                "Kilometraje: " + u.Kilometraje + Environment.NewLine +
                "Precio de compra: " + u.PrecioCompra.ToString("N2") + Environment.NewLine +
                "Fecha de ingreso: " + u.FechaIngreso.ToString("g") + Environment.NewLine +
                "Descripción: " + (u.Descripcion ?? "") + Environment.NewLine +
                Environment.NewLine +
                "-- Vendedor --" + Environment.NewLine +
                (p == null ? Tr("vendedorNoEncontrado", "(no encontrado)") :
                    "Tipo: " + tipoP + Environment.NewLine +
                    "Documento: " + p.Documento + Environment.NewLine +
                    "Nombre / R.S.: " + p.Nombre + Environment.NewLine +
                    "Domicilio: " + (p.Domicilio ?? "") + Environment.NewLine +
                    "Teléfono: " + (p.Telefono ?? "") + Environment.NewLine +
                    "Email: " + (p.Email ?? ""));

            // Si la unidad está Vendida o Reservada, agregamos la info del comprador y la operación.
            if (u.EstadoActual == EstadoUnidad.Vendido || u.EstadoActual == EstadoUnidad.Reservado)
            {
                try
                {
                    var venta = u.Ventas.Find(v => v.Activa);
                    if (venta != null)
                    {
                        var comp = venta.Comprador;
                        string tipoC = comp == null ? "" :
                            (comp.TipoPersona == 'J' ? Tr("vendedorTipoJuridica", "Jurídica") : Tr("vendedorTipoFisica", "Física"));
                        string titulo = venta.Tipo == TipoOperacionVenta.Venta ? "-- Comprador --" : "-- Reservado por --";
                        string etiquetaPrecio = venta.Tipo == TipoOperacionVenta.Venta ? "Precio de venta: " : "Precio acordado: ";

                        datos += Environment.NewLine + Environment.NewLine + titulo + Environment.NewLine +
                            (comp == null ? Tr("vendedorNoEncontrado", "(no encontrado)") :
                                "Tipo: " + tipoC + Environment.NewLine +
                                "Documento: " + comp.Documento + Environment.NewLine +
                                "Nombre / R.S.: " + comp.Nombre + Environment.NewLine +
                                "Domicilio: " + (comp.Domicilio ?? "") + Environment.NewLine +
                                "Teléfono: " + (comp.Telefono ?? "") + Environment.NewLine +
                                "Email: " + (comp.Email ?? "")) + Environment.NewLine +
                            Environment.NewLine +
                            etiquetaPrecio + venta.PrecioAcordado.ToString("N2") + Environment.NewLine +
                            "Fecha operación: " + venta.FechaOperacion.ToString("g");
                        if (venta.MontoSena.HasValue)
                            datos += Environment.NewLine + "Seña: " + venta.MontoSena.Value.ToString("N2");
                        if (venta.FechaEstimadaFin.HasValue)
                            datos += Environment.NewLine + "Fecha estimada fin: " + venta.FechaEstimadaFin.Value.ToString("d");
                        if (!string.IsNullOrWhiteSpace(venta.Descripcion))
                            datos += Environment.NewLine + "Descripción: " + venta.Descripcion;
                    }
                }
                catch { /* si falla la consulta, dejamos la sección de Vendedor como estaba */ }
            }

            txtDatos.Text = datos;
        }

        private void PintarChecklist()
        {
            if (_unidad.Checklist == null)
            {
                lblChecklistMsg.Text = Tr("lblChecklistVacio", "Aún no hay checklist para esta unidad.");
                dgvChecklist.DataSource = null;
                // Sin checklist (unidad en Ingresado) escondemos la grilla y los controles de edición.
                dgvChecklist.Visible = false;
                btnAgregarItemExtra.Visible = false;
                btnQuitarItemExtra.Visible = false;
                lblResultado.Visible = false;
                txtResultadoItem.Visible = false;
                btnMarcarItemEjecutado.Visible = false;
                btnMarcarObservado.Visible = false;
                return;
            }
            // Hay checklist: aseguramos que la grilla esté visible (ActualizarBotonesAccion
            // decide después qué botones mostrar según el estado).
            dgvChecklist.Visible = true;
            lblChecklistMsg.Text = string.Format(Tr("lblChecklistInfo", "Checklist creado el {0} — {1} items."),
                _unidad.Checklist.FechaCreacion.ToString("g"), _unidad.Checklist.Items.Count);
            dgvChecklist.AutoGenerateColumns = false;
            dgvChecklist.DataSource = null;
            dgvChecklist.DataSource = _unidad.Checklist.Items.ToList();

            // Preservar la selección del ítem si el handler lo pidió (ej. tras Marcar OK / Observado).
            if (_itemChecklistASeleccionar.HasValue)
            {
                int idBuscado = _itemChecklistASeleccionar.Value;
                _itemChecklistASeleccionar = null;
                foreach (DataGridViewRow row in dgvChecklist.Rows)
                {
                    if (row.DataBoundItem is ChecklistItem ci && ci.Id == idBuscado)
                    {
                        row.Selected = true;
                        dgvChecklist.CurrentCell = row.Cells[0];
                        break;
                    }
                }
            }
        }

        private void PintarHistorial()
        {
            dgvHistorial.AutoGenerateColumns = false;
            dgvHistorial.DataSource = null;
            dgvHistorial.DataSource = _unidad.Historial;
        }

        private void dgvChecklist_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var col = dgvChecklist.Columns[e.ColumnIndex].DataPropertyName;
            if (col == "CostoEstimado")
            {
                // Los items del template no tienen costo (sólo los extras) → mostramos "--".
                var row = dgvChecklist.Rows[e.RowIndex].DataBoundItem as ChecklistItem;
                if (row != null && row.EsDelTemplate)
                {
                    e.Value = "--";
                    e.FormattingApplied = true;
                    e.CellStyle.ForeColor = System.Drawing.Color.Gray;
                    e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                }
                else if (e.Value is decimal cost)
                {
                    e.Value = cost.ToString("N2");
                    e.FormattingApplied = true;
                }
            }
            if (col == "ResultadoRevision" && e.Value is ResultadoRevision rr)
            {
                e.Value = TrRevision(rr);
                e.FormattingApplied = true;
                if (rr == ResultadoRevision.OK)        { e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(30, 150, 70);   e.CellStyle.SelectionForeColor = System.Drawing.Color.White; e.CellStyle.Font = new System.Drawing.Font(dgvChecklist.Font, System.Drawing.FontStyle.Bold); }
                if (rr == ResultadoRevision.Observado) { e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 140, 10);  e.CellStyle.SelectionForeColor = System.Drawing.Color.White; e.CellStyle.Font = new System.Drawing.Font(dgvChecklist.Font, System.Drawing.FontStyle.Bold); }
            }
        }

        private void dgvHistorial_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var col = dgvHistorial.Columns[e.ColumnIndex].DataPropertyName;
            if (col == "EstadoOrigen" && e.Value is EstadoUnidad o)
            {
                e.Value = TrEstado(o);
                e.FormattingApplied = true;
                var c = ColoresEstadoUnidad.Obtener(o);
                e.CellStyle.ForeColor = c;
                e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                e.CellStyle.Font = new System.Drawing.Font(dgvHistorial.Font, System.Drawing.FontStyle.Bold);
            }
            if (col == "Usuario" && e.Value is Usuario usr)
            {
                e.Value = usr.Username;
                e.FormattingApplied = true;
            }
            if (col == "EstadoDestino" && e.Value is EstadoUnidad d)
            {
                e.Value = TrEstado(d);
                e.FormattingApplied = true;
                var c = ColoresEstadoUnidad.Obtener(d);
                e.CellStyle.ForeColor = c;
                e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                e.CellStyle.Font = new System.Drawing.Font(dgvHistorial.Font, System.Drawing.FontStyle.Bold);
            }
        }

        // ------------------------------------------------------------
        // Botones de acción contextual
        // ------------------------------------------------------------

        private bool Tiene(string permiso)
        {
            var u = SesionUsuario.GetInstancia().Usuario;
            if (u != null && u.Username.ToLower() == "admin") return true;
            return u != null && _permisoService.UsuarioTienePermiso(u, permiso);
        }

        private void ActualizarBotonesAccion()
        {
            var estado = _unidad.EstadoActual;

            // El Encargado toma la unidad (Ingresado → EnPreparacion). Ahora en el nuevo flujo
            // ya no hay "marcar impecable" ni "enviar a autorización": se reemplazan por
            // "Preparar Unidad" (y luego "Finalizar preparación" si no detecta nada).
            btnTomarPreparacion.Visible = estado == EstadoUnidad.Ingresado && Tiene("REVISAR_UNIDAD");
            btnEnviarPresupuesto.Visible = estado == EstadoUnidad.EnPreparacion && Tiene("REVISAR_UNIDAD");
            // Gerente aprueba/rechaza el presupuesto de extras.
            btnAprobarPresupuesto.Visible = estado == EstadoUnidad.RequiereAprobacionPresupuesto && Tiene("AUTORIZAR_CHECKLIST");
            btnRechazarPresupuesto.Visible = estado == EstadoUnidad.RequiereAprobacionPresupuesto && Tiene("AUTORIZAR_CHECKLIST");
            btnFinalizarPreparacion.Visible = estado == EstadoUnidad.EnPreparacion && Tiene("EJECUTAR_PREPARACION");
            btnAutorizarPublicacion.Visible = estado == EstadoUnidad.PendienteVenta && Tiene("AUTORIZAR_PUBLICACION");
            btnRechazarPublicacion.Visible = estado == EstadoUnidad.PendienteVenta && Tiene("AUTORIZAR_PUBLICACION");

            // Los extras se agregan/quitan en EnPreparacion (ya no en Ingresado).
            bool puedeEditarItems = estado == EstadoUnidad.EnPreparacion && Tiene("REVISAR_UNIDAD");
            btnAgregarItemExtra.Visible = puedeEditarItems;
            btnQuitarItemExtra.Visible = puedeEditarItems;

            // N02 — Botones de venta/reserva/pausa + publicación.
            bool enEstadoVendible = estado == EstadoUnidad.EnVenta || estado == EstadoUnidad.Reservado
                                                                   || estado == EstadoUnidad.Pausado
                                                                   || estado == EstadoUnidad.PendienteVenta;
            btnPublicacion.Visible     = enEstadoVendible && Tiene("GESTIONAR_PUBLICACION");
            btnVender.Visible          = (estado == EstadoUnidad.EnVenta || estado == EstadoUnidad.Reservado) && Tiene("REGISTRAR_VENTA");
            btnReservar.Visible        = estado == EstadoUnidad.EnVenta  && Tiene("GESTIONAR_RESERVA");
            btnPausar.Visible          = estado == EstadoUnidad.EnVenta  && Tiene("PAUSAR_UNIDAD");
            btnReanudar.Visible        = estado == EstadoUnidad.Pausado  && Tiene("PAUSAR_UNIDAD");
            btnCancelarReserva.Visible = estado == EstadoUnidad.Reservado && Tiene("GESTIONAR_RESERVA");

            bool puedeEjecutarItems = estado == EstadoUnidad.EnPreparacion && Tiene("EJECUTAR_PREPARACION");
            btnMarcarItemEjecutado.Visible = puedeEjecutarItems;
            btnMarcarObservado.Visible     = puedeEjecutarItems;
            txtResultadoItem.Visible       = puedeEjecutarItems;
            lblResultado.Visible           = puedeEjecutarItems;
        }

        // ------------------------------------------------------------
        // Handlers de acción
        // ------------------------------------------------------------

        private void btnTomarPreparacion_Click(object sender, EventArgs e)
        {
            // "Preparar Unidad": Ingresado → EnPreparacion. Crea el checklist si no existe.
            EjecutarAccion(() => _unidadService.TomarParaPreparacion(_unidadId));
        }

        private void btnEnviarPresupuesto_Click(object sender, EventArgs e)
        {
            // "Enviar presupuesto": EnPreparacion → RequiereAprobacionPresupuesto.
            EjecutarAccion(() => _unidadService.EnviarPresupuestoAGerente(_unidadId));
        }

        private void btnAprobarPresupuesto_Click(object sender, EventArgs e)
        {
            // Gerente aprueba el presupuesto de extras: RequiereAprobacionPresupuesto → EnPreparacion.
            string obs = PromptInput(Tr("promptObservaciones", "Observaciones (opcional):"));
            EjecutarAccion(() => _unidadService.AprobarPresupuesto(_unidadId, obs));
        }

        private void btnRechazarPresupuesto_Click(object sender, EventArgs e)
        {
            // Gerente rechaza el presupuesto de extras: RequiereAprobacionPresupuesto → EnPreparacion.
            string motivo = PromptInput(Tr("promptMotivoRechazo", "Motivo del rechazo (obligatorio):"));
            if (string.IsNullOrWhiteSpace(motivo)) return;
            EjecutarAccion(() => _unidadService.RechazarPresupuesto(_unidadId, motivo));
        }

        private void btnFinalizarPreparacion_Click(object sender, EventArgs e)
        {
            EjecutarAccion(() => _unidadService.FinalizarPreparacion(_unidadId));
        }

        private void btnAutorizarPublicacion_Click(object sender, EventArgs e)
        {
            string obs = PromptInput(Tr("promptObservaciones", "Observaciones (opcional):"));
            EjecutarAccion(() => _unidadService.AutorizarPublicacion(_unidadId, obs));
        }

        private void btnRechazarPublicacion_Click(object sender, EventArgs e)
        {
            string motivo = PromptInput(Tr("promptMotivoRechazo", "Motivo del rechazo (obligatorio):"));
            if (string.IsNullOrWhiteSpace(motivo)) return;
            EjecutarAccion(() => _unidadService.RechazarPublicacion(_unidadId, motivo));
        }

        private void btnAgregarItemExtra_Click(object sender, EventArgs e)
        {
            string nombre = PromptInput(Tr("promptNombreItem", "Nombre del item extra:"));
            if (string.IsNullOrWhiteSpace(nombre)) return;
            string desc = PromptInput(Tr("promptDescripcionItem", "Descripción (opcional):"));
            string costoTxt = PromptInput(Tr("promptCostoItem", "Costo estimado (en pesos):"));
            decimal? costo = null;
            if (!string.IsNullOrWhiteSpace(costoTxt))
            {
                if (!decimal.TryParse(costoTxt.Trim(), System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.CurrentCulture, out decimal c) || c < 0)
                {
                    MessageBox.Show(Tr("msgCostoInvalido", "Costo inválido."),
                        Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                costo = c;
            }
            EjecutarAccion(() => _unidadService.AgregarItemExtra(_unidadId, nombre, desc, costo));
        }

        private void btnQuitarItemExtra_Click(object sender, EventArgs e)
        {
            var item = ItemChecklistSeleccionado();
            if (item == null) { MessageBox.Show(Tr("msgSeleccionarItem", "Seleccioná un ítem."), Tr("msgInformacion", "Info")); return; }
            if (item.EsDelTemplate)
            {
                MessageBox.Show(Tr("msgNoBorrarTemplate", "No se puede eliminar un ítem del template."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EjecutarAccion(() => _unidadService.EliminarItemExtra(_unidadId, item.Id));
        }

        private void btnMarcarItemEjecutado_Click(object sender, EventArgs e)
        {
            var item = ItemChecklistSeleccionado();
            if (item == null) { MessageBox.Show(Tr("msgSeleccionarItem", "Seleccioná un ítem."), Tr("msgInformacion", "Info")); return; }
            string comentario = txtResultadoItem.Text?.Trim();
            _itemChecklistASeleccionar = item.Id;
            EjecutarAccion(() =>
            {
                _unidadService.MarcarItemRevisado(_unidadId, item.Id, ResultadoRevision.OK, comentario);
                txtResultadoItem.Clear();
            });
        }

        private void btnMarcarObservado_Click(object sender, EventArgs e)
        {
            var item = ItemChecklistSeleccionado();
            if (item == null) { MessageBox.Show(Tr("msgSeleccionarItem", "Seleccioná un ítem."), Tr("msgInformacion", "Info")); return; }
            string comentario = txtResultadoItem.Text?.Trim();
            if (string.IsNullOrWhiteSpace(comentario))
            {
                MessageBox.Show(Tr("msgComentarioObservadoObligatorio",
                    "Al marcar un ítem como Observado el comentario es obligatorio."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtResultadoItem.Focus();
                return;
            }
            _itemChecklistASeleccionar = item.Id;
            EjecutarAccion(() =>
            {
                _unidadService.MarcarItemRevisado(_unidadId, item.Id, ResultadoRevision.Observado, comentario);
                txtResultadoItem.Clear();
            });
        }

        private ChecklistItem ItemChecklistSeleccionado()
        {
            return dgvChecklist.CurrentRow?.DataBoundItem as ChecklistItem;
        }

        // ------------------------------------------------------------
        // N02 — Handlers de publicación / venta / reserva / pausa
        // ------------------------------------------------------------

        private void btnPublicacion_Click(object sender, EventArgs e)
        {
            using (var f = new FormPublicacion(_unidadId))
                f.ShowDialog(this);
            Recargar();
        }

        private void btnVender_Click(object sender, EventArgs e)
        {
            using (var f = new FormVender())
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;
                EjecutarAccion(() => _ventaService.Vender(_unidadId, f.IdPersonaComprador,
                    f.FechaOperacion, f.PrecioFinal, f.Descripcion));
            }
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            using (var f = new FormReservar())
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;
                EjecutarAccion(() => _ventaService.Reservar(_unidadId, f.IdPersonaComprador,
                    f.FechaOperacion, f.PrecioAcordado, f.MontoSena, f.FechaEstimadaFin, f.Descripcion));
            }
        }

        private void btnPausar_Click(object sender, EventArgs e)
        {
            using (var f = new FormPausar())
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;
                EjecutarAccion(() => _ventaService.Pausar(_unidadId, f.Motivo));
            }
        }

        private void btnReanudar_Click(object sender, EventArgs e)
        {
            EjecutarAccion(() => _ventaService.Reanudar(_unidadId));
        }

        private void btnCancelarReserva_Click(object sender, EventArgs e)
        {
            string com = PromptInput(Tr("promptMotivoCancelacion",
                "Motivo de la cancelación de la reserva (obligatorio):"));
            // PromptInput devuelve null cuando se cancela el diálogo → abortamos sin tocar el estado.
            if (com == null) return;
            if (string.IsNullOrWhiteSpace(com))
            {
                MessageBox.Show(Tr("msgMotivoCancelacionObligatorio",
                    "El motivo de la cancelación es obligatorio."),
                    Tr("msgAdvertencia", "Advertencia"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EjecutarAccion(() => _ventaService.CancelarReserva(_unidadId, com));
        }

        private void EjecutarAccion(Action accion)
        {
            try
            {
                accion();
                Recargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------------
        // Utils
        // ------------------------------------------------------------

        private string PromptInput(string label, string defaultValue = "")
        {
            using (var form = new Form())
            {
                form.Text = Tr("promptTitulo", "Ingresar");
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new System.Drawing.Size(420, 130);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;

                var lbl = new Label { Text = label, Left = 12, Top = 12, AutoSize = true };
                var txt = new TextBox { Left = 12, Top = 40, Width = 395, Text = defaultValue };
                var btnOk = new Button { Text = Tr("btnAceptar", "Aceptar"), Left = 235, Top = 80, DialogResult = DialogResult.OK, Width = 80 };
                var btnCancel = new Button { Text = Tr("btnCancelar", "Cancelar"), Left = 320, Top = 80, DialogResult = DialogResult.Cancel, Width = 80 };
                form.Controls.AddRange(new System.Windows.Forms.Control[] { lbl, txt, btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }

        private void FormDetalleUnidad_Load(object sender, EventArgs e)
        {
            AjustarAPantalla();
            this.Resize += (s, ev) => AcomodarLayout();
            // Las pestañas no visibles pueden recibir su tamaño recién al mostrarse.
            tabChecklist.Resize += (s, ev) => AcomodarTabChecklist();
            AcomodarLayout();
        }

        // Abre el modal ocupando ~92% del alto de la pantalla (y sin pasarse del ancho), centrado.
        // En monitores grandes queda más alto que el diseño; en notebooks entra completo sin tener que agrandarlo.
        private void AjustarAPantalla()
        {
            var area = Screen.FromControl(Owner ?? this).WorkingArea;
            int alto  = (int)(area.Height * 0.92);
            int ancho = Math.Min(this.Width, (int)(area.Width * 0.95));
            this.Bounds = new System.Drawing.Rectangle(
                area.Left + (area.Width - ancho) / 2,
                area.Top + (area.Height - alto) / 2,
                ancho, alto);
        }

        // Las pestañas ocupan todo el espacio disponible y el panel de acciones queda siempre pegado abajo.
        private void AcomodarLayout()
        {
            const int margen = 12;
            int ancho = Math.Max(300, ClientSize.Width - tabs.Left * 2);

            pnlAcciones.Width = ancho;
            pnlAcciones.Top   = ClientSize.Height - pnlAcciones.Height - margen;

            tabs.Width  = ancho;
            tabs.Height = Math.Max(150, pnlAcciones.Top - margen - tabs.Top);

            AcomodarTabChecklist();
        }

        // Controles que van debajo de la grilla del checklist, con su distancia vertical original
        // al borde inferior de la grilla (se calcula una sola vez, con el layout del diseñador).
        private System.Collections.Generic.Dictionary<System.Windows.Forms.Control, int> _offsetsBajoChecklist;

        // La grilla del checklist crece/se achica con la pestaña; los botones y el campo de resultado
        // la siguen manteniendo su separación original.
        private void AcomodarTabChecklist()
        {
            const int margen = 12;
            System.Windows.Forms.Control[] bajoGrilla =
            {
                btnAgregarItemExtra, btnQuitarItemExtra, lblResultado,
                txtResultadoItem, btnMarcarItemEjecutado, btnMarcarObservado
            };

            if (_offsetsBajoChecklist == null)
            {
                _offsetsBajoChecklist = new System.Collections.Generic.Dictionary<System.Windows.Forms.Control, int>();
                foreach (var c in bajoGrilla)
                    _offsetsBajoChecklist[c] = c.Top - dgvChecklist.Bottom;
            }

            int altoBloque = bajoGrilla.Max(c => _offsetsBajoChecklist[c] + c.Height);
            var pagina = tabChecklist.ClientSize;

            dgvChecklist.Width  = Math.Max(200, pagina.Width - dgvChecklist.Left * 2);
            dgvChecklist.Height = Math.Max(80, pagina.Height - dgvChecklist.Top - altoBloque - margen);

            foreach (var c in bajoGrilla)
                c.Top = dgvChecklist.Bottom + _offsetsBajoChecklist[c];
        }
    }
}
