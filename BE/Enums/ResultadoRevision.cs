using System.ComponentModel;

namespace BE.Enums
{
    // Resultado de la revisión de un item del checklist por parte del Encargado.
    // Un item arranca en Pendiente; al revisarlo pasa a OK (comentario opcional)
    // u Observado (comentario obligatorio).
    public enum ResultadoRevision
    {
        [Description("Pendiente")]
        Pendiente = 1,

        [Description("OK")]
        OK = 2,

        [Description("Observado")]
        Observado = 3,
    }
}
