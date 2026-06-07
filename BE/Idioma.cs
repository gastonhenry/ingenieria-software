namespace BE
{
    public class Idioma
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

        private bool completo;
        public bool Completo
        {
            get { return completo; }
            set { completo = value; }
        }

        public override string ToString()
        {
            return completo ? nombre : nombre + "  (en proceso)";
        }
    }
}
