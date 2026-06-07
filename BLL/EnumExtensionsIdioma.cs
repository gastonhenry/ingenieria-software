using HELPERS;
using System;

namespace BLL
{
    public static class EnumExtensionsIdioma
    {
        public static string GetDescripcionTraducida<TEnum>(this TEnum valor) where TEnum : Enum
        {
            var gestor = GestorIdiomas.GetInstancia();
            string traducido = null;
            try { traducido = gestor?.Traducir(typeof(TEnum).Name, valor.ToString()); }
            catch { }
            return string.IsNullOrEmpty(traducido) ? valor.GetDescripcion() : traducido;
        }
    }
}
