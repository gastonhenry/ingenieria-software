using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UI
{
    public class BotonPlano : Button
    {
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int w, int h);

        private int radio = 12;

        public int Radio
        {
            get { return radio; }
            set { radio = value; RefrescarRegion(); }
        }

        public BotonPlano()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RefrescarRegion();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RefrescarRegion();
        }

        private void RefrescarRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, radio, radio));
        }
    }
}
