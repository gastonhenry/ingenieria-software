using System;

namespace BE
{
    public class Bitacora
    {
        public Bitacora(Usuario usuario, TipoBitacora tipo, string Detalle = null)
        {
            this.usuario = usuario;
            this.tipo = tipo;
            this.Detalle = Detalle;
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

        private TipoBitacora tipo;
        public TipoBitacora Tipo
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

        private string  detalle;
        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }
    }
}