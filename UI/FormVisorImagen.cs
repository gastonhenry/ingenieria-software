using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    // Visor de fotos a pantalla completa. Recibe las imágenes ya cargadas (no las libera:
    // las sigue administrando el form que lo abre) y el texto de ayuda ya traducido.
    public partial class FormVisorImagen : Form
    {
        private readonly IList<Image> _imagenes;
        private int _indice;

        public FormVisorImagen(IList<Image> imagenes, int indiceInicial, string textoAyuda)
        {
            InitializeComponent();
            _imagenes = imagenes ?? new List<Image>();
            _indice = Math.Max(0, Math.Min(indiceInicial, _imagenes.Count - 1));

            if (!string.IsNullOrEmpty(textoAyuda)) lblAyuda.Text = textoAyuda;

            bool variasFotos = _imagenes.Count > 1;
            btnAnterior.Visible = variasFotos;
            btnSiguiente.Visible = variasFotos;

            // Cubre toda la pantalla del monitor donde está la aplicación (incluida la barra de tareas).
            this.Load += (s, e) =>
            {
                this.Bounds = Screen.FromControl(Owner ?? this).Bounds;
                Mostrar();
            };
        }

        private void Mostrar()
        {
            if (_imagenes.Count == 0) { picImagen.Image = null; lblContador.Text = ""; return; }
            picImagen.Image = _imagenes[_indice];
            lblContador.Text = $"{_indice + 1} / {_imagenes.Count}";
        }

        private void Mover(int delta)
        {
            if (_imagenes.Count < 2) return;
            _indice = (_indice + delta + _imagenes.Count) % _imagenes.Count; // circular
            Mostrar();
        }

        private void FormVisorImagen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: Close(); break;
                case Keys.Left:   Mover(-1); break;
                case Keys.Right:  Mover(+1); break;
                default: return;
            }
            e.Handled = true;
        }

        private void btnAnterior_Click(object sender, EventArgs e) => Mover(-1);
        private void btnSiguiente_Click(object sender, EventArgs e) => Mover(+1);
        private void btnCerrar_Click(object sender, EventArgs e) => Close();

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            picImagen.Image = null; // las imágenes son del form padre; no se liberan acá
            base.OnFormClosed(e);
        }
    }
}
