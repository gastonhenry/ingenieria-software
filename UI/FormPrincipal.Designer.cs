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
            this.menuAsignacionPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGestionPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVerBitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIdiomas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGestionIdiomas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeleccionIdioma = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSesion = new System.Windows.Forms.ToolStripLabel();
            this.lblEstadoSesion = new System.Windows.Forms.ToolStripLabel();
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
            this.menuPermisos,
            this.menuBitacora,
            this.menuIdiomas,
            this.menuSeleccionIdioma,
            this.menuLogout,
            this.lblSesion,
            this.lblEstadoSesion});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuInicio
            // 
            this.menuInicio.ForeColor = System.Drawing.Color.White;
            this.menuInicio.Name = "menuInicio";
            this.menuInicio.Size = new System.Drawing.Size(50, 25);
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
            this.menuUsuarios.Size = new System.Drawing.Size(71, 25);
            this.menuUsuarios.Text = "Usuarios";
            // 
            // menuInsertarUsuario
            // 
            this.menuInsertarUsuario.Name = "menuInsertarUsuario";
            this.menuInsertarUsuario.Size = new System.Drawing.Size(215, 22);
            this.menuInsertarUsuario.Text = "Registrar Usuario";
            this.menuInsertarUsuario.Click += new System.EventHandler(this.menuInsertarUsuario_Click);
            // 
            // menuVerUsuarios
            // 
            this.menuVerUsuarios.Name = "menuVerUsuarios";
            this.menuVerUsuarios.Size = new System.Drawing.Size(215, 22);
            this.menuVerUsuarios.Text = "Gestión de Usuarios";
            this.menuVerUsuarios.Click += new System.EventHandler(this.menuVerUsuarios_Click);
            // 
            // menuAsignacionPermisos
            // 
            this.menuAsignacionPermisos.Name = "menuAsignacionPermisos";
            this.menuAsignacionPermisos.Size = new System.Drawing.Size(260, 22);
            this.menuAsignacionPermisos.Text = "Asignación de Permisos a Usuarios";
            this.menuAsignacionPermisos.Click += new System.EventHandler(this.menuAsignacionPermisos_Click);
            // 
            // menuPermisos
            // 
            this.menuPermisos.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGestionPermisos,
            this.menuAsignacionPermisos});
            this.menuPermisos.ForeColor = System.Drawing.Color.White;
            this.menuPermisos.Name = "menuPermisos";
            this.menuPermisos.Size = new System.Drawing.Size(73, 25);
            this.menuPermisos.Text = "Permisos";
            // 
            // menuGestionPermisos
            // 
            this.menuGestionPermisos.Name = "menuGestionPermisos";
            this.menuGestionPermisos.Size = new System.Drawing.Size(196, 22);
            this.menuGestionPermisos.Text = "Gestión de Permisos";
            this.menuGestionPermisos.Click += new System.EventHandler(this.menuGestionPermisos_Click);
            //
            // menuBitacora
            // 
            this.menuBitacora.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuVerBitacora});
            this.menuBitacora.ForeColor = System.Drawing.Color.White;
            this.menuBitacora.Name = "menuBitacora";
            this.menuBitacora.Size = new System.Drawing.Size(67, 25);
            this.menuBitacora.Text = "Bitácora";
            // 
            // menuVerBitacora
            // 
            this.menuVerBitacora.Name = "menuVerBitacora";
            this.menuVerBitacora.Size = new System.Drawing.Size(146, 22);
            this.menuVerBitacora.Text = "Ver Bitácora";
            this.menuVerBitacora.Click += new System.EventHandler(this.menuVerBitacora_Click);
            //
            // menuIdiomas
            //
            this.menuIdiomas.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGestionIdiomas});
            this.menuIdiomas.ForeColor = System.Drawing.Color.White;
            this.menuIdiomas.Name = "menuIdiomas";
            this.menuIdiomas.Size = new System.Drawing.Size(63, 25);
            this.menuIdiomas.Text = "Idiomas";
            //
            // menuGestionIdiomas
            //
            this.menuGestionIdiomas.Name = "menuGestionIdiomas";
            this.menuGestionIdiomas.Size = new System.Drawing.Size(180, 22);
            this.menuGestionIdiomas.Text = "Gestión de Idiomas";
            this.menuGestionIdiomas.Click += new System.EventHandler(this.menuGestionIdiomas_Click);
            //
            // menuSeleccionIdioma
            //
            this.menuSeleccionIdioma.ForeColor = System.Drawing.Color.White;
            this.menuSeleccionIdioma.Name = "menuSeleccionIdioma";
            this.menuSeleccionIdioma.Size = new System.Drawing.Size(70, 25);
            this.menuSeleccionIdioma.Text = "Idioma ▾";
            //
            // menuLogout
            //
            this.menuLogout.ForeColor = System.Drawing.Color.White;
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(61, 25);
            this.menuLogout.Text = "Logout";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // lblSesion
            // 
            this.lblSesion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblSesion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSesion.ForeColor = System.Drawing.Color.White;
            this.lblSesion.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.lblSesion.Name = "lblSesion";
            this.lblSesion.Size = new System.Drawing.Size(61, 25);
            this.lblSesion.Text = "Sesión: -";
            // 
            // lblEstadoSesion
            // 
            this.lblEstadoSesion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblEstadoSesion.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblEstadoSesion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(255)))), ((int)(((byte)(20)))));
            this.lblEstadoSesion.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblEstadoSesion.Name = "lblEstadoSesion";
            this.lblEstadoSesion.Size = new System.Drawing.Size(23, 25);
            this.lblEstadoSesion.Text = "●";
            this.lblEstadoSesion.ToolTipText = "Sesión activa";
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
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
        private System.Windows.Forms.ToolStripMenuItem menuAsignacionPermisos;
        private System.Windows.Forms.ToolStripMenuItem menuBitacora;
        private System.Windows.Forms.ToolStripMenuItem menuVerBitacora;
        private System.Windows.Forms.ToolStripMenuItem menuPermisos;
        private System.Windows.Forms.ToolStripMenuItem menuGestionPermisos;
        private System.Windows.Forms.ToolStripMenuItem menuIdiomas;
        private System.Windows.Forms.ToolStripMenuItem menuGestionIdiomas;
        private System.Windows.Forms.ToolStripMenuItem menuSeleccionIdioma;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripLabel lblSesion;
        private System.Windows.Forms.ToolStripLabel lblEstadoSesion;
    }
}
