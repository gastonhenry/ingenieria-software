using System;
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

        private string marca;
        public string Marca
        {
            get { return marca; }
            set { marca = value; }
        }

        private string modelo;
        public string Modelo
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

        private int idPersona;
        public int IdPersona
        {
            get { return idPersona; }
            set { idPersona = value; }
        }

        private int idCompradorUsuario;
        public int IdCompradorUsuario
        {
            get { return idCompradorUsuario; }
            set { idCompradorUsuario = value; }
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

        public override string ToString()
        {
            return dominio + " — " + marca + " " + modelo + " (" + anio + ")";
        }
    }
}
