using BE;
using BLL;
using System;
using System.IO;
using System.Windows.Forms;

namespace UI
{
    public partial class FormMantenimiento : Form
    {
        private const string CODIGO_FORM = "FormMantenimiento";

        private readonly IMantenimientoService _mantenimientoService;
        private readonly IIdiomaService _idiomaService;
        private ResultadoIntegridadGlobal _resultadoActual;

        public bool RestauracionEjecutada { get; private set; }

        public FormMantenimiento(ResultadoIntegridadGlobal resultadoInicial)
        {
            InitializeComponent();
            _mantenimientoService = new MantenimientoService();
            try { _idiomaService = new IdiomaService(); } catch { _idiomaService = null; }
            _resultadoActual = resultadoInicial;

            AplicarTextos();
            RefrescarVista();
            RefrescarListadoBackups();
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

        private void AplicarTextos()
        {
            this.Text                  = Tr("title",                "Mantenimiento");
            lblBackupSeleccionado.Text = Tr("lblBackupSeleccionado","Backup a restaurar:");
            btnBackup.Text             = Tr("btnBackup",            "Backup ahora");
            btnRestaurar.Text          = Tr("btnRestaurar",         "Restaurar selección");
            btnRecalcular.Text         = Tr("btnRecalcular",        "Recalcular DVs");
            btnReverificar.Text        = Tr("btnReverificar",       "Reverificar");
            btnSalir.Text              = Tr("btnSalir",             "Salir");
            lblUsuarioMant.Text        = Tr("lblUsuarioMant",       "Sesión: mantenimiento");
        }

        private void RefrescarVista()
        {
            if (_resultadoActual == null || _resultadoActual.Ok)
            {
                lblTitulo.Text = Tr("lblTituloOk", "Modo Mantenimiento — Integridad OK");
                lblTitulo.ForeColor = System.Drawing.Color.FromArgb(40, 140, 40);
                lblEstado.Text = Tr("lblEstadoOk",
                    "La base de datos pasó la verificación de integridad. Podés cerrar este form.");
                txtDetalle.Text = string.Empty;
                return;
            }

            lblTitulo.Text = Tr("lblTituloError", "Modo Mantenimiento — Error de integridad detectado");
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(180, 0, 0);
            lblEstado.Text = Tr("lblEstadoError", "Detalle por entidad:");

            string fmtTabla        = Tr("detalleTabla",      "== Tabla {0} ==");
            string lineaOk         = Tr("detalleOk",         "  OK");
            string fmtDvhInvalido  = Tr("detalleDvhInvalido","  DVH inválido en {0} fila(s). Ids: {1}");
            string lineaDvvInvalido= Tr("detalleDvvInvalido","  DVV inválido (la suma de DVHs no coincide con el almacenado).");

            var sb = new System.Text.StringBuilder();
            foreach (var par in _resultadoActual.PorTabla)
            {
                sb.AppendLine(string.Format(fmtTabla, par.Key));
                if (par.Value.Ok)
                {
                    sb.AppendLine(lineaOk);
                }
                else
                {
                    if (par.Value.FilasInvalidas.Count > 0)
                        sb.AppendLine(string.Format(fmtDvhInvalido,
                            par.Value.FilasInvalidas.Count, string.Join(", ", par.Value.FilasInvalidas)));
                    if (par.Value.DVVInvalido)
                        sb.AppendLine(lineaDvvInvalido);
                }
                sb.AppendLine();
            }
            txtDetalle.Text = sb.ToString();
        }

        private void RefrescarListadoBackups()
        {
            cmbBackups.Items.Clear();
            foreach (var ruta in _mantenimientoService.ListarBackupsDisponibles())
                cmbBackups.Items.Add(ruta);
            if (cmbBackups.Items.Count > 0)
                cmbBackups.SelectedIndex = 0;
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string ruta = _mantenimientoService.Backup();
                MessageBox.Show(string.Format(Tr("msgBackupCreado", "Backup creado:\n{0}"), ruta),
                    Tr("tituloBackup", "Backup"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefrescarListadoBackups();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgBackupError", "No se pudo crear el backup.") + "\n\n" + ex.Message,
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            var seleccion = cmbBackups.SelectedItem as string;
            if (string.IsNullOrEmpty(seleccion) || !File.Exists(seleccion))
            {
                MessageBox.Show(Tr("msgRestoreSinSeleccion", "Seleccioná un archivo de backup válido."),
                    Tr("tituloRestaurar", "Restaurar"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                string.Format(Tr("msgRestoreConfirmar",
                    "¿Confirmás restaurar la base de datos desde:\n{0}\n\nSe sobrescribirá el estado actual."), seleccion),
                Tr("msgRestoreConfirmarTitulo", "Confirmar restauración"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _mantenimientoService.Restore(seleccion);
                RestauracionEjecutada = true;
                MessageBox.Show(Tr("msgRestoreOk", "Restore completado."),
                    Tr("tituloRestaurar", "Restaurar"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reverificar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgRestoreError", "No se pudo restaurar el backup.") + "\n\n" + ex.Message,
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                Tr("msgRecalcularConfirmar",
                    "Recalcular los DVs sobrescribe los valores almacenados con lo calculado a partir del estado actual de la base.\n\n" +
                    "Sólo hacelo si confiás en que los datos actuales son correctos (por ejemplo, después de un Restore o de una intervención manual autorizada).\n\n" +
                    "¿Continuar?"),
                Tr("msgRecalcularConfirmarTitulo", "Confirmar recálculo"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _mantenimientoService.RecalcularTodo();
                MessageBox.Show(Tr("msgRecalcularOk", "DVs recalculados correctamente."),
                    Tr("tituloRecalcular", "Recalcular"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reverificar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgRecalcularError", "No se pudieron recalcular los DVs.") + "\n\n" + ex.Message,
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReverificar_Click(object sender, EventArgs e)
        {
            Reverificar();
        }

        private void Reverificar()
        {
            try
            {
                _resultadoActual = _mantenimientoService.VerificarTodo();
                RefrescarVista();
                if (_resultadoActual.Ok)
                    MessageBox.Show(Tr("msgIntegridadOk",
                            "La base de datos quedó íntegra. Podés salir y volver al login."),
                        Tr("tituloVerificacion", "Verificación"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Tr("msgReverificarError", "Error al reverificar la integridad.") + "\n\n" + ex.Message,
                    Tr("msgError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
