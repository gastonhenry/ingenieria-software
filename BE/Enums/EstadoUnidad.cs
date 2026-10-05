using System.ComponentModel;

namespace BE.Enums
{
    public enum EstadoUnidad
    {
        [Description("Ingresado")]
        Ingresado = 1,

        [Description("Requiere aprobación de presupuesto")]
        RequiereAprobacionPresupuesto = 2,

        [Description("En preparación")]
        EnPreparacion = 3,

        [Description("Pendiente de venta")]
        PendienteVenta = 4,

        [Description("En venta")]
        EnVenta = 5,

        [Description("Vendido")]
        Vendido = 6,

        [Description("Reservado")]
        Reservado = 7,

        [Description("Pausado")]
        Pausado = 8,
    }
}
