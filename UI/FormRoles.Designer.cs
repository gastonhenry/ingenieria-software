namespace UI
{
    partial class FormRoles
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

            this.tableMain = new System.Windows.Forms.TableLayoutPanel();

            this.pnlCol1 = new System.Windows.Forms.Panel();
            this.lblCol1 = new System.Windows.Forms.Label();
            this.lstRoles = new System.Windows.Forms.ListBox();
            this.pnlCol1Botones = new System.Windows.Forms.Panel();
            this.btnNuevoRol = new System.Windows.Forms.Button();
            this.btnEliminarRol = new System.Windows.Forms.Button();

            this.pnlCol2 = new System.Windows.Forms.Panel();
            this.lblCol2 = new System.Windows.Forms.Label();
            this.lstContenido = new System.Windows.Forms.ListBox();
            this.pnlCol2Botones = new System.Windows.Forms.Panel();
            this.btnQuitar = new System.Windows.Forms.Button();

            this.pnlCol3 = new System.Windows.Forms.Panel();
            this.lblCol3 = new System.Windows.Forms.Label();
            this.lstDisponibles = new System.Windows.Forms.ListBox();
            this.pnlCol3Botones = new System.Windows.Forms.Panel();
            this.btnAgregar = new System.Windows.Forms.Button();

            this.pnlTitulo.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.pnlCol1.SuspendLayout();
            this.pnlCol1Botones.SuspendLayout();
            this.pnlCol2.SuspendLayout();
            this.pnlCol2Botones.SuspendLayout();
            this.pnlCol3.SuspendLayout();
            this.pnlCol3Botones.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlTitulo
            //
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(960, 60);
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 22);
            this.lblTitulo.Text = "Gestión de Roles";

            //
            // tableMain
            //
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.ColumnCount = 3;
            this.tableMain.RowCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.pnlCol1, 0, 0);
            this.tableMain.Controls.Add(this.pnlCol2, 1, 0);
            this.tableMain.Controls.Add(this.pnlCol3, 2, 0);
            this.tableMain.Location = new System.Drawing.Point(0, 50);
            this.tableMain.Size = new System.Drawing.Size(960, 580);

            //
            // pnlCol1 — Roles
            //
            this.pnlCol1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCol1.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCol1.Controls.Add(this.lstRoles);
            this.pnlCol1.Controls.Add(this.pnlCol1Botones);
            this.pnlCol1.Controls.Add(this.lblCol1);
            //
            this.lblCol1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCol1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCol1.Height = 28;
            this.lblCol1.Text = "Roles";
            this.lblCol1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.lstRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRoles.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstRoles.IntegralHeight = false;
            this.lstRoles.HorizontalScrollbar = true;
            this.lstRoles.SelectedIndexChanged += new System.EventHandler(this.lstRoles_SelectedIndexChanged);
            //
            this.pnlCol1Botones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCol1Botones.Height = 50;
            this.pnlCol1Botones.Controls.Add(this.btnNuevoRol);
            this.pnlCol1Botones.Controls.Add(this.btnEliminarRol);
            //
            this.btnNuevoRol.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.btnNuevoRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevoRol.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNuevoRol.ForeColor = System.Drawing.Color.White;
            this.btnNuevoRol.Location = new System.Drawing.Point(0, 10);
            this.btnNuevoRol.Size = new System.Drawing.Size(135, 32);
            this.btnNuevoRol.Text = "Nuevo Rol";
            this.btnNuevoRol.UseVisualStyleBackColor = false;
            this.btnNuevoRol.Click += new System.EventHandler(this.btnNuevoRol_Click);
            //
            this.btnEliminarRol.BackColor = System.Drawing.Color.FromArgb(190, 60, 60);
            this.btnEliminarRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarRol.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEliminarRol.ForeColor = System.Drawing.Color.White;
            this.btnEliminarRol.Location = new System.Drawing.Point(140, 10);
            this.btnEliminarRol.Size = new System.Drawing.Size(135, 32);
            this.btnEliminarRol.Text = "Eliminar Rol";
            this.btnEliminarRol.UseVisualStyleBackColor = false;
            this.btnEliminarRol.Click += new System.EventHandler(this.btnEliminarRol_Click);

            //
            // pnlCol2 — Contenido
            //
            this.pnlCol2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCol2.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCol2.Controls.Add(this.lstContenido);
            this.pnlCol2.Controls.Add(this.pnlCol2Botones);
            this.pnlCol2.Controls.Add(this.lblCol2);
            //
            this.lblCol2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCol2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCol2.Height = 28;
            this.lblCol2.Text = "Permisos del rol";
            this.lblCol2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.lstContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstContenido.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstContenido.IntegralHeight = false;
            this.lstContenido.HorizontalScrollbar = true;
            this.lstContenido.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            //
            this.pnlCol2Botones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCol2Botones.Height = 50;
            this.pnlCol2Botones.Controls.Add(this.btnQuitar);
            //
            this.btnQuitar.BackColor = System.Drawing.Color.FromArgb(190, 60, 60);
            this.btnQuitar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuitar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnQuitar.ForeColor = System.Drawing.Color.White;
            this.btnQuitar.Location = new System.Drawing.Point(0, 10);
            this.btnQuitar.Size = new System.Drawing.Size(200, 32);
            this.btnQuitar.Text = "Quitar del rol →";
            this.btnQuitar.UseVisualStyleBackColor = false;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);

            //
            // pnlCol3 — Permisos disponibles
            //
            this.pnlCol3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCol3.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCol3.Controls.Add(this.lstDisponibles);
            this.pnlCol3.Controls.Add(this.pnlCol3Botones);
            this.pnlCol3.Controls.Add(this.lblCol3);
            //
            this.lblCol3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCol3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCol3.Height = 28;
            this.lblCol3.Text = "Permisos disponibles";
            this.lblCol3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.lstDisponibles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDisponibles.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstDisponibles.IntegralHeight = false;
            this.lstDisponibles.HorizontalScrollbar = true;
            this.lstDisponibles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            //
            this.pnlCol3Botones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCol3Botones.Height = 50;
            this.pnlCol3Botones.Controls.Add(this.btnAgregar);
            //
            this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(40, 140, 90);
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(0, 10);
            this.btnAgregar.Size = new System.Drawing.Size(200, 32);
            this.btnAgregar.Text = "← Agregar al rol";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            //
            // FormRoles
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 630);
            this.Controls.Add(this.tableMain);
            this.Controls.Add(this.pnlTitulo);
            this.Name = "FormRoles";
            this.Text = "Gestión de Roles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.tableMain.ResumeLayout(false);
            this.pnlCol1.ResumeLayout(false);
            this.pnlCol1Botones.ResumeLayout(false);
            this.pnlCol2.ResumeLayout(false);
            this.pnlCol2Botones.ResumeLayout(false);
            this.pnlCol3.ResumeLayout(false);
            this.pnlCol3Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;

        private System.Windows.Forms.TableLayoutPanel tableMain;

        private System.Windows.Forms.Panel pnlCol1;
        private System.Windows.Forms.Label lblCol1;
        private System.Windows.Forms.ListBox lstRoles;
        private System.Windows.Forms.Panel pnlCol1Botones;
        private System.Windows.Forms.Button btnNuevoRol;
        private System.Windows.Forms.Button btnEliminarRol;

        private System.Windows.Forms.Panel pnlCol2;
        private System.Windows.Forms.Label lblCol2;
        private System.Windows.Forms.ListBox lstContenido;
        private System.Windows.Forms.Panel pnlCol2Botones;
        private System.Windows.Forms.Button btnQuitar;

        private System.Windows.Forms.Panel pnlCol3;
        private System.Windows.Forms.Label lblCol3;
        private System.Windows.Forms.ListBox lstDisponibles;
        private System.Windows.Forms.Panel pnlCol3Botones;
        private System.Windows.Forms.Button btnAgregar;
    }
}
