using System.Windows.Forms;

namespace UI
{
    partial class FormUsuarios
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
            this.pnlTitulo        = new System.Windows.Forms.Panel();
            this.lblTitulo        = new System.Windows.Forms.Label();
            this.grpFiltros       = new System.Windows.Forms.GroupBox();
            this.lblEstado        = new System.Windows.Forms.Label();
            this.cmbEstado        = new System.Windows.Forms.ComboBox();
            this.lblUsuarioFiltro = new System.Windows.Forms.Label();
            this.txtUsuarioFiltro = new System.Windows.Forms.TextBox();
            this.btnLimpiar       = new UI.BotonPlano();
            this.pnlAcciones      = new System.Windows.Forms.Panel();
            this.pnlBotones       = new System.Windows.Forms.Panel();
            this.btnActualizar    = new UI.BotonPlano();
            this.btnDesbloquear   = new UI.BotonPlano();
            this.btnBloquear      = new UI.BotonPlano();
            this.btnEditar        = new UI.BotonPlano();
            this.btnHistorial     = new UI.BotonPlano();
            this.lblHint          = new System.Windows.Forms.Label();
            this.pnlResumen       = new System.Windows.Forms.Panel();
            this.lblActivos       = new System.Windows.Forms.Label();
            this.lblBloqueados    = new System.Windows.Forms.Label();
            this.lblTotal         = new System.Windows.Forms.Label();
            this.dgvUsuarios      = new System.Windows.Forms.DataGridView();

            this.pnlTitulo.SuspendLayout();
            this.grpFiltros.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            this.pnlBotones.SuspendLayout();
            this.pnlResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();

            // ── pnlTitulo ───────────────────────────────────────────────────
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(30, 90, 200);
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(800, 60);

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(15, 22);
            this.lblTitulo.Text = "Gestión de Usuarios";

            // ── grpFiltros ──────────────────────────────────────────────────
            this.grpFiltros.Controls.Add(this.lblEstado);
            this.grpFiltros.Controls.Add(this.cmbEstado);
            this.grpFiltros.Controls.Add(this.lblUsuarioFiltro);
            this.grpFiltros.Controls.Add(this.txtUsuarioFiltro);
            this.grpFiltros.Controls.Add(this.btnLimpiar);
            this.grpFiltros.Dock   = System.Windows.Forms.DockStyle.Top;
            this.grpFiltros.Font   = new System.Drawing.Font("Segoe UI", 9f);
            this.grpFiltros.Height = 62;
            this.grpFiltros.Padding = new Padding(10);
            this.grpFiltros.Name   = "grpFiltros";
            this.grpFiltros.Text   = "Filtros";

            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(12, 23);
            this.lblEstado.Text     = "Estado:";

            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Location      = new System.Drawing.Point(65, 19);
            this.cmbEstado.Name          = "cmbEstado";
            this.cmbEstado.Size          = new System.Drawing.Size(130, 23);
            this.cmbEstado.TabIndex      = 0;

            this.lblUsuarioFiltro.AutoSize = true;
            this.lblUsuarioFiltro.Location = new System.Drawing.Point(210, 23);
            this.lblUsuarioFiltro.Text     = "Buscar usuario:";

            this.txtUsuarioFiltro.Location = new System.Drawing.Point(305, 20);
            this.txtUsuarioFiltro.Name     = "txtUsuarioFiltro";
            this.txtUsuarioFiltro.Size     = new System.Drawing.Size(150, 23);
            this.txtUsuarioFiltro.TabIndex = 1;

