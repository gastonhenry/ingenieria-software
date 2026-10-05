using System;
using BE.Enums;

namespace BE
{
    public class ChecklistItem
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int idChecklist;
        public int IdChecklist
        {
            get { return idChecklist; }
            set { idChecklist = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private bool esDelTemplate;
        public bool EsDelTemplate
        {
            get { return esDelTemplate; }
            set { esDelTemplate = value; }
        }

        private int? idTemplateOrigen;
        public int? IdTemplateOrigen
        {
            get { return idTemplateOrigen; }
            set { idTemplateOrigen = value; }
        }

        // Resultado de la revisión del Encargado: Pendiente / OK / Observado.
        private ResultadoRevision resultadoRevision;
        public ResultadoRevision ResultadoRevision
        {
            get { return resultadoRevision; }
            set { resultadoRevision = value; }
        }

        // Comentario de la revisión. Opcional si ResultadoRevision = OK, obligatorio si = Observado.
        private string comentarioRevision;
        public string ComentarioRevision
        {
            get { return comentarioRevision; }
            set { comentarioRevision = value; }
        }

        private DateTime? fechaRevision;
        public DateTime? FechaRevision
        {
            get { return fechaRevision; }
            set { fechaRevision = value; }
        }

        private int? idUsuarioRevisor;
        public int? IdUsuarioRevisor
        {
            get { return idUsuarioRevisor; }
            set { idUsuarioRevisor = value; }
        }

        // Costo estimado del item. Sólo aplica a extras; para los del template es null.
        private decimal? costoEstimado;
        public decimal? CostoEstimado
        {
            get { return costoEstimado; }
            set { costoEstimado = value; }
        }

        // Estado de aprobación. Los items del template arrancan y permanecen en Aprobado;
        // los extras arrancan en Propuesto y pasan a Aprobado o Rechazado por decisión del Gerente.
        private EstadoAprobacionItem estadoAprobacion;
        public EstadoAprobacionItem EstadoAprobacion
        {
            get { return estadoAprobacion; }
            set { estadoAprobacion = value; }
        }

        // Un item se considera revisado cuando ResultadoRevision != Pendiente.
        public bool EstaRevisado => resultadoRevision != ResultadoRevision.Pendiente;
    }
}
