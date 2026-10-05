namespace UI
{
    partial class FormDetalleUnidad
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabDatos;
        private System.Windows.Forms.TextBox txtDatos;
        private System.Windows.Forms.TabPage tabChecklist;
        private System.Windows.Forms.Label lblChecklistMsg;
        private System.Windows.Forms.DataGridView dgvChecklist;
        private System.Windows.Forms.Button btnAgregarItemExtra;
        private System.Windows.Forms.Button btnQuitarItemExtra;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.TextBox txtResultadoItem;
        private System.Windows.Forms.Button btnMarcarOK;
        private System.Windows.Forms.Button btnMarcarObservado;
        private System.Windows.Forms.TabPage tabHistorial;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnTomarPreparacion;
        private System.Windows.Forms.Button btnEnviarPresupuesto;
        private System.Windows.Forms.Button btnAprobarPresupuesto;
        private System.Windows.Forms.Button btnRechazarPresupuesto;
        private System.Windows.Forms.Button btnFinalizarPreparacion;
        private System.Windows.Forms.Button btnAutorizarPublicacion;
        private System.Windows.Forms.Button btnRechazarPublicacion;
        private System.Windows.Forms.Button btnPublicacion;
        private System.Windows.Forms.Button btnVender;
        private System.Windows.Forms.Button btnReservar;
        private System.Windows.Forms.Button btnPausar;
        private System.Windows.Forms.Button btnReanudar;
        private System.Windows.Forms.Button btnCancelarReserva;

        private System.Windows.Forms.DataGridViewTextBoxColumn colChkNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkCosto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkRevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkComentario;

        private System.Windows.Forms.DataGridViewTextBoxColumn colHistFechaHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistOrigen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistDestino;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistMotivo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabDatos = new System.Windows.Forms.TabPage();
            this.txtDatos = new System.Windows.Forms.TextBox();
            this.tabChecklist = new System.Windows.Forms.TabPage();
            this.lblChecklistMsg = new System.Windows.Forms.Label();
            this.dgvChecklist = new System.Windows.Forms.DataGridView();
            this.btnAgregarItemExtra = new System.Windows.Forms.Button();
            this.btnQuitarItemExtra = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.txtResultadoItem = new System.Windows.Forms.TextBox();
            this.btnMarcarOK = new System.Windows.Forms.Button();
            this.btnMarcarObservado = new System.Windows.Forms.Button();
            this.tabHistorial = new System.Windows.Forms.TabPage();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.btnTomarPreparacion = new System.Windows.Forms.Button();
            this.btnEnviarPresupuesto = new System.Windows.Forms.Button();
            this.btnAprobarPresupuesto = new System.Windows.Forms.Button();
            this.btnRechazarPresupuesto = new System.Windows.Forms.Button();
            this.btnFinalizarPreparacion = new System.Windows.Forms.Button();
            this.btnAutorizarPublicacion = new System.Windows.Forms.Button();
            this.btnRechazarPublicacion = new System.Windows.Forms.Button();
            this.btnPublicacion = new System.Windows.Forms.Button();
            this.btnVender = new System.Windows.Forms.Button();
            this.btnReservar = new System.Windows.Forms.Button();
            this.btnPausar = new System.Windows.Forms.Button();
            this.btnReanudar = new System.Windows.Forms.Button();
            this.btnCancelarReserva = new System.Windows.Forms.Button();
            this.colChkNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkCosto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkComentario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistFechaHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistOrigen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistDestino = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistMotivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabs.SuspendLayout();
            this.tabDatos.SuspendLayout();
            this.tabChecklist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecklist)).BeginInit();
            this.tabHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(26, 20);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(249, 38);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Detalle de unidad";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblEstado.Location = new System.Drawing.Point(26, 67);
            this.lblEstado.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(106, 28);
            this.lblEstado.TabIndex = 1;
            this.lblEstado.Text = "Estado: —";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabDatos);
            this.tabs.Controls.Add(this.tabChecklist);
            this.tabs.Controls.Add(this.tabHistorial);
            this.tabs.Location = new System.Drawing.Point(26, 107);
            this.tabs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1054, 667);
            this.tabs.TabIndex = 2;
            // 
            // tabDatos
            // 
            this.tabDatos.Controls.Add(this.txtDatos);
            this.tabDatos.Location = new System.Drawing.Point(4, 29);
            this.tabDatos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabDatos.Name = "tabDatos";
            this.tabDatos.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.tabDatos.Size = new System.Drawing.Size(1046, 634);
            this.tabDatos.TabIndex = 0;
            this.tabDatos.Text = "Datos";
            this.tabDatos.UseVisualStyleBackColor = true;
            // 
            // txtDatos
            // 
            this.txtDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDatos.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtDatos.Location = new System.Drawing.Point(13, 13);
            this.txtDatos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDatos.Multiline = true;
            this.txtDatos.Name = "txtDatos";
            this.txtDatos.ReadOnly = true;
            this.txtDatos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDatos.Size = new System.Drawing.Size(1020, 608);
            this.txtDatos.TabIndex = 0;
            // 
            // tabChecklist
            // 
            this.tabChecklist.Controls.Add(this.lblChecklistMsg);
            this.tabChecklist.Controls.Add(this.dgvChecklist);
            this.tabChecklist.Controls.Add(this.btnAgregarItemExtra);
            this.tabChecklist.Controls.Add(this.btnQuitarItemExtra);
            this.tabChecklist.Controls.Add(this.lblResultado);
            this.tabChecklist.Controls.Add(this.txtResultadoItem);
            this.tabChecklist.Controls.Add(this.btnMarcarOK);
            this.tabChecklist.Controls.Add(this.btnMarcarObservado);
            this.tabChecklist.Location = new System.Drawing.Point(4, 29);
            this.tabChecklist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabChecklist.Name = "tabChecklist";
            this.tabChecklist.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.tabChecklist.Size = new System.Drawing.Size(1046, 634);
            this.tabChecklist.TabIndex = 1;
            this.tabChecklist.Text = "Checklist";
            this.tabChecklist.UseVisualStyleBackColor = true;
            // 
            // lblChecklistMsg
            // 
            this.lblChecklistMsg.AutoSize = true;
            this.lblChecklistMsg.Location = new System.Drawing.Point(19, 13);
            this.lblChecklistMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChecklistMsg.Name = "lblChecklistMsg";
            this.lblChecklistMsg.Size = new System.Drawing.Size(23, 20);
            this.lblChecklistMsg.TabIndex = 0;
            this.lblChecklistMsg.Text = "—";
            // 
            // dgvChecklist
            // 
            this.dgvChecklist.AllowUserToAddRows = false;
            this.dgvChecklist.AllowUserToDeleteRows = false;
            this.dgvChecklist.AutoGenerateColumns = false;
            this.dgvChecklist.ColumnHeadersHeight = 34;
            this.dgvChecklist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChkNombre,
            this.colChkDescripcion,
            this.colChkCosto,
            this.colChkRevision,
            this.colChkComentario});
            this.dgvChecklist.Location = new System.Drawing.Point(19, 53);
            this.dgvChecklist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvChecklist.MultiSelect = false;
            this.dgvChecklist.Name = "dgvChecklist";
            this.dgvChecklist.ReadOnly = true;
            this.dgvChecklist.RowHeadersVisible = false;
            this.dgvChecklist.RowHeadersWidth = 62;
            this.dgvChecklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChecklist.Size = new System.Drawing.Size(1003, 350);
            this.dgvChecklist.TabIndex = 1;
            this.dgvChecklist.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvChecklist_CellFormatting);
            // 
            // btnAgregarItemExtra
            // 
            this.btnAgregarItemExtra.Location = new System.Drawing.Point(19, 415);
            this.btnAgregarItemExtra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAgregarItemExtra.Name = "btnAgregarItemExtra";
            this.btnAgregarItemExtra.Size = new System.Drawing.Size(180, 35);
            this.btnAgregarItemExtra.TabIndex = 2;
            this.btnAgregarItemExtra.Text = "Agregar item extra";
            this.btnAgregarItemExtra.UseVisualStyleBackColor = true;
            this.btnAgregarItemExtra.Click += new System.EventHandler(this.btnAgregarItemExtra_Click);
            // 
            // btnQuitarItemExtra
            // 
            this.btnQuitarItemExtra.Location = new System.Drawing.Point(212, 415);
            this.btnQuitarItemExtra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnQuitarItemExtra.Name = "btnQuitarItemExtra";
            this.btnQuitarItemExtra.Size = new System.Drawing.Size(180, 35);
            this.btnQuitarItemExtra.TabIndex = 3;
            this.btnQuitarItemExtra.Text = "Quitar item extra";
            this.btnQuitarItemExtra.UseVisualStyleBackColor = true;
            this.btnQuitarItemExtra.Click += new System.EventHandler(this.btnQuitarItemExtra_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(19, 465);
            this.lblResultado.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(242, 20);
            this.lblResultado.TabIndex = 4;
            this.lblResultado.Text = "Resultado del ítem seleccionado:";
            // 
            // txtResultadoItem
            // 
            this.txtResultadoItem.Location = new System.Drawing.Point(19, 495);
            this.txtResultadoItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtResultadoItem.Name = "txtResultadoItem";
            this.txtResultadoItem.Size = new System.Drawing.Size(590, 26);
            this.txtResultadoItem.TabIndex = 5;
            // 
            // btnMarcarOK
            // 
            this.btnMarcarOK.Location = new System.Drawing.Point(624, 492);
            this.btnMarcarOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMarcarOK.Name = "btnMarcarOK";
            this.btnMarcarOK.Size = new System.Drawing.Size(193, 37);
            this.btnMarcarOK.TabIndex = 6;
            this.btnMarcarOK.Text = "Marcar OK";
            this.btnMarcarOK.UseVisualStyleBackColor = true;
            this.btnMarcarOK.Click += new System.EventHandler(this.btnMarcarOK_Click);
            // 
            // btnMarcarObservado
            // 
            this.btnMarcarObservado.Location = new System.Drawing.Point(829, 492);
            this.btnMarcarObservado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMarcarObservado.Name = "btnMarcarObservado";
            this.btnMarcarObservado.Size = new System.Drawing.Size(193, 37);
            this.btnMarcarObservado.TabIndex = 7;
            this.btnMarcarObservado.Text = "Marcar Observado";
            this.btnMarcarObservado.UseVisualStyleBackColor = true;
            this.btnMarcarObservado.Click += new System.EventHandler(this.btnMarcarObservado_Click);
            // 
            // tabHistorial
            // 
            this.tabHistorial.Controls.Add(this.dgvHistorial);
            this.tabHistorial.Location = new System.Drawing.Point(4, 29);
            this.tabHistorial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabHistorial.Name = "tabHistorial";
            this.tabHistorial.Padding = new System.Windows.Forms.Padding(13, 13, 13, 13);
            this.tabHistorial.Size = new System.Drawing.Size(1046, 634);
            this.tabHistorial.TabIndex = 2;
            this.tabHistorial.Text = "Historial";
            this.tabHistorial.UseVisualStyleBackColor = true;
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.AutoGenerateColumns = false;
            this.dgvHistorial.ColumnHeadersHeight = 34;
            this.dgvHistorial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHistFechaHora,
            this.colHistOrigen,
            this.colHistDestino,
            this.colHistUsuario,
            this.colHistMotivo});
            this.dgvHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistorial.Location = new System.Drawing.Point(13, 13);
            this.dgvHistorial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersVisible = false;
            this.dgvHistorial.RowHeadersWidth = 62;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.Size = new System.Drawing.Size(1020, 608);
            this.dgvHistorial.TabIndex = 0;
            this.dgvHistorial.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvHistorial_CellFormatting);
            //
            // colChkNombre
            //
            this.colChkNombre.DataPropertyName = "Nombre";
            this.colChkNombre.HeaderText = "Nombre";
            this.colChkNombre.Name = "colChkNombre";
            this.colChkNombre.Width = 200;
            //
            // colChkDescripcion
            //
            this.colChkDescripcion.DataPropertyName = "Descripcion";
            this.colChkDescripcion.HeaderText = "Descripción";
            this.colChkDescripcion.Name = "colChkDescripcion";
            this.colChkDescripcion.Width = 220;
            //
            // colChkCosto
            //
            this.colChkCosto.DataPropertyName = "CostoEstimado";
            this.colChkCosto.HeaderText = "Costo";
            this.colChkCosto.Name = "colChkCosto";
            this.colChkCosto.Width = 90;
            //
            // colChkRevision
            //
            this.colChkRevision.DataPropertyName = "ResultadoRevision";
            this.colChkRevision.HeaderText = "Revisión";
            this.colChkRevision.Name = "colChkRevision";
            this.colChkRevision.Width = 90;
            //
            // colChkComentario
            //
            this.colChkComentario.DataPropertyName = "ComentarioRevision";
            this.colChkComentario.HeaderText = "Comentario";
            this.colChkComentario.Name = "colChkComentario";
            this.colChkComentario.Width = 200;
            //
            // colHistFechaHora
            //
            this.colHistFechaHora.DataPropertyName = "FechaHora";
            this.colHistFechaHora.HeaderText = "Fecha/Hora";
            this.colHistFechaHora.Name = "colHistFechaHora";
            this.colHistFechaHora.Width = 140;
            //
            // colHistOrigen
            //
            this.colHistOrigen.DataPropertyName = "EstadoOrigen";
            this.colHistOrigen.HeaderText = "Origen";
            this.colHistOrigen.Name = "colHistOrigen";
            this.colHistOrigen.Width = 170;
            //
            // colHistDestino
            //
            this.colHistDestino.DataPropertyName = "EstadoDestino";
            this.colHistDestino.HeaderText = "Destino";
            this.colHistDestino.Name = "colHistDestino";
            this.colHistDestino.Width = 170;
            //
            // colHistUsuario
            //
            this.colHistUsuario.DataPropertyName = "Usuario";
            this.colHistUsuario.HeaderText = "Usuario";
            this.colHistUsuario.Name = "colHistUsuario";
            this.colHistUsuario.Width = 120;
            //
            // colHistMotivo
            //
            this.colHistMotivo.DataPropertyName = "Motivo";
            this.colHistMotivo.HeaderText = "Motivo / obs.";
            this.colHistMotivo.Name = "colHistMotivo";
            this.colHistMotivo.Width = 170;
            //
            // pnlAcciones
            // 
            this.pnlAcciones.Controls.Add(this.btnTomarPreparacion);
            this.pnlAcciones.Controls.Add(this.btnEnviarPresupuesto);
            this.pnlAcciones.Controls.Add(this.btnAprobarPresupuesto);
            this.pnlAcciones.Controls.Add(this.btnRechazarPresupuesto);
            this.pnlAcciones.Controls.Add(this.btnFinalizarPreparacion);
            this.pnlAcciones.Controls.Add(this.btnAutorizarPublicacion);
            this.pnlAcciones.Controls.Add(this.btnRechazarPublicacion);
            this.pnlAcciones.Controls.Add(this.btnPublicacion);
            this.pnlAcciones.Controls.Add(this.btnVender);
            this.pnlAcciones.Controls.Add(this.btnReservar);
            this.pnlAcciones.Controls.Add(this.btnPausar);
            this.pnlAcciones.Controls.Add(this.btnReanudar);
            this.pnlAcciones.Controls.Add(this.btnCancelarReserva);
            this.pnlAcciones.Location = new System.Drawing.Point(26, 787);
            this.pnlAcciones.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(1054, 60);
            this.pnlAcciones.TabIndex = 3;
            // 
            // btnTomarPreparacion
            // 
            this.btnTomarPreparacion.Location = new System.Drawing.Point(13, 11);
            this.btnTomarPreparacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTomarPreparacion.Name = "btnTomarPreparacion";
            this.btnTomarPreparacion.Size = new System.Drawing.Size(193, 40);
            this.btnTomarPreparacion.TabIndex = 0;
            this.btnTomarPreparacion.Text = "Preparar Unidad";
            this.btnTomarPreparacion.UseVisualStyleBackColor = true;
            this.btnTomarPreparacion.Visible = false;
            this.btnTomarPreparacion.Click += new System.EventHandler(this.btnTomarPreparacion_Click);
            // 
            // btnEnviarPresupuesto
            // 
            this.btnEnviarPresupuesto.Location = new System.Drawing.Point(219, 11);
            this.btnEnviarPresupuesto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEnviarPresupuesto.Name = "btnEnviarPresupuesto";
            this.btnEnviarPresupuesto.Size = new System.Drawing.Size(193, 40);
            this.btnEnviarPresupuesto.TabIndex = 1;
            this.btnEnviarPresupuesto.Text = "Enviar presupuesto";
            this.btnEnviarPresupuesto.UseVisualStyleBackColor = true;
            this.btnEnviarPresupuesto.Visible = false;
            this.btnEnviarPresupuesto.Click += new System.EventHandler(this.btnEnviarPresupuesto_Click);
            // 
            // btnAprobarPresupuesto
            // 
            this.btnAprobarPresupuesto.Location = new System.Drawing.Point(424, 11);
            this.btnAprobarPresupuesto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAprobarPresupuesto.Name = "btnAprobarPresupuesto";
            this.btnAprobarPresupuesto.Size = new System.Drawing.Size(193, 40);
            this.btnAprobarPresupuesto.TabIndex = 2;
            this.btnAprobarPresupuesto.Text = "Aprobar presupuesto";
            this.btnAprobarPresupuesto.UseVisualStyleBackColor = true;
            this.btnAprobarPresupuesto.Visible = false;
            this.btnAprobarPresupuesto.Click += new System.EventHandler(this.btnAprobarPresupuesto_Click);
            // 
            // btnRechazarPresupuesto
            // 
            this.btnRechazarPresupuesto.Location = new System.Drawing.Point(630, 11);
            this.btnRechazarPresupuesto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRechazarPresupuesto.Name = "btnRechazarPresupuesto";
            this.btnRechazarPresupuesto.Size = new System.Drawing.Size(193, 40);
            this.btnRechazarPresupuesto.TabIndex = 3;
            this.btnRechazarPresupuesto.Text = "Rechazar presupuesto";
            this.btnRechazarPresupuesto.UseVisualStyleBackColor = true;
            this.btnRechazarPresupuesto.Visible = false;
            this.btnRechazarPresupuesto.Click += new System.EventHandler(this.btnRechazarPresupuesto_Click);
            // 
            // btnFinalizarPreparacion
            // 
            this.btnFinalizarPreparacion.Location = new System.Drawing.Point(424, 11);
            this.btnFinalizarPreparacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFinalizarPreparacion.Name = "btnFinalizarPreparacion";
            this.btnFinalizarPreparacion.Size = new System.Drawing.Size(193, 40);
            this.btnFinalizarPreparacion.TabIndex = 4;
            this.btnFinalizarPreparacion.Text = "Finalizar preparación";
            this.btnFinalizarPreparacion.UseVisualStyleBackColor = true;
            this.btnFinalizarPreparacion.Visible = false;
            this.btnFinalizarPreparacion.Click += new System.EventHandler(this.btnFinalizarPreparacion_Click);
            // 
            // btnAutorizarPublicacion
            // 
            this.btnAutorizarPublicacion.Location = new System.Drawing.Point(424, 11);
            this.btnAutorizarPublicacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAutorizarPublicacion.Name = "btnAutorizarPublicacion";
            this.btnAutorizarPublicacion.Size = new System.Drawing.Size(193, 40);
            this.btnAutorizarPublicacion.TabIndex = 5;
            this.btnAutorizarPublicacion.Text = "Autorizar publicación";
            this.btnAutorizarPublicacion.UseVisualStyleBackColor = true;
            this.btnAutorizarPublicacion.Visible = false;
            this.btnAutorizarPublicacion.Click += new System.EventHandler(this.btnAutorizarPublicacion_Click);
            // 
            // btnRechazarPublicacion
            // 
            this.btnRechazarPublicacion.Location = new System.Drawing.Point(630, 11);
            this.btnRechazarPublicacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRechazarPublicacion.Name = "btnRechazarPublicacion";
            this.btnRechazarPublicacion.Size = new System.Drawing.Size(193, 40);
            this.btnRechazarPublicacion.TabIndex = 6;
            this.btnRechazarPublicacion.Text = "Rechazar publicación";
            this.btnRechazarPublicacion.UseVisualStyleBackColor = true;
            this.btnRechazarPublicacion.Visible = false;
            this.btnRechazarPublicacion.Click += new System.EventHandler(this.btnRechazarPublicacion_Click);
            // 
            // btnPublicacion
            // 
            this.btnPublicacion.Location = new System.Drawing.Point(13, 11);
            this.btnPublicacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPublicacion.Name = "btnPublicacion";
            this.btnPublicacion.Size = new System.Drawing.Size(193, 40);
            this.btnPublicacion.TabIndex = 7;
            this.btnPublicacion.Text = "Publicación";
            this.btnPublicacion.UseVisualStyleBackColor = true;
            this.btnPublicacion.Visible = false;
            this.btnPublicacion.Click += new System.EventHandler(this.btnPublicacion_Click);
            // 
            // btnVender
            // 
            this.btnVender.Location = new System.Drawing.Point(219, 11);
            this.btnVender.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVender.Name = "btnVender";
            this.btnVender.Size = new System.Drawing.Size(193, 40);
            this.btnVender.TabIndex = 8;
            this.btnVender.Text = "Vender";
            this.btnVender.UseVisualStyleBackColor = true;
            this.btnVender.Visible = false;
            this.btnVender.Click += new System.EventHandler(this.btnVender_Click);
            // 
            // btnReservar
            // 
            this.btnReservar.Location = new System.Drawing.Point(424, 11);
            this.btnReservar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Size = new System.Drawing.Size(193, 40);
            this.btnReservar.TabIndex = 9;
            this.btnReservar.Text = "Reservar";
            this.btnReservar.UseVisualStyleBackColor = true;
            this.btnReservar.Visible = false;
            this.btnReservar.Click += new System.EventHandler(this.btnReservar_Click);
            // 
            // btnPausar
            // 
            this.btnPausar.Location = new System.Drawing.Point(630, 11);
            this.btnPausar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPausar.Name = "btnPausar";
            this.btnPausar.Size = new System.Drawing.Size(193, 40);
            this.btnPausar.TabIndex = 10;
            this.btnPausar.Text = "Pausar";
            this.btnPausar.UseVisualStyleBackColor = true;
            this.btnPausar.Visible = false;
            this.btnPausar.Click += new System.EventHandler(this.btnPausar_Click);
            // 
            // btnReanudar
            // 
            this.btnReanudar.Location = new System.Drawing.Point(219, 11);
            this.btnReanudar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReanudar.Name = "btnReanudar";
            this.btnReanudar.Size = new System.Drawing.Size(193, 40);
            this.btnReanudar.TabIndex = 11;
            this.btnReanudar.Text = "Reanudar";
            this.btnReanudar.UseVisualStyleBackColor = true;
            this.btnReanudar.Visible = false;
            this.btnReanudar.Click += new System.EventHandler(this.btnReanudar_Click);
            // 
            // btnCancelarReserva
            // 
            this.btnCancelarReserva.Location = new System.Drawing.Point(424, 11);
            this.btnCancelarReserva.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelarReserva.Name = "btnCancelarReserva";
            this.btnCancelarReserva.Size = new System.Drawing.Size(193, 40);
            this.btnCancelarReserva.TabIndex = 12;
            this.btnCancelarReserva.Text = "Cancelar reserva";
            this.btnCancelarReserva.UseVisualStyleBackColor = true;
            this.btnCancelarReserva.Visible = false;
            this.btnCancelarReserva.Click += new System.EventHandler(this.btnCancelarReserva_Click);
            // 
            // FormDetalleUnidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 867);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.pnlAcciones);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormDetalleUnidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalle de unidad";
            this.Load += new System.EventHandler(this.FormDetalleUnidad_Load);
            this.tabs.ResumeLayout(false);
            this.tabDatos.ResumeLayout(false);
            this.tabDatos.PerformLayout();
            this.tabChecklist.ResumeLayout(false);
            this.tabChecklist.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecklist)).EndInit();
            this.tabHistorial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
