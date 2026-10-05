namespace UI
{
    partial class FormRegistrarUnidad
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpUnidad;
        private System.Windows.Forms.Label lblDominio;
        private System.Windows.Forms.TextBox txtDominio;
        private System.Windows.Forms.Label lblMarca;
        private System.Windows.Forms.ComboBox cboMarca;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.ComboBox cboModelo;
        private System.Windows.Forms.Label lblAnio;
        private System.Windows.Forms.TextBox txtAnio;
        private System.Windows.Forms.Label lblKilometraje;
        private System.Windows.Forms.TextBox txtKilometraje;
        private System.Windows.Forms.Label lblPrecioCompra;
        private System.Windows.Forms.TextBox txtPrecioCompra;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.GroupBox grpPersona;
        private System.Windows.Forms.Label lblDocBusqueda;
        private System.Windows.Forms.TextBox txtDocBusqueda;
        private System.Windows.Forms.Button btnBuscarPersona;
        private System.Windows.Forms.Label lblCoincidencias;
        private System.Windows.Forms.ListBox lstCoincidencias;
        private System.Windows.Forms.Button btnRegistrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.grpUnidad = new System.Windows.Forms.GroupBox();
            this.lblDominio = new System.Windows.Forms.Label();
            this.txtDominio = new System.Windows.Forms.TextBox();
            this.lblMarca = new System.Windows.Forms.Label();
            this.cboMarca = new System.Windows.Forms.ComboBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.cboModelo = new System.Windows.Forms.ComboBox();
            this.lblAnio = new System.Windows.Forms.Label();
            this.txtAnio = new System.Windows.Forms.TextBox();
            this.lblKilometraje = new System.Windows.Forms.Label();
            this.txtKilometraje = new System.Windows.Forms.TextBox();
            this.lblPrecioCompra = new System.Windows.Forms.Label();
            this.txtPrecioCompra = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.grpPersona = new System.Windows.Forms.GroupBox();
            this.lblDocBusqueda = new System.Windows.Forms.Label();
            this.txtDocBusqueda = new System.Windows.Forms.TextBox();
            this.btnBuscarPersona = new System.Windows.Forms.Button();
            this.lblCoincidencias = new System.Windows.Forms.Label();
            this.lstCoincidencias = new System.Windows.Forms.ListBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpUnidad.SuspendLayout();
            this.grpPersona.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(26, 20);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(236, 38);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registrar Unidad";
            // 
            // grpUnidad
            // 
            this.grpUnidad.Controls.Add(this.label1);
            this.grpUnidad.Controls.Add(this.lblDominio);
            this.grpUnidad.Controls.Add(this.txtDominio);
            this.grpUnidad.Controls.Add(this.lblMarca);
            this.grpUnidad.Controls.Add(this.cboMarca);
            this.grpUnidad.Controls.Add(this.lblModelo);
            this.grpUnidad.Controls.Add(this.cboModelo);
            this.grpUnidad.Controls.Add(this.lblAnio);
            this.grpUnidad.Controls.Add(this.txtAnio);
            this.grpUnidad.Controls.Add(this.lblKilometraje);
            this.grpUnidad.Controls.Add(this.txtKilometraje);
            this.grpUnidad.Controls.Add(this.lblPrecioCompra);
            this.grpUnidad.Controls.Add(this.txtPrecioCompra);
            this.grpUnidad.Controls.Add(this.lblDescripcion);
            this.grpUnidad.Controls.Add(this.txtDescripcion);
            this.grpUnidad.Location = new System.Drawing.Point(26, 75);
            this.grpUnidad.Margin = new System.Windows.Forms.Padding(4);
            this.grpUnidad.Name = "grpUnidad";
            this.grpUnidad.Padding = new System.Windows.Forms.Padding(4);
            this.grpUnidad.Size = new System.Drawing.Size(700, 400);
            this.grpUnidad.TabIndex = 1;
            this.grpUnidad.TabStop = false;
            this.grpUnidad.Text = "Datos de la unidad";
            // 
            // lblDominio
            // 
            this.lblDominio.AutoSize = true;
            this.lblDominio.Location = new System.Drawing.Point(20, 42);
            this.lblDominio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDominio.Name = "lblDominio";
            this.lblDominio.Size = new System.Drawing.Size(71, 20);
            this.lblDominio.TabIndex = 0;
            this.lblDominio.Text = "Dominio:";
            // 
            // txtDominio
            // 
            this.txtDominio.Location = new System.Drawing.Point(170, 38);
            this.txtDominio.Margin = new System.Windows.Forms.Padding(4);
            this.txtDominio.Name = "txtDominio";
            this.txtDominio.Size = new System.Drawing.Size(115, 26);
            this.txtDominio.TabIndex = 1;
            // 
            // lblMarca
            // 
            this.lblMarca.AutoSize = true;
            this.lblMarca.Location = new System.Drawing.Point(20, 85);
            this.lblMarca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMarca.Name = "lblMarca";
            this.lblMarca.Size = new System.Drawing.Size(57, 20);
            this.lblMarca.TabIndex = 2;
            this.lblMarca.Text = "Marca:";
            // 
            // cboMarca
            // 
            this.cboMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarca.Location = new System.Drawing.Point(170, 81);
            this.cboMarca.Margin = new System.Windows.Forms.Padding(4);
            this.cboMarca.Name = "cboMarca";
            this.cboMarca.Size = new System.Drawing.Size(280, 28);
            this.cboMarca.TabIndex = 3;
            // 
            // lblModelo
            // 
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(20, 128);
            this.lblModelo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(65, 20);
            this.lblModelo.TabIndex = 4;
            this.lblModelo.Text = "Modelo:";
            // 
            // cboModelo
            // 
            this.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelo.Location = new System.Drawing.Point(170, 124);
            this.cboModelo.Margin = new System.Windows.Forms.Padding(4);
            this.cboModelo.Name = "cboModelo";
            this.cboModelo.Size = new System.Drawing.Size(280, 28);
            this.cboModelo.TabIndex = 5;
            // 
            // lblAnio
            // 
            this.lblAnio.AutoSize = true;
            this.lblAnio.Location = new System.Drawing.Point(20, 171);
            this.lblAnio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnio.Name = "lblAnio";
            this.lblAnio.Size = new System.Drawing.Size(42, 20);
            this.lblAnio.TabIndex = 6;
            this.lblAnio.Text = "Año:";
            // 
            // txtAnio
            // 
            this.txtAnio.Location = new System.Drawing.Point(170, 167);
            this.txtAnio.Margin = new System.Windows.Forms.Padding(4);
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Size = new System.Drawing.Size(115, 26);
            this.txtAnio.TabIndex = 7;
            // 
            // lblKilometraje
            // 
            this.lblKilometraje.AutoSize = true;
            this.lblKilometraje.Location = new System.Drawing.Point(408, 42);
            this.lblKilometraje.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKilometraje.Name = "lblKilometraje";
            this.lblKilometraje.Size = new System.Drawing.Size(91, 20);
            this.lblKilometraje.TabIndex = 8;
            this.lblKilometraje.Text = "Kilometraje:";
            // 
            // txtKilometraje
            // 
            this.txtKilometraje.Location = new System.Drawing.Point(520, 36);
            this.txtKilometraje.Margin = new System.Windows.Forms.Padding(4);
            this.txtKilometraje.Name = "txtKilometraje";
            this.txtKilometraje.Size = new System.Drawing.Size(150, 26);
            this.txtKilometraje.TabIndex = 9;
            // 
            // lblPrecioCompra
            // 
            this.lblPrecioCompra.AutoSize = true;
            this.lblPrecioCompra.Location = new System.Drawing.Point(20, 214);
            this.lblPrecioCompra.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrecioCompra.Name = "lblPrecioCompra";
            this.lblPrecioCompra.Size = new System.Drawing.Size(114, 20);
            this.lblPrecioCompra.TabIndex = 10;
            this.lblPrecioCompra.Text = "Precio compra:";
            // 
            // txtPrecioCompra
            // 
            this.txtPrecioCompra.Location = new System.Drawing.Point(170, 210);
            this.txtPrecioCompra.Margin = new System.Windows.Forms.Padding(4);
            this.txtPrecioCompra.Name = "txtPrecioCompra";
            this.txtPrecioCompra.Size = new System.Drawing.Size(280, 26);
            this.txtPrecioCompra.TabIndex = 11;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 257);
            this.lblDescripcion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(96, 20);
            this.lblDescripcion.TabIndex = 12;
            this.lblDescripcion.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(170, 253);
            this.txtDescripcion.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(500, 110);
            this.txtDescripcion.TabIndex = 13;
            // 
            // grpPersona
            // 
            this.grpPersona.Controls.Add(this.lblDocBusqueda);
            this.grpPersona.Controls.Add(this.txtDocBusqueda);
            this.grpPersona.Controls.Add(this.btnBuscarPersona);
            this.grpPersona.Controls.Add(this.lblCoincidencias);
            this.grpPersona.Controls.Add(this.lstCoincidencias);
            this.grpPersona.Location = new System.Drawing.Point(26, 490);
            this.grpPersona.Margin = new System.Windows.Forms.Padding(4);
            this.grpPersona.Name = "grpPersona";
            this.grpPersona.Padding = new System.Windows.Forms.Padding(4);
            this.grpPersona.Size = new System.Drawing.Size(700, 260);
            this.grpPersona.TabIndex = 2;
            this.grpPersona.TabStop = false;
            this.grpPersona.Text = "Persona vendedora";
            //
            // lblDocBusqueda
            //
            this.lblDocBusqueda.AutoSize = true;
            this.lblDocBusqueda.Location = new System.Drawing.Point(20, 45);
            this.lblDocBusqueda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDocBusqueda.Name = "lblDocBusqueda";
            this.lblDocBusqueda.Size = new System.Drawing.Size(103, 20);
            this.lblDocBusqueda.TabIndex = 0;
            this.lblDocBusqueda.Text = "DNI / CUIT:";
            //
            // txtDocBusqueda
            //
            this.txtDocBusqueda.Location = new System.Drawing.Point(170, 41);
            this.txtDocBusqueda.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocBusqueda.Name = "txtDocBusqueda";
            this.txtDocBusqueda.Size = new System.Drawing.Size(300, 26);
            this.txtDocBusqueda.TabIndex = 1;
            //
            // btnBuscarPersona
            //
            this.btnBuscarPersona.Location = new System.Drawing.Point(490, 37);
            this.btnBuscarPersona.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuscarPersona.Name = "btnBuscarPersona";
            this.btnBuscarPersona.Size = new System.Drawing.Size(140, 34);
            this.btnBuscarPersona.TabIndex = 2;
            this.btnBuscarPersona.Text = "Buscar";
            this.btnBuscarPersona.Click += new System.EventHandler(this.btnBuscarPersona_Click);
            //
            // lblCoincidencias
            //
            this.lblCoincidencias.AutoSize = true;
            this.lblCoincidencias.Location = new System.Drawing.Point(20, 90);
            this.lblCoincidencias.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCoincidencias.Name = "lblCoincidencias";
            this.lblCoincidencias.Size = new System.Drawing.Size(123, 20);
            this.lblCoincidencias.TabIndex = 3;
            this.lblCoincidencias.Text = "Coincidencias:";
            //
            // lstCoincidencias
            //
            this.lstCoincidencias.IntegralHeight = false;
            this.lstCoincidencias.ItemHeight = 20;
            this.lstCoincidencias.Location = new System.Drawing.Point(20, 115);
            this.lstCoincidencias.Margin = new System.Windows.Forms.Padding(4);
            this.lstCoincidencias.Name = "lstCoincidencias";
            this.lstCoincidencias.Size = new System.Drawing.Size(660, 130);
            this.lstCoincidencias.TabIndex = 4;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Location = new System.Drawing.Point(560, 770);
            this.btnRegistrar.Margin = new System.Windows.Forms.Padding(4);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(166, 45);
            this.btnRegistrar.TabIndex = 3;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(458, 213);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "AR$";
            // 
            // FormRegistrarUnidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 840);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.grpUnidad);
            this.Controls.Add(this.grpPersona);
            this.Controls.Add(this.btnRegistrar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormRegistrarUnidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar Unidad";
            this.grpUnidad.ResumeLayout(false);
            this.grpUnidad.PerformLayout();
            this.grpPersona.ResumeLayout(false);
            this.grpPersona.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
    }
}
