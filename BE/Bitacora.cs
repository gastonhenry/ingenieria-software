using System;

namespace BE
{
    public class Bitacora
    {
        public Bitacora(Usuario usuario)
        {
            this.usuario = usuario;
        }

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Usuario usuario;
        public Usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        private DateTime fechaHoraInicio;
        public DateTime FechaHoraInicio
        {
            get { return fechaHoraInicio; }
            set { fechaHoraInicio = value; }
        }

        private DateTime? fechaHoraFin;
        public DateTime? FechaHoraFin
        {
            get { return fechaHoraFin; }
            set { fechaHoraFin = value; }
        }
    }
}