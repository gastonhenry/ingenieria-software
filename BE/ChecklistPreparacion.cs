using System;
using System.Collections.Generic;

namespace BE
{
    public class ChecklistPreparacion
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

        private DateTime fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        private int idCreadorUsuario;
        public int IdCreadorUsuario
        {
            get { return idCreadorUsuario; }
            set { idCreadorUsuario = value; }
        }

        private List<ChecklistItem> items = new List<ChecklistItem>();
        public List<ChecklistItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
