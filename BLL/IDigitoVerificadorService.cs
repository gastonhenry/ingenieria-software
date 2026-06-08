using BE;
using System;
using System.Collections.Generic;

namespace BLL
{
    public interface IDigitoVerificadorService
    {
        ResultadoIntegridad Verificar<T>(string nombreTabla, IEnumerable<T> filas,
            Func<T, IList<KeyValuePair<string, string>>> obtenerCampos)
            where T : IDigitoVerificable;

        void Recalcular<T>(string nombreTabla, IEnumerable<T> filas,
            Func<T, IList<KeyValuePair<string, string>>> obtenerCampos,
            Action<int, string> persistirDVH)
            where T : IDigitoVerificable;

        string CalcularDVH(IList<KeyValuePair<string, string>> campos);
    }
}
