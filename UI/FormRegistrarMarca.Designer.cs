namespace UI
{
    partial class FormRegistrarMarca
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnDarDeAlta;
        private System.Windows.Forms.Label lblExistentes;
        private System.Windows.Forms.ListBox lstMarcas;
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
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnDarDeAlta = new System.Windows.Forms.Button();
            this.lblExistentes = new System.Windows.Forms.Label();
            this.lstMarcas = new System.Windows.Forms.ListBox();
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
            this.lblTitulo.Size = new System.Drawing.Size(223, 38);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registrar Marca";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(26, 87);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(69, 20);
            this.lblNombre.TabIndex = 1;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(116, 83);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(346, 26);
            this.txtNombre.TabIndex = 2;
            // 
            // btnDarDeAlta
            // 
            this.btnDarDeAlta.Location = new System.Drawing.Point(476, 80);
            this.btnDarDeAlta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDarDeAlta.Name = "btnDarDeAlta";
            this.btnDarDeAlta.Size = new System.Drawing.Size(141, 37);
            this.btnDarDeAlta.TabIndex = 3;
            this.btnDarDeAlta.Text = "Dar de alta";
            this.btnDarDeAlta.Click += new System.EventHandler(this.btnDarDeAlta_Click);
            // 
            // lblExistentes
            // 
            this.lblExistentes.AutoSize = true;
            this.lblExistentes.Location = new System.Drawing.Point(26, 147);
            this.lblExistentes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExistentes.Name = "lblExistentes";
            this.lblExistentes.Size = new System.Drawing.Size(141, 20);
            this.lblExistentes.TabIndex = 4;
            this.lblExistentes.Text = "Marcas existentes:";
            // 
            // lstMarcas
            // 
            this.lstMarcas.IntegralHeight = false;
            this.lstMarcas.ItemHeight = 20;
            this.lstMarcas.Location = new System.Drawing.Point(26, 180);
            this.lstMarcas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstMarcas.Name = "lstMarcas";
            this.lstMarcas.Size = new System.Drawing.Size(436, 425);
            this.lstMarcas.TabIndex = 5;
            this.lstMarcas.SelectedIndexChanged += new System.EventHandler(this.lstMarcas_SelectedIndexChanged);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(476, 180);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(141, 37);
            this.btnEditar.TabIndex = 6;
            this.btnEditar.Text = "Editar";
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(476, 227);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(141, 37);
            this.btnEliminar.TabIndex = 7;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // FormRegistrarMarca
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 640);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.btnDarDeAlta);
            this.Controls.Add(this.lblExistentes);
            this.Controls.Add(this.lstMarcas);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormRegistrarMarca";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar Marca";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
