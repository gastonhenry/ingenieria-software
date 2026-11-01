namespace UI
{
    partial class FormConfiguracionEmail
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPuerto;
        private System.Windows.Forms.NumericUpDown numPuerto;
        private System.Windows.Forms.CheckBox chkSsl;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkMostrarPassword;
        private System.Windows.Forms.Label lblRemitente;
        private System.Windows.Forms.TextBox txtRemitente;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Label lblSeparador;
        private System.Windows.Forms.Label lblDestinoPrueba;
        private System.Windows.Forms.TextBox txtDestinoPrueba;
        private System.Windows.Forms.Button btnEnviarPrueba;
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
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblPuerto = new System.Windows.Forms.Label();
            this.numPuerto = new System.Windows.Forms.NumericUpDown();
            this.chkSsl = new System.Windows.Forms.CheckBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkMostrarPassword = new System.Windows.Forms.CheckBox();
            this.lblRemitente = new System.Windows.Forms.Label();
            this.txtRemitente = new System.Windows.Forms.TextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.lblSeparador = new System.Windows.Forms.Label();
            this.lblDestinoPrueba = new System.Windows.Forms.Label();
            this.txtDestinoPrueba = new System.Windows.Forms.TextBox();
            this.btnEnviarPrueba = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPuerto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.SuspendLayout();
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Configurar cuenta email";
            //
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(24, 68);
            this.lblHost.Name = "lblHost";
            this.lblHost.Text = "Servidor SMTP:";
            //
            this.txtHost.Location = new System.Drawing.Point(230, 65);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(300, 27);
            this.txtHost.TabIndex = 0;
            //
            this.lblPuerto.AutoSize = true;
            this.lblPuerto.Location = new System.Drawing.Point(24, 110);
            this.lblPuerto.Name = "lblPuerto";
            this.lblPuerto.Text = "Puerto:";
            //
            this.numPuerto.Location = new System.Drawing.Point(230, 107);
            this.numPuerto.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            this.numPuerto.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPuerto.Name = "numPuerto";
            this.numPuerto.Size = new System.Drawing.Size(120, 27);
            this.numPuerto.TabIndex = 1;
            this.numPuerto.Value = new decimal(new int[] { 587, 0, 0, 0 });
            //
            this.chkSsl.AutoSize = true;
            this.chkSsl.Location = new System.Drawing.Point(230, 149);
            this.chkSsl.Name = "chkSsl";
            this.chkSsl.TabIndex = 2;
            this.chkSsl.Text = "Usar conexión segura (SSL/TLS)";
            //
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(24, 194);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Text = "Usuario:";
            //
            this.txtUsuario.Location = new System.Drawing.Point(230, 191);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(300, 27);
            this.txtUsuario.TabIndex = 3;
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(24, 236);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Text = "Contraseña:";
            //
            this.txtPassword.Location = new System.Drawing.Point(230, 233);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(300, 27);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            this.chkMostrarPassword.AutoSize = true;
            this.chkMostrarPassword.Location = new System.Drawing.Point(540, 235);
            this.chkMostrarPassword.Name = "chkMostrarPassword";
            this.chkMostrarPassword.TabIndex = 5;
            this.chkMostrarPassword.Text = "Mostrar";
            this.chkMostrarPassword.CheckedChanged += new System.EventHandler(this.chkMostrarPassword_CheckedChanged);
            //
            this.lblRemitente.AutoSize = true;
            this.lblRemitente.Location = new System.Drawing.Point(24, 278);
            this.lblRemitente.Name = "lblRemitente";
            this.lblRemitente.Text = "Email remitente:";
            //
            this.txtRemitente.Location = new System.Drawing.Point(230, 275);
            this.txtRemitente.Name = "txtRemitente";
            this.txtRemitente.Size = new System.Drawing.Size(300, 27);
            this.txtRemitente.TabIndex = 6;
            //
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(24, 320);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Text = "Tiempo de espera (ms):";
            //
            this.numTimeout.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numTimeout.Location = new System.Drawing.Point(230, 317);
            this.numTimeout.Maximum = new decimal(new int[] { 120000, 0, 0, 0 });
            this.numTimeout.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(120, 27);
            this.numTimeout.TabIndex = 7;
            this.numTimeout.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            //
            this.lblAyuda.ForeColor = System.Drawing.Color.FromArgb(90, 90, 100);
            this.lblAyuda.Location = new System.Drawing.Point(24, 360);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(600, 46);
            this.lblAyuda.Text = "Para Gmail: servidor smtp.gmail.com, puerto 587, conexión segura y una contraseña de aplicación.";
            //
            this.lblSeparador.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparador.Location = new System.Drawing.Point(24, 412);
            this.lblSeparador.Name = "lblSeparador";
            this.lblSeparador.Size = new System.Drawing.Size(600, 2);
            //
            this.lblDestinoPrueba.AutoSize = true;
            this.lblDestinoPrueba.Location = new System.Drawing.Point(24, 433);
            this.lblDestinoPrueba.Name = "lblDestinoPrueba";
            this.lblDestinoPrueba.Text = "Enviar prueba a:";
            //
            this.txtDestinoPrueba.Location = new System.Drawing.Point(230, 430);
            this.txtDestinoPrueba.Name = "txtDestinoPrueba";
            this.txtDestinoPrueba.Size = new System.Drawing.Size(300, 27);
            this.txtDestinoPrueba.TabIndex = 8;
            //
            this.btnEnviarPrueba.Location = new System.Drawing.Point(230, 468);
            this.btnEnviarPrueba.Name = "btnEnviarPrueba";
            this.btnEnviarPrueba.Size = new System.Drawing.Size(220, 34);
            this.btnEnviarPrueba.TabIndex = 9;
            this.btnEnviarPrueba.Text = "Enviar correo de prueba";
            this.btnEnviarPrueba.Click += new System.EventHandler(this.btnEnviarPrueba_Click);
            //
            this.btnGuardar.Location = new System.Drawing.Point(394, 530);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 34);
            this.btnGuardar.TabIndex = 10;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            this.btnCancelar.Location = new System.Drawing.Point(514, 530);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 34);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(650, 585);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblPuerto);
            this.Controls.Add(this.numPuerto);
            this.Controls.Add(this.chkSsl);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.chkMostrarPassword);
            this.Controls.Add(this.lblRemitente);
            this.Controls.Add(this.txtRemitente);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.numTimeout);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.lblSeparador);
            this.Controls.Add(this.lblDestinoPrueba);
            this.Controls.Add(this.txtDestinoPrueba);
            this.Controls.Add(this.btnEnviarPrueba);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfiguracionEmail";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurar cuenta email";
            ((System.ComponentModel.ISupportInitialize)(this.numPuerto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
