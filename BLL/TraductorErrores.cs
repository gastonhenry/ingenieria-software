using System;

namespace BLL
{
    public static class TraductorErrores
    {
        public const string CODIGO_FORM = "Errores";

        public static string TraducirError(Exception ex, IIdiomaService idiomaService)
        {
            if (ex == null) return string.Empty;

            BLLException bllEx = ex as BLLException;
            if (bllEx != null)
            {
                try
                {
                    string plantilla = idiomaService?.Traducir(CODIGO_FORM, bllEx.Codigo);
                    if (!string.IsNullOrEmpty(plantilla))
                    {
                        return bllEx.Args.Length == 0
                            ? plantilla
                            : string.Format(plantilla, bllEx.Args);
                    }
                }
                catch { }
                return bllEx.Message;
            }
            return ex.Message;
        }
    }
}
