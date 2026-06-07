using System.Collections.Generic;

namespace BE
{
    public class Rol : Permiso
    {
        private List<Permiso> hijos = new List<Permiso>();
        public List<Permiso> Hijos
        {
            get { return hijos; }
            set { hijos = value; }
        }

        public override string ToString()
        {
            string desc = string.IsNullOrWhiteSpace(Descripcion) ? "" : " — " + Descripcion;
            return "[Rol] " + Codigo + desc;
        }
    }
}
    