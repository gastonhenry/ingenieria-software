using System.ComponentModel;

namespace BE.Enums
{
    public enum TipoBitacora
    {
        [Description("Inicio de sesión")]
        Login = 0,

        [Description("Cierre de sesión")]
        Logout = 1,

        [Description("Registro de usuario")]
        RegistroUsuario = 2,

        [Description("Desbloqueo de usuario")]
        DesbloqueoUsuario = 3,

        [Description("Bloqueo de usuario")]
        BloqueoUsuario = 4,

        [Description("Error")]
        Error = 5,

        [Description("Inicio de sesión fallido")]
        LoginFallido = 6,

        [Description("Intento acceso bloqueado")]
        IntentoAccesoBloqueado = 7,

        [Description("Alta de permiso")]
        AltaPermiso = 8,

        [Description("Asignación de permiso")]
        AsignacionPermiso = 9,

        [Description("Alta de rol")]
        AltaRol = 10,

        [Description("Asignación de rol")]
        AsignacionRol = 11,

        [Description("Alta de idioma")]
        AltaIdioma = 12,

        [Description("Edición de usuario")]
        EdicionUsuario = 13,

        [Description("Mantenimiento")]
        Mantenimiento = 14,
    }
}