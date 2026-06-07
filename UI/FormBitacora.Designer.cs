using System.Windows.Forms;

namespace UI
{
    partial class FormBitacora
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblUsuarioFiltro = new System.Windows.Forms.Label();
            this.txtUsuarioFiltro = new System.Windows.Forms.TextBox();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnLimpiar = new UI.BotonPlano();
            this.pnlResumen = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnActualizar = new UI.BotonPlano();
            this.dgvBitacora = new System.Windows.Forms.DataGridView();
            this.pnlTitulo.SuspendLayout();
            this.grpFiltros.SuspendLayout();
            this.pnlResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(90)))), ((int)(((byte)(200)))));
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(750, 74);
            this.pnlTitulo.TabIndex = 3;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 34);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Bitácora";
            // 
            // grpFiltros
            // 
            this.grpFiltros.Controls.Add(this.lblTipo);
            this.grpFiltros.Controls.Add(this.cmbTipo);
            this.grpFiltros.Controls.Add(this.lblUsuarioFiltro);
            this.grpFiltros.Controls.Add(this.txtUsuarioFiltro);
            this.grpFiltros.Controls.Add(this.lblDesde);
            this.grpFiltros.Controls.Add(this.dtpDesde);
            this.grpFiltros.Controls.Add(this.lblHasta);
            this.grpFiltros.Controls.Add(this.dtpHasta);
            this.grpFiltros.Controls.Add(this.btnLimpiar);
            this.grpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiltros.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpFiltros.Location = new System.Drawing.Point(0, 74);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Size = new System.Drawing.Size(750, 100);
            this.grpFiltros.TabIndex = 2;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Padding = new Padding(20);
            this.grpFiltros.Text = "Filtros";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(12, 23);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(34, 15);
            this.lblTipo.TabIndex = 0;
            this.lblTipo.Text = "Tipo:";
            // 
            // cmbTipo
            // 
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Location = new System.Drawing.Point(55, 19);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(150, 23);
            this.cmbTipo.TabIndex = 0;
            // 
            // lblUsuarioFiltro
            // 
            this.lblUsuarioFiltro.AutoSize = true;
            this.lblUsuarioFiltro.Location = new System.Drawing.Point(220, 23);
            this.lblUsuarioFiltro.Name = "lblUsuarioFiltro";
            this.lblUsuarioFiltro.Size = new System.Drawing.Size(50, 15);
            this.lblUsuarioFiltro.TabIndex = 1;
            this.lblUsuarioFiltro.Text = "Usuario:";
            // 
            // txtUsuarioFiltro
            // 
            this.txtUsuarioFiltro.Location = new System.Drawing.Point(280, 20);
            this.txtUsuarioFiltro.Name = "txtUsuarioFiltro";
            this.txtUsuarioFiltro.Size = new System.Drawing.Size(160, 23);
            this.txtUsuarioFiltro.TabIndex = 1;
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(12, 64);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(42, 15);
            this.lblDesde.TabIndex = 4;
            this.lblDesde.Text = "Desde:";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Checked = false;
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(60, 60);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.ShowCheckBox = true;
            this.dtpDesde.Size = new System.Drawing.Size(145, 23);
            this.dtpDesde.TabIndex = 5;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(220, 64);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(40, 15);
            this.lblHasta.TabIndex = 6;
            this.lblHasta.Text = "Hasta:";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Checked = false;
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(265, 60);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.ShowCheckBox = true;
            this.dtpHasta.Size = new System.Drawing.Size(145, 23);
            this.dtpHasta.TabIndex = 7;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(458, 18);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(90, 26);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // pnlResumen
            // 
            this.pnlResumen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.pnlResumen.Controls.Add(this.lblTotal);
            this.pnlResumen.Controls.Add(this.btnActualizar);
            this.pnlResumen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlResumen.Location = new System.Drawing.Point(0, 430);
            this.pnlResumen.Name = "pnlResumen";
            this.pnlResumen.Size = new System.Drawing.Size(750, 50);
            this.pnlResumen.TabIndex = 1;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(15, 16);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(47, 15);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Total: 0";
            // 
            // btnActualizar
            // 
            this.btnActualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizar.Location = new System.Drawing.Point(1184, 12);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(100, 28);
            this.btnActualizar.TabIndex = 4;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // dgvBitacora
            // 
            this.dgvBitacora.AllowUserToAddRows = false;
            this.dgvBitacora.AllowUserToDeleteRows = false;
            this.dgvBitacora.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBitacora.Location = new System.Drawing.Point(0, 174);
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.ReadOnly = true;
            this.dgvBitacora.RowHeadersVisible = false;
            this.dgvBitacora.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBitacora.Size = new System.Drawing.Size(750, 256);
            this.dgvBitacora.TabIndex = 0;
            //
            // pnlSeparador
            //
            Panel pnlSeparador = new Panel();
            pnlSeparador.Dock = DockStyle.Top;
            pnlSeparador.Height = 10;
            // 
            // FormBitacora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 480);
            this.Controls.Add(this.dgvBitacora);
            this.Controls.Add(this.pnlResumen);
            this.Controls.Add(this.grpFiltros);
            this.Controls.Add(pnlSeparador);
            this.Controls.Add(this.pnlTitulo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBitacora";
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.pnlResumen.ResumeLayout(false);
            this.pnlResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblUsuarioFiltro;
        private System.Windows.Forms.TextBox txtUsuarioFiltro;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private UI.BotonPlano btnLimpiar;
        private System.Windows.Forms.Panel pnlResumen;
        private System.Windows.Forms.Label lblTotal;
        private UI.BotonPlano btnActualizar;
        private System.Windows.Forms.DataGridView dgvBitacora;
    }
}
