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

        // Item de la plantilla del que se copió. Null en los extras.
        private ChecklistItemTemplate templateOrigen;
        public ChecklistItemTemplate TemplateOrigen
        {
            get { return templateOrigen; }
            set { templateOrigen = value; }
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

        // Null mientras el item no fue revisado.
        private Usuario revisor;
        public Usuario Revisor
        {
            get { return revisor; }
            set { revisor = value; }
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
