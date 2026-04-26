namespace UI
{
    partial class FormPrincipal
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuInicio = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsertarUsuario = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVerUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVerBitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(90)))), ((int)(((byte)(200)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.menuStrip1.ForeColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInicio,
            this.menuUsuarios,
            this.menuBitacora,
            this.menuLogout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuInicio
            // 
            this.menuInicio.ForeColor = System.Drawing.Color.White;
            this.menuInicio.Name = "menuInicio";
            this.menuInicio.Size = new System.Drawing.Size(50, 21);
            this.menuInicio.Text = "Inicio";
            this.menuInicio.Click += new System.EventHandler(this.menuInicio_Click);
            // 
            // menuUsuarios
            // 
            this.menuUsuarios.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInsertarUsuario,
            this.menuVerUsuarios});
            this.menuUsuarios.ForeColor = System.Drawing.Color.White;
            this.menuUsuarios.Name = "menuUsuarios";
            this.menuUsuarios.Size = new System.Drawing.Size(71, 21);
            this.menuUsuarios.Text = "Usuarios";
            // 
            // menuInsertarUsuario
            // 
            this.menuInsertarUsuario.Name = "menuInsertarUsuario";
            this.menuInsertarUsuario.Size = new System.Drawing.Size(194, 22);
            this.menuInsertarUsuario.Text = "Insertar Usuario";
            this.menuInsertarUsuario.Click += new System.EventHandler(this.menuInsertarUsuario_Click);
            // 
            // menuVerUsuarios
            // 
            this.menuVerUsuarios.Name = "menuVerUsuarios";
            this.menuVerUsuarios.Size = new System.Drawing.Size(194, 22);
            this.menuVerUsuarios.Text = "Gestión de Usuarios";
            this.menuVerUsuarios.Click += new System.EventHandler(this.menuVerUsuarios_Click);
            // 
            // menuBitacora
            // 
            this.menuBitacora.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuVerBitacora});
            this.menuBitacora.ForeColor = System.Drawing.Color.White;
            this.menuBitacora.Name = "menuBitacora";
            this.menuBitacora.Size = new System.Drawing.Size(67, 21);
            this.menuBitacora.Text = "Bitácora";
            // 
            // menuVerBitacora
            // 
            this.menuVerBitacora.Name = "menuVerBitacora";
            this.menuVerBitacora.Size = new System.Drawing.Size(146, 22);
            this.menuVerBitacora.Text = "Ver Bitácora";
            this.menuVerBitacora.Click += new System.EventHandler(this.menuVerBitacora_Click);
            // 
            // menuLogout
            // 
            this.menuLogout.ForeColor = System.Drawing.Color.White;
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(61, 21);
            this.menuLogout.Text = "Logout";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPrincipal_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuInicio;
        private System.Windows.Forms.ToolStripMenuItem menuUsuarios;
        private System.Windows.Forms.ToolStripMenuItem menuInsertarUsuario;
        private System.Windows.Forms.ToolStripMenuItem menuVerUsuarios;
        private System.Windows.Forms.ToolStripMenuItem menuBitacora;
        private System.Windows.Forms.ToolStripMenuItem menuVerBitacora;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
    }
}
