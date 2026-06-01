using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
        }

        private void btnAccesoRapido_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Próximamente", "Acceso rápido",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
