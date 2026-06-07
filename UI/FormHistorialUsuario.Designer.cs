using System.Windows.Forms;

namespace UI
{
    partial class FormHistorialUsuario
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
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnRestaurar = new UI.BotonPlano();
            this.btnCerrar = new UI.BotonPlano();
            this.lblLeyendaActual = new System.Windows.Forms.Label();
            this.pnlTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitulo
            //
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(1000, 60);
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 22);
            this.lblTitulo.Text = "Historial de Usuario";
            //
            // dgvHistorial
            //
            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistorial.Location = new System.Drawing.Point(0, 60);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersVisible = false;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.MultiSelect = false;
            this.dgvHistorial.Size = new System.Drawing.Size(1000, 480);
            this.dgvHistorial.TabIndex = 0;
            //
            // pnlBotones
            //
            this.pnlBotones.BackColor = System.Drawing.Color.FromArgb(235, 242, 255);
            this.pnlBotones.Controls.Add(this.btnRestaurar);
            this.pnlBotones.Controls.Add(this.lblLeyendaActual);
            this.pnlBotones.Controls.Add(this.btnCerrar);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.Location = new System.Drawing.Point(0, 540);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(1000, 60);
            //
            // btnRestaurar
            //
            this.btnRestaurar.BackColor = System.Drawing.Color.FromArgb(40, 140, 90);
            this.btnRestaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestaurar.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar.Location = new System.Drawing.Point(15, 14);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(220, 32);
            this.btnRestaurar.Text = "Restaurar a esta versión";
            this.btnRestaurar.UseVisualStyleBackColor = false;
            this.btnRestaurar.Enabled = false;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            //
            // lblLeyendaActual
            //
            this.lblLeyendaActual.AutoSize = true;
            this.lblLeyendaActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLeyendaActual.ForeColor = System.Drawing.Color.FromArgb(40, 130, 60);
            this.lblLeyendaActual.Location = new System.Drawing.Point(255, 22);
            this.lblLeyendaActual.Name = "lblLeyendaActual";
            this.lblLeyendaActual.Text = "● Versión actual";
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(880, 14);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(100, 32);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FormHistorialUsuario
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.pnlTitulo);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Name = "FormHistorialUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historial de Usuario";
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Panel pnlBotones;
        private UI.BotonPlano btnRestaurar;
        private UI.BotonPlano btnCerrar;
        private System.Windows.Forms.Label lblLeyendaActual;
    }
}
