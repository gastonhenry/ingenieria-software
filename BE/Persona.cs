using System;
using BE.Enums;

namespace BE
{
    public class Persona : IDigitoVerificable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        // 'F' Física / 'J' Jurídica
        private char tipoPersona;
        public char TipoPersona
        {
            get { return tipoPersona; }
            set { tipoPersona = value; }
        }

        private string documento;
        public string Documento
        {
            get { return documento; }
            set { documento = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string domicilio;
        public string Domicilio
        {
            get { return domicilio; }
            set { domicilio = value; }
        }

        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private EstadoCivil estadoCivil;
        public EstadoCivil EstadoCivil
        {
            get { return estadoCivil; }
            set { estadoCivil = value; }
        }

        private DateTime fechaNacimiento;
        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        private string dvh;
        public string DVH
        {
            get { return dvh; }
            set { dvh = value; }
        }

        public override string ToString()
        {
            return nombre + " (" + documento + ")";
        }
    }
}
