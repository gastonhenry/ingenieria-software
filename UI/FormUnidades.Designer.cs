namespace UI
{
    partial class FormUnidades
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblBuscarDominio;
        private System.Windows.Forms.TextBox txtBuscarDominio;
        private System.Windows.Forms.Label lblFiltroMarca;
        private System.Windows.Forms.ComboBox cmbMarca;
        private System.Windows.Forms.Label lblFiltroModelo;
        private System.Windows.Forms.ComboBox cmbModelo;
        private System.Windows.Forms.Label lblFiltroAnio;
        private System.Windows.Forms.ComboBox cmbAnio;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnAyudaEstados;
        private System.Windows.Forms.Label lblFiltroEstado;
        private System.Windows.Forms.CheckedListBox chkEstados;
        private System.Windows.Forms.Button btnMarcarTodos;
        private System.Windows.Forms.Button btnDesmarcarTodos;
        private System.Windows.Forms.DataGridView dgvUnidades;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnVerDetalle;

        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDominio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIngreso;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblBuscarDominio = new System.Windows.Forms.Label();
            this.txtBuscarDominio = new System.Windows.Forms.TextBox();
            this.lblFiltroMarca = new System.Windows.Forms.Label();
            this.cmbMarca = new System.Windows.Forms.ComboBox();
            this.lblFiltroModelo = new System.Windows.Forms.Label();
            this.cmbModelo = new System.Windows.Forms.ComboBox();
            this.lblFiltroAnio = new System.Windows.Forms.Label();
            this.cmbAnio = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnAyudaEstados = new System.Windows.Forms.Button();
            this.lblFiltroEstado = new System.Windows.Forms.Label();
            this.chkEstados = new System.Windows.Forms.CheckedListBox();
            this.btnMarcarTodos = new System.Windows.Forms.Button();
            this.btnDesmarcarTodos = new System.Windows.Forms.Button();
            this.dgvUnidades = new System.Windows.Forms.DataGridView();
            this.colDominio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIngreso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnVerDetalle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnidades)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(26, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(138, 38);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Unidades";
            // 
            // lblBuscarDominio
            // 
            this.lblBuscarDominio.AutoSize = true;
            this.lblBuscarDominio.Location = new System.Drawing.Point(26, 85);
            this.lblBuscarDominio.Name = "lblBuscarDominio";
            this.lblBuscarDominio.Size = new System.Drawing.Size(71, 20);
            this.lblBuscarDominio.TabIndex = 1;
            this.lblBuscarDominio.Text = "Dominio:";
            // 
            // txtBuscarDominio
            // 
            this.txtBuscarDominio.Location = new System.Drawing.Point(105, 81);
            this.txtBuscarDominio.Name = "txtBuscarDominio";
            this.txtBuscarDominio.Size = new System.Drawing.Size(108, 26);
            this.txtBuscarDominio.TabIndex = 2;
            // 
            // lblFiltroMarca
            // 
            this.lblFiltroMarca.AutoSize = true;
            this.lblFiltroMarca.Location = new System.Drawing.Point(229, 85);
            this.lblFiltroMarca.Name = "lblFiltroMarca";
            this.lblFiltroMarca.Size = new System.Drawing.Size(57, 20);
            this.lblFiltroMarca.TabIndex = 3;
            this.lblFiltroMarca.Text = "Marca:";
            // 
            // cmbMarca
            // 
            this.cmbMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMarca.Location = new System.Drawing.Point(289, 81);
            this.cmbMarca.Name = "cmbMarca";
            this.cmbMarca.Size = new System.Drawing.Size(180, 28);
            this.cmbMarca.TabIndex = 4;
            this.cmbMarca.SelectedIndexChanged += new System.EventHandler(this.cmbMarca_SelectedIndexChanged);
            // 
            // lblFiltroModelo
            // 
            this.lblFiltroModelo.AutoSize = true;
            this.lblFiltroModelo.Location = new System.Drawing.Point(489, 85);
            this.lblFiltroModelo.Name = "lblFiltroModelo";
            this.lblFiltroModelo.Size = new System.Drawing.Size(65, 20);
            this.lblFiltroModelo.TabIndex = 5;
            this.lblFiltroModelo.Text = "Modelo:";
            // 
            // cmbModelo
            // 
            this.cmbModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModelo.Location = new System.Drawing.Point(559, 81);
            this.cmbModelo.Name = "cmbModelo";
            this.cmbModelo.Size = new System.Drawing.Size(180, 28);
            this.cmbModelo.TabIndex = 6;
            this.cmbModelo.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            // 
            // lblFiltroAnio
            // 
            this.lblFiltroAnio.AutoSize = true;
            this.lblFiltroAnio.Location = new System.Drawing.Point(759, 85);
            this.lblFiltroAnio.Name = "lblFiltroAnio";
            this.lblFiltroAnio.Size = new System.Drawing.Size(42, 20);
            this.lblFiltroAnio.TabIndex = 7;
            this.lblFiltroAnio.Text = "Año:";
            // 
            // cmbAnio
            // 
            this.cmbAnio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnio.Location = new System.Drawing.Point(804, 81);
            this.cmbAnio.Name = "cmbAnio";
            this.cmbAnio.Size = new System.Drawing.Size(110, 28);
            this.cmbAnio.TabIndex = 8;
            this.cmbAnio.SelectedIndexChanged += new System.EventHandler(this.cmbFiltro_SelectedIndexChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(943, 77);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 34);
            this.btnBuscar.TabIndex = 9;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(1069, 77);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(130, 32);
            this.btnActualizar.TabIndex = 10;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnAyudaEstados
            // 
            this.btnAyudaEstados.Location = new System.Drawing.Point(943, 199);
            this.btnAyudaEstados.Name = "btnAyudaEstados";
            this.btnAyudaEstados.Size = new System.Drawing.Size(256, 34);
            this.btnAyudaEstados.TabIndex = 11;
            this.btnAyudaEstados.Text = "Flujo de estados";
            this.btnAyudaEstados.Click += new System.EventHandler(this.btnAyudaEstados_Click);
            // 
            // lblFiltroEstado
            // 
            this.lblFiltroEstado.AutoSize = true;
            this.lblFiltroEstado.Location = new System.Drawing.Point(26, 130);
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.Size = new System.Drawing.Size(72, 20);
            this.lblFiltroEstado.TabIndex = 12;
            this.lblFiltroEstado.Text = "Estados:";
            // 
            // chkEstados
            // 
            this.chkEstados.CheckOnClick = true;
            this.chkEstados.ColumnWidth = 240;
            this.chkEstados.IntegralHeight = false;
            this.chkEstados.Location = new System.Drawing.Point(105, 125);
            this.chkEstados.MultiColumn = true;
            this.chkEstados.Name = "chkEstados";
            this.chkEstados.Size = new System.Drawing.Size(809, 110);
            this.chkEstados.TabIndex = 13;
            this.chkEstados.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkEstados_ItemCheck);
            // 
            // btnMarcarTodos
            // 
            this.btnMarcarTodos.Location = new System.Drawing.Point(943, 123);
            this.btnMarcarTodos.Name = "btnMarcarTodos";
            this.btnMarcarTodos.Size = new System.Drawing.Size(256, 30);
            this.btnMarcarTodos.TabIndex = 14;
            this.btnMarcarTodos.Text = "Marcar todos";
            this.btnMarcarTodos.Click += new System.EventHandler(this.btnMarcarTodos_Click);
            // 
            // btnDesmarcarTodos
            // 
            this.btnDesmarcarTodos.Location = new System.Drawing.Point(943, 159);
            this.btnDesmarcarTodos.Name = "btnDesmarcarTodos";
            this.btnDesmarcarTodos.Size = new System.Drawing.Size(256, 30);
            this.btnDesmarcarTodos.TabIndex = 15;
            this.btnDesmarcarTodos.Text = "Desmarcar todos";
            this.btnDesmarcarTodos.Click += new System.EventHandler(this.btnDesmarcarTodos_Click);
            // 
            // dgvUnidades
            // 
            this.dgvUnidades.AllowUserToAddRows = false;
            this.dgvUnidades.AllowUserToDeleteRows = false;
            this.dgvUnidades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnidades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDominio,
            this.colMarca,
            this.colModelo,
            this.colAnio,
            this.colKm,
            this.colEstado,
            this.colIngreso});
            this.dgvUnidades.Location = new System.Drawing.Point(26, 250);
            this.dgvUnidades.Margin = new System.Windows.Forms.Padding(4);
            this.dgvUnidades.MultiSelect = false;
            this.dgvUnidades.Name = "dgvUnidades";
            this.dgvUnidades.ReadOnly = true;
            this.dgvUnidades.RowHeadersVisible = false;
            this.dgvUnidades.RowHeadersWidth = 51;
            this.dgvUnidades.RowTemplate.Height = 24;
            this.dgvUnidades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnidades.Size = new System.Drawing.Size(1541, 685);
            this.dgvUnidades.TabIndex = 20;
            this.dgvUnidades.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnidades_CellDoubleClick);
            this.dgvUnidades.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvUnidades_CellFormatting);
            this.dgvUnidades.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUnidades_ColumnHeaderMouseClick);
            // 
            // colDominio
            // 
            this.colDominio.DataPropertyName = "Dominio";
            this.colDominio.HeaderText = "Dominio";
            this.colDominio.MinimumWidth = 8;
            this.colDominio.Name = "colDominio";
            this.colDominio.ReadOnly = true;
            this.colDominio.Width = 110;
            // 
            // colMarca
            // 
            this.colMarca.DataPropertyName = "Marca";
            this.colMarca.HeaderText = "Marca";
            this.colMarca.MinimumWidth = 8;
            this.colMarca.Name = "colMarca";
            this.colMarca.ReadOnly = true;
            this.colMarca.Width = 140;
            // 
            // colModelo
            // 
            this.colModelo.DataPropertyName = "Modelo";
            this.colModelo.HeaderText = "Modelo";
            this.colModelo.MinimumWidth = 8;
            this.colModelo.Name = "colModelo";
            this.colModelo.ReadOnly = true;
            this.colModelo.Width = 160;
            // 
            // colAnio
            // 
            this.colAnio.DataPropertyName = "Anio";
            this.colAnio.HeaderText = "Año";
            this.colAnio.MinimumWidth = 8;
            this.colAnio.Name = "colAnio";
            this.colAnio.ReadOnly = true;
            this.colAnio.Width = 80;
            // 
            // colKm
            // 
            this.colKm.DataPropertyName = "Kilometraje";
            this.colKm.HeaderText = "Km";
            this.colKm.MinimumWidth = 8;
            this.colKm.Name = "colKm";
            this.colKm.ReadOnly = true;
            this.colKm.Width = 150;
            // 
            // colEstado
            // 
            this.colEstado.DataPropertyName = "EstadoActual";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.MinimumWidth = 8;
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 200;
            // 
            // colIngreso
            // 
            this.colIngreso.DataPropertyName = "FechaIngreso";
            this.colIngreso.HeaderText = "Ingreso";
            this.colIngreso.MinimumWidth = 8;
            this.colIngreso.Name = "colIngreso";
            this.colIngreso.ReadOnly = true;
            this.colIngreso.Width = 160;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.MinimumWidth = 8;
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 60;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(22, 951);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(61, 20);
            this.lblTotal.TabIndex = 21;
            this.lblTotal.Text = "Total: 0";
            // 
            // btnVerDetalle
            // 
            this.btnVerDetalle.Location = new System.Drawing.Point(1421, 943);
            this.btnVerDetalle.Name = "btnVerDetalle";
            this.btnVerDetalle.Size = new System.Drawing.Size(146, 37);
            this.btnVerDetalle.TabIndex = 22;
            this.btnVerDetalle.Text = "Ver detalle";
            this.btnVerDetalle.Click += new System.EventHandler(this.btnVerDetalle_Click);
            // 
            // FormUnidades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1691, 1044);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblBuscarDominio);
            this.Controls.Add(this.txtBuscarDominio);
            this.Controls.Add(this.lblFiltroMarca);
            this.Controls.Add(this.cmbMarca);
            this.Controls.Add(this.lblFiltroModelo);
            this.Controls.Add(this.cmbModelo);
            this.Controls.Add(this.lblFiltroAnio);
            this.Controls.Add(this.cmbAnio);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnAyudaEstados);
            this.Controls.Add(this.lblFiltroEstado);
            this.Controls.Add(this.chkEstados);
            this.Controls.Add(this.btnMarcarTodos);
            this.Controls.Add(this.btnDesmarcarTodos);
            this.Controls.Add(this.dgvUnidades);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnVerDetalle);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormUnidades";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unidades";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnidades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
