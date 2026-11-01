namespace UI
{
    partial class FormParametrosCaptacion
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.CheckBox chkNotificacionesActivas;
        private System.Windows.Forms.Label lblAyudaNotificaciones;
        private System.Windows.Forms.Label lblVigencia;
        private System.Windows.Forms.NumericUpDown numVigencia;
        private System.Windows.Forms.Label lblAlerta;
        private System.Windows.Forms.NumericUpDown numAlerta;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.chkNotificacionesActivas = new System.Windows.Forms.CheckBox();
            this.lblAyudaNotificaciones = new System.Windows.Forms.Label();
            this.lblVigencia = new System.Windows.Forms.Label();
            this.numVigencia = new System.Windows.Forms.NumericUpDown();
            this.lblAlerta = new System.Windows.Forms.Label();
            this.numAlerta = new System.Windows.Forms.NumericUpDown();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numVigencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlerta)).BeginInit();
            this.SuspendLayout();
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Parámetros de captación";
            //
            this.chkNotificacionesActivas.AutoSize = true;
            this.chkNotificacionesActivas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkNotificacionesActivas.Location = new System.Drawing.Point(24, 65);
            this.chkNotificacionesActivas.Name = "chkNotificacionesActivas";
            this.chkNotificacionesActivas.TabIndex = 0;
            this.chkNotificacionesActivas.Text = "Envío automático de avisos por correo activado";
            //
            this.lblAyudaNotificaciones.ForeColor = System.Drawing.Color.FromArgb(90, 90, 100);
            this.lblAyudaNotificaciones.Location = new System.Drawing.Point(44, 95);
            this.lblAyudaNotificaciones.Name = "lblAyudaNotificaciones";
            this.lblAyudaNotificaciones.Size = new System.Drawing.Size(500, 46);
            this.lblAyudaNotificaciones.Text = "Si está desactivado, al publicar una unidad no se envía ningún aviso. El tablero sigue mostrando a los interesados.";
            //
            this.lblVigencia.AutoSize = true;
            this.lblVigencia.Location = new System.Drawing.Point(24, 160);
            this.lblVigencia.Name = "lblVigencia";
            this.lblVigencia.Text = "Vigencia de una búsqueda (días):";
            //
            this.numVigencia.Location = new System.Drawing.Point(400, 157);
            this.numVigencia.Maximum = new decimal(new int[] { 3650, 0, 0, 0 });
            this.numVigencia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numVigencia.Name = "numVigencia";
            this.numVigencia.Size = new System.Drawing.Size(120, 27);
            this.numVigencia.TabIndex = 1;
            this.numVigencia.Value = new decimal(new int[] { 90, 0, 0, 0 });
            //
            this.lblAlerta.AutoSize = true;
            this.lblAlerta.Location = new System.Drawing.Point(24, 205);
            this.lblAlerta.Name = "lblAlerta";
            this.lblAlerta.Text = "Alerta de unidad sin interesados (días):";
            //
            this.numAlerta.Location = new System.Drawing.Point(400, 202);
            this.numAlerta.Maximum = new decimal(new int[] { 3650, 0, 0, 0 });
            this.numAlerta.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numAlerta.Name = "numAlerta";
            this.numAlerta.Size = new System.Drawing.Size(120, 27);
            this.numAlerta.TabIndex = 2;
            this.numAlerta.Value = new decimal(new int[] { 30, 0, 0, 0 });
            //
            this.btnGuardar.Location = new System.Drawing.Point(290, 260);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 34);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            this.btnCancelar.Location = new System.Drawing.Point(410, 260);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 34);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(560, 315);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.chkNotificacionesActivas);
            this.Controls.Add(this.lblAyudaNotificaciones);
            this.Controls.Add(this.lblVigencia);
            this.Controls.Add(this.numVigencia);
            this.Controls.Add(this.lblAlerta);
            this.Controls.Add(this.numAlerta);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormParametrosCaptacion";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parámetros de captación";
            ((System.ComponentModel.ISupportInitialize)(this.numVigencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlerta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
