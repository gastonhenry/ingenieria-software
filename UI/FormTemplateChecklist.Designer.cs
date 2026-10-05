namespace UI
{
    partial class FormTemplateChecklist
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditarDescripcion;
        private System.Windows.Forms.Button btnBajaLogica;
        private System.Windows.Forms.Button btnReactivar;

        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colActivo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditarDescripcion = new System.Windows.Forms.Button();
            this.btnBajaLogica = new System.Windows.Forms.Button();
            this.btnReactivar = new System.Windows.Forms.Button();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActivo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(26, 20);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(302, 38);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Template de Checklist";
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AutoGenerateColumns = false;
            this.dgvItems.ColumnHeadersHeight = 34;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNombre,
            this.colDescripcion,
            this.colActivo});
            this.dgvItems.Location = new System.Drawing.Point(26, 73);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(4);
            this.dgvItems.MultiSelect = false;
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 62;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(1205, 507);
            this.dgvItems.TabIndex = 1;
            //
            // colId
            //
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.Width = 50;
            //
            // colNombre
            //
            this.colNombre.DataPropertyName = "Nombre";
            this.colNombre.HeaderText = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Width = 220;
            //
            // colDescripcion
            //
            this.colDescripcion.DataPropertyName = "Descripcion";
            this.colDescripcion.HeaderText = "Descripción";
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.Width = 300;
            //
            // colActivo
            //
            this.colActivo.DataPropertyName = "Activo";
            this.colActivo.HeaderText = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.Width = 60;
            //
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(26, 614);
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(4);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(256, 40);
            this.btnNuevo.TabIndex = 2;
            this.btnNuevo.Text = "Nuevo item";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnEditarDescripcion
            // 
            this.btnEditarDescripcion.Location = new System.Drawing.Point(343, 614);
            this.btnEditarDescripcion.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditarDescripcion.Name = "btnEditarDescripcion";
            this.btnEditarDescripcion.Size = new System.Drawing.Size(256, 40);
            this.btnEditarDescripcion.TabIndex = 3;
            this.btnEditarDescripcion.Text = "Editar descripción";
            this.btnEditarDescripcion.UseVisualStyleBackColor = true;
            this.btnEditarDescripcion.Click += new System.EventHandler(this.btnEditarDescripcion_Click);
            // 
            // btnBajaLogica
            // 
            this.btnBajaLogica.Location = new System.Drawing.Point(975, 614);
            this.btnBajaLogica.Margin = new System.Windows.Forms.Padding(4);
            this.btnBajaLogica.Name = "btnBajaLogica";
            this.btnBajaLogica.Size = new System.Drawing.Size(256, 40);
            this.btnBajaLogica.TabIndex = 4;
            this.btnBajaLogica.Text = "Dar de baja";
            this.btnBajaLogica.UseVisualStyleBackColor = true;
            this.btnBajaLogica.Click += new System.EventHandler(this.btnBajaLogica_Click);
            // 
            // btnReactivar
            // 
            this.btnReactivar.Location = new System.Drawing.Point(660, 614);
            this.btnReactivar.Margin = new System.Windows.Forms.Padding(4);
            this.btnReactivar.Name = "btnReactivar";
            this.btnReactivar.Size = new System.Drawing.Size(256, 40);
            this.btnReactivar.TabIndex = 5;
            this.btnReactivar.Text = "Reactivar";
            this.btnReactivar.UseVisualStyleBackColor = true;
            this.btnReactivar.Click += new System.EventHandler(this.btnReactivar_Click);
            // 
            // FormTemplateChecklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1273, 725);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.btnEditarDescripcion);
            this.Controls.Add(this.btnBajaLogica);
            this.Controls.Add(this.btnReactivar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormTemplateChecklist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template de Checklist";
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
