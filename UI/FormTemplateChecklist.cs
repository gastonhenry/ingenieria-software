using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class FormTemplateChecklist : Form, IObservadorIdioma
    {
        private const string CODIGO_FORM = "FormTemplateChecklist";

        private readonly IChecklistTemplateService _templateService;
        private readonly IIdiomaService _idiomaService;
        private bool _suscrito;

        public FormTemplateChecklist()
        {
            InitializeComponent();
            _templateService = new ChecklistTemplateService();
            _idiomaService = new IdiomaService();

            _idiomaService.Suscribir(this);
            _suscrito = true;
            ActualizarIdioma(_idiomaService.IdiomaActual());

            this.Load += (s, e) => Recargar();
        }

        private string Tr(string codigo, string fallback)
        {
            try
            {
                string t = _idiomaService?.Traducir(CODIGO_FORM, codigo);
                return string.IsNullOrEmpty(t) ? fallback : t;
            }
            catch { return fallback; }
        }

        private string TrError(Exception ex) => TraductorErrores.TraducirError(ex, _idiomaService);

        public void ActualizarIdioma(Idioma nuevoIdioma)
        {
            this.Text                    = Tr("title",                "Template de Checklist");
            lblTitulo.Text               = Tr("lblTitulo",            "Template de Checklist");
            btnNuevo.Text                = Tr("btnNuevo",             "Nuevo item");
            btnEditarDescripcion.Text    = Tr("btnEditarDescripcion", "Editar descripción");
            btnBajaLogica.Text           = Tr("btnBajaLogica",        "Dar de baja");
            btnReactivar.Text            = Tr("btnReactivar",         "Reactivar");
            if (dgvItems.Columns.Count >= 4)
            {
                dgvItems.Columns[0].HeaderText = Tr("colId",          "Id");
                dgvItems.Columns[1].HeaderText = Tr("colNombre",      "Nombre");
                dgvItems.Columns[2].HeaderText = Tr("colDescripcion", "Descripción");
                dgvItems.Columns[3].HeaderText = Tr("colActivo",      "Activo");
            }
        }

        private void Recargar()
        {
            try
            {
                dgvItems.AutoGenerateColumns = false;
                dgvItems.DataSource = null;
                dgvItems.DataSource = _templateService.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ChecklistItemTemplate ItemSeleccionado()
        {
            if (dgvItems.CurrentRow == null) return null;
            return dgvItems.CurrentRow.DataBoundItem as ChecklistItemTemplate;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            string nombre = PromptInput(Tr("promptNombre", "Nombre del item (obligatorio):"));
            if (string.IsNullOrWhiteSpace(nombre)) return;
            string descripcion = PromptInput(Tr("promptDescripcion", "Descripción (opcional):"));
            try
            {
                _templateService.AltaItem(nombre, descripcion);
                Recargar();
            }
            catch (Exception ex) { MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnEditarDescripcion_Click(object sender, EventArgs e)
        {
            var t = ItemSeleccionado();
            if (t == null) { MessageBox.Show(Tr("msgSeleccionar", "Seleccioná un item."), Tr("msgInformacion", "Info")); return; }

            string descripcion = PromptInput(Tr("promptEditarDesc", "Nueva descripción (dejar vacía para borrar):"), t.Descripcion ?? "");
            try
            {
                _templateService.EditarDescripcion(t.Id, descripcion);
                Recargar();
            }
            catch (Exception ex) { MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnBajaLogica_Click(object sender, EventArgs e)
        {
            var t = ItemSeleccionado();
            if (t == null || !t.Activo) return;
            if (MessageBox.Show(string.Format(Tr("msgConfirmarBaja", "¿Dar de baja lógica al item '{0}'?"), t.Nombre),
                Tr("msgConfirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                _templateService.BajaLogica(t.Id);
                Recargar();
            }
            catch (Exception ex) { MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnReactivar_Click(object sender, EventArgs e)
        {
            var t = ItemSeleccionado();
            if (t == null || t.Activo) return;
            try
            {
                _templateService.Reactivar(t.Id);
                Recargar();
            }
            catch (Exception ex) { MessageBox.Show(TrError(ex), Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private static string PromptInput(string label, string defaultValue = "")
        {
            using (var form = new Form())
            {
                form.Text = "";
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new System.Drawing.Size(400, 130);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;

                var lbl = new Label { Text = label, Left = 12, Top = 12, AutoSize = true };
                var txt = new TextBox { Left = 12, Top = 40, Width = 375, Text = defaultValue };
                var btnOk = new Button { Text = "OK", Left = 220, Top = 80, DialogResult = DialogResult.OK, Width = 80 };
                var btnCancel = new Button { Text = "Cancel", Left = 305, Top = 80, DialogResult = DialogResult.Cancel, Width = 80 };
                form.Controls.AddRange(new System.Windows.Forms.Control[] { lbl, txt, btnOk, btnCancel });
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_suscrito) { try { _idiomaService.Desuscribir(this); } catch { } _suscrito = false; }
            base.OnFormClosed(e);
        }
    }
}
