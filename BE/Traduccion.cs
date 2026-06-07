namespace BE
{
    public class Traduccion
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int idIdioma;
        public int IdIdioma
        {
            get { return idIdioma; }
            set { idIdioma = value; }
        }

        private int idControl;
        public int IdControl
        {
            get { return idControl; }
            set { idControl = value; }
        }

        private string texto;
        public string Texto
        {
            get { return texto; }
            set { texto = value; }
        }

        private Control control;
        public Control Control
        {
            get { return control; }
            set { control = value; }
        }
    }
}