            this.btnLimpiar.Location            = new System.Drawing.Point(470, 18);
            this.btnLimpiar.Name                = "btnLimpiar";
            this.btnLimpiar.Size                = new System.Drawing.Size(90, 26);
            this.btnLimpiar.TabIndex            = 3;
            this.btnLimpiar.Text                = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);

            // ── pnlAcciones (Bottom, 55px) ──────────────────────────────────
            this.pnlAcciones.Controls.Add(this.pnlResumen);
            this.pnlAcciones.Controls.Add(this.pnlBotones);
            this.pnlAcciones.BackColor = System.Drawing.Color.FromArgb(230, 238, 255);
            this.pnlAcciones.Dock      = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAcciones.Height    = 55;
            this.pnlAcciones.Name      = "pnlAcciones";

            // ── pnlBotones (Dock=Right dentro de pnlAcciones) ───────────────
            this.pnlBotones.Controls.Add(this.btnActualizar);
            this.pnlBotones.Controls.Add(this.btnDesbloquear);
            this.pnlBotones.Controls.Add(this.btnBloquear);
            this.pnlBotones.Controls.Add(this.btnHistorial);
            this.pnlBotones.Controls.Add(this.btnEditar);
            this.pnlBotones.Controls.Add(this.lblHint);
            this.pnlBotones.BackColor = System.Drawing.Color.Transparent;
            this.pnlBotones.Dock      = System.Windows.Forms.DockStyle.Right;
            this.pnlBotones.Width     = 590;
            this.pnlBotones.Name      = "pnlBotones";

            this.lblHint.AutoSize  = true;
            this.lblHint.Font      = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Italic);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(100, 100, 140);
            this.lblHint.Location  = new System.Drawing.Point(4, 18);
            this.lblHint.Name      = "lblHint";
            this.lblHint.Text      = "← Seleccioná un usuario";

            this.btnEditar.Location              = new System.Drawing.Point(145, 13);
            this.btnEditar.Name                  = "btnEditar";
            this.btnEditar.Size                  = new System.Drawing.Size(90, 28);
            this.btnEditar.TabIndex              = 0;
            this.btnEditar.Text                  = "Editar";
            this.btnEditar.Enabled               = false;
            this.btnEditar.Visible               = false;
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            this.btnHistorial.Location           = new System.Drawing.Point(245, 13);
            this.btnHistorial.Name               = "btnHistorial";
            this.btnHistorial.Size               = new System.Drawing.Size(95, 28);
            this.btnHistorial.TabIndex           = 1;
            this.btnHistorial.Text               = "Historial";
            this.btnHistorial.Enabled            = false;
            this.btnHistorial.Visible            = false;
            this.btnHistorial.UseVisualStyleBackColor = true;
            this.btnHistorial.Click += new System.EventHandler(this.btnHistorial_Click);

            this.btnBloquear.Location            = new System.Drawing.Point(350, 13);
            this.btnBloquear.Name                = "btnBloquear";
            this.btnBloquear.Size                = new System.Drawing.Size(95, 28);
            this.btnBloquear.TabIndex            = 2;
            this.btnBloquear.Text                = "Bloquear";
            this.btnBloquear.Enabled             = false;
            this.btnBloquear.UseVisualStyleBackColor = true;
            this.btnBloquear.Click += new System.EventHandler(this.btnBloquear_Click);

            this.btnDesbloquear.Location            = new System.Drawing.Point(452, 13);
            this.btnDesbloquear.Name                = "btnDesbloquear";
            this.btnDesbloquear.Size                = new System.Drawing.Size(110, 28);
            this.btnDesbloquear.TabIndex            = 3;
            this.btnDesbloquear.Text                = "Desbloquear";
            this.btnDesbloquear.Enabled             = false;
            this.btnDesbloquear.UseVisualStyleBackColor = true;
            this.btnDesbloquear.Click += new System.EventHandler(this.btnDesbloquear_Click);

            this.btnActualizar.Location            = new System.Drawing.Point(564, 13);
            this.btnActualizar.Name                = "btnActualizar";
            this.btnActualizar.Size                = new System.Drawing.Size(22, 28);
            this.btnActualizar.TabIndex            = 4;
            this.btnActualizar.Text                = "↺";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);

            // ── pnlResumen (Dock=Fill dentro de pnlAcciones) ─────────────────
            this.pnlResumen.Controls.Add(this.lblTotal);
            this.pnlResumen.Controls.Add(this.lblBloqueados);
            this.pnlResumen.Controls.Add(this.lblActivos);
            this.pnlResumen.BackColor = System.Drawing.Color.Transparent;
            this.pnlResumen.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlResumen.Name      = "pnlResumen";

            this.lblTotal.AutoSize = true;
            this.lblTotal.Font     = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(15, 18);
            this.lblTotal.Name     = "lblTotal";
            this.lblTotal.Text     = "Total: 0";

            this.lblBloqueados.AutoSize = true;
            this.lblBloqueados.Font     = new System.Drawing.Font("Segoe UI", 9f);
            this.lblBloqueados.Location = new System.Drawing.Point(110, 18);
            this.lblBloqueados.Name     = "lblBloqueados";
            this.lblBloqueados.Text     = "Bloqueados: 0";

            this.lblActivos.AutoSize = true;
            this.lblActivos.Font     = new System.Drawing.Font("Segoe UI", 9f);
            this.lblActivos.Location = new System.Drawing.Point(230, 18);
            this.lblActivos.Name     = "lblActivos";
            this.lblActivos.Text     = "Activos: 0";

            // ── dgvUsuarios ──────────────────────────────────────────────────
            this.dgvUsuarios.AllowUserToAddRows    = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.ReadOnly              = true;
            this.dgvUsuarios.Dock                  = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsuarios.SelectionMode         = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.MultiSelect           = false;
            this.dgvUsuarios.RowHeadersVisible     = false;
            this.dgvUsuarios.Name                  = "dgvUsuarios";
            this.dgvUsuarios.TabIndex              = 0;

            // ── FormUsuarios ─────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.grpFiltros);
            this.Controls.Add(this.pnlTitulo);
            this.Name = "FormUsuarios";
            this.Text = "Gestión de Usuarios";

            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.pnlResumen.ResumeLayout(false);
            this.pnlResumen.PerformLayout();
            this.pnlBotones.ResumeLayout(false);
            this.pnlBotones.PerformLayout();
            this.pnlAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblUsuarioFiltro;
        private System.Windows.Forms.TextBox txtUsuarioFiltro;
        private UI.BotonPlano btnLimpiar;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Panel pnlResumen;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblBloqueados;
        private System.Windows.Forms.Label lblActivos;
        private System.Windows.Forms.Label lblHint;
        private UI.BotonPlano btnEditar;
        private UI.BotonPlano btnHistorial;
        private UI.BotonPlano btnBloquear;
        private UI.BotonPlano btnDesbloquear;
        private UI.BotonPlano btnActualizar;
        private System.Windows.Forms.DataGridView dgvUsuarios;
    }
}
