namespace UI
{
    partial class FormRegistrarModelo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.ComboBox cboMarca;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblTipoCarroceria;
        private System.Windows.Forms.ComboBox cboTipoCarroceria;
        private System.Windows.Forms.Button btnDarDeAlta;
        private System.Windows.Forms.Label lblExistentes;
        private System.Windows.Forms.ComboBox cboMarcaFiltro;
        private System.Windows.Forms.ListBox lstModelos;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblMarca = new System.Windows.Forms.Label();
            this.cboMarca = new System.Windows.Forms.ComboBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblTipoCarroceria = new System.Windows.Forms.Label();
            this.cboTipoCarroceria = new System.Windows.Forms.ComboBox();
            this.btnDarDeAlta = new System.Windows.Forms.Button();
            this.lblExistentes = new System.Windows.Forms.Label();
            this.cboMarcaFiltro = new System.Windows.Forms.ComboBox();
            this.lstModelos = new System.Windows.Forms.ListBox();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(26, 20);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(243, 38);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registrar Modelo";
            // 
            // lblMarca
            // 
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(26, 87);
            this.lblMarca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(57, 20);
            this.lblMarca.TabIndex = 1;
            this.lblMarca.Text = "Marca:";
            // 
            // cboMarca
            // 
            this.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarca.Location = new System.Drawing.Point(180, 83);
            this.cboMarca.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboMarca.Name = "cboMarca";
            this.cboMarca.Size = new System.Drawing.Size(410, 28);
            this.cboMarca.TabIndex = 2;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(26, 133);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(69, 20);
            this.lblNombre.TabIndex = 3;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(180, 129);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(411, 26);
            this.txtNombre.TabIndex = 4;
            // 
            // lblTipoCarroceria
            // 
            this.lblTipoCarroceria.AutoSize = true;
            this.lblTipoCarroceria.Location = new System.Drawing.Point(26, 180);
            this.lblTipoCarroceria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipoCarroceria.Name = "lblTipoCarroceria";
            this.lblTipoCarroceria.Size = new System.Drawing.Size(117, 20);
            this.lblTipoCarroceria.TabIndex = 5;
            this.lblTipoCarroceria.Text = "Tipo carrocería:";
            // 
            // cboTipoCarroceria
            // 
            this.cboTipoCarroceria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoCarroceria.Location = new System.Drawing.Point(180, 176);
            this.cboTipoCarroceria.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTipoCarroceria.Name = "cboTipoCarroceria";
            this.cboTipoCarroceria.Size = new System.Drawing.Size(411, 28);
            this.cboTipoCarroceria.TabIndex = 6;
            // 
            // btnDarDeAlta
            // 
            this.btnDarDeAlta.Location = new System.Drawing.Point(604, 79);
            this.btnDarDeAlta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDarDeAlta.Name = "btnDarDeAlta";
            this.btnDarDeAlta.Size = new System.Drawing.Size(141, 37);
            this.btnDarDeAlta.TabIndex = 7;
            this.btnDarDeAlta.Text = "Dar de alta";
            this.btnDarDeAlta.Click += new System.EventHandler(this.btnDarDeAlta_Click);
            // 
            // lblExistentes
            // 
            this.lblExistentes.AutoSize = true;
            this.lblExistentes.Location = new System.Drawing.Point(26, 240);
            this.lblExistentes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExistentes.Name = "lblExistentes";
            this.lblExistentes.Size = new System.Drawing.Size(268, 20);
            this.lblExistentes.TabIndex = 8;
            this.lblExistentes.Text = "Modelos existentes (filtra por marca):";
            // 
            // cboMarcaFiltro
            // 
            this.cboMarcaFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarcaFiltro.Location = new System.Drawing.Point(334, 236);
            this.cboMarcaFiltro.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboMarcaFiltro.Name = "cboMarcaFiltro";
            this.cboMarcaFiltro.Size = new System.Drawing.Size(256, 28);
            this.cboMarcaFiltro.TabIndex = 9;
            this.cboMarcaFiltro.SelectedIndexChanged += new System.EventHandler(this.cboMarcaFiltro_SelectedIndexChanged);
            // 
            // lstModelos
            // 
            this.lstModelos.IntegralHeight = false;
            this.lstModelos.ItemHeight = 20;
            this.lstModelos.Location = new System.Drawing.Point(26, 280);
            this.lstModelos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstModelos.Name = "lstModelos";
            this.lstModelos.Size = new System.Drawing.Size(565, 372);
            this.lstModelos.TabIndex = 10;
            this.lstModelos.SelectedIndexChanged += new System.EventHandler(this.lstModelos_SelectedIndexChanged);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(604, 280);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(141, 37);
            this.btnEditar.TabIndex = 11;
            this.btnEditar.Text = "Editar";
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(604, 327);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(141, 37);
            this.btnEliminar.TabIndex = 12;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // FormRegistrarModelo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 680);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblMarca);
            this.Controls.Add(this.cboMarca);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblTipoCarroceria);
            this.Controls.Add(this.cboTipoCarroceria);
            this.Controls.Add(this.btnDarDeAlta);
            this.Controls.Add(this.lblExistentes);
            this.Controls.Add(this.cboMarcaFiltro);
            this.Controls.Add(this.lstModelos);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormRegistrarModelo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar Modelo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
