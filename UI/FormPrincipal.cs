using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void menuInsertarUsuario_Click(object sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is FormInsertarUsuario)
                {
                    child.Activate();
                    return;
                }
            }

            var form = new FormInsertarUsuario();
            form.MdiParent = this;
            form.Show();
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
