using System;

namespace BLL
{
    public class BLLException : Exception
    {
        public string Codigo { get; }
        public object[] Args { get; }

        public BLLException(string codigo, string mensajeDefault, params object[] args)
            : base(FormatearMensaje(mensajeDefault, args))
        {
            Codigo = codigo;
            Args = args ?? new object[0];
        }

        private static string FormatearMensaje(string plantilla, object[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(plantilla))
                return plantilla;
            try { return string.Format(plantilla, args); }
            catch { return plantilla; }
        }
    }
}
