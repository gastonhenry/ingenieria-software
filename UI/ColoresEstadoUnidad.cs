using System.Collections.Generic;
using System.Drawing;
using BE.Enums;

namespace UI
{
    // Paleta única para los estados de Unidad. Usá siempre este helper
    // en vez de hardcodear colores — así todos los lugares (grillas, labels, badges)
    // muestran los mismos tonos.
    public static class ColoresEstadoUnidad
    {
        private static readonly Dictionary<EstadoUnidad, Color> _mapa = new Dictionary<EstadoUnidad, Color>
        {
            { EstadoUnidad.Ingresado,             Color.FromArgb( 30,  90, 200) },  // azul
            { EstadoUnidad.RequiereAprobacionPresupuesto, Color.FromArgb(220, 140,  10) },  // ámbar
            { EstadoUnidad.EnPreparacion,         Color.FromArgb(140,  60, 180) },  // violeta
            { EstadoUnidad.PendienteVenta,        Color.FromArgb(200,  90,  40) },  // terracota
            { EstadoUnidad.EnVenta,               Color.FromArgb( 30, 150,  70) },  // verde
            { EstadoUnidad.Vendido,               Color.FromArgb( 90,  90,  90) },  // gris oscuro — operación cerrada
            { EstadoUnidad.Reservado,             Color.FromArgb( 20, 100, 160) },  // azul petróleo
            { EstadoUnidad.Pausado,               Color.FromArgb(130, 130, 130) },  // gris claro — fuera de circuito activo
        };

        public static Color Obtener(EstadoUnidad estado)
        {
            return _mapa.TryGetValue(estado, out var c) ? c : Color.Black;
        }
    }
}
