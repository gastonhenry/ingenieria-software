using BE.Enums;
using System;
using System.Collections.Generic;

namespace BE
{
    // Búsqueda registrada en la entrevista a un cliente potencial (N03 — CU-CAP-01).
    // Marcas, modelos y carrocerías son multiselección; lista vacía = "cualquiera".
    // La regla "al menos una marca o una carrocería" la valida el BLL.
    public class BusquedaCliente
    {
        public int Id { get; set; }
        public Persona Cliente { get; set; }
        public Usuario Entrevistador { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public List<Marca> Marcas { get; set; } = new List<Marca>();
        // Cada modelo trae su Marca: restringe esa marca a los modelos elegidos.
        public List<Modelo> Modelos { get; set; } = new List<Modelo>();
        public List<TipoCarroceria> Carrocerias { get; set; } = new List<TipoCarroceria>();
        public int? AnioDesde { get; set; }
        public int? AnioHasta { get; set; }
        public decimal? PrecioMaximo { get; set; }
        public string Observaciones { get; set; }

        public bool AceptaAvisos { get; set; }
        public EstadoBusqueda Estado { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string MotivoCierre { get; set; }

        // Se completa al listar: true si el cliente tiene los avisos suspendidos.
        public bool AvisosSuspendidos { get; set; }
    }
}
