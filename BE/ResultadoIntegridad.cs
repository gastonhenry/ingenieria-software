using System.Collections.Generic;

namespace BE
{
    public class ResultadoIntegridad
    {
        public ResultadoIntegridad()
        {
            FilasInvalidas = new List<int>();
        }

        public bool Ok { get; set; }
        public bool DVVInvalido { get; set; }
        public bool RequiereInicializacion { get; set; }
        public List<int> FilasInvalidas { get; set; }
    }
}