using System.ComponentModel;
using System.Reflection;

namespace BE
{
    public enum BitacoraEnum
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
    }

    public static class BitacoraEnumExtensions
    {
        public static string GetDescripcion(this BitacoraEnum valor)
        {
            FieldInfo field = valor.GetType().GetField(valor.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr != null ? attr.Description : valor.ToString();
        }
    }
}