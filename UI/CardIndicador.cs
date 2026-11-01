using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    // N03 — tarjeta de indicador del tablero de captación: título (hasta 2 líneas), número grande,
    // detalle y link "Ver ▸". Las posiciones se calculan según la fuente y la escala de pantalla
    // (no con píxeles fijos), así no se corta el texto con DPI alto.
    public class CardIndicador : UserControl
    {
        private const int ANCHO_BASE = 220;     // ancho a 96 DPI (5 tarjetas entran en una fila a 1920 px)
        private const int ACENTO_BASE = 5;

        private readonly Label lblTitulo;
        private readonly Label lblValor;
        private readonly Label lblDetalle;
        private readonly LinkLabel lnkVer;
        private Color _colorAcento = Color.FromArgb(30, 90, 200);

        public event EventHandler VerClick;

        public CardIndicador()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Margin = new Padding(0, 0, 12, 8);

            lblTitulo = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 90, 100),
                TextAlign = ContentAlignment.TopLeft
            };
            lblValor = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                TextAlign = ContentAlignment.MiddleLeft
            };
            lblDetalle = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(120, 120, 130),
                TextAlign = ContentAlignment.MiddleLeft
            };
            lnkVer = new LinkLabel
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                LinkColor = Color.FromArgb(30, 90, 200),
                ActiveLinkColor = Color.FromArgb(20, 65, 160),
                LinkBehavior = LinkBehavior.HoverUnderline,
                Text = "Ver ▸"
            };
            lnkVer.LinkClicked += (s, e) => VerClick?.Invoke(this, EventArgs.Empty);

            Controls.Add(lblTitulo);
            Controls.Add(lblValor);
            Controls.Add(lblDetalle);
            Controls.Add(lnkVer);
        }

        public string Titulo { get => lblTitulo.Text; set { lblTitulo.Text = value; PerformLayout(); } }
        public string Valor { get => lblValor.Text; set => lblValor.Text = value; }
        public string Detalle { get => lblDetalle.Text; set => lblDetalle.Text = value; }
        public string TextoVer { get => lnkVer.Text; set { lnkVer.Text = value; PerformLayout(); } }
        public bool MostrarVer { get => lnkVer.Visible; set { lnkVer.Visible = value; PerformLayout(); } }
        public Color ColorAcento { get => _colorAcento; set { _colorAcento = value; Invalidate(); } }

        private float Escala => DeviceDpi / 96f;
        private int S(int valor) => (int)Math.Round(valor * Escala);

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            if (lblTitulo == null) return;

            int ancho = S(ANCHO_BASE);
            int izq = S(ACENTO_BASE) + S(10);
            int der = S(10);
            int arriba = S(8);
            int util = ancho - izq - der;

            // Título: siempre 2 líneas de alto (así todas las tarjetas miden lo mismo).
            int altoTitulo = lblTitulo.Font.Height * 2;
            lblTitulo.SetBounds(izq, arriba, util, altoTitulo);

            int altoValor = (int)(lblValor.Font.Height * 1.15);
            lblValor.SetBounds(izq - S(2), arriba + altoTitulo, util + S(2), altoValor);

            // Fila inferior: detalle a la izquierda, "Ver ▸" a la derecha.
            int yFila = arriba + altoTitulo + altoValor + S(2);
            int altoFila = Math.Max(lblDetalle.Font.Height, lnkVer.Font.Height) + S(4);
            int anchoLink = lnkVer.Visible ? lnkVer.PreferredSize.Width : 0;
            lnkVer.Location = new Point(ancho - der - anchoLink, yFila + (altoFila - lnkVer.PreferredSize.Height) / 2);
            lblDetalle.SetBounds(izq, yFila, util - anchoLink - (anchoLink > 0 ? S(6) : 0), altoFila);

            var tamanio = new Size(ancho, yFila + altoFila + S(8));
            if (Size != tamanio) Size = tamanio;
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            PerformLayout();
        }

        // Barra de color a la izquierda.
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pincel = new SolidBrush(_colorAcento))
                e.Graphics.FillRectangle(pincel, 0, 0, S(ACENTO_BASE), ClientSize.Height);
        }
    }
}
