namespace UI
{
    partial class FormMantenimiento
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.txtDetalle = new System.Windows.Forms.TextBox();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnRecalcular = new System.Windows.Forms.Button();
            this.btnReverificar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.lblBackupSeleccionado = new System.Windows.Forms.Label();
            this.cmbBackups = new System.Windows.Forms.ComboBox();
            this.lblUsuarioMant = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(660, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Modo Mantenimiento — Error de integridad detectado";
            //
            // lblEstado
            //
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstado.Location = new System.Drawing.Point(20, 55);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(660, 20);
            this.lblEstado.TabIndex = 1;
            this.lblEstado.Text = "Estado:";
            //
            // txtDetalle
            //
            this.txtDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetalle.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtDetalle.Location = new System.Drawing.Point(20, 80);
            this.txtDetalle.Multiline = true;
            this.txtDetalle.Name = "txtDetalle";
            this.txtDetalle.ReadOnly = true;
            this.txtDetalle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetalle.Size = new System.Drawing.Size(660, 140);
            this.txtDetalle.TabIndex = 2;
            //
            // lblBackupSeleccionado
            //
            this.lblBackupSeleccionado.AutoSize = true;
            this.lblBackupSeleccionado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBackupSeleccionado.Location = new System.Drawing.Point(20, 235);
            this.lblBackupSeleccionado.Name = "lblBackupSeleccionado";
            this.lblBackupSeleccionado.Size = new System.Drawing.Size(120, 15);
            this.lblBackupSeleccionado.TabIndex = 3;
            this.lblBackupSeleccionado.Text = "Backup a restaurar:";
            //
            // cmbBackups
            //
            this.cmbBackups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBackups.FormattingEnabled = true;
            this.cmbBackups.Location = new System.Drawing.Point(150, 232);
            this.cmbBackups.Name = "cmbBackups";
            this.cmbBackups.Size = new System.Drawing.Size(530, 23);
            this.cmbBackups.TabIndex = 4;
            //
            // btnBackup
            //
            this.btnBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(90)))), ((int)(((byte)(200)))));
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.FlatAppearance.BorderSize = 0;
            this.btnBackup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBackup.ForeColor = System.Drawing.Color.White;
            this.btnBackup.Location = new System.Drawing.Point(20, 275);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(155, 38);
            this.btnBackup.TabIndex = 5;
            this.btnBackup.Text = "Backup ahora";
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            //
            // btnRestaurar
            //
            this.btnRestaurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(0)))));
            this.btnRestaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurar.FlatAppearance.BorderSize = 0;
            this.btnRestaurar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestaurar.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar.Location = new System.Drawing.Point(185, 275);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(180, 38);
            this.btnRestaurar.TabIndex = 6;
            this.btnRestaurar.Text = "Restaurar selección";
            this.btnRestaurar.UseVisualStyleBackColor = false;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            //
            // btnRecalcular
            //
            this.btnRecalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnRecalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecalcular.FlatAppearance.BorderSize = 0;
            this.btnRecalcular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRecalcular.ForeColor = System.Drawing.Color.White;
            this.btnRecalcular.Location = new System.Drawing.Point(375, 275);
            this.btnRecalcular.Name = "btnRecalcular";
            this.btnRecalcular.Size = new System.Drawing.Size(155, 38);
            this.btnRecalcular.TabIndex = 7;
            this.btnRecalcular.Text = "Recalcular DVs";
            this.btnRecalcular.UseVisualStyleBackColor = false;
            this.btnRecalcular.Click += new System.EventHandler(this.btnRecalcular_Click);
            //
            // btnReverificar
            //
            this.btnReverificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.btnReverificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReverificar.FlatAppearance.BorderSize = 0;
            this.btnReverificar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReverificar.ForeColor = System.Drawing.Color.White;
            this.btnReverificar.Location = new System.Drawing.Point(540, 275);
            this.btnReverificar.Name = "btnReverificar";
            this.btnReverificar.Size = new System.Drawing.Size(140, 38);
            this.btnReverificar.TabIndex = 8;
            this.btnReverificar.Text = "Reverificar";
            this.btnReverificar.UseVisualStyleBackColor = false;
            this.btnReverificar.Click += new System.EventHandler(this.btnReverificar_Click);
            //
            // btnSalir
            //
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSalir.Location = new System.Drawing.Point(540, 325);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(140, 30);
            this.btnSalir.TabIndex = 9;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            //
            // lblUsuarioMant
            //
            this.lblUsuarioMant.AutoSize = true;
            this.lblUsuarioMant.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblUsuarioMant.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblUsuarioMant.Location = new System.Drawing.Point(20, 332);
            this.lblUsuarioMant.Name = "lblUsuarioMant";
            this.lblUsuarioMant.Size = new System.Drawing.Size(120, 15);
            this.lblUsuarioMant.TabIndex = 10;
            this.lblUsuarioMant.Text = "Sesión: mantenimiento";
            //
            // FormMantenimiento
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 370);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.txtDetalle);
            this.Controls.Add(this.lblBackupSeleccionado);
            this.Controls.Add(this.cmbBackups);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnRestaurar);
            this.Controls.Add(this.btnRecalcular);
            this.Controls.Add(this.btnReverificar);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblUsuarioMant);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMantenimiento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mantenimiento";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label    lblTitulo;
        private System.Windows.Forms.Label    lblEstado;
        private System.Windows.Forms.TextBox  txtDetalle;
        private System.Windows.Forms.Label    lblBackupSeleccionado;
        private System.Windows.Forms.ComboBox cmbBackups;
        private System.Windows.Forms.Button   btnBackup;
        private System.Windows.Forms.Button   btnRestaurar;
        private System.Windows.Forms.Button   btnRecalcular;
        private System.Windows.Forms.Button   btnReverificar;
        private System.Windows.Forms.Button   btnSalir;
        private System.Windows.Forms.Label    lblUsuarioMant;
    }
}
