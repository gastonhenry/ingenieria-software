using BE.Enums;

namespace BE
{
    public class Modelo
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

        private Marca marca;
        public Marca Marca
        {
            get { return marca; }
            set { marca = value; }
        }

        private TipoCarroceria tipoCarroceria;
        public TipoCarroceria TipoCarroceria
        {
            get { return tipoCarroceria; }
            set { tipoCarroceria = value; }
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
