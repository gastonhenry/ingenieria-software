namespace BE
{
    public class Usuario
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string salt;
        public string Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        private string hash;
        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }
    }
}
