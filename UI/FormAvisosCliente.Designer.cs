namespace UI
{
    partial class FormAvisosCliente
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDocBusqueda;
        private System.Windows.Forms.TextBox txtDocBusqueda;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ListBox lstClientes;
        private System.Windows.Forms.Panel pnlCliente;
        private System.Windows.Forms.Label lblClienteNombre;
        private System.Windows.Forms.Label lblClienteContacto;
        private System.Windows.Forms.Label lblEstadoAvisos;
        private System.Windows.Forms.Button btnSuspender;
        private System.Windows.Forms.Button btnReanudar;
        private System.Windows.Forms.Label lblBusquedas;
        private System.Windows.Forms.DataGridView dgvBusquedas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBusca;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAceptaAvisos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVence;
        private System.Windows.Forms.Button btnCerrarBusqueda;
        private System.Windows.Forms.Label lblAvisos;
        private System.Windows.Forms.DataGridView dgvAvisos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvisoFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvisoUnidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvisoResultado;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDocBusqueda = new System.Windows.Forms.Label();
            this.txtDocBusqueda = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lstClientes = new System.Windows.Forms.ListBox();
            this.pnlCliente = new System.Windows.Forms.Panel();
            this.lblClienteNombre = new System.Windows.Forms.Label();
            this.lblClienteContacto = new System.Windows.Forms.Label();
            this.lblEstadoAvisos = new System.Windows.Forms.Label();
            this.btnSuspender = new System.Windows.Forms.Button();
            this.btnReanudar = new System.Windows.Forms.Button();
            this.lblBusquedas = new System.Windows.Forms.Label();
            this.dgvBusquedas = new System.Windows.Forms.DataGridView();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBusca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAceptaAvisos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCerrarBusqueda = new System.Windows.Forms.Button();
            this.lblAvisos = new System.Windows.Forms.Label();
            this.dgvAvisos = new System.Windows.Forms.DataGridView();
            this.colAvisoFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvisoUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvisoResultado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusquedas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvisos)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Avisos a clientes";
            //
            // búsqueda de cliente
            //
            this.lblDocBusqueda.AutoSize = true;
            this.lblDocBusqueda.Location = new System.Drawing.Point(20, 62);
            this.lblDocBusqueda.Name = "lblDocBusqueda";
            this.lblDocBusqueda.Text = "Cliente (DNI/CUIT o nombre):";
            //
            this.txtDocBusqueda.Location = new System.Drawing.Point(20, 87);
            this.txtDocBusqueda.Name = "txtDocBusqueda";
            this.txtDocBusqueda.Size = new System.Drawing.Size(200, 27);
            this.txtDocBusqueda.TabIndex = 0;
            //
            this.btnBuscar.Location = new System.Drawing.Point(230, 84);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 32);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            this.lstClientes.FormattingEnabled = true;
            this.lstClientes.ItemHeight = 20;
            this.lstClientes.Location = new System.Drawing.Point(20, 125);
            this.lstClientes.Name = "lstClientes";
            this.lstClientes.Size = new System.Drawing.Size(410, 124);
            this.lstClientes.TabIndex = 2;
            this.lstClientes.SelectedIndexChanged += new System.EventHandler(this.lstClientes_SelectedIndexChanged);
            //
            // pnlCliente
            //
            this.pnlCliente.BackColor = System.Drawing.Color.White;
            this.pnlCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCliente.Controls.Add(this.lblClienteNombre);
            this.pnlCliente.Controls.Add(this.lblClienteContacto);
            this.pnlCliente.Controls.Add(this.lblEstadoAvisos);
            this.pnlCliente.Controls.Add(this.btnSuspender);
            this.pnlCliente.Controls.Add(this.btnReanudar);
            this.pnlCliente.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlCliente.Location = new System.Drawing.Point(450, 62);
            this.pnlCliente.Name = "pnlCliente";
            this.pnlCliente.Size = new System.Drawing.Size(575, 187);
            //
            this.lblClienteNombre.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblClienteNombre.AutoEllipsis = true;
            this.lblClienteNombre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblClienteNombre.Location = new System.Drawing.Point(12, 8);
            this.lblClienteNombre.Name = "lblClienteNombre";
            this.lblClienteNombre.Size = new System.Drawing.Size(550, 28);
            //
            this.lblClienteContacto.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblClienteContacto.AutoEllipsis = true;
            this.lblClienteContacto.ForeColor = System.Drawing.Color.FromArgb(90, 90, 100);
            this.lblClienteContacto.Location = new System.Drawing.Point(12, 40);
            this.lblClienteContacto.Name = "lblClienteContacto";
            this.lblClienteContacto.Size = new System.Drawing.Size(550, 24);
            //
            this.lblEstadoAvisos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lblEstadoAvisos.AutoEllipsis = true;
            this.lblEstadoAvisos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstadoAvisos.Location = new System.Drawing.Point(12, 70);
            this.lblEstadoAvisos.Name = "lblEstadoAvisos";
            this.lblEstadoAvisos.Size = new System.Drawing.Size(550, 60);
            //
            this.btnSuspender.Location = new System.Drawing.Point(12, 138);
            this.btnSuspender.Name = "btnSuspender";
            this.btnSuspender.Size = new System.Drawing.Size(130, 34);
            this.btnSuspender.TabIndex = 1;
            this.btnSuspender.Text = "Suspender avisos";
            this.btnSuspender.Click += new System.EventHandler(this.btnSuspender_Click);
            //
            this.btnReanudar.Location = new System.Drawing.Point(152, 138);
            this.btnReanudar.Name = "btnReanudar";
            this.btnReanudar.Size = new System.Drawing.Size(130, 34);
            this.btnReanudar.TabIndex = 2;
            this.btnReanudar.Text = "Reanudar avisos";
            this.btnReanudar.Click += new System.EventHandler(this.btnReanudar_Click);
            //
            // lblBusquedas / dgvBusquedas
            //
            this.lblBusquedas.AutoSize = true;
            this.lblBusquedas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblBusquedas.Location = new System.Drawing.Point(20, 265);
            this.lblBusquedas.Name = "lblBusquedas";
            this.lblBusquedas.Text = "Búsquedas del cliente";
            //
            this.dgvBusquedas.AllowUserToAddRows = false;
            this.dgvBusquedas.AllowUserToDeleteRows = false;
            this.dgvBusquedas.AllowUserToResizeRows = false;
            this.dgvBusquedas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBusquedas.BackgroundColor = System.Drawing.Color.White;
            this.dgvBusquedas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colFecha, this.colBusca, this.colAceptaAvisos, this.colEstado, this.colVence});
            this.dgvBusquedas.Location = new System.Drawing.Point(20, 295);
            this.dgvBusquedas.MultiSelect = false;
            this.dgvBusquedas.Name = "dgvBusquedas";
            this.dgvBusquedas.ReadOnly = true;
            this.dgvBusquedas.RowHeadersVisible = false;
            this.dgvBusquedas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBusquedas.Size = new System.Drawing.Size(825, 170);
            this.dgvBusquedas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvBusquedas.TabIndex = 3;
            this.dgvBusquedas.SelectionChanged += new System.EventHandler(this.dgvBusquedas_SelectionChanged);
            //
            this.colFecha.HeaderText = "Entrevista";
            this.colFecha.Name = "colFecha";
            this.colFecha.FillWeight = 15F;
            this.colBusca.HeaderText = "Busca";
            this.colBusca.Name = "colBusca";
            this.colBusca.FillWeight = 45F;
            this.colAceptaAvisos.HeaderText = "Acepta avisos";
            this.colAceptaAvisos.Name = "colAceptaAvisos";
            this.colAceptaAvisos.FillWeight = 12F;
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.FillWeight = 13F;
            this.colVence.HeaderText = "Vence";
            this.colVence.Name = "colVence";
            this.colVence.FillWeight = 15F;
            //
            this.btnCerrarBusqueda.Location = new System.Drawing.Point(855, 295);
            this.btnCerrarBusqueda.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCerrarBusqueda.Name = "btnCerrarBusqueda";
            this.btnCerrarBusqueda.Size = new System.Drawing.Size(170, 56);
            this.btnCerrarBusqueda.TabIndex = 4;
            this.btnCerrarBusqueda.Text = "Cerrar búsqueda seleccionada";
            this.btnCerrarBusqueda.Click += new System.EventHandler(this.btnCerrarBusqueda_Click);
            //
            // lblAvisos / dgvAvisos
            //
            this.lblAvisos.AutoSize = true;
            this.lblAvisos.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAvisos.Location = new System.Drawing.Point(20, 480);
            this.lblAvisos.Name = "lblAvisos";
            this.lblAvisos.Text = "Avisos enviados";
            //
            this.dgvAvisos.AllowUserToAddRows = false;
            this.dgvAvisos.AllowUserToDeleteRows = false;
            this.dgvAvisos.AllowUserToResizeRows = false;
            this.dgvAvisos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAvisos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAvisos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colAvisoFecha, this.colAvisoUnidad, this.colAvisoResultado});
            this.dgvAvisos.Location = new System.Drawing.Point(20, 510);
            this.dgvAvisos.MultiSelect = false;
            this.dgvAvisos.Name = "dgvAvisos";
            this.dgvAvisos.ReadOnly = true;
            this.dgvAvisos.RowHeadersVisible = false;
            this.dgvAvisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvisos.Size = new System.Drawing.Size(1005, 150);
            this.dgvAvisos.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvAvisos.TabIndex = 5;
            //
            this.colAvisoFecha.HeaderText = "Fecha";
            this.colAvisoFecha.Name = "colAvisoFecha";
            this.colAvisoFecha.FillWeight = 20F;
            this.colAvisoUnidad.HeaderText = "Unidad";
            this.colAvisoUnidad.Name = "colAvisoUnidad";
            this.colAvisoUnidad.FillWeight = 55F;
            this.colAvisoResultado.HeaderText = "Resultado";
            this.colAvisoResultado.Name = "colAvisoResultado";
            this.colAvisoResultado.FillWeight = 25F;
            //
            // FormAvisosCliente
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1045, 680);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblDocBusqueda);
            this.Controls.Add(this.txtDocBusqueda);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lstClientes);
            this.Controls.Add(this.pnlCliente);
            this.Controls.Add(this.lblBusquedas);
            this.Controls.Add(this.dgvBusquedas);
            this.Controls.Add(this.btnCerrarBusqueda);
            this.Controls.Add(this.lblAvisos);
            this.Controls.Add(this.dgvAvisos);
            this.Name = "FormAvisosCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Avisos a clientes";
            this.pnlCliente.ResumeLayout(false);
            this.pnlCliente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusquedas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvisos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
