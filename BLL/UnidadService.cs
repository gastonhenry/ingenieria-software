using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class UnidadService : IUnidadService
    {
        // Máquina de estados de Unidad — CU-COM-07.
        //   Ingresado                        → EnPreparacion                   (Encargado toma la unidad)
        //   EnPreparacion                    → RequiereAprobacionPresupuesto   (Encargado envía extras al Gerente)
        //   EnPreparacion                    → PendienteVenta                  (Encargado finaliza preparación)
        //   RequiereAprobacionPresupuesto    → EnPreparacion                   (Gerente aprueba/rechaza extras)
        //   PendienteVenta                   → EnVenta                         (Gerente autoriza publicación)
        //   PendienteVenta                   → EnPreparacion                   (Gerente rechaza publicación)
        private static readonly Dictionary<EstadoUnidad, HashSet<EstadoUnidad>> TransicionesValidas
            = new Dictionary<EstadoUnidad, HashSet<EstadoUnidad>>
            {
                { EstadoUnidad.Ingresado,                     new HashSet<EstadoUnidad> { EstadoUnidad.EnPreparacion } },
                { EstadoUnidad.EnPreparacion,                 new HashSet<EstadoUnidad> { EstadoUnidad.RequiereAprobacionPresupuesto, EstadoUnidad.PendienteVenta } },
                { EstadoUnidad.RequiereAprobacionPresupuesto, new HashSet<EstadoUnidad> { EstadoUnidad.EnPreparacion } },
                { EstadoUnidad.PendienteVenta,                new HashSet<EstadoUnidad> { EstadoUnidad.EnVenta, EstadoUnidad.EnPreparacion } },
                { EstadoUnidad.EnVenta,                       new HashSet<EstadoUnidad> { EstadoUnidad.Vendido, EstadoUnidad.Reservado, EstadoUnidad.Pausado } },
                { EstadoUnidad.Vendido,                       new HashSet<EstadoUnidad>() },
                { EstadoUnidad.Reservado,                     new HashSet<EstadoUnidad> { EstadoUnidad.Vendido, EstadoUnidad.EnVenta } },
                { EstadoUnidad.Pausado,                       new HashSet<EstadoUnidad> { EstadoUnidad.EnVenta } },
            };

        private readonly MapperUnidad _mapperUnidad;
        private readonly MapperChecklist _mapperChecklist;
        private readonly MapperHistorialUnidad _mapperHistorial;
        private readonly PersonaService _personaService;
        private readonly PermisoService _permisoService;
        private readonly BitacoraService _bitacoraService;

        public UnidadService()
        {
            _mapperUnidad = new MapperUnidad();
            _mapperChecklist = new MapperChecklist();
            _mapperHistorial = new MapperHistorialUnidad();
            _personaService = new PersonaService();
            _permisoService = new PermisoService();
            _bitacoraService = new BitacoraService();
        }

        // ------------------------------------------------------------
        // Permisos
        // ------------------------------------------------------------

        private Usuario UsuarioActual() => SesionUsuario.GetInstancia().Usuario;

        private bool EsAdmin()
        {
            var u = UsuarioActual();
            return u != null && u.Username.ToLower() == "admin";
        }

        private void RequerirPermiso(string codigoPermiso, string accion)
        {
            if (EsAdmin()) return;
            var u = UsuarioActual();
            if (u != null && _permisoService.UsuarioTienePermiso(u, codigoPermiso)) return;

            _bitacoraService.Insertar(u, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }

        // ------------------------------------------------------------
        // Alta
        // ------------------------------------------------------------

        public int RegistrarUnidad(Unidad unidad, int idPersona)
        {
            RequerirPermiso("REGISTRAR_UNIDAD", "RegistrarUnidad");

            if (unidad == null
                || string.IsNullOrWhiteSpace(unidad.Dominio)
                || string.IsNullOrWhiteSpace(unidad.Marca)
                || string.IsNullOrWhiteSpace(unidad.Modelo)
                || unidad.Anio <= 0
                || unidad.Kilometraje < 0
                || unidad.PrecioCompra <= 0)
                throw new BLLException("ERR_UNIDAD_DATOS_INCOMPLETOS", "Faltan datos obligatorios de la unidad.");

            unidad.Dominio = unidad.Dominio.Trim().ToUpper();

            if (!EsDominioValido(unidad.Dominio))
                throw new BLLException("ERR_DOMINIO_INVALIDO",
                    "El dominio '{0}' no tiene un formato válido (AAA123 o AB123CD).", unidad.Dominio);

            if (_mapperUnidad.ObtenerPorDominio(unidad.Dominio) != null)
                throw new BLLException("ERR_DOMINIO_DUPLICADO",
                    "Ya existe una unidad registrada con el dominio '{0}'.", unidad.Dominio);

            var persona = _personaService.ObtenerPorId(idPersona);
            if (persona == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "La persona vendedora no existe.");

            unidad.IdPersona = idPersona;
            unidad.IdCompradorUsuario = UsuarioActual().Id;
            unidad.EstadoActual = EstadoUnidad.Ingresado;

            int nuevoId = _mapperUnidad.Insertar(unidad);
            unidad.Id = nuevoId;

            new VerificadorUnidad().RecalcularDVs();

            _bitacoraService.Insertar(UsuarioActual(), TipoBitacora.RegistroUnidad,
                $"Unidad registrada: {unidad.Dominio} (Id {nuevoId}).");

            return nuevoId;
        }

        // ------------------------------------------------------------
        // Consultas
        // ------------------------------------------------------------

        public Unidad Obtener(int unidadId) => _mapperUnidad.Obtener(unidadId);

        public Unidad ObtenerPorDominio(string dominio)
        {
            if (string.IsNullOrWhiteSpace(dominio)) return null;
            return _mapperUnidad.ObtenerPorDominio(dominio.Trim().ToUpper());
        }

        public List<Unidad> ListarPorEstado(EstadoUnidad estado) => _mapperUnidad.ListarPorEstado(estado);

        public List<Unidad> Listar() => _mapperUnidad.Listar();

        public TrazabilidadUnidad ObtenerTrazabilidad(int unidadId)
        {
            RequerirPermiso("CONSULTAR_UNIDAD", "ObtenerTrazabilidad");

            var unidad = _mapperUnidad.Obtener(unidadId);
            if (unidad == null)
                throw new BLLException("ERR_UNIDAD_NO_ENCONTRADA", "No se encontró la unidad solicitada.");

            return new TrazabilidadUnidad
            {
                Unidad = unidad,
                Persona = _personaService.ObtenerPorId(unidad.IdPersona),
                Checklist = _mapperChecklist.ObtenerPorUnidad(unidadId),
                Historial = _mapperHistorial.ListarPorUnidad(unidadId)
            };
        }

        // ------------------------------------------------------------
        // Transiciones — todas pasan por TransicionarConHistorial que valida
        // contra la máquina de estados, invoca al SP atómico y recalcula DVs.
        // ------------------------------------------------------------

        public void TomarParaPreparacion(int unidadId)
        {
            RequerirPermiso("REVISAR_UNIDAD", "TomarParaPreparacion");

            var unidad = ObtenerOFallar(unidadId);
            if (unidad.EstadoActual != EstadoUnidad.Ingresado)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Solo se puede tomar para preparación una unidad en estado Ingresado.");

            // Crea el checklist (snapshot del template) si todavía no existe.
            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            if (checklist == null)
                _mapperChecklist.CrearParaUnidad(unidadId, UsuarioActual().Id);

            TransicionarConHistorial(unidadId, EstadoUnidad.Ingresado, EstadoUnidad.EnPreparacion,
                null, motivoObligatorio: false);
        }

        public void EnviarPresupuestoAGerente(int unidadId)
        {
            RequerirPermiso("REVISAR_UNIDAD", "EnviarPresupuestoAGerente");

            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            if (checklist == null)
                throw new BLLException("ERR_CHECKLIST_NO_EXISTE", "La unidad no tiene checklist asociado.");

            int propuestos = _mapperChecklist.ContarItemsExtraPropuestos(checklist.Id);
            if (propuestos == 0)
                throw new BLLException("ERR_SIN_EXTRAS_PROPUESTOS",
                    "No hay items extra propuestos para enviar al Gerente. Agregá al menos uno.");

            TransicionarConHistorial(unidadId, EstadoUnidad.EnPreparacion, EstadoUnidad.RequiereAprobacionPresupuesto,
                null, motivoObligatorio: false);
        }

        public void AprobarPresupuesto(int unidadId, string observaciones)
        {
            RequerirPermiso("AUTORIZAR_CHECKLIST", "AprobarPresupuesto");

            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            if (checklist == null)
                throw new BLLException("ERR_CHECKLIST_NO_EXISTE", "La unidad no tiene checklist asociado.");

            _mapperChecklist.AprobarItemsExtraPropuestos(checklist.Id);

            TransicionarConHistorial(unidadId, EstadoUnidad.RequiereAprobacionPresupuesto, EstadoUnidad.EnPreparacion,
                string.IsNullOrWhiteSpace(observaciones) ? "Presupuesto aprobado." : "Presupuesto aprobado. " + observaciones,
                motivoObligatorio: false);
        }

        public void RechazarPresupuesto(int unidadId, string motivo)
        {
            RequerirPermiso("AUTORIZAR_CHECKLIST", "RechazarPresupuesto");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new BLLException("ERR_MOTIVO_RECHAZO_VACIO", "El motivo del rechazo es obligatorio.");

            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            if (checklist == null)
                throw new BLLException("ERR_CHECKLIST_NO_EXISTE", "La unidad no tiene checklist asociado.");

            _mapperChecklist.RechazarItemsExtraPropuestos(checklist.Id);

            TransicionarConHistorial(unidadId, EstadoUnidad.RequiereAprobacionPresupuesto, EstadoUnidad.EnPreparacion,
                "Presupuesto rechazado. " + motivo.Trim(), motivoObligatorio: false);
        }

        public void FinalizarPreparacion(int unidadId)
        {
            RequerirPermiso("EJECUTAR_PREPARACION", "FinalizarPreparacion");

            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            if (checklist == null)
                throw new BLLException("ERR_CHECKLIST_NO_EXISTE",
                    "La unidad no tiene checklist asociado.");

            // Para finalizar: todos los items Aprobados (template + extras aprobados) tienen
            // que estar revisados (ResultadoRevision != Pendiente). Los Propuestos bloquean
            // la finalización — el Encargado debe enviar el presupuesto o eliminarlos primero.
            // Los Rechazados se ignoran (descartados por el Gerente).
            foreach (var item in checklist.Items)
            {
                if (item.EstadoAprobacion == EstadoAprobacionItem.Propuesto)
                    throw new BLLException("ERR_EXTRAS_PENDIENTES_APROBACION",
                        "Hay ítems extra pendientes de aprobación del Gerente. Enviá el presupuesto o eliminalos antes de finalizar.");

                if (item.EstadoAprobacion == EstadoAprobacionItem.Aprobado && !item.EstaRevisado)
                    throw new BLLException("ERR_CHECKLIST_ITEMS_PENDIENTES",
                        "Hay ítems sin revisar. Marcá todos como OK u Observado antes de finalizar.");
            }

            TransicionarConHistorial(unidadId, EstadoUnidad.EnPreparacion, EstadoUnidad.PendienteVenta,
                null, motivoObligatorio: false);
        }

        public void AutorizarPublicacion(int unidadId, string observaciones)
        {
            RequerirPermiso("AUTORIZAR_PUBLICACION", "AutorizarPublicacion");
            TransicionarConHistorial(unidadId, EstadoUnidad.PendienteVenta, EstadoUnidad.EnVenta,
                observaciones, motivoObligatorio: false);
        }

        public void RechazarPublicacion(int unidadId, string motivo)
        {
            RequerirPermiso("AUTORIZAR_PUBLICACION", "RechazarPublicacion");
            TransicionarConHistorial(unidadId, EstadoUnidad.PendienteVenta, EstadoUnidad.EnPreparacion,
                motivo, motivoObligatorio: true);
        }

        // internal: lo usan los services de N02 (VentaService) para disparar transiciones
        // sin duplicar la máquina de estados. No lo exponemos como public porque la validación
        // de permisos la hace cada caller.
        internal void TransicionarConHistorial(int unidadId, EstadoUnidad origenEsperado, EstadoUnidad destino,
                                               string motivo, bool motivoObligatorio)
        {
            var unidad = ObtenerOFallar(unidadId);

            if (unidad.EstadoActual != origenEsperado)
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "La unidad no está en el estado esperado ({0}); estado actual: {1}.",
                    origenEsperado, unidad.EstadoActual);

            if (!TransicionesValidas.TryGetValue(unidad.EstadoActual, out var permitidos) || !permitidos.Contains(destino))
                throw new BLLException("ERR_TRANSICION_INVALIDA",
                    "Transición no permitida: {0} → {1}.", unidad.EstadoActual, destino);

            if (motivoObligatorio && string.IsNullOrWhiteSpace(motivo))
                throw new BLLException("ERR_MOTIVO_RECHAZO_VACIO",
                    "El motivo del rechazo es obligatorio.");

            _mapperUnidad.TransicionarEstado(unidadId, unidad.EstadoActual, destino, UsuarioActual().Id,
                string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim());

            new VerificadorUnidad().RecalcularDVs();

            _bitacoraService.Insertar(UsuarioActual(), TipoBitacora.TransicionUnidad,
                $"Unidad Id {unidadId}: {unidad.EstadoActual} → {destino}"
                + (string.IsNullOrWhiteSpace(motivo) ? "" : $". Motivo/obs: {motivo.Trim()}"));
        }

        // Formatos de patente argentinos soportados:
        //  - Vieja (hasta 2016): 3 letras + 3 números (ej. ABC123)
        //  - Mercosur (desde 2016): 2 letras + 3 números + 2 letras (ej. AB123CD)
        private static readonly Regex RegexDominioViejo    = new Regex(@"^[A-Z]{3}[0-9]{3}$",         RegexOptions.Compiled);
        private static readonly Regex RegexDominioMercosur = new Regex(@"^[A-Z]{2}[0-9]{3}[A-Z]{2}$", RegexOptions.Compiled);

        private static bool EsDominioValido(string dominio)
        {
            if (string.IsNullOrEmpty(dominio)) return false;
            return RegexDominioViejo.IsMatch(dominio) || RegexDominioMercosur.IsMatch(dominio);
        }

        private Unidad ObtenerOFallar(int unidadId)
        {
            var unidad = _mapperUnidad.Obtener(unidadId);
            if (unidad == null)
                throw new BLLException("ERR_UNIDAD_NO_ENCONTRADA", "No se encontró la unidad solicitada.");
            return unidad;
        }

        // ------------------------------------------------------------
        // Checklist — items extra y ejecución
        // ------------------------------------------------------------

        public ChecklistPreparacion ObtenerChecklistDeUnidad(int unidadId)
        {
            return _mapperChecklist.ObtenerPorUnidad(unidadId);
        }

        public int AgregarItemExtra(int unidadId, string nombre, string descripcion, decimal? costoEstimado)
        {
            RequerirPermiso("REVISAR_UNIDAD", "AgregarItemExtra");

            var unidad = ObtenerOFallar(unidadId);
            // Los extras se agregan mientras la unidad está EnPreparacion (el Encargado detecta cosas
            // nuevas al revisar). Quedan en estado Propuesto hasta que el Gerente los apruebe o rechace.
            if (unidad.EstadoActual != EstadoUnidad.EnPreparacion)
                throw new BLLException("ERR_CHECKLIST_ITEMS_INMUTABLES",
                    "Solo se pueden agregar items extra mientras la unidad está En preparación.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new BLLException("ERR_ITEM_NOMBRE_VACIO", "El nombre del ítem es obligatorio.");

            if (costoEstimado.HasValue && costoEstimado.Value < 0)
                throw new BLLException("ERR_COSTO_INVALIDO", "El costo estimado no puede ser negativo.");

            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            if (checklist == null)
                throw new BLLException("ERR_CHECKLIST_NO_EXISTE", "La unidad no tiene checklist asociado.");

            int id = _mapperChecklist.AgregarItemExtra(checklist.Id, nombre.Trim(),
                string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                costoEstimado);

            _bitacoraService.Insertar(UsuarioActual(), TipoBitacora.TransicionUnidad,
                $"Unidad Id {unidadId}: agregado item extra '{nombre.Trim()}' (costo {(costoEstimado?.ToString("N2") ?? "N/A")}).");
            return id;
        }

        public void EliminarItemExtra(int unidadId, int idItem)
        {
            RequerirPermiso("REVISAR_UNIDAD", "EliminarItemExtra");

            var unidad = ObtenerOFallar(unidadId);
            if (unidad.EstadoActual != EstadoUnidad.EnPreparacion)
                throw new BLLException("ERR_CHECKLIST_ITEMS_INMUTABLES",
                    "Solo se pueden eliminar items extra mientras la unidad está En preparación.");

            _mapperChecklist.EliminarItemExtra(idItem);
            _bitacoraService.Insertar(UsuarioActual(), TipoBitacora.TransicionUnidad,
                $"Unidad Id {unidadId}: eliminado item extra Id {idItem} del checklist.");
        }

        public void MarcarItemRevisado(int unidadId, int idItem, ResultadoRevision resultado, string comentario)
        {
            RequerirPermiso("EJECUTAR_PREPARACION", "MarcarItemRevisado");

            var unidad = ObtenerOFallar(unidadId);
            if (unidad.EstadoActual != EstadoUnidad.EnPreparacion)
                throw new BLLException("ERR_ITEM_NO_REVISABLE",
                    "Solo se pueden revisar ítems del checklist cuando la unidad está En preparación.");

            if (resultado == ResultadoRevision.Pendiente)
                throw new BLLException("ERR_RESULTADO_INVALIDO",
                    "El resultado debe ser OK u Observado.");

            if (resultado == ResultadoRevision.Observado && string.IsNullOrWhiteSpace(comentario))
                throw new BLLException("ERR_COMENTARIO_OBSERVADO_OBLIGATORIO",
                    "Al marcar un ítem como Observado el comentario es obligatorio.");

            // Solo items aprobados pueden revisarse: los propuestos esperan al Gerente,
            // los rechazados quedaron descartados.
            var checklist = _mapperChecklist.ObtenerPorUnidad(unidadId);
            var item = checklist?.Items?.Find(i => i.Id == idItem);
            if (item == null)
                throw new BLLException("ERR_ITEM_NO_ENCONTRADO", "No se encontró el ítem del checklist.");
            if (item.EstadoAprobacion != EstadoAprobacionItem.Aprobado)
                throw new BLLException("ERR_ITEM_NO_APROBADO",
                    "Solo se pueden revisar items en estado Aprobado.");

            _mapperChecklist.MarcarItemRevisado(idItem, resultado, comentario, UsuarioActual().Id);
        }
    }
}
