using BE.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    // Modal que explica la máquina de estados de Unidad. Reutiliza ColoresEstadoUnidad
    // para pintar cada estado con el mismo color que la grilla.
    public partial class FormAyudaFlujoEstados : Form
    {
        private static readonly (EstadoUnidad Estado, string Descripcion)[] Secuencia = new[]
        {
            (EstadoUnidad.Ingresado,
             "La unidad fue recién comprada e ingresada a la concesionaria. Espera que el Encargado de Taller la tome para preparación."),
            (EstadoUnidad.EnPreparacion,
             "El Encargado está ejecutando el checklist de preparación. Si detecta algo fuera del template, agrega items extras con costo estimado y manda el presupuesto al Gerente. Si no, finaliza y pasa a Pendiente de venta."),
            (EstadoUnidad.RequiereAprobacionPresupuesto,
             "El Encargado envió uno o más items extras con costo estimado. El Gerente debe aprobarlos o rechazarlos (con un motivo). En ambos casos la unidad vuelve a En preparación."),
            (EstadoUnidad.PendienteVenta,
             "El Encargado finalizó la preparación. El Gerente debe verificar que los trabajos quedaron OK y aprobar la publicación, o devolverla a En preparación con observaciones."),
            (EstadoUnidad.EnVenta,
             "El Gerente aprobó la publicación. La unidad quedó publicada y está disponible para la venta. Desde acá se puede Vender, Reservar o Pausar."),
            (EstadoUnidad.Reservado,
             "Un comprador reservó la unidad con una seña. Mientras está Reservada no está publicada. Desde acá se puede Concretar la venta o Cancelar la reserva (vuelve a En venta)."),
            (EstadoUnidad.Pausado,
             "La unidad fue retirada temporalmente del listado de venta por decisión del Gerente (motivo obligatorio). Desde acá se puede Reanudar y volver a En venta."),
            (EstadoUnidad.Vendido,
             "La unidad fue vendida. Es un estado terminal: no admite más transiciones.")
        };

        public FormAyudaFlujoEstados()
        {
            InitializeComponent();
            ArmarContenido();
        }

        private void ArmarContenido()
        {
            int y = 20;
            foreach (var (estado, desc) in Secuencia)
            {
                var lblEstado = new Label
                {
                    AutoSize = true,
                    Font = new Font(this.Font.FontFamily, 12F, FontStyle.Bold),
                    ForeColor = ColoresEstadoUnidad.Obtener(estado),
                    Location = new Point(20, y),
                    Text = "► " + NombreEstado(estado)
                };
                pnlContenido.Controls.Add(lblEstado);
                y += lblEstado.PreferredHeight + 4;

                var lblDesc = new Label
                {
                    AutoSize = false,
                    Location = new Point(40, y),
                    Size = new Size(pnlContenido.Width - 60, 50),
                    Text = desc
                };
                pnlContenido.Controls.Add(lblDesc);
                y += 60;
            }
        }

        private static string NombreEstado(EstadoUnidad e)
        {
            var field = typeof(EstadoUnidad).GetField(e.ToString());
            var attrs = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attrs != null && attrs.Length > 0)
                ? ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description
                : e.ToString();
        }

        private void btnCerrar_Click(object sender, EventArgs e) => this.Close();
    }
}
