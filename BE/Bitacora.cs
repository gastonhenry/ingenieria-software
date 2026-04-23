using System;

namespace BE
{
    public class Bitacora
    {
        public Bitacora(Usuario usuario, BitacoraEnum tipo)
        {
            this.usuario = usuario;
            this.tipo = tipo;
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

        private BitacoraEnum tipo;
        public BitacoraEnum Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private DateTime fechaHora;
        public DateTime FechaHora
        {
            get { return fechaHora; }
            set { fechaHora = value; }
        }

        private DateTime? fechaHoraFin;
        public DateTime? FechaHoraFin
        {
            get { return fechaHoraFin; }
            set { fechaHoraFin = value; }
        }
    }
}