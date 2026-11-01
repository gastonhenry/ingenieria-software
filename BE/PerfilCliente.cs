using System;
using System.Xml.Serialization;

namespace BE
{
    // A02 — Serialización: perfil de una persona que se exporta / importa en XML.
    [XmlRoot("PerfilCliente")]
    public class PerfilCliente
    {
        public const string VERSION_ACTUAL = "1.0";

        [XmlAttribute("version")]
        public string Version { get; set; }

        public DateTime FechaExportacion { get; set; }
        public string ExportadoPor { get; set; }
        public PersonaPerfil Persona { get; set; }
    }

    public class PersonaPerfil
    {
        public string TipoPersona { get; set; }      // "F" / "J"
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string EstadoCivil { get; set; }      // nombre del enum EstadoCivil
        public DateTime FechaNacimiento { get; set; }
    }

    // Resultado de importar un perfil, para informarlo en pantalla.
    public class ResultadoImportacionPerfil
    {
        public Persona Persona { get; set; }
        public bool PersonaCreada { get; set; }
        // La persona ya existía (mismo DNI/CUIT) y se actualizaron sus datos con los del archivo.
        public bool PersonaActualizada { get; set; }
    }
}
