using System.ComponentModel;
using System.Reflection;

namespace BE
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
    }
}