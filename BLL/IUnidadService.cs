using BE;
using BE.Enums;
using System.Collections.Generic;

namespace BLL
{
    public interface IUnidadService
    {
        // Alta / consultas
        int RegistrarUnidad(Unidad unidad, int idPersona);
        Unidad Obtener(int unidadId);
        Unidad ObtenerPorDominio(string dominio);
        List<Unidad> ListarPorEstado(EstadoUnidad estado);
        List<Unidad> Listar();
        Unidad ObtenerTrazabilidad(int unidadId);                               // unidad con vendedor, checklist, historial, publicación, imágenes y ventas

        // Transiciones (nuevo flujo N01)
        void TomarParaPreparacion(int unidadId);                                // Ingresado → EnPreparacion (crea checklist si falta)
        void EnviarPresupuestoAGerente(int unidadId);                           // EnPreparacion → RequiereAprobacionPresupuesto (debe haber extras propuestos)
        void AprobarPresupuesto(int unidadId, string observaciones);            // RequiereAprobacionPresupuesto → EnPreparacion (marca extras Aprobados)
        void RechazarPresupuesto(int unidadId, string motivo);                  // RequiereAprobacionPresupuesto → EnPreparacion (marca extras Rechazados)
        void FinalizarPreparacion(int unidadId);                                // EnPreparacion → PendienteVenta (todos los items aprobados deben estar ejecutados)
        void AutorizarPublicacion(int unidadId, string observaciones);          // PendienteVenta → EnVenta
        void RechazarPublicacion(int unidadId, string motivo);                  // PendienteVenta → EnPreparacion

        // Checklist
        ChecklistPreparacion ObtenerChecklistDeUnidad(int unidadId);
        int AgregarItemExtra(int unidadId, string nombre, string descripcion, decimal? costoEstimado);
        void EliminarItemExtra(int unidadId, int idItem);
        void MarcarItemRevisado(int unidadId, int idItem, BE.Enums.ResultadoRevision resultado, string comentario);
    }
}
