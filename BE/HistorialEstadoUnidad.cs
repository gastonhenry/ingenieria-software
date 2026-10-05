using System;
using BE.Enums;

namespace BE
{
    public class HistorialEstadoUnidad
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        // NULL en el primer registro (alta de la unidad).
        private EstadoUnidad? estadoOrigen;
        public EstadoUnidad? EstadoOrigen
        {
            get { return estadoOrigen; }
            set { estadoOrigen = value; }
        }

        private EstadoUnidad estadoDestino;
        public EstadoUnidad EstadoDestino
        {
            get { return estadoDestino; }
            set { estadoDestino = value; }
        }

        // Usuario que ejecutó la transición.
        private Usuario usuario;
        public Usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        private DateTime fechaHora;
        public DateTime FechaHora
        {
            get { return fechaHora; }
            set { fechaHora = value; }
        }

        private string motivo;
        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }
    }
}
