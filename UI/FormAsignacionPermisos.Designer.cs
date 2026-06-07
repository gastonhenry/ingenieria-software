namespace UI
{
    partial class FormAsignacionPermisos
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
            this.lstUsuarios = new System.Windows.Forms.ListBox();

            this.pnlCol2 = new System.Windows.Forms.Panel();
            this.lblCol2 = new System.Windows.Forms.Label();
            this.lstAsignado = new System.Windows.Forms.ListBox();
            this.pnlCol2Botones = new System.Windows.Forms.Panel();
            this.btnQuitar = new UI.BotonPlano();

            this.pnlCol3 = new System.Windows.Forms.Panel();
            this.lblCol3 = new System.Windows.Forms.Label();
            this.lstDisponible = new System.Windows.Forms.ListBox();
            this.pnlCol3Botones = new System.Windows.Forms.Panel();
            this.btnAsignar = new UI.BotonPlano();

            this.pnlTitulo.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.pnlCol1.SuspendLayout();
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
            this.lblTitulo.Text = "Asignación de Permisos a Usuarios";

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
            // pnlCol1 — Usuarios
            //
            this.pnlCol1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCol1.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCol1.Controls.Add(this.lstUsuarios);
            this.pnlCol1.Controls.Add(this.lblCol1);
            //
            this.lblCol1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCol1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCol1.Height = 28;
            this.lblCol1.Text = "Usuarios";
            this.lblCol1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.lstUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstUsuarios.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstUsuarios.IntegralHeight = false;
            this.lstUsuarios.HorizontalScrollbar = true;
            this.lstUsuarios.SelectedIndexChanged += new System.EventHandler(this.lstUsuarios_SelectedIndexChanged);

            //
            // pnlCol2 — Asignaciones
            //
            this.pnlCol2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCol2.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCol2.Controls.Add(this.lstAsignado);
            this.pnlCol2.Controls.Add(this.pnlCol2Botones);
            this.pnlCol2.Controls.Add(this.lblCol2);
            //
            this.lblCol2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCol2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCol2.Height = 28;
            this.lblCol2.Text = "Asignaciones del usuario";
            this.lblCol2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.lstAsignado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAsignado.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstAsignado.IntegralHeight = false;
            this.lstAsignado.HorizontalScrollbar = true;
            this.lstAsignado.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
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
            this.btnQuitar.Text = "Quitar del usuario →";
            this.btnQuitar.UseVisualStyleBackColor = false;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);

            //
            // pnlCol3 — Disponibles
            //
            this.pnlCol3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCol3.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCol3.Controls.Add(this.lstDisponible);
            this.pnlCol3.Controls.Add(this.pnlCol3Botones);
            this.pnlCol3.Controls.Add(this.lblCol3);
            //
            this.lblCol3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCol3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCol3.Height = 28;
            this.lblCol3.Text = "Roles y Permisos disponibles";
            this.lblCol3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.lstDisponible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDisponible.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstDisponible.IntegralHeight = false;
            this.lstDisponible.HorizontalScrollbar = true;
            this.lstDisponible.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            //
            this.pnlCol3Botones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCol3Botones.Height = 50;
            this.pnlCol3Botones.Controls.Add(this.btnAsignar);
            //
            this.btnAsignar.BackColor = System.Drawing.Color.FromArgb(40, 140, 90);
            this.btnAsignar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAsignar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAsignar.ForeColor = System.Drawing.Color.White;
            this.btnAsignar.Location = new System.Drawing.Point(0, 10);
            this.btnAsignar.Size = new System.Drawing.Size(200, 32);
            this.btnAsignar.Text = "← Asignar al usuario";
            this.btnAsignar.UseVisualStyleBackColor = false;
            this.btnAsignar.Click += new System.EventHandler(this.btnAsignar_Click);

            //
            // FormAsignacionPermisos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 630);
            this.Controls.Add(this.tableMain);
            this.Controls.Add(this.pnlTitulo);
            this.Name = "FormAsignacionPermisos";
            this.Text = "Asignación de Permisos a Usuarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.tableMain.ResumeLayout(false);
            this.pnlCol1.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox lstUsuarios;

        private System.Windows.Forms.Panel pnlCol2;
        private System.Windows.Forms.Label lblCol2;
        private System.Windows.Forms.ListBox lstAsignado;
        private System.Windows.Forms.Panel pnlCol2Botones;
        private UI.BotonPlano btnQuitar;

        private System.Windows.Forms.Panel pnlCol3;
        private System.Windows.Forms.Label lblCol3;
        private System.Windows.Forms.ListBox lstDisponible;
        private System.Windows.Forms.Panel pnlCol3Botones;
        private UI.BotonPlano btnAsignar;
    }
}
