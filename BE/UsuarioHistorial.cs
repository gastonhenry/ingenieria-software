using BE.Enums;
using System;

namespace BE
{
    public class UsuarioHistorial
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public AccionUsuarioHistorial Accion { get; set; }
        public int? ModificadoPorUsuarioId { get; set; }
        public string ModificadoPorUsername { get; set; }
        public int? RestauracionId { get; set; }
        public DateTime? RestauracionFechaHora { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Documento { get; set; }
        public string Domicilio { get; set; }
        public bool Bloqueado { get; set; }
    }
}
