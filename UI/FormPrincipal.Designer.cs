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
            this.menuMantenimiento = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUnidades = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegistrarUnidad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVerUnidades = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTemplateChecklist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegistrarMarca = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegistrarModelo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPublicaciones = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVerPublicaciones = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPersonas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegistrarPersona = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuUnidades,
            this.menuPublicaciones,
            this.menuPersonas,
            this.menuUsuarios,
            this.menuPermisos,
            this.menuBitacora,
            this.menuIdiomas,
            this.menuSeleccionIdioma,
            this.menuMantenimiento,
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
            // menuUnidades — N01
            //
            this.menuUnidades.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuRegistrarUnidad,
                this.menuVerUnidades,
                this.menuTemplateChecklist,
                this.menuRegistrarMarca,
                this.menuRegistrarModelo});
            this.menuUnidades.ForeColor = System.Drawing.Color.White;
            this.menuUnidades.Name = "menuUnidades";
            this.menuUnidades.Size = new System.Drawing.Size(75, 25);
            this.menuUnidades.Text = "Unidades";
            //
            this.menuRegistrarUnidad.Name = "menuRegistrarUnidad";
            this.menuRegistrarUnidad.Size = new System.Drawing.Size(220, 22);
            this.menuRegistrarUnidad.Text = "Registrar Unidad";
            this.menuRegistrarUnidad.Click += new System.EventHandler(this.menuRegistrarUnidad_Click);
            //
            this.menuVerUnidades.Name = "menuVerUnidades";
            this.menuVerUnidades.Size = new System.Drawing.Size(220, 22);
            this.menuVerUnidades.Text = "Ver Unidades";
            this.menuVerUnidades.Click += new System.EventHandler(this.menuVerUnidades_Click);
            //
            this.menuTemplateChecklist.Name = "menuTemplateChecklist";
            this.menuTemplateChecklist.Size = new System.Drawing.Size(220, 22);
            this.menuTemplateChecklist.Text = "Template de Checklist";
            this.menuTemplateChecklist.Click += new System.EventHandler(this.menuTemplateChecklist_Click);
            //
            this.menuRegistrarMarca.Name = "menuRegistrarMarca";
            this.menuRegistrarMarca.Size = new System.Drawing.Size(220, 22);
            this.menuRegistrarMarca.Text = "Registrar Marca";
            this.menuRegistrarMarca.Click += new System.EventHandler(this.menuRegistrarMarca_Click);
            //
            this.menuRegistrarModelo.Name = "menuRegistrarModelo";
            this.menuRegistrarModelo.Size = new System.Drawing.Size(220, 22);
            this.menuRegistrarModelo.Text = "Registrar Modelo";
            this.menuRegistrarModelo.Click += new System.EventHandler(this.menuRegistrarModelo_Click);
            //
            // menuPublicaciones (top-level)
            //
            this.menuPublicaciones.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuVerPublicaciones});
            this.menuPublicaciones.ForeColor = System.Drawing.Color.White;
            this.menuPublicaciones.Name = "menuPublicaciones";
            this.menuPublicaciones.Size = new System.Drawing.Size(95, 25);
            this.menuPublicaciones.Text = "Publicaciones";
            //
            this.menuVerPublicaciones.Name = "menuVerPublicaciones";
            this.menuVerPublicaciones.Size = new System.Drawing.Size(220, 22);
            this.menuVerPublicaciones.Text = "Ver Publicaciones";
            this.menuVerPublicaciones.Click += new System.EventHandler(this.menuVerPublicaciones_Click);
            //
            // menuPersonas (top-level)
            //
            this.menuPersonas.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuRegistrarPersona});
            this.menuPersonas.ForeColor = System.Drawing.Color.White;
            this.menuPersonas.Name = "menuPersonas";
            this.menuPersonas.Size = new System.Drawing.Size(75, 25);
            this.menuPersonas.Text = "Personas";
            //
            this.menuRegistrarPersona.Name = "menuRegistrarPersona";
            this.menuRegistrarPersona.Size = new System.Drawing.Size(220, 22);
            this.menuRegistrarPersona.Text = "Registrar Persona";
            this.menuRegistrarPersona.Click += new System.EventHandler(this.menuRegistrarPersona_Click);
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
            // menuMantenimiento
            //
            this.menuMantenimiento.ForeColor = System.Drawing.Color.White;
            this.menuMantenimiento.Name = "menuMantenimiento";
            this.menuMantenimiento.Size = new System.Drawing.Size(100, 25);
            this.menuMantenimiento.Text = "Mantenimiento";
            this.menuMantenimiento.Click += new System.EventHandler(this.menuMantenimiento_Click);
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
        private System.Windows.Forms.ToolStripMenuItem menuMantenimiento;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripMenuItem menuUnidades;
        private System.Windows.Forms.ToolStripMenuItem menuRegistrarUnidad;
        private System.Windows.Forms.ToolStripMenuItem menuVerUnidades;
        private System.Windows.Forms.ToolStripMenuItem menuTemplateChecklist;
        private System.Windows.Forms.ToolStripMenuItem menuRegistrarMarca;
        private System.Windows.Forms.ToolStripMenuItem menuRegistrarModelo;
        private System.Windows.Forms.ToolStripMenuItem menuPublicaciones;
        private System.Windows.Forms.ToolStripMenuItem menuVerPublicaciones;
        private System.Windows.Forms.ToolStripMenuItem menuPersonas;
        private System.Windows.Forms.ToolStripMenuItem menuRegistrarPersona;
        private System.Windows.Forms.ToolStripLabel lblSesion;
        private System.Windows.Forms.ToolStripLabel lblEstadoSesion;
    }
}
