namespace UI
{
    partial class FormEditarUsuario
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUsernameValor = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblDomicilio = new System.Windows.Forms.Label();
            this.txtDomicilio = new System.Windows.Forms.TextBox();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.lblIdiomaValor = new System.Windows.Forms.Label();
            this.btnGuardar = new UI.BotonPlano();
            this.pnlTitulo.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitulo
            //
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(422, 60);
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 22);
            this.lblTitulo.Text = "Editar Usuario";
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(40, 80);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Text = "Usuario:";
            //
            // lblUsernameValor
            //
            this.lblUsernameValor.AutoSize = false;
            this.lblUsernameValor.Location = new System.Drawing.Point(170, 80);
            this.lblUsernameValor.Name = "lblUsernameValor";
            this.lblUsernameValor.Size = new System.Drawing.Size(200, 20);
            this.lblUsernameValor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblUsernameValor.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.lblUsernameValor.Text = "—";
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(40, 117);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(170, 114);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(200, 20);
            this.txtNombre.TabIndex = 1;
            //
            // lblApellido
            //
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(40, 154);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Text = "Apellido:";
            //
            // txtApellido
            //
            this.txtApellido.Location = new System.Drawing.Point(170, 151);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(200, 20);
            this.txtApellido.TabIndex = 2;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(40, 191);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Text = "Email:";
            //
            // txtEmail
            //
            this.txtEmail.Location = new System.Drawing.Point(170, 188);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(200, 20);
            this.txtEmail.TabIndex = 3;
            //
            // lblTelefono
            //
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(40, 228);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Text = "Teléfono:";
            //
            // txtTelefono
            //
            this.txtTelefono.Location = new System.Drawing.Point(170, 225);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(200, 20);
            this.txtTelefono.TabIndex = 4;
            //
            // lblDocumento
            //
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Location = new System.Drawing.Point(40, 265);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Text = "Documento:";
            //
            // txtDocumento
            //
            this.txtDocumento.Location = new System.Drawing.Point(170, 262);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(200, 20);
            this.txtDocumento.TabIndex = 5;
            //
            // lblDomicilio
            //
            this.lblDomicilio.AutoSize = true;
            this.lblDomicilio.Location = new System.Drawing.Point(40, 302);
            this.lblDomicilio.Name = "lblDomicilio";
            this.lblDomicilio.Text = "Domicilio:";
            //
            // txtDomicilio
            //
            this.txtDomicilio.Location = new System.Drawing.Point(170, 299);
            this.txtDomicilio.Name = "txtDomicilio";
            this.txtDomicilio.Size = new System.Drawing.Size(200, 20);
            this.txtDomicilio.TabIndex = 6;
            //
            // lblIdioma
            //
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.Location = new System.Drawing.Point(40, 339);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Text = "Idioma preferido:";
            //
            // lblIdiomaValor
            //
            this.lblIdiomaValor.AutoSize = false;
            this.lblIdiomaValor.Location = new System.Drawing.Point(170, 339);
            this.lblIdiomaValor.Name = "lblIdiomaValor";
            this.lblIdiomaValor.Size = new System.Drawing.Size(200, 20);
            this.lblIdiomaValor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblIdiomaValor.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.lblIdiomaValor.Text = "—";
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(170, 378);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(200, 32);
            this.btnGuardar.TabIndex = 7;
            this.btnGuardar.Text = "Guardar cambios";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // FormEditarUsuario
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 430);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblUsernameValor);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblTelefono);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.lblDocumento);
            this.Controls.Add(this.txtDocumento);
            this.Controls.Add(this.lblDomicilio);
            this.Controls.Add(this.txtDomicilio);
            this.Controls.Add(this.lblIdioma);
            this.Controls.Add(this.lblIdiomaValor);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.pnlTitulo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditarUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editar Usuario";
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUsernameValor;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblDomicilio;
        private System.Windows.Forms.TextBox txtDomicilio;
        private System.Windows.Forms.Label lblIdioma;
        private System.Windows.Forms.Label lblIdiomaValor;
        private UI.BotonPlano btnGuardar;
    }
}
