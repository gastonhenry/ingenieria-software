namespace UI
{
    partial class FormPublicacion
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label lblImagenes;
        private System.Windows.Forms.ListBox lstImagenes;
        private System.Windows.Forms.Button btnAgregarImagen;
        private System.Windows.Forms.Button btnQuitarImagen;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblImagenes = new System.Windows.Forms.Label();
            this.lstImagenes = new System.Windows.Forms.ListBox();
            this.btnAgregarImagen = new System.Windows.Forms.Button();
            this.btnQuitarImagen = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Publicación de la unidad";

            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(20, 65);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(140, 20);
            this.lblPrecio.TabIndex = 1;
            this.lblPrecio.Text = "Precio ($):";

            this.txtPrecio.Location = new System.Drawing.Point(220, 61);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(170, 26);
            this.txtPrecio.TabIndex = 2;

            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 100);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(120, 20);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripción:";

            this.txtDescripcion.Location = new System.Drawing.Point(20, 125);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(600, 140);
            this.txtDescripcion.TabIndex = 4;

            this.btnGuardar.Location = new System.Drawing.Point(480, 500);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(140, 34);
            this.btnGuardar.TabIndex = 10;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            this.lblImagenes.AutoSize = true;
            this.lblImagenes.Location = new System.Drawing.Point(20, 280);
            this.lblImagenes.Name = "lblImagenes";
            this.lblImagenes.Size = new System.Drawing.Size(140, 20);
            this.lblImagenes.TabIndex = 6;
            this.lblImagenes.Text = "Imágenes:";

            this.lstImagenes.IntegralHeight = false;
            this.lstImagenes.ItemHeight = 20;
            this.lstImagenes.Location = new System.Drawing.Point(20, 305);
            this.lstImagenes.Name = "lstImagenes";
            this.lstImagenes.Size = new System.Drawing.Size(440, 180);
            this.lstImagenes.TabIndex = 7;

            this.btnAgregarImagen.Location = new System.Drawing.Point(475, 305);
            this.btnAgregarImagen.Name = "btnAgregarImagen";
            this.btnAgregarImagen.Size = new System.Drawing.Size(145, 34);
            this.btnAgregarImagen.TabIndex = 8;
            this.btnAgregarImagen.Text = "Agregar imagen";
            this.btnAgregarImagen.Click += new System.EventHandler(this.btnAgregarImagen_Click);

            this.btnQuitarImagen.Location = new System.Drawing.Point(475, 345);
            this.btnQuitarImagen.Name = "btnQuitarImagen";
            this.btnQuitarImagen.Size = new System.Drawing.Size(145, 34);
            this.btnQuitarImagen.TabIndex = 9;
            this.btnQuitarImagen.Text = "Quitar imagen";
            this.btnQuitarImagen.Click += new System.EventHandler(this.btnQuitarImagen_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 555);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.lblImagenes);
            this.Controls.Add(this.lstImagenes);
            this.Controls.Add(this.btnAgregarImagen);
            this.Controls.Add(this.btnQuitarImagen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPublicacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publicación";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
