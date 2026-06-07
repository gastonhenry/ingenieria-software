using System.ComponentModel;

namespace BE.Enums
{
    public enum AccionUsuarioHistorial
    {
        [Description("Alta")]
        Alta = 0,

        [Description("Edición")]
        Edicion = 1,

        [Description("Bloqueo")]
        Bloqueo = 2,

        [Description("Desbloqueo")]
        Desbloqueo = 3,

        [Description("Restauración")]
        Restauracion = 4,
    }
}
