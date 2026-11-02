namespace UI
{
    partial class FormTableroCaptacion
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblEstadoAvisos;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        private CardIndicador cardEnVenta;
        private CardIndicador cardPorSalir;
        private CardIndicador cardPosiblesCompradores;
        private CardIndicador cardSinInteresados;
        private CardIndicador cardInteresadosSinUnidades;
        private CardIndicador cardConversion;
        private System.Windows.Forms.TableLayoutPanel tlpCuerpo;
        private System.Windows.Forms.Panel pnlUnidades;
        private System.Windows.Forms.Label lblUnidades;
        private System.Windows.Forms.DataGridView dgvUnidades;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDias;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInteresados;
        private System.Windows.Forms.FlowLayoutPanel flpAccionesUnidades;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Panel pnlInteresados;
        private System.Windows.Forms.Label lblInteresados;
        private System.Windows.Forms.DataGridView dgvInteresados;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTelefono;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBusca;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntrevista;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvisado;
        private System.Windows.Forms.FlowLayoutPanel flpAccionesInteresados;
        private System.Windows.Forms.Button btnGestionarAvisos;
        private System.Windows.Forms.FlowLayoutPanel flpResumen;
        private System.Windows.Forms.Label lblResumen;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEstadoAvisos = new System.Windows.Forms.Label();
            this.flpCards = new System.Windows.Forms.FlowLayoutPanel();
            this.cardEnVenta = new CardIndicador();
            this.cardPorSalir = new CardIndicador();
            this.cardPosiblesCompradores = new CardIndicador();
            this.cardSinInteresados = new CardIndicador();
            this.cardInteresadosSinUnidades = new CardIndicador();
            this.cardConversion = new CardIndicador();
            this.tlpCuerpo = new System.Windows.Forms.TableLayoutPanel();
            this.pnlUnidades = new System.Windows.Forms.Panel();
            this.lblUnidades = new System.Windows.Forms.Label();
            this.dgvUnidades = new System.Windows.Forms.DataGridView();
            this.colUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInteresados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpAccionesUnidades = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.pnlInteresados = new System.Windows.Forms.Panel();
            this.lblInteresados = new System.Windows.Forms.Label();
            this.dgvInteresados = new System.Windows.Forms.DataGridView();
            this.colCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTelefono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBusca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntrevista = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvisado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpAccionesInteresados = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGestionarAvisos = new System.Windows.Forms.Button();
            this.flpResumen = new System.Windows.Forms.FlowLayoutPanel();
            this.lblResumen = new System.Windows.Forms.Label();
            this.pnlEncabezado.SuspendLayout();
            this.flpCards.SuspendLayout();
            this.tlpCuerpo.SuspendLayout();
            this.pnlUnidades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnidades)).BeginInit();
            this.flpAccionesUnidades.SuspendLayout();
            this.pnlInteresados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInteresados)).BeginInit();
            this.flpAccionesInteresados.SuspendLayout();
            this.flpResumen.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlEncabezado: título + estado del envío automático
            //
            this.pnlEncabezado.Controls.Add(this.lblTitulo);
            this.pnlEncabezado.Controls.Add(this.lblEstadoAvisos);
            this.pnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Size = new System.Drawing.Size(1200, 85);
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Tablero de captación";
            //
            this.lblEstadoAvisos.AutoSize = true;
            this.lblEstadoAvisos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstadoAvisos.Location = new System.Drawing.Point(22, 55);
            this.lblEstadoAvisos.Name = "lblEstadoAvisos";
            //
            // flpCards: indicadores
            //
            this.flpCards.AutoSize = true;
            this.flpCards.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCards.Controls.Add(this.cardEnVenta);
            this.flpCards.Controls.Add(this.cardPorSalir);
            this.flpCards.Controls.Add(this.cardPosiblesCompradores);
            this.flpCards.Controls.Add(this.cardSinInteresados);
            this.flpCards.Controls.Add(this.cardInteresadosSinUnidades);
            this.flpCards.Controls.Add(this.cardConversion);
            this.flpCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpCards.Name = "flpCards";
            this.flpCards.Padding = new System.Windows.Forms.Padding(20, 4, 20, 8);
            this.flpCards.WrapContents = true;
            //
            this.cardEnVenta.Name = "cardEnVenta";
            this.cardEnVenta.ColorAcento = System.Drawing.Color.FromArgb(30, 150, 70);
            this.cardPorSalir.Name = "cardPorSalir";
            this.cardPorSalir.ColorAcento = System.Drawing.Color.FromArgb(200, 160, 0);
            this.cardPosiblesCompradores.Name = "cardPosiblesCompradores";
            this.cardPosiblesCompradores.ColorAcento = System.Drawing.Color.FromArgb(30, 90, 200);
            this.cardPosiblesCompradores.VerClick += new System.EventHandler(this.cardPosiblesCompradores_VerClick);
            this.cardSinInteresados.Name = "cardSinInteresados";
            this.cardSinInteresados.ColorAcento = System.Drawing.Color.FromArgb(220, 130, 20);
            this.cardSinInteresados.VerClick += new System.EventHandler(this.cardSinInteresados_VerClick);
            this.cardInteresadosSinUnidades.Name = "cardInteresadosSinUnidades";
            this.cardInteresadosSinUnidades.ColorAcento = System.Drawing.Color.FromArgb(120, 80, 190);
            this.cardInteresadosSinUnidades.VerClick += new System.EventHandler(this.cardInteresadosSinUnidades_VerClick);
            this.cardConversion.Name = "cardConversion";
            this.cardConversion.ColorAcento = System.Drawing.Color.FromArgb(20, 150, 160);
            this.cardConversion.VerClick += new System.EventHandler(this.cardConversion_VerClick);
            //
            // tlpCuerpo: unidades (izq) | interesados en la unidad elegida (der)
            //
            this.tlpCuerpo.ColumnCount = 2;
            this.tlpCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpCuerpo.Controls.Add(this.pnlUnidades, 0, 0);
            this.tlpCuerpo.Controls.Add(this.pnlInteresados, 1, 0);
            this.tlpCuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCuerpo.Name = "tlpCuerpo";
            this.tlpCuerpo.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this.tlpCuerpo.RowCount = 1;
            this.tlpCuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // pnlUnidades
            //
            this.pnlUnidades.Controls.Add(this.dgvUnidades);
            this.pnlUnidades.Controls.Add(this.flpAccionesUnidades);
            this.pnlUnidades.Controls.Add(this.lblUnidades);
            this.pnlUnidades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUnidades.Name = "pnlUnidades";
            //
            this.lblUnidades.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUnidades.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUnidades.Name = "lblUnidades";
            this.lblUnidades.Size = new System.Drawing.Size(500, 34);
            this.lblUnidades.Text = "Unidades en venta";
            this.lblUnidades.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.dgvUnidades.AllowUserToAddRows = false;
            this.dgvUnidades.AllowUserToDeleteRows = false;
            this.dgvUnidades.AllowUserToResizeRows = false;
            this.dgvUnidades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUnidades.BackgroundColor = System.Drawing.Color.White;
            this.dgvUnidades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colUnidad, this.colPrecio, this.colDias, this.colInteresados});
            this.dgvUnidades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUnidades.MultiSelect = false;
            this.dgvUnidades.Name = "dgvUnidades";
            this.dgvUnidades.ReadOnly = true;
            this.dgvUnidades.RowHeadersVisible = false;
            this.dgvUnidades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnidades.SelectionChanged += new System.EventHandler(this.dgvUnidades_SelectionChanged);
            //
            this.colUnidad.HeaderText = "Unidad";
            this.colUnidad.Name = "colUnidad";
            this.colUnidad.FillWeight = 50F;
            this.colPrecio.HeaderText = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.FillWeight = 20F;
            this.colDias.HeaderText = "Días publicada";
            this.colDias.Name = "colDias";
            this.colDias.FillWeight = 15F;
            this.colInteresados.HeaderText = "Interesados";
            this.colInteresados.Name = "colInteresados";
            this.colInteresados.FillWeight = 15F;
            //
            this.flpAccionesUnidades.AutoSize = true;
            this.flpAccionesUnidades.Controls.Add(this.btnExportarPdf);
            this.flpAccionesUnidades.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpAccionesUnidades.Name = "flpAccionesUnidades";
            this.flpAccionesUnidades.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            //
            this.btnExportarPdf.AutoSize = true;
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.btnExportarPdf.Text = "Exportar PDF";
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            //
            // pnlInteresados
            //
            this.pnlInteresados.Controls.Add(this.dgvInteresados);
            this.pnlInteresados.Controls.Add(this.flpAccionesInteresados);
            this.pnlInteresados.Controls.Add(this.lblInteresados);
            this.pnlInteresados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInteresados.Name = "pnlInteresados";
            this.pnlInteresados.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            //
            this.lblInteresados.AutoEllipsis = true;
            this.lblInteresados.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInteresados.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblInteresados.Name = "lblInteresados";
            this.lblInteresados.Size = new System.Drawing.Size(600, 34);
            this.lblInteresados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.dgvInteresados.AllowUserToAddRows = false;
            this.dgvInteresados.AllowUserToDeleteRows = false;
            this.dgvInteresados.AllowUserToResizeRows = false;
            this.dgvInteresados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInteresados.BackgroundColor = System.Drawing.Color.White;
            this.dgvInteresados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colCliente, this.colTelefono, this.colBusca, this.colEntrevista, this.colAvisado});
            this.dgvInteresados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInteresados.MultiSelect = false;
            this.dgvInteresados.Name = "dgvInteresados";
            this.dgvInteresados.ReadOnly = true;
            this.dgvInteresados.RowHeadersVisible = false;
            this.dgvInteresados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInteresados.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInteresados_CellDoubleClick);
            //
            this.colCliente.HeaderText = "Cliente";
            this.colCliente.Name = "colCliente";
            this.colCliente.FillWeight = 24F;
            this.colTelefono.HeaderText = "Teléfono";
            this.colTelefono.Name = "colTelefono";
            this.colTelefono.FillWeight = 17F;
            this.colBusca.HeaderText = "Busca";
            this.colBusca.Name = "colBusca";
            this.colBusca.FillWeight = 31F;
            this.colEntrevista.HeaderText = "Entrevista";
            this.colEntrevista.Name = "colEntrevista";
            this.colEntrevista.FillWeight = 12F;
            this.colAvisado.HeaderText = "Avisado";
            this.colAvisado.Name = "colAvisado";
            this.colAvisado.FillWeight = 16F;
            //
            this.flpAccionesInteresados.AutoSize = true;
            this.flpAccionesInteresados.Controls.Add(this.btnGestionarAvisos);
            this.flpAccionesInteresados.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpAccionesInteresados.Name = "flpAccionesInteresados";
            this.flpAccionesInteresados.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            //
            this.btnGestionarAvisos.AutoSize = true;
            this.btnGestionarAvisos.Name = "btnGestionarAvisos";
            this.btnGestionarAvisos.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.btnGestionarAvisos.Text = "Avisos del cliente...";
            this.btnGestionarAvisos.Click += new System.EventHandler(this.btnGestionarAvisos_Click);
            //
            // flpResumen: avisos y conversión de los últimos 30 días
            //
            this.flpResumen.AutoSize = true;
            this.flpResumen.Controls.Add(this.lblResumen);
            this.flpResumen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpResumen.Name = "flpResumen";
            this.flpResumen.Padding = new System.Windows.Forms.Padding(20, 8, 20, 10);
            //
            this.lblResumen.AutoSize = true;
            this.lblResumen.ForeColor = System.Drawing.Color.FromArgb(70, 70, 80);
            this.lblResumen.Margin = new System.Windows.Forms.Padding(0, 3, 16, 0);
            this.lblResumen.Name = "lblResumen";
            //
            //
            //
            // FormTableroCaptacion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.tlpCuerpo);
            this.Controls.Add(this.flpResumen);
            this.Controls.Add(this.flpCards);
            this.Controls.Add(this.pnlEncabezado);
            this.Name = "FormTableroCaptacion";
            this.Text = "Tablero de captación";
            this.pnlEncabezado.ResumeLayout(false);
            this.pnlEncabezado.PerformLayout();
            this.flpCards.ResumeLayout(false);
            this.tlpCuerpo.ResumeLayout(false);
            this.pnlUnidades.ResumeLayout(false);
            this.pnlUnidades.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnidades)).EndInit();
            this.flpAccionesUnidades.ResumeLayout(false);
            this.flpAccionesUnidades.PerformLayout();
            this.pnlInteresados.ResumeLayout(false);
            this.pnlInteresados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInteresados)).EndInit();
            this.flpAccionesInteresados.ResumeLayout(false);
            this.flpAccionesInteresados.PerformLayout();
            this.flpResumen.ResumeLayout(false);
            this.flpResumen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
