using System.ComponentModel;

namespace BE.Enums
{
    // Tipo de operación comercial registrada sobre una unidad.
    public enum TipoOperacionVenta
    {
        [Description("Venta")]
        Venta = 1,

        [Description("Reserva")]
        Reserva = 2,
    }
}
