using System;
using System.ComponentModel;
using System.Reflection;

namespace HELPERS
{
    public static class EnumExtensions
    {
        public static string GetDescripcion<TEnum>(this TEnum valor) where TEnum : Enum
        {
            FieldInfo field = valor.GetType().GetField(valor.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr != null ? attr.Description : valor.ToString();
        }
    }
}