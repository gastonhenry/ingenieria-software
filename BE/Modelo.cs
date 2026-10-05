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

        private int idMarca;
        public int IdMarca
        {
            get { return idMarca; }
            set { idMarca = value; }
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
