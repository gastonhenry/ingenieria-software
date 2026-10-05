namespace UI
{
    partial class FormVisorImagen
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.picImagen = new System.Windows.Forms.PictureBox();
            this.pnlSuperior = new System.Windows.Forms.Panel();
            this.lblContador = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picImagen)).BeginInit();
            this.pnlSuperior.SuspendLayout();
            this.SuspendLayout();
            //
            // picImagen
            //
            this.picImagen.BackColor = System.Drawing.Color.Black;
            this.picImagen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImagen.Name = "picImagen";
            this.picImagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImagen.TabStop = false;
            //
            // pnlSuperior
            //
            this.pnlSuperior.BackColor = System.Drawing.Color.Black;
            this.pnlSuperior.Controls.Add(this.lblContador);
            this.pnlSuperior.Controls.Add(this.btnCerrar);
            this.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSuperior.Name = "pnlSuperior";
            this.pnlSuperior.Size = new System.Drawing.Size(1200, 48);
            //
            // lblContador
            //
            this.lblContador.AutoSize = true;
            this.lblContador.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblContador.ForeColor = System.Drawing.Color.White;
            this.lblContador.Location = new System.Drawing.Point(16, 12);
            this.lblContador.Name = "lblContador";
            this.lblContador.Text = "1 / 1";
            //
            // btnCerrar
            //
            this.btnCerrar.BackColor = System.Drawing.Color.Black;
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(200, 40, 40);
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(60, 48);
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Text = "✕";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // lblAyuda
            //
            this.lblAyuda.BackColor = System.Drawing.Color.Black;
            this.lblAyuda.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAyuda.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblAyuda.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(1200, 36);
            this.lblAyuda.Text = "Esc para cerrar  ·  ← → para navegar";
            this.lblAyuda.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnAnterior
            //
            this.btnAnterior.BackColor = System.Drawing.Color.Black;
            this.btnAnterior.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnterior.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAnterior.FlatAppearance.BorderSize = 0;
            this.btnAnterior.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnterior.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.btnAnterior.ForeColor = System.Drawing.Color.White;
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(80, 600);
            this.btnAnterior.TabStop = false;
            this.btnAnterior.Text = "‹";
            this.btnAnterior.UseVisualStyleBackColor = false;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            //
            // btnSiguiente
            //
            this.btnSiguiente.BackColor = System.Drawing.Color.Black;
            this.btnSiguiente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSiguiente.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSiguiente.FlatAppearance.BorderSize = 0;
            this.btnSiguiente.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguiente.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.btnSiguiente.ForeColor = System.Drawing.Color.White;
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(80, 600);
            this.btnSiguiente.TabStop = false;
            this.btnSiguiente.Text = "›";
            this.btnSiguiente.UseVisualStyleBackColor = false;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            //
            // FormVisorImagen
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            // Orden de Add: el Fill primero para que los dockeados a los bordes se acomoden antes.
            this.Controls.Add(this.picImagen);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.pnlSuperior);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormVisorImagen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Visor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormVisorImagen_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picImagen)).EndInit();
            this.pnlSuperior.ResumeLayout(false);
            this.pnlSuperior.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox picImagen;
        private System.Windows.Forms.Panel pnlSuperior;
        private System.Windows.Forms.Label lblContador;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguiente;
    }
}
