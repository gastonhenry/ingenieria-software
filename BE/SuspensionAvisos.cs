using System;

namespace BE
{
    // Suspensión de los avisos por mail a un cliente (N03 — CU-CAP-05).
    // Si existe el registro, el cliente no recibe avisos; reanudar lo elimina.
    public class SuspensionAvisos
    {
        public int IdPersona { get; set; }
        public DateTime FechaSuspension { get; set; }
        public Usuario Usuario { get; set; }
        public string Motivo { get; set; }
    }
}
