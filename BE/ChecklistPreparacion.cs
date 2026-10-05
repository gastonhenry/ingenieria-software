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

        private DateTime fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        // Encargado de Taller que tomó la unidad para preparación.
        private Usuario creador;
        public Usuario Creador
        {
            get { return creador; }
            set { creador = value; }
        }

        private List<ChecklistItem> items = new List<ChecklistItem>();
        public List<ChecklistItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
