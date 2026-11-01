using System.ComponentModel;

namespace BE.Enums
{
    // Estado de la búsqueda de un cliente potencial (N03).
    public enum EstadoBusqueda
    {
        [Description("Activa")]
        Activa = 1,

        [Description("Cerrada")]
        Cerrada = 2,

        [Description("Vencida")]
        Vencida = 3,
    }
}
