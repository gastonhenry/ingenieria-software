using BE;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class DigitoVerificadorService : IDigitoVerificadorService
    {
        private const int ChecksumModulo = 1000003;

        private readonly MapperDigitoVerificador _mapperDigitoVerificador;

        public DigitoVerificadorService()
        {
            _mapperDigitoVerificador = new MapperDigitoVerificador();
        }

        public ResultadoIntegridad Verificar<T>(string nombreTabla, IEnumerable<T> filas,
            Func<T, IList<KeyValuePair<string, string>>> obtenerCampos)
            where T : IDigitoVerificable
        {
            var resultado = new ResultadoIntegridad { Ok = true };
            var lista = filas.ToList();
            var dvvAlmacenado = _mapperDigitoVerificador.Obtener(nombreTabla);

            bool sinInicializar = dvvAlmacenado == null && lista.All(f => string.IsNullOrEmpty(f.DVH));
            if (sinInicializar)
            {
                resultado.RequiereInicializacion = true;
                return resultado;
            }

            foreach (var f in lista)
            {
                string dvhCalculado = CalcularDVH(obtenerCampos(f));
                if (dvhCalculado != f.DVH)
                {
                    resultado.Ok = false;
                    resultado.FilasInvalidas.Add(f.Id);
                }
            }

            string dvvCalculado = CalcularDVV(lista.Select(f => f.DVH ?? string.Empty));
            if (dvvAlmacenado == null || dvvCalculado != dvvAlmacenado.DVV)
            {
                resultado.Ok = false;
                resultado.DVVInvalido = true;
            }

            return resultado;
        }

        public void Recalcular<T>(string nombreTabla, IEnumerable<T> filas,
            Func<T, IList<KeyValuePair<string, string>>> obtenerCampos,
            Action<int, string> persistirDVH)
            where T : IDigitoVerificable
        {
            var dvhsNuevos = new List<string>();
            foreach (var f in filas)
            {
                string dvh = CalcularDVH(obtenerCampos(f));
                persistirDVH(f.Id, dvh);
                dvhsNuevos.Add(dvh);
            }

            string dvv = CalcularDVV(dvhsNuevos);
            _mapperDigitoVerificador.Guardar(nombreTabla, dvv);
        }

        public string CalcularDVH(IList<KeyValuePair<string, string>> campos)
        {
            string canonico = Canonicalizar(campos);
            int checksumPosicional = ChecksumPonderadoPorPosicionDeCaracter(canonico);
            return PasswordHasher.Hash(canonico + "::cs=" + checksumPosicional);
        }

        private string CalcularDVV(IEnumerable<string> dvhs)
        {
            var lista = dvhs.ToList();
            var sb = new StringBuilder();
            for (int i = 0; i < lista.Count; i++)
            {
                if (i > 0) sb.Append('|');
                sb.Append(i + 1);
                sb.Append(':');
                sb.Append(lista[i] ?? string.Empty);
            }
            string canonico = sb.ToString();
            int checksumPosicional = ChecksumPonderadoPorPosicionDeCaracter(canonico);
            return PasswordHasher.Hash(canonico + "::cs=" + checksumPosicional);
        }

        private static string Canonicalizar(IList<KeyValuePair<string, string>> campos)
        {
            if (campos == null || campos.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            for (int i = 0; i < campos.Count; i++)
            {
                if (i > 0) sb.Append('|');
                sb.Append(i + 1);
                sb.Append(':');
                sb.Append(campos[i].Key ?? string.Empty);
                sb.Append('=');
                sb.Append(campos[i].Value ?? string.Empty);
            }
            return sb.ToString();
        }

        private static int ChecksumPonderadoPorPosicionDeCaracter(string entrada)
        {
            if (string.IsNullOrEmpty(entrada)) return 0;
            long acumulador = 0;
            for (int i = 0; i < entrada.Length; i++)
                acumulador = (acumulador + (long)entrada[i] * (i + 1)) % ChecksumModulo;
            return (int)acumulador;
        }
    }
}
