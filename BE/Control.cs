namespace BE
{
    public class Control
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

        private string form;
        public string Form
        {
            get { return form; }
            set { form = value; }
        }

        public override string ToString()
        {
            return form + "." + codigo;
        }
    }
}
