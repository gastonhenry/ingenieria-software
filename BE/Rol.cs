using System.Collections.Generic;

namespace BE
{
    public class Rol : IDigitoVerificable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private List<Permiso> permisos = new List<Permiso>();
        public List<Permiso> Permisos
        {
            get { return permisos; }
            set { permisos = value; }
        }

        private string dvh;
        public string DVH
        {
            get { return dvh; }
            set { dvh = value; }
        }

        public override string ToString()
        {
            string desc = string.IsNullOrWhiteSpace(descripcion) ? "" : " — " + descripcion;
            return "[Rol] " + nombre + desc;
        }
    }
}
