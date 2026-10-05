namespace UI
{
    partial class FormPausar
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.TextBox txtMotivo;
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
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(220, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Pausar unidad";
            //
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Location = new System.Drawing.Point(20, 65);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(200, 20);
            this.lblMotivo.TabIndex = 1;
            this.lblMotivo.Text = "Motivo (obligatorio):";
            //
            this.txtMotivo.Location = new System.Drawing.Point(20, 90);
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(500, 150);
            this.txtMotivo.TabIndex = 2;
            //
            this.btnAceptar.Location = new System.Drawing.Point(290, 260);
            this.btnAceptar.Size = new System.Drawing.Size(110, 34);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.TabIndex = 3;
            this.btnAceptar.Text = "Pausar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            //
            this.btnCancelar.Location = new System.Drawing.Point(410, 260);
            this.btnCancelar.Size = new System.Drawing.Size(110, 34);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 310);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblMotivo);
            this.Controls.Add(this.txtMotivo);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPausar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pausar unidad";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
