using System;
using System.Windows.Forms;

namespace UI
{
    // Modal simple para capturar el motivo de pausa.
    public partial class FormPausar : Form
    {
        public string Motivo { get; private set; }

        public FormPausar()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string m = txtMotivo.Text.Trim();
            if (string.IsNullOrEmpty(m))
            {
                MessageBox.Show("El motivo de la pausa es obligatorio.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Motivo = m;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
