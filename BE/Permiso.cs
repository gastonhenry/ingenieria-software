namespace BE
{
    public abstract class Permiso : IDigitoVerificable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private string dvh;
        public string DVH
        {
            get { return dvh; }
            set { dvh = value; }
        }
    }
}
