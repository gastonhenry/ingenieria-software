namespace UI
{
    partial class FormEntrevistaCliente
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpCliente;
        private System.Windows.Forms.Label lblDocBusqueda;
        private System.Windows.Forms.TextBox txtDocBusqueda;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnPersonas;
        private System.Windows.Forms.Label lblCoincidencias;
        private System.Windows.Forms.ListBox lstCoincidencias;
        private System.Windows.Forms.Label lblEmailCliente;
        private System.Windows.Forms.GroupBox grpBusqueda;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.TextBox txtBuscarMarca;
        private System.Windows.Forms.CheckedListBox clbMarcas;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.CheckedListBox clbModelos;
        private System.Windows.Forms.Label lblCarroceria;
        private System.Windows.Forms.CheckedListBox clbCarrocerias;
        private System.Windows.Forms.Label lblAyudaCriterios;
        private System.Windows.Forms.Label lblAnioDesde;
        private System.Windows.Forms.TextBox txtAnioDesde;
        private System.Windows.Forms.Label lblAnioHasta;
        private System.Windows.Forms.TextBox txtAnioHasta;
        private System.Windows.Forms.Label lblPrecioMax;
        private System.Windows.Forms.TextBox txtPrecioMax;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.CheckBox chkAceptaAvisos;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Label lblUnidadesCoincidentes;
        private System.Windows.Forms.DataGridView dgvCoincidentes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCarroceria;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio;
        private System.Windows.Forms.Button btnExportarPdf;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.grpCliente = new System.Windows.Forms.GroupBox();
            this.lblDocBusqueda = new System.Windows.Forms.Label();
            this.txtDocBusqueda = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnPersonas = new System.Windows.Forms.Button();
            this.lblCoincidencias = new System.Windows.Forms.Label();
            this.lstCoincidencias = new System.Windows.Forms.ListBox();
            this.lblEmailCliente = new System.Windows.Forms.Label();
            this.grpBusqueda = new System.Windows.Forms.GroupBox();
            this.lblMarca = new System.Windows.Forms.Label();
            this.txtBuscarMarca = new System.Windows.Forms.TextBox();
            this.clbMarcas = new System.Windows.Forms.CheckedListBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.clbModelos = new System.Windows.Forms.CheckedListBox();
            this.lblCarroceria = new System.Windows.Forms.Label();
            this.clbCarrocerias = new System.Windows.Forms.CheckedListBox();
            this.lblAyudaCriterios = new System.Windows.Forms.Label();
            this.lblAnioDesde = new System.Windows.Forms.Label();
            this.txtAnioDesde = new System.Windows.Forms.TextBox();
            this.lblAnioHasta = new System.Windows.Forms.Label();
            this.txtAnioHasta = new System.Windows.Forms.TextBox();
            this.lblPrecioMax = new System.Windows.Forms.Label();
            this.txtPrecioMax = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.chkAceptaAvisos = new System.Windows.Forms.CheckBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.lblUnidadesCoincidentes = new System.Windows.Forms.Label();
            this.dgvCoincidentes = new System.Windows.Forms.DataGridView();
            this.colUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCarroceria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.grpCliente.SuspendLayout();
            this.grpBusqueda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCoincidentes)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Registrar entrevista de cliente potencial";
            //
            // grpCliente
            //
            this.grpCliente.Controls.Add(this.lblDocBusqueda);
            this.grpCliente.Controls.Add(this.txtDocBusqueda);
            this.grpCliente.Controls.Add(this.btnBuscar);
            this.grpCliente.Controls.Add(this.btnPersonas);
            this.grpCliente.Controls.Add(this.lblCoincidencias);
            this.grpCliente.Controls.Add(this.lstCoincidencias);
            this.grpCliente.Controls.Add(this.lblEmailCliente);
            this.grpCliente.Location = new System.Drawing.Point(20, 60);
            this.grpCliente.Name = "grpCliente";
            this.grpCliente.Size = new System.Drawing.Size(460, 470);
            this.grpCliente.TabIndex = 0;
            this.grpCliente.TabStop = false;
            this.grpCliente.Text = "Cliente";
            //
            this.lblDocBusqueda.AutoSize = true;
            this.lblDocBusqueda.Location = new System.Drawing.Point(15, 30);
            this.lblDocBusqueda.Name = "lblDocBusqueda";
            this.lblDocBusqueda.Text = "Cliente (DNI/CUIT o nombre):";
            //
            this.txtDocBusqueda.Location = new System.Drawing.Point(15, 55);
            this.txtDocBusqueda.Name = "txtDocBusqueda";
            this.txtDocBusqueda.Size = new System.Drawing.Size(200, 27);
            this.txtDocBusqueda.TabIndex = 0;
            //
            this.btnBuscar.Location = new System.Drawing.Point(225, 52);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(90, 32);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            this.btnPersonas.Location = new System.Drawing.Point(325, 52);
            this.btnPersonas.Name = "btnPersonas";
            this.btnPersonas.Size = new System.Drawing.Size(120, 32);
            this.btnPersonas.TabIndex = 2;
            this.btnPersonas.Text = "Personas...";
            this.btnPersonas.Click += new System.EventHandler(this.btnPersonas_Click);
            //
            this.lblCoincidencias.AutoSize = true;
            this.lblCoincidencias.Location = new System.Drawing.Point(15, 95);
            this.lblCoincidencias.Name = "lblCoincidencias";
            this.lblCoincidencias.Text = "Coincidencias:";
            //
            this.lstCoincidencias.FormattingEnabled = true;
            this.lstCoincidencias.IntegralHeight = false;
            this.lstCoincidencias.ItemHeight = 20;
            this.lstCoincidencias.Location = new System.Drawing.Point(15, 120);
            this.lstCoincidencias.Name = "lstCoincidencias";
            this.lstCoincidencias.Size = new System.Drawing.Size(430, 280);
            this.lstCoincidencias.TabIndex = 3;
            this.lstCoincidencias.SelectedIndexChanged += new System.EventHandler(this.lstCoincidencias_SelectedIndexChanged);
            //
            this.lblEmailCliente.AutoEllipsis = true;
            this.lblEmailCliente.ForeColor = System.Drawing.Color.FromArgb(90, 90, 100);
            this.lblEmailCliente.Location = new System.Drawing.Point(15, 410);
            this.lblEmailCliente.Name = "lblEmailCliente";
            this.lblEmailCliente.Size = new System.Drawing.Size(430, 45);
            //
            // grpBusqueda
            //
            this.grpBusqueda.Controls.Add(this.lblMarca);
            this.grpBusqueda.Controls.Add(this.txtBuscarMarca);
            this.grpBusqueda.Controls.Add(this.clbMarcas);
            this.grpBusqueda.Controls.Add(this.lblModelo);
            this.grpBusqueda.Controls.Add(this.clbModelos);
            this.grpBusqueda.Controls.Add(this.lblCarroceria);
            this.grpBusqueda.Controls.Add(this.clbCarrocerias);
            this.grpBusqueda.Controls.Add(this.lblAyudaCriterios);
            this.grpBusqueda.Controls.Add(this.lblAnioDesde);
            this.grpBusqueda.Controls.Add(this.txtAnioDesde);
            this.grpBusqueda.Controls.Add(this.lblAnioHasta);
            this.grpBusqueda.Controls.Add(this.txtAnioHasta);
            this.grpBusqueda.Controls.Add(this.lblPrecioMax);
            this.grpBusqueda.Controls.Add(this.txtPrecioMax);
            this.grpBusqueda.Controls.Add(this.lblObservaciones);
            this.grpBusqueda.Controls.Add(this.txtObservaciones);
            this.grpBusqueda.Controls.Add(this.chkAceptaAvisos);
            this.grpBusqueda.Location = new System.Drawing.Point(500, 60);
            this.grpBusqueda.Name = "grpBusqueda";
            this.grpBusqueda.Size = new System.Drawing.Size(560, 470);
            this.grpBusqueda.TabIndex = 1;
            this.grpBusqueda.TabStop = false;
            this.grpBusqueda.Text = "Qué busca";
            //
            // Marcas: buscador + lista con casillas
            //
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(15, 28);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Text = "Marcas:";
            //
            this.txtBuscarMarca.Location = new System.Drawing.Point(15, 52);
            this.txtBuscarMarca.Name = "txtBuscarMarca";
            this.txtBuscarMarca.Size = new System.Drawing.Size(170, 27);
            this.txtBuscarMarca.TabIndex = 0;
            this.txtBuscarMarca.TextChanged += new System.EventHandler(this.txtBuscarMarca_TextChanged);
            //
            this.clbMarcas.CheckOnClick = true;
            this.clbMarcas.FormattingEnabled = true;
            this.clbMarcas.IntegralHeight = false;
            this.clbMarcas.Location = new System.Drawing.Point(15, 84);
            this.clbMarcas.Name = "clbMarcas";
            this.clbMarcas.Size = new System.Drawing.Size(170, 150);
            this.clbMarcas.TabIndex = 1;
            this.clbMarcas.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbMarcas_ItemCheck);
            //
            // Modelos de las marcas tildadas
            //
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(195, 28);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Text = "Modelos:";
            //
            this.clbModelos.CheckOnClick = true;
            this.clbModelos.FormattingEnabled = true;
            this.clbModelos.IntegralHeight = false;
            this.clbModelos.Location = new System.Drawing.Point(195, 52);
            this.clbModelos.Name = "clbModelos";
            this.clbModelos.Size = new System.Drawing.Size(190, 182);
            this.clbModelos.TabIndex = 2;
            this.clbModelos.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbModelos_ItemCheck);
            //
            // Carrocerías
            //
            this.lblCarroceria.AutoSize = true;
            this.lblCarroceria.Location = new System.Drawing.Point(395, 28);
            this.lblCarroceria.Name = "lblCarroceria";
            this.lblCarroceria.Text = "Carrocerías:";
            //
            this.clbCarrocerias.CheckOnClick = true;
            this.clbCarrocerias.FormattingEnabled = true;
            this.clbCarrocerias.IntegralHeight = false;
            this.clbCarrocerias.Location = new System.Drawing.Point(395, 52);
            this.clbCarrocerias.Name = "clbCarrocerias";
            this.clbCarrocerias.Size = new System.Drawing.Size(150, 182);
            this.clbCarrocerias.TabIndex = 3;
            //
            this.lblAyudaCriterios.AutoEllipsis = true;
            this.lblAyudaCriterios.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblAyudaCriterios.ForeColor = System.Drawing.Color.FromArgb(110, 110, 120);
            this.lblAyudaCriterios.Location = new System.Drawing.Point(15, 238);
            this.lblAyudaCriterios.Name = "lblAyudaCriterios";
            this.lblAyudaCriterios.Size = new System.Drawing.Size(530, 20);
            this.lblAyudaCriterios.Text = "Sin tildar = cualquiera. Los modelos tildados limitan su marca a esos modelos.";
            //
            // Año, precio, observaciones, avisos
            //
            this.lblAnioDesde.AutoSize = true;
            this.lblAnioDesde.Location = new System.Drawing.Point(15, 265);
            this.lblAnioDesde.Name = "lblAnioDesde";
            this.lblAnioDesde.Text = "Año desde:";
            //
            this.txtAnioDesde.Location = new System.Drawing.Point(15, 288);
            this.txtAnioDesde.MaxLength = 4;
            this.txtAnioDesde.Name = "txtAnioDesde";
            this.txtAnioDesde.Size = new System.Drawing.Size(110, 27);
            this.txtAnioDesde.TabIndex = 4;
            //
            this.lblAnioHasta.AutoSize = true;
            this.lblAnioHasta.Location = new System.Drawing.Point(140, 265);
            this.lblAnioHasta.Name = "lblAnioHasta";
            this.lblAnioHasta.Text = "Año hasta:";
            //
            this.txtAnioHasta.Location = new System.Drawing.Point(140, 288);
            this.txtAnioHasta.MaxLength = 4;
            this.txtAnioHasta.Name = "txtAnioHasta";
            this.txtAnioHasta.Size = new System.Drawing.Size(110, 27);
            this.txtAnioHasta.TabIndex = 5;
            //
            this.lblPrecioMax.AutoSize = true;
            this.lblPrecioMax.Location = new System.Drawing.Point(265, 265);
            this.lblPrecioMax.Name = "lblPrecioMax";
            this.lblPrecioMax.Text = "Precio máximo ($):";
            //
            this.txtPrecioMax.Location = new System.Drawing.Point(265, 288);
            this.txtPrecioMax.Name = "txtPrecioMax";
            this.txtPrecioMax.Size = new System.Drawing.Size(280, 27);
            this.txtPrecioMax.TabIndex = 6;
            //
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(15, 325);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Text = "Observaciones:";
            //
            this.txtObservaciones.Location = new System.Drawing.Point(15, 348);
            this.txtObservaciones.MaxLength = 1000;
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(530, 65);
            this.txtObservaciones.TabIndex = 7;
            //
            this.chkAceptaAvisos.Location = new System.Drawing.Point(15, 420);
            this.chkAceptaAvisos.Name = "chkAceptaAvisos";
            this.chkAceptaAvisos.Size = new System.Drawing.Size(530, 40);
            this.chkAceptaAvisos.TabIndex = 8;
            this.chkAceptaAvisos.Text = "El cliente acepta recibir avisos por correo electrónico";
            //
            // btnRegistrar / btnLimpiar
            //
            this.btnRegistrar.Location = new System.Drawing.Point(800, 540);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(130, 36);
            this.btnRegistrar.TabIndex = 2;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            //
            this.btnLimpiar.Location = new System.Drawing.Point(940, 540);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(120, 36);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            //
            // lblUnidadesCoincidentes
            //
            this.lblUnidadesCoincidentes.AutoSize = true;
            this.lblUnidadesCoincidentes.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUnidadesCoincidentes.Location = new System.Drawing.Point(20, 590);
            this.lblUnidadesCoincidentes.Name = "lblUnidadesCoincidentes";
            this.lblUnidadesCoincidentes.Text = "Unidades en venta que coinciden con la búsqueda";
            //
            // dgvCoincidentes
            //
            this.dgvCoincidentes.AllowUserToAddRows = false;
            this.dgvCoincidentes.AllowUserToDeleteRows = false;
            this.dgvCoincidentes.AllowUserToResizeRows = false;
            this.dgvCoincidentes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCoincidentes.BackgroundColor = System.Drawing.Color.White;
            this.dgvCoincidentes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colUnidad, this.colCarroceria, this.colKm, this.colPrecio});
            this.dgvCoincidentes.Location = new System.Drawing.Point(20, 620);
            this.dgvCoincidentes.MultiSelect = false;
            this.dgvCoincidentes.Name = "dgvCoincidentes";
            this.dgvCoincidentes.ReadOnly = true;
            this.dgvCoincidentes.RowHeadersVisible = false;
            this.dgvCoincidentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCoincidentes.Size = new System.Drawing.Size(900, 200);
            this.dgvCoincidentes.TabIndex = 4;
            this.dgvCoincidentes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCoincidentes_CellDoubleClick);
            //
            this.colUnidad.HeaderText = "Unidad";
            this.colUnidad.Name = "colUnidad";
            this.colUnidad.FillWeight = 45F;
            this.colCarroceria.HeaderText = "Carrocería";
            this.colCarroceria.Name = "colCarroceria";
            this.colCarroceria.FillWeight = 20F;
            this.colKm.HeaderText = "Km";
            this.colKm.Name = "colKm";
            this.colKm.FillWeight = 15F;
            this.colPrecio.HeaderText = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.FillWeight = 20F;
            //
            this.btnExportarPdf.Location = new System.Drawing.Point(930, 620);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(130, 36);
            this.btnExportarPdf.TabIndex = 5;
            this.btnExportarPdf.Text = "Exportar PDF";
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            //
            // FormEntrevistaCliente
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1090, 840);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.grpCliente);
            this.Controls.Add(this.grpBusqueda);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.lblUnidadesCoincidentes);
            this.Controls.Add(this.dgvCoincidentes);
            this.Controls.Add(this.btnExportarPdf);
            this.Name = "FormEntrevistaCliente";
            this.Text = "Registrar entrevista";
            this.grpCliente.ResumeLayout(false);
            this.grpCliente.PerformLayout();
            this.grpBusqueda.ResumeLayout(false);
            this.grpBusqueda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCoincidentes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
