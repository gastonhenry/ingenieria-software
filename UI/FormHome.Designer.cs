namespace UI
{
    partial class FormHome
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

            this.tableCards = new System.Windows.Forms.TableLayoutPanel();

            this.pnlNovedades = new System.Windows.Forms.Panel();
            this.pnlNovedadesHeader = new System.Windows.Forms.Panel();
            this.lblNovedadesTitulo = new System.Windows.Forms.Label();
            this.lblNovedadesBody = new System.Windows.Forms.Label();

            this.pnlAccesos = new System.Windows.Forms.Panel();
            this.pnlAccesosHeader = new System.Windows.Forms.Panel();
            this.lblAccesosTitulo = new System.Windows.Forms.Label();
            this.flowAccesos = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAcceso1 = new System.Windows.Forms.Button();
            this.btnAcceso2 = new System.Windows.Forms.Button();
            this.btnAcceso3 = new System.Windows.Forms.Button();

            this.pnlTitulo.SuspendLayout();
            this.tableCards.SuspendLayout();
            this.pnlNovedades.SuspendLayout();
            this.pnlNovedadesHeader.SuspendLayout();
            this.pnlAccesos.SuspendLayout();
            this.pnlAccesosHeader.SuspendLayout();
            this.flowAccesos.SuspendLayout();
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
            this.lblTitulo.Text = "Inicio";

            //
            // tableCards
            //
            this.tableCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCards.ColumnCount = 2;
            this.tableCards.RowCount = 1;
            this.tableCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCards.Padding = new System.Windows.Forms.Padding(20);
            this.tableCards.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.tableCards.Controls.Add(this.pnlNovedades, 0, 0);
            this.tableCards.Controls.Add(this.pnlAccesos, 1, 0);

            //
            // pnlNovedades (card izquierda)
            //
            this.pnlNovedades.BackColor = System.Drawing.Color.White;
            this.pnlNovedades.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNovedades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNovedades.Margin = new System.Windows.Forms.Padding(10);
            this.pnlNovedades.Controls.Add(this.lblNovedadesBody);
            this.pnlNovedades.Controls.Add(this.pnlNovedadesHeader);
            //
            // pnlNovedadesHeader
            //
            this.pnlNovedadesHeader.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.pnlNovedadesHeader.Controls.Add(this.lblNovedadesTitulo);
            this.pnlNovedadesHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNovedadesHeader.Height = 42;
            //
            // lblNovedadesTitulo
            //
            this.lblNovedadesTitulo.AutoSize = true;
            this.lblNovedadesTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNovedadesTitulo.ForeColor = System.Drawing.Color.White;
            this.lblNovedadesTitulo.Location = new System.Drawing.Point(14, 10);
            this.lblNovedadesTitulo.Text = "Novedades";
            //
            // lblNovedadesBody
            //
            this.lblNovedadesBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNovedadesBody.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNovedadesBody.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            this.lblNovedadesBody.Padding = new System.Windows.Forms.Padding(14, 14, 14, 14);
            this.lblNovedadesBody.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";

            //
            // pnlAccesos (card derecha)
            //
            this.pnlAccesos.BackColor = System.Drawing.Color.White;
            this.pnlAccesos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAccesos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAccesos.Margin = new System.Windows.Forms.Padding(10);
            this.pnlAccesos.Controls.Add(this.flowAccesos);
            this.pnlAccesos.Controls.Add(this.pnlAccesosHeader);
            //
            // pnlAccesosHeader
            //
            this.pnlAccesosHeader.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.pnlAccesosHeader.Controls.Add(this.lblAccesosTitulo);
            this.pnlAccesosHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAccesosHeader.Height = 42;
            //
            // lblAccesosTitulo
            //
            this.lblAccesosTitulo.AutoSize = true;
            this.lblAccesosTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAccesosTitulo.ForeColor = System.Drawing.Color.White;
            this.lblAccesosTitulo.Location = new System.Drawing.Point(14, 10);
            this.lblAccesosTitulo.Text = "Accesos Rápidos";
            //
            // flowAccesos
            //
            this.flowAccesos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowAccesos.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowAccesos.WrapContents = false;
            this.flowAccesos.Padding = new System.Windows.Forms.Padding(14, 14, 14, 14);
            this.flowAccesos.Controls.Add(this.btnAcceso1);
            this.flowAccesos.Controls.Add(this.btnAcceso2);
            this.flowAccesos.Controls.Add(this.btnAcceso3);
            //
            // botones fantasma
            //
            ConfigurarBotonFantasma(this.btnAcceso1, "Ver últimas novedades");
            ConfigurarBotonFantasma(this.btnAcceso2, "Mis tareas pendientes");
            ConfigurarBotonFantasma(this.btnAcceso3, "Ayuda");

            this.btnAcceso1.Click += new System.EventHandler(this.btnAccesoRapido_Click);
            this.btnAcceso2.Click += new System.EventHandler(this.btnAccesoRapido_Click);
            this.btnAcceso3.Click += new System.EventHandler(this.btnAccesoRapido_Click);

            //
            // FormHome
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this.ClientSize = new System.Drawing.Size(960, 630);
            this.Controls.Add(this.tableCards);
            this.Controls.Add(this.pnlTitulo);
            this.Name = "FormHome";
            this.Text = "Inicio";

            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.tableCards.ResumeLayout(false);
            this.pnlNovedades.ResumeLayout(false);
            this.pnlNovedadesHeader.ResumeLayout(false);
            this.pnlNovedadesHeader.PerformLayout();
            this.pnlAccesos.ResumeLayout(false);
            this.pnlAccesosHeader.ResumeLayout(false);
            this.pnlAccesosHeader.PerformLayout();
            this.flowAccesos.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void ConfigurarBotonFantasma(System.Windows.Forms.Button btn, string texto)
        {
            btn.Width = 280;
            btn.Height = 40;
            btn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 90, 200);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(235, 242, 255);
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(215, 228, 250);
            btn.BackColor = System.Drawing.Color.White;
            btn.ForeColor = System.Drawing.Color.FromArgb(30, 90, 200);
            btn.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            btn.Text = texto;
            btn.UseVisualStyleBackColor = false;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TableLayoutPanel tableCards;

        private System.Windows.Forms.Panel pnlNovedades;
        private System.Windows.Forms.Panel pnlNovedadesHeader;
        private System.Windows.Forms.Label lblNovedadesTitulo;
        private System.Windows.Forms.Label lblNovedadesBody;

        private System.Windows.Forms.Panel pnlAccesos;
        private System.Windows.Forms.Panel pnlAccesosHeader;
        private System.Windows.Forms.Label lblAccesosTitulo;
        private System.Windows.Forms.FlowLayoutPanel flowAccesos;
        private System.Windows.Forms.Button btnAcceso1;
        private System.Windows.Forms.Button btnAcceso2;
        private System.Windows.Forms.Button btnAcceso3;
    }
}
