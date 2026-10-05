using System;
using System.Collections.Generic;
using BE.Enums;

namespace BE
{
    public class Unidad : IDigitoVerificable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string dominio;
        public string Dominio
        {
            get { return dominio; }
            set { dominio = value; }
        }

        // El modelo trae su marca (Modelo.Marca).
        private Modelo modelo;
        public Modelo Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }

        private int anio;
        public int Anio
        {
            get { return anio; }
            set { anio = value; }
        }

        private int kilometraje;
        public int Kilometraje
        {
            get { return kilometraje; }
            set { kilometraje = value; }
        }

        private decimal precioCompra;
        public decimal PrecioCompra
        {
            get { return precioCompra; }
            set { precioCompra = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private EstadoUnidad estadoActual;
        public EstadoUnidad EstadoActual
        {
            get { return estadoActual; }
            set { estadoActual = value; }
        }

        // Persona que le vendió la unidad a la concesionaria.
        private Persona vendedor;
        public Persona Vendedor
        {
            get { return vendedor; }
            set { vendedor = value; }
        }

        // Usuario (Comprador) que registró la compra.
        private Usuario comprador;
        public Usuario Comprador
        {
            get { return comprador; }
            set { comprador = value; }
        }

        private DateTime fechaIngreso;
        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set { fechaIngreso = value; }
        }

        private string dvh;
        public string DVH
        {
            get { return dvh; }
            set { dvh = value; }
        }

        // Las colecciones siguientes se cargan sólo al consultar la trazabilidad completa
        // (UnidadService.ObtenerTrazabilidad); en los listados quedan vacías.

        // Null mientras la unidad no fue tomada para preparación.
        private ChecklistPreparacion checklist;
        public ChecklistPreparacion Checklist
        {
            get { return checklist; }
            set { checklist = value; }
        }

        private List<HistorialEstadoUnidad> historial = new List<HistorialEstadoUnidad>();
        public List<HistorialEstadoUnidad> Historial
        {
            get { return historial; }
            set { historial = value; }
        }

        // Null mientras no se inició la publicación.
        private PublicacionUnidad publicacion;
        public PublicacionUnidad Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; }
        }

        private List<ImagenUnidad> imagenes = new List<ImagenUnidad>();
        public List<ImagenUnidad> Imagenes
        {
            get { return imagenes; }
            set { imagenes = value; }
        }

        // Reservas y ventas (activas y canceladas).
        private List<VentaUnidad> ventas = new List<VentaUnidad>();
        public List<VentaUnidad> Ventas
        {
            get { return ventas; }
            set { ventas = value; }
        }

        public override string ToString()
        {
            string marca = modelo?.Marca?.Nombre ?? "";
            string nombreModelo = modelo?.Nombre ?? "";
            return dominio + " — " + marca + " " + nombreModelo + " (" + anio + ")";
        }
    }
}
