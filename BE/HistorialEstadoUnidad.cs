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

        private int idUnidad;
        public int IdUnidad
        {
            get { return idUnidad; }
            set { idUnidad = value; }
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

        private int idUsuario;
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        // Denormalizado desde el JOIN con Usuario, solo lectura.
        private string usernameUsuario;
        public string UsernameUsuario
        {
            get { return usernameUsuario; }
            set { usernameUsuario = value; }
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
