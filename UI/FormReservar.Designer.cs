namespace UI
{
    partial class FormReservar
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDocBusqueda;
        private System.Windows.Forms.TextBox txtDocBusqueda;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblCoincidencias;
        private System.Windows.Forms.ListBox lstCoincidencias;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblSena;
        private System.Windows.Forms.TextBox txtSena;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDocBusqueda = new System.Windows.Forms.Label();
            this.txtDocBusqueda = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblCoincidencias = new System.Windows.Forms.Label();
            this.lstCoincidencias = new System.Windows.Forms.ListBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.lblSena = new System.Windows.Forms.Label();
            this.txtSena = new System.Windows.Forms.TextBox();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(220, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reservar unidad";

            this.lblDocBusqueda.AutoSize = true;
            this.lblDocBusqueda.Location = new System.Drawing.Point(20, 65);
            this.lblDocBusqueda.Name = "lblDocBusqueda";
            this.lblDocBusqueda.Size = new System.Drawing.Size(120, 20);
            this.lblDocBusqueda.TabIndex = 1;
            this.lblDocBusqueda.Text = "Comprador (DNI/CUIT):";

            this.txtDocBusqueda.Location = new System.Drawing.Point(220, 61);
            this.txtDocBusqueda.Name = "txtDocBusqueda";
            this.txtDocBusqueda.Size = new System.Drawing.Size(220, 26);
            this.txtDocBusqueda.TabIndex = 2;

            this.btnBuscar.Location = new System.Drawing.Point(455, 58);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 32);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);

            this.lblCoincidencias.AutoSize = true;
            this.lblCoincidencias.Location = new System.Drawing.Point(20, 100);
            this.lblCoincidencias.Name = "lblCoincidencias";
            this.lblCoincidencias.Size = new System.Drawing.Size(120, 20);
            this.lblCoincidencias.TabIndex = 4;
            this.lblCoincidencias.Text = "Coincidencias:";

            this.lstCoincidencias.IntegralHeight = false;
            this.lstCoincidencias.ItemHeight = 20;
            this.lstCoincidencias.Location = new System.Drawing.Point(20, 125);
            this.lstCoincidencias.Name = "lstCoincidencias";
            this.lstCoincidencias.Size = new System.Drawing.Size(535, 120);
            this.lstCoincidencias.TabIndex = 5;

            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(20, 260);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(120, 20);
            this.lblFecha.TabIndex = 6;
            this.lblFecha.Text = "Fecha operación:";

            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(220, 256);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(170, 26);
            this.dtpFecha.TabIndex = 7;

            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(20, 295);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(140, 20);
            this.lblPrecio.TabIndex = 8;
            this.lblPrecio.Text = "Precio acordado ($):";

            this.txtPrecio.Location = new System.Drawing.Point(220, 291);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(170, 26);
            this.txtPrecio.TabIndex = 9;

            this.lblSena.AutoSize = true;
            this.lblSena.Location = new System.Drawing.Point(20, 330);
            this.lblSena.Name = "lblSena";
            this.lblSena.Size = new System.Drawing.Size(120, 20);
            this.lblSena.TabIndex = 10;
            this.lblSena.Text = "Seña ($):";

            this.txtSena.Location = new System.Drawing.Point(220, 326);
            this.txtSena.Name = "txtSena";
            this.txtSena.Size = new System.Drawing.Size(170, 26);
            this.txtSena.TabIndex = 11;

            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Location = new System.Drawing.Point(20, 365);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(170, 20);
            this.lblFechaFin.TabIndex = 12;
            this.lblFechaFin.Text = "Fecha estimada fin:";

            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(220, 361);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(170, 26);
            this.dtpFechaFin.TabIndex = 13;

            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 400);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(100, 20);
            this.lblDescripcion.TabIndex = 14;
            this.lblDescripcion.Text = "Descripción:";

            this.txtDescripcion.Location = new System.Drawing.Point(20, 425);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(535, 70);
            this.txtDescripcion.TabIndex = 15;

            this.btnAceptar.Location = new System.Drawing.Point(325, 510);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(110, 34);
            this.btnAceptar.TabIndex = 16;
            this.btnAceptar.Text = "Reservar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);

            this.btnCancelar.Location = new System.Drawing.Point(445, 510);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 34);
            this.btnCancelar.TabIndex = 17;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 560);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblDocBusqueda);
            this.Controls.Add(this.txtDocBusqueda);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lblCoincidencias);
            this.Controls.Add(this.lstCoincidencias);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.lblSena);
            this.Controls.Add(this.txtSena);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.dtpFechaFin);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReservar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reservar";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
