using System.ComponentModel;

namespace BE.Enums
{
    // Estado de aprobación de un item del checklist (sólo relevante para items extras).
    // Los items que vienen del template se consideran siempre Aprobado.
    public enum EstadoAprobacionItem
    {
        [Description("Aprobado")]
        Aprobado = 1,

        [Description("Propuesto")]
        Propuesto = 2,

        [Description("Rechazado")]
        Rechazado = 3,
    }
}
