using System.Collections.Generic;

namespace BE
{
    public class ResultadoIntegridadGlobal
    {
        public ResultadoIntegridadGlobal()
        {
            PorTabla = new Dictionary<string, ResultadoIntegridad>();
        }

        public bool Ok { get; set; }
        public Dictionary<string, ResultadoIntegridad> PorTabla { get; set; }
    }
}
