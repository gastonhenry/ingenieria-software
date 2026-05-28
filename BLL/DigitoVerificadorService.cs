using BE;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class DigitoVerificadorService : IDigitoVerificadorService
    {
        private readonly MapperDigitoVerificador _mapperDigitoVerificador;

        public DigitoVerificadorService()
        {
            _mapperDigitoVerificador = new MapperDigitoVerificador();
        }

        public ResultadoIntegridad Verificar<T>(string nombreTabla, IEnumerable<T> filas, Func<T, string> obtenerCampos)
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

        public void Recalcular<T>(string nombreTabla, IEnumerable<T> filas, Func<T, string> obtenerCampos, Action<int, string> persistirDVH)
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

        private string CalcularDVH(string datos)
        {
            return PasswordHasher.Hash(datos ?? string.Empty);
        }

        private string CalcularDVV(IEnumerable<string> dvhs)
        {
            return PasswordHasher.Hash(string.Join("|", dvhs));
        }
    }
}
