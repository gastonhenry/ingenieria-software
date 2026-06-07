namespace UI
{
    partial class FormIdiomas
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

            this.pnlIzq = new System.Windows.Forms.Panel();
            this.lblIdiomas = new System.Windows.Forms.Label();
            this.lstIdiomas = new System.Windows.Forms.ListBox();
            this.pnlIzqBotones = new System.Windows.Forms.Panel();
            this.btnNuevoIdioma = new UI.BotonPlano();
            this.btnEliminarIdioma = new UI.BotonPlano();

            this.pnlDer = new System.Windows.Forms.Panel();
            this.lblTraducciones = new System.Windows.Forms.Label();
            this.pnlAlerta = new System.Windows.Forms.Panel();
            this.lblAlerta = new System.Windows.Forms.Label();
            this.grdTraducciones = new System.Windows.Forms.DataGridView();
            this.colForm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTexto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDerBotones = new System.Windows.Forms.Panel();
            this.btnGuardar = new UI.BotonPlano();

            this.pnlTitulo.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.pnlIzq.SuspendLayout();
            this.pnlIzqBotones.SuspendLayout();
            this.pnlDer.SuspendLayout();
            this.pnlAlerta.SuspendLayout();
            this.pnlDerBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTraducciones)).BeginInit();
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
            this.lblTitulo.Text = "Gestión de Idiomas";

            //
            // tableMain
            //
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.ColumnCount = 2;
            this.tableMain.RowCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.pnlIzq, 0, 0);
            this.tableMain.Controls.Add(this.pnlDer, 1, 0);
            this.tableMain.Location = new System.Drawing.Point(0, 60);
            this.tableMain.Size = new System.Drawing.Size(960, 580);

            //
            // pnlIzq — Idiomas
            //
            this.pnlIzq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIzq.Padding = new System.Windows.Forms.Padding(8);
            this.pnlIzq.Controls.Add(this.lstIdiomas);
            this.pnlIzq.Controls.Add(this.pnlIzqBotones);
            this.pnlIzq.Controls.Add(this.lblIdiomas);

            //
            // lblIdiomas
            //
            this.lblIdiomas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIdiomas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIdiomas.Height = 28;
            this.lblIdiomas.Text = "Idiomas disponibles";
            this.lblIdiomas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // lstIdiomas
            //
            this.lstIdiomas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstIdiomas.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstIdiomas.IntegralHeight = false;
            this.lstIdiomas.HorizontalScrollbar = true;
            this.lstIdiomas.SelectedIndexChanged += new System.EventHandler(this.lstIdiomas_SelectedIndexChanged);

            //
            // pnlIzqBotones
            //
            this.pnlIzqBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlIzqBotones.Height = 84;
            this.pnlIzqBotones.Controls.Add(this.btnNuevoIdioma);
            this.pnlIzqBotones.Controls.Add(this.btnEliminarIdioma);

            //
            // btnNuevoIdioma
            //
            this.btnNuevoIdioma.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNuevoIdioma.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.btnNuevoIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevoIdioma.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNuevoIdioma.ForeColor = System.Drawing.Color.White;
            this.btnNuevoIdioma.Location = new System.Drawing.Point(0, 6);
            this.btnNuevoIdioma.Size = new System.Drawing.Size(145, 32);
            this.btnNuevoIdioma.Text = "Nuevo Idioma";
            this.btnNuevoIdioma.UseVisualStyleBackColor = false;
            this.btnNuevoIdioma.Click += new System.EventHandler(this.btnNuevoIdioma_Click);

            //
            // btnEliminarIdioma
            //
            this.btnEliminarIdioma.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminarIdioma.BackColor = System.Drawing.Color.FromArgb(190, 60, 60);
            this.btnEliminarIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarIdioma.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEliminarIdioma.ForeColor = System.Drawing.Color.White;
            this.btnEliminarIdioma.Location = new System.Drawing.Point(0, 44);
            this.btnEliminarIdioma.Size = new System.Drawing.Size(145, 32);
            this.btnEliminarIdioma.Text = "Eliminar Idioma";
            this.btnEliminarIdioma.UseVisualStyleBackColor = false;
            this.btnEliminarIdioma.Click += new System.EventHandler(this.btnEliminarIdioma_Click);

            //
            // pnlDer — Traducciones
            //
            this.pnlDer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDer.Padding = new System.Windows.Forms.Padding(15);
            this.pnlDer.Controls.Add(this.grdTraducciones);
            this.pnlDer.Controls.Add(this.pnlAlerta);
            this.pnlDer.Controls.Add(this.pnlDerBotones);
            this.pnlDer.Controls.Add(this.lblTraducciones);

            //
            // pnlAlerta
            //
            this.pnlAlerta.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlerta.BackColor = System.Drawing.Color.FromArgb(255, 243, 205);
            this.pnlAlerta.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlAlerta.Height = 40;
            this.pnlAlerta.Visible = false;
            this.pnlAlerta.Controls.Add(this.lblAlerta);

            //
            // lblAlerta
            //
            this.lblAlerta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlerta.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAlerta.ForeColor = System.Drawing.Color.FromArgb(133, 100, 4);
            this.lblAlerta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAlerta.Text = "⚠  Idioma en proceso de creación — completá todas las traducciones para poder activarlo.";

            //
            // lblTraducciones
            //
            this.lblTraducciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTraducciones.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTraducciones.Height = 28;
            this.lblTraducciones.Text = "Traducciones";
            this.lblTraducciones.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // grdTraducciones
            //
            this.grdTraducciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTraducciones.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.grdTraducciones.AutoGenerateColumns = false;
            this.grdTraducciones.AllowUserToAddRows = false;
            this.grdTraducciones.AllowUserToDeleteRows = false;
            this.grdTraducciones.AllowUserToResizeRows = false;
            this.grdTraducciones.RowHeadersVisible = false;
            this.grdTraducciones.MultiSelect = false;
            this.grdTraducciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdTraducciones.BackgroundColor = System.Drawing.Color.White;
            this.grdTraducciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdTraducciones.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 235, 245);
            this.grdTraducciones.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grdTraducciones.EnableHeadersVisualStyles = false;
            this.grdTraducciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colForm,
                this.colCodigo,
                this.colTexto});

            //
            // colForm
            //
            this.colForm.HeaderText = "Form";
            this.colForm.Name = "colForm";
            this.colForm.ReadOnly = true;
            this.colForm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;

            //
            // colCodigo
            //
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.ReadOnly = true;
            this.colCodigo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;

            //
            // colTexto
            //
            this.colTexto.HeaderText = "Texto";
            this.colTexto.Name = "colTexto";
            this.colTexto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            //
            // pnlDerBotones
            //
            this.pnlDerBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDerBotones.Height = 50;
            this.pnlDerBotones.Controls.Add(this.btnGuardar);

            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(40, 140, 90);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(0, 10);
            this.btnGuardar.Size = new System.Drawing.Size(170, 32);
            this.btnGuardar.Text = "Guardar cambios";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            //
            // FormIdiomas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 640);
            this.Controls.Add(this.tableMain);
            this.Controls.Add(this.pnlTitulo);
            this.Name = "FormIdiomas";
            this.Text = "Gestión de Idiomas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.tableMain.ResumeLayout(false);
            this.pnlIzq.ResumeLayout(false);
            this.pnlIzqBotones.ResumeLayout(false);
            this.pnlDer.ResumeLayout(false);
            this.pnlAlerta.ResumeLayout(false);
            this.pnlDerBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTraducciones)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;

        private System.Windows.Forms.TableLayoutPanel tableMain;

        private System.Windows.Forms.Panel pnlIzq;
        private System.Windows.Forms.Label lblIdiomas;
        private System.Windows.Forms.ListBox lstIdiomas;
        private System.Windows.Forms.Panel pnlIzqBotones;
        private UI.BotonPlano btnNuevoIdioma;
        private UI.BotonPlano btnEliminarIdioma;

        private System.Windows.Forms.Panel pnlDer;
        private System.Windows.Forms.Label lblTraducciones;
        private System.Windows.Forms.Panel pnlAlerta;
        private System.Windows.Forms.Label lblAlerta;
        private System.Windows.Forms.DataGridView grdTraducciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn colForm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTexto;
        private System.Windows.Forms.Panel pnlDerBotones;
        private UI.BotonPlano btnGuardar;
    }
}
