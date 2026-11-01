using System;
using System.Collections.Generic;

namespace BE
{
    // Resultado del tablero de captación (N03 — CU-CAP-02). Lo arma el BLL; la UI solo lo muestra.
    public class TableroCaptacion
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }

        public List<FilaUnidadTablero> Unidades { get; set; } = new List<FilaUnidadTablero>();
        public List<InteresadoTablero> Interesados { get; set; } = new List<InteresadoTablero>();
        // Clientes con búsqueda activa que hoy no coincide con ninguna unidad en venta ("esperando stock").
        public List<InteresadoTablero> InteresadosSinUnidades { get; set; } = new List<InteresadoTablero>();
        public List<NotificacionEnviada> Avisos { get; set; } = new List<NotificacionEnviada>();
        public List<ConversionTablero> Conversiones { get; set; } = new List<ConversionTablero>();
        // Todos los avisos (sin filtro de período) de las unidades que hoy están en venta:
        // permite mostrar si cada interesado ya fue avisado de la unidad elegida.
        public List<NotificacionEnviada> AvisosUnidadesEnVenta { get; set; } = new List<NotificacionEnviada>();

        public int VehiculosEnVenta => Unidades.Count;
        public int PosiblesCompradores => Interesados.Count;
        public int UnidadesSinInteresados => Unidades.FindAll(u => u.CantidadInteresados == 0).Count;
        public int AvisosEnviados => Avisos.FindAll(a => a.Exitoso).Count;
        public int ClientesAvisados { get; set; }
        public int ClientesConvertidos => Conversiones.Count;
        public decimal PorcentajeConversion =>
            ClientesAvisados == 0 ? 0 : Math.Round(ClientesConvertidos * 100m / ClientesAvisados, 1);
    }

    public class FilaUnidadTablero
    {
        public UnidadEnVenta Unidad { get; set; }
        public int CantidadInteresados { get; set; }
        public int DiasPublicada { get; set; }
        public bool Alerta { get; set; }
    }

    public class InteresadoTablero
    {
        public Persona Cliente { get; set; }
        public List<BusquedaCliente> Busquedas { get; set; } = new List<BusquedaCliente>();
        public List<UnidadEnVenta> Coincidencias { get; set; } = new List<UnidadEnVenta>();
        public bool AvisosSuspendidos { get; set; }
        public DateTime FechaUltimaEntrevista { get; set; }
    }

    public class ConversionTablero
    {
        public int IdPersona { get; set; }
        public string ClienteNombre { get; set; }
        public string UnidadComprada { get; set; }
        public DateTime FechaAviso { get; set; }   // último aviso antes de la compra
        public DateTime FechaVenta { get; set; }
        public decimal Precio { get; set; }
    }
}
