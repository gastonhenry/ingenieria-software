namespace UI
{
    partial class FormPublicaciones
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblBuscarDominio = new System.Windows.Forms.Label();
            this.txtBuscarDominio = new System.Windows.Forms.TextBox();
            this.lblFiltroMarca = new System.Windows.Forms.Label();
            this.cmbMarca = new System.Windows.Forms.ComboBox();
            this.lblFiltroModelo = new System.Windows.Forms.Label();
            this.cmbModelo = new System.Windows.Forms.ComboBox();
            this.lblFiltroAnio = new System.Windows.Forms.Label();
            this.cmbAnio = new System.Windows.Forms.ComboBox();
            this.lblPrecioDesde = new System.Windows.Forms.Label();
            this.txtPrecioDesde = new System.Windows.Forms.TextBox();
            this.lblPrecioHasta = new System.Windows.Forms.Label();
            this.txtPrecioHasta = new System.Windows.Forms.TextBox();
            this.lblKmHasta = new System.Windows.Forms.Label();
            this.txtKmHasta = new System.Windows.Forms.TextBox();
            this.lblBuscarTexto = new System.Windows.Forms.Label();
            this.txtBuscarTexto = new System.Windows.Forms.TextBox();
            this.lblFiltroEstado = new System.Windows.Forms.Label();
            this.chkEnVenta = new System.Windows.Forms.CheckBox();
            this.chkPendienteVenta = new System.Windows.Forms.CheckBox();
            this.chkSoloConPrecio = new System.Windows.Forms.CheckBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.pnlPie = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnVerDetalle = new System.Windows.Forms.Button();
            this.tlpCuerpo = new System.Windows.Forms.TableLayoutPanel();
            this.dgvPublicaciones = new System.Windows.Forms.DataGridView();
            this.colDominio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFotos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualizacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetalle = new System.Windows.Forms.Panel();
            this.lblDetalleVacio = new System.Windows.Forms.Label();
            this.tlpDetalle = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDetalleHeader = new System.Windows.Forms.Panel();
            this.lblEstadoBadge = new System.Windows.Forms.Label();
            this.btnEditarPublicacion = new System.Windows.Forms.Button();
            this.lblTituloPub = new System.Windows.Forms.Label();
            this.lblSubtituloPub = new System.Windows.Forms.Label();
            this.lblPrecioPub = new System.Windows.Forms.Label();
            this.lblFechasPub = new System.Windows.Forms.Label();
            this.lblDescripcionTitulo = new System.Windows.Forms.Label();
            this.txtDescripcionPub = new System.Windows.Forms.TextBox();
            this.lblFotosTitulo = new System.Windows.Forms.Label();
            this.picPrincipal = new System.Windows.Forms.PictureBox();
            this.lblSinFotos = new System.Windows.Forms.Label();
            this.flowMiniaturas = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFiltros.SuspendLayout();
            this.pnlPie.SuspendLayout();
            this.tlpCuerpo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPublicaciones)).BeginInit();
            this.pnlDetalle.SuspendLayout();
            this.tlpDetalle.SuspendLayout();
            this.pnlDetalleHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPrincipal)).BeginInit();
            this.SuspendLayout();
            //
            // pnlFiltros
            //
            this.pnlFiltros.Controls.Add(this.lblTitulo);
            this.pnlFiltros.Controls.Add(this.lblBuscarDominio);
            this.pnlFiltros.Controls.Add(this.txtBuscarDominio);
            this.pnlFiltros.Controls.Add(this.lblFiltroMarca);
            this.pnlFiltros.Controls.Add(this.cmbMarca);
            this.pnlFiltros.Controls.Add(this.lblFiltroModelo);
            this.pnlFiltros.Controls.Add(this.cmbModelo);
            this.pnlFiltros.Controls.Add(this.lblFiltroAnio);
            this.pnlFiltros.Controls.Add(this.cmbAnio);
            this.pnlFiltros.Controls.Add(this.lblPrecioDesde);
            this.pnlFiltros.Controls.Add(this.txtPrecioDesde);
            this.pnlFiltros.Controls.Add(this.lblPrecioHasta);
            this.pnlFiltros.Controls.Add(this.txtPrecioHasta);
            this.pnlFiltros.Controls.Add(this.lblKmHasta);
            this.pnlFiltros.Controls.Add(this.txtKmHasta);
            this.pnlFiltros.Controls.Add(this.lblBuscarTexto);
            this.pnlFiltros.Controls.Add(this.txtBuscarTexto);
            this.pnlFiltros.Controls.Add(this.lblFiltroEstado);
            this.pnlFiltros.Controls.Add(this.chkEnVenta);
            this.pnlFiltros.Controls.Add(this.chkPendienteVenta);
            this.pnlFiltros.Controls.Add(this.chkSoloConPrecio);
            this.pnlFiltros.Controls.Add(this.btnLimpiar);
            this.pnlFiltros.Controls.Add(this.btnActualizar);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 0);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(1200, 172);
            this.pnlFiltros.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(134, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Publicaciones";
            //
            // lblBuscarDominio
            //
            this.lblBuscarDominio.AutoSize = true;
            this.lblBuscarDominio.Location = new System.Drawing.Point(18, 57);
            this.lblBuscarDominio.Name = "lblBuscarDominio";
            this.lblBuscarDominio.TabIndex = 1;
            this.lblBuscarDominio.Text = "Dominio:";
            //
            // txtBuscarDominio
            //
            this.txtBuscarDominio.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBuscarDominio.Location = new System.Drawing.Point(110, 54);
            this.txtBuscarDominio.Name = "txtBuscarDominio";
            this.txtBuscarDominio.Size = new System.Drawing.Size(100, 23);
            this.txtBuscarDominio.TabIndex = 2;
            this.txtBuscarDominio.TextChanged += new System.EventHandler(this.filtroTexto_TextChanged);
            //
            // lblFiltroMarca
            //
            this.lblFiltroMarca.AutoSize = true;
            this.lblFiltroMarca.Location = new System.Drawing.Point(228, 57);
            this.lblFiltroMarca.Name = "lblFiltroMarca";
            this.lblFiltroMarca.TabIndex = 3;
            this.lblFiltroMarca.Text = "Marca:";
            //
            // cmbMarca
            //
            this.cmbMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMarca.Location = new System.Drawing.Point(290, 54);
            this.cmbMarca.Name = "cmbMarca";
            this.cmbMarca.Size = new System.Drawing.Size(150, 23);
            this.cmbMarca.TabIndex = 4;
            this.cmbMarca.SelectedIndexChanged += new System.EventHandler(this.cmbMarca_SelectedIndexChanged);
            //
            // lblFiltroModelo
            //
            this.lblFiltroModelo.AutoSize = true;
            this.lblFiltroModelo.Location = new System.Drawing.Point(458, 57);
            this.lblFiltroModelo.Name = "lblFiltroModelo";
            this.lblFiltroModelo.TabIndex = 5;
            this.lblFiltroModelo.Text = "Modelo:";
            //
            // cmbModelo
            //
            this.cmbModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModelo.Location = new System.Drawing.Point(525, 54);
            this.cmbModelo.Name = "cmbModelo";
            this.cmbModelo.Size = new System.Drawing.Size(150, 23);
            this.cmbModelo.TabIndex = 6;
            this.cmbModelo.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            //
            // lblFiltroAnio
            //
            this.lblFiltroAnio.AutoSize = true;
            this.lblFiltroAnio.Location = new System.Drawing.Point(693, 57);
            this.lblFiltroAnio.Name = "lblFiltroAnio";
            this.lblFiltroAnio.TabIndex = 7;
            this.lblFiltroAnio.Text = "Año:";
            //
            // cmbAnio
            //
            this.cmbAnio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnio.Location = new System.Drawing.Point(740, 54);
            this.cmbAnio.Name = "cmbAnio";
            this.cmbAnio.Size = new System.Drawing.Size(90, 23);
            this.cmbAnio.TabIndex = 8;
            this.cmbAnio.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            //
            // lblBuscarTexto
            //
            this.lblBuscarTexto.AutoSize = true;
            this.lblBuscarTexto.Location = new System.Drawing.Point(693, 97);
            this.lblBuscarTexto.Name = "lblBuscarTexto";
            this.lblBuscarTexto.TabIndex = 9;
            this.lblBuscarTexto.Text = "Descripción:";
            //
            // txtBuscarTexto
            //
            this.txtBuscarTexto.Location = new System.Drawing.Point(775, 94);
            this.txtBuscarTexto.Name = "txtBuscarTexto";
            this.txtBuscarTexto.Size = new System.Drawing.Size(140, 23);
            this.txtBuscarTexto.TabIndex = 10;
            this.txtBuscarTexto.TextChanged += new System.EventHandler(this.filtroTexto_TextChanged);
            //
            // lblPrecioDesde
            //
            this.lblPrecioDesde.AutoSize = true;
            this.lblPrecioDesde.Location = new System.Drawing.Point(18, 97);
            this.lblPrecioDesde.Name = "lblPrecioDesde";
            this.lblPrecioDesde.TabIndex = 11;
            this.lblPrecioDesde.Text = "Precio desde:";
            //
            // txtPrecioDesde
            //
            this.txtPrecioDesde.Location = new System.Drawing.Point(110, 94);
            this.txtPrecioDesde.Name = "txtPrecioDesde";
            this.txtPrecioDesde.Size = new System.Drawing.Size(100, 23);
            this.txtPrecioDesde.TabIndex = 12;
            this.txtPrecioDesde.TextChanged += new System.EventHandler(this.filtroTexto_TextChanged);
            //
            // lblPrecioHasta
            //
            this.lblPrecioHasta.AutoSize = true;
            this.lblPrecioHasta.Location = new System.Drawing.Point(228, 97);
            this.lblPrecioHasta.Name = "lblPrecioHasta";
            this.lblPrecioHasta.TabIndex = 13;
            this.lblPrecioHasta.Text = "Hasta:";
            //
            // txtPrecioHasta
            //
            this.txtPrecioHasta.Location = new System.Drawing.Point(290, 94);
            this.txtPrecioHasta.Name = "txtPrecioHasta";
            this.txtPrecioHasta.Size = new System.Drawing.Size(150, 23);
            this.txtPrecioHasta.TabIndex = 14;
            this.txtPrecioHasta.TextChanged += new System.EventHandler(this.filtroTexto_TextChanged);
            //
            // lblKmHasta
            //
            this.lblKmHasta.AutoSize = true;
            this.lblKmHasta.Location = new System.Drawing.Point(458, 97);
            this.lblKmHasta.Name = "lblKmHasta";
            this.lblKmHasta.TabIndex = 15;
            this.lblKmHasta.Text = "Km hasta:";
            //
            // txtKmHasta
            //
            this.txtKmHasta.Location = new System.Drawing.Point(525, 94);
            this.txtKmHasta.Name = "txtKmHasta";
            this.txtKmHasta.Size = new System.Drawing.Size(150, 23);
            this.txtKmHasta.TabIndex = 16;
            this.txtKmHasta.TextChanged += new System.EventHandler(this.filtroTexto_TextChanged);
            //
            // lblFiltroEstado
            //
            this.lblFiltroEstado.AutoSize = true;
            this.lblFiltroEstado.Location = new System.Drawing.Point(18, 137);
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.TabIndex = 17;
            this.lblFiltroEstado.Text = "Estado:";
            //
            // chkEnVenta
            //
            this.chkEnVenta.AutoSize = true;
            this.chkEnVenta.Checked = true;
            this.chkEnVenta.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnVenta.Location = new System.Drawing.Point(110, 135);
            this.chkEnVenta.Name = "chkEnVenta";
            this.chkEnVenta.TabIndex = 18;
            this.chkEnVenta.Text = "En venta";
            this.chkEnVenta.CheckedChanged += new System.EventHandler(this.filtroCheck_CheckedChanged);
            //
            // chkPendienteVenta
            //
            this.chkPendienteVenta.AutoSize = true;
            this.chkPendienteVenta.Checked = true;
            this.chkPendienteVenta.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPendienteVenta.Location = new System.Drawing.Point(210, 135);
            this.chkPendienteVenta.Name = "chkPendienteVenta";
            this.chkPendienteVenta.TabIndex = 19;
            this.chkPendienteVenta.Text = "Pendiente de venta";
            this.chkPendienteVenta.CheckedChanged += new System.EventHandler(this.filtroCheck_CheckedChanged);
            //
            // chkSoloConPrecio
            //
            this.chkSoloConPrecio.AutoSize = true;
            this.chkSoloConPrecio.Location = new System.Drawing.Point(390, 135);
            this.chkSoloConPrecio.Name = "chkSoloConPrecio";
            this.chkSoloConPrecio.TabIndex = 21;
            this.chkSoloConPrecio.Text = "Sólo con precio";
            this.chkSoloConPrecio.CheckedChanged += new System.EventHandler(this.filtroCheck_CheckedChanged);
            //
            // btnLimpiar
            //
            this.btnLimpiar.Location = new System.Drawing.Point(935, 51);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(130, 28);
            this.btnLimpiar.TabIndex = 22;
            this.btnLimpiar.Text = "Limpiar filtros";
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            //
            // btnActualizar
            //
            this.btnActualizar.Location = new System.Drawing.Point(935, 91);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(130, 28);
            this.btnActualizar.TabIndex = 23;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            //
            // pnlPie
            //
            this.pnlPie.Controls.Add(this.lblTotal);
            this.pnlPie.Controls.Add(this.btnVerDetalle);
            this.pnlPie.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPie.Name = "pnlPie";
            this.pnlPie.Size = new System.Drawing.Size(1200, 50);
            this.pnlPie.TabIndex = 2;
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(18, 17);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Total: 0";
            //
            // btnVerDetalle
            //
            this.btnVerDetalle.Location = new System.Drawing.Point(440, 9);
            this.btnVerDetalle.Name = "btnVerDetalle";
            this.btnVerDetalle.Size = new System.Drawing.Size(140, 32);
            this.btnVerDetalle.TabIndex = 1;
            this.btnVerDetalle.Text = "Ver detalle";
            this.btnVerDetalle.Click += new System.EventHandler(this.btnVerDetalle_Click);
            //
            // tlpCuerpo
            //
            this.tlpCuerpo.ColumnCount = 2;
            this.tlpCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCuerpo.Controls.Add(this.dgvPublicaciones, 0, 0);
            this.tlpCuerpo.Controls.Add(this.pnlDetalle, 1, 0);
            this.tlpCuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCuerpo.Name = "tlpCuerpo";
            this.tlpCuerpo.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tlpCuerpo.RowCount = 1;
            this.tlpCuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCuerpo.TabIndex = 1;
            //
            // dgvPublicaciones
            //
            this.dgvPublicaciones.AllowUserToAddRows = false;
            this.dgvPublicaciones.AllowUserToDeleteRows = false;
            this.dgvPublicaciones.AllowUserToResizeRows = false;
            this.dgvPublicaciones.BackgroundColor = System.Drawing.Color.White;
            this.dgvPublicaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPublicaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDominio,
            this.colMarca,
            this.colModelo,
            this.colAnio,
            this.colKm,
            this.colEstado,
            this.colPrecio,
            this.colFotos,
            this.colActualizacion});
            this.dgvPublicaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPublicaciones.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.dgvPublicaciones.MultiSelect = false;
            this.dgvPublicaciones.Name = "dgvPublicaciones";
            this.dgvPublicaciones.ReadOnly = true;
            this.dgvPublicaciones.RowHeadersVisible = false;
            this.dgvPublicaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPublicaciones.TabIndex = 0;
            this.dgvPublicaciones.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPublicaciones_CellDoubleClick);
            this.dgvPublicaciones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPublicaciones_CellFormatting);
            this.dgvPublicaciones.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPublicaciones_ColumnHeaderMouseClick);
            //
            // colDominio
            //
            this.colDominio.DataPropertyName = "Dominio";
            this.colDominio.HeaderText = "Dominio";
            this.colDominio.Name = "colDominio";
            this.colDominio.ReadOnly = true;
            //
            // colMarca
            //
            this.colMarca.DataPropertyName = "Marca";
            this.colMarca.HeaderText = "Marca";
            this.colMarca.Name = "colMarca";
            this.colMarca.ReadOnly = true;
            //
            // colModelo
            //
            this.colModelo.DataPropertyName = "Modelo";
            this.colModelo.HeaderText = "Modelo";
            this.colModelo.Name = "colModelo";
            this.colModelo.ReadOnly = true;
            //
            // colAnio
            //
            this.colAnio.DataPropertyName = "Anio";
            this.colAnio.HeaderText = "Año";
            this.colAnio.Name = "colAnio";
            this.colAnio.ReadOnly = true;
            //
            // colKm
            //
            this.colKm.DataPropertyName = "Kilometraje";
            this.colKm.HeaderText = "Km";
            this.colKm.Name = "colKm";
            this.colKm.ReadOnly = true;
            //
            // colEstado
            //
            this.colEstado.DataPropertyName = "EstadoActual";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            //
            // colPrecio
            //
            this.colPrecio.DataPropertyName = "PrecioPublicacion";
            this.colPrecio.HeaderText = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.ReadOnly = true;
            //
            // colFotos
            //
            this.colFotos.DataPropertyName = "CantidadImagenes";
            this.colFotos.HeaderText = "Fotos";
            this.colFotos.Name = "colFotos";
            this.colFotos.ReadOnly = true;
            //
            // colActualizacion
            //
            this.colActualizacion.DataPropertyName = "FechaActualizacion";
            this.colActualizacion.HeaderText = "Última edición";
            this.colActualizacion.Name = "colActualizacion";
            this.colActualizacion.ReadOnly = true;
            //
            // pnlDetalle
            //
            this.pnlDetalle.AutoScroll = true;
            this.pnlDetalle.BackColor = System.Drawing.Color.White;
            this.pnlDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetalle.Controls.Add(this.tlpDetalle);
            this.pnlDetalle.Controls.Add(this.lblDetalleVacio);
            this.pnlDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetalle.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlDetalle.Name = "pnlDetalle";
            this.pnlDetalle.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlDetalle.TabIndex = 1;
            //
            // lblDetalleVacio
            //
            this.lblDetalleVacio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetalleVacio.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic);
            this.lblDetalleVacio.ForeColor = System.Drawing.Color.FromArgb(130, 130, 140);
            this.lblDetalleVacio.Name = "lblDetalleVacio";
            this.lblDetalleVacio.TabIndex = 0;
            this.lblDetalleVacio.Text = "Seleccioná una publicación y presioná \"Ver detalle\".";
            this.lblDetalleVacio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // tlpDetalle
            //
            this.tlpDetalle.ColumnCount = 1;
            this.tlpDetalle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetalle.Controls.Add(this.pnlDetalleHeader, 0, 0);
            this.tlpDetalle.Controls.Add(this.lblTituloPub, 0, 1);
            this.tlpDetalle.Controls.Add(this.lblSubtituloPub, 0, 2);
            this.tlpDetalle.Controls.Add(this.lblPrecioPub, 0, 3);
            this.tlpDetalle.Controls.Add(this.lblFechasPub, 0, 4);
            this.tlpDetalle.Controls.Add(this.lblDescripcionTitulo, 0, 5);
            this.tlpDetalle.Controls.Add(this.txtDescripcionPub, 0, 6);
            this.tlpDetalle.Controls.Add(this.lblFotosTitulo, 0, 7);
            this.tlpDetalle.Controls.Add(this.picPrincipal, 0, 8);
            this.tlpDetalle.Controls.Add(this.lblSinFotos, 0, 9);
            this.tlpDetalle.Controls.Add(this.flowMiniaturas, 0, 10);
            this.tlpDetalle.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDetalle.Name = "tlpDetalle";
            this.tlpDetalle.RowCount = 11;
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tlpDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tlpDetalle.Size = new System.Drawing.Size(560, 762);
            this.tlpDetalle.TabIndex = 1;
            this.tlpDetalle.Visible = false;
            //
            // pnlDetalleHeader
            //
            this.pnlDetalleHeader.Controls.Add(this.lblEstadoBadge);
            this.pnlDetalleHeader.Controls.Add(this.btnEditarPublicacion);
            this.pnlDetalleHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetalleHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDetalleHeader.Name = "pnlDetalleHeader";
            //
            // lblEstadoBadge
            //
            this.lblEstadoBadge.AutoSize = true;
            this.lblEstadoBadge.BackColor = System.Drawing.Color.FromArgb(30, 150, 70);
            this.lblEstadoBadge.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEstadoBadge.ForeColor = System.Drawing.Color.White;
            this.lblEstadoBadge.Location = new System.Drawing.Point(0, 6);
            this.lblEstadoBadge.Name = "lblEstadoBadge";
            this.lblEstadoBadge.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.lblEstadoBadge.Text = "EN VENTA";
            //
            // btnEditarPublicacion
            //
            this.btnEditarPublicacion.Location = new System.Drawing.Point(390, 2);
            this.btnEditarPublicacion.Name = "btnEditarPublicacion";
            this.btnEditarPublicacion.Size = new System.Drawing.Size(170, 32);
            this.btnEditarPublicacion.TabIndex = 0;
            this.btnEditarPublicacion.Text = "Editar publicación";
            this.btnEditarPublicacion.Click += new System.EventHandler(this.btnEditarPublicacion_Click);
            //
            // lblTituloPub
            //
            this.lblTituloPub.AutoEllipsis = true;
            this.lblTituloPub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTituloPub.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTituloPub.ForeColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.lblTituloPub.Margin = new System.Windows.Forms.Padding(0);
            this.lblTituloPub.Name = "lblTituloPub";
            this.lblTituloPub.Text = "Marca Modelo Año";
            this.lblTituloPub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblSubtituloPub
            //
            this.lblSubtituloPub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtituloPub.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtituloPub.ForeColor = System.Drawing.Color.FromArgb(100, 100, 110);
            this.lblSubtituloPub.Margin = new System.Windows.Forms.Padding(0);
            this.lblSubtituloPub.Name = "lblSubtituloPub";
            this.lblSubtituloPub.Text = "Dominio · Km";
            this.lblSubtituloPub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblPrecioPub
            //
            this.lblPrecioPub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrecioPub.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblPrecioPub.ForeColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.lblPrecioPub.Margin = new System.Windows.Forms.Padding(0);
            this.lblPrecioPub.Name = "lblPrecioPub";
            this.lblPrecioPub.Text = "$ 0";
            this.lblPrecioPub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblFechasPub
            //
            this.lblFechasPub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFechasPub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFechasPub.ForeColor = System.Drawing.Color.FromArgb(130, 130, 140);
            this.lblFechasPub.Margin = new System.Windows.Forms.Padding(0);
            this.lblFechasPub.Name = "lblFechasPub";
            this.lblFechasPub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblDescripcionTitulo
            //
            this.lblDescripcionTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescripcionTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDescripcionTitulo.Margin = new System.Windows.Forms.Padding(0);
            this.lblDescripcionTitulo.Name = "lblDescripcionTitulo";
            this.lblDescripcionTitulo.Text = "Descripción";
            this.lblDescripcionTitulo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // txtDescripcionPub
            //
            this.txtDescripcionPub.BackColor = System.Drawing.Color.White;
            this.txtDescripcionPub.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescripcionPub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescripcionPub.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDescripcionPub.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.txtDescripcionPub.Multiline = true;
            this.txtDescripcionPub.Name = "txtDescripcionPub";
            this.txtDescripcionPub.ReadOnly = true;
            this.txtDescripcionPub.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcionPub.TabStop = false;
            //
            // lblFotosTitulo
            //
            this.lblFotosTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFotosTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblFotosTitulo.Margin = new System.Windows.Forms.Padding(0);
            this.lblFotosTitulo.Name = "lblFotosTitulo";
            this.lblFotosTitulo.Text = "Fotos";
            this.lblFotosTitulo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // picPrincipal
            //
            this.picPrincipal.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.picPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPrincipal.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.picPrincipal.Name = "picPrincipal";
            this.picPrincipal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPrincipal.TabStop = false;
            //
            // lblSinFotos
            //
            this.lblSinFotos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSinFotos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblSinFotos.ForeColor = System.Drawing.Color.FromArgb(130, 130, 140);
            this.lblSinFotos.Margin = new System.Windows.Forms.Padding(0);
            this.lblSinFotos.Name = "lblSinFotos";
            this.lblSinFotos.Text = "Esta unidad no tiene fotos cargadas.";
            this.lblSinFotos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // flowMiniaturas
            //
            this.flowMiniaturas.AutoScroll = true;
            this.flowMiniaturas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMiniaturas.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.flowMiniaturas.Name = "flowMiniaturas";
            this.flowMiniaturas.WrapContents = false;
            //
            // pnlFiltros/pie/cuerpo → FormPublicaciones
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.tlpCuerpo);
            this.Controls.Add(this.pnlPie);
            this.Controls.Add(this.pnlFiltros);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormPublicaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publicaciones";
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.pnlPie.ResumeLayout(false);
            this.pnlPie.PerformLayout();
            this.tlpCuerpo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPublicaciones)).EndInit();
            this.pnlDetalle.ResumeLayout(false);
            this.tlpDetalle.ResumeLayout(false);
            this.tlpDetalle.PerformLayout();
            this.pnlDetalleHeader.ResumeLayout(false);
            this.pnlDetalleHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPrincipal)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblBuscarDominio;
        private System.Windows.Forms.TextBox txtBuscarDominio;
        private System.Windows.Forms.Label lblFiltroMarca;
        private System.Windows.Forms.ComboBox cmbMarca;
        private System.Windows.Forms.Label lblFiltroModelo;
        private System.Windows.Forms.ComboBox cmbModelo;
        private System.Windows.Forms.Label lblFiltroAnio;
        private System.Windows.Forms.ComboBox cmbAnio;
        private System.Windows.Forms.Label lblPrecioDesde;
        private System.Windows.Forms.TextBox txtPrecioDesde;
        private System.Windows.Forms.Label lblPrecioHasta;
        private System.Windows.Forms.TextBox txtPrecioHasta;
        private System.Windows.Forms.Label lblKmHasta;
        private System.Windows.Forms.TextBox txtKmHasta;
        private System.Windows.Forms.Label lblBuscarTexto;
        private System.Windows.Forms.TextBox txtBuscarTexto;
        private System.Windows.Forms.Label lblFiltroEstado;
        private System.Windows.Forms.CheckBox chkEnVenta;
        private System.Windows.Forms.CheckBox chkPendienteVenta;
        private System.Windows.Forms.CheckBox chkSoloConPrecio;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Panel pnlPie;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnVerDetalle;
        private System.Windows.Forms.TableLayoutPanel tlpCuerpo;
        private System.Windows.Forms.DataGridView dgvPublicaciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDominio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFotos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualizacion;
        private System.Windows.Forms.Panel pnlDetalle;
        private System.Windows.Forms.Label lblDetalleVacio;
        private System.Windows.Forms.TableLayoutPanel tlpDetalle;
        private System.Windows.Forms.Panel pnlDetalleHeader;
        private System.Windows.Forms.Label lblEstadoBadge;
        private System.Windows.Forms.Button btnEditarPublicacion;
        private System.Windows.Forms.Label lblTituloPub;
        private System.Windows.Forms.Label lblSubtituloPub;
        private System.Windows.Forms.Label lblPrecioPub;
        private System.Windows.Forms.Label lblFechasPub;
        private System.Windows.Forms.Label lblDescripcionTitulo;
        private System.Windows.Forms.TextBox txtDescripcionPub;
        private System.Windows.Forms.Label lblFotosTitulo;
        private System.Windows.Forms.PictureBox picPrincipal;
        private System.Windows.Forms.Label lblSinFotos;
        private System.Windows.Forms.FlowLayoutPanel flowMiniaturas;
    }
}
