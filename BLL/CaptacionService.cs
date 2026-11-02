using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class CaptacionService : ICaptacionService
    {
        private const int ANIO_MINIMO = 1950;
        private const string MOTIVO_NO_ACEPTA_AVISOS = "No aceptó avisos en la última entrevista.";

        private readonly MapperBusquedaCliente _mapperBusqueda;
        private readonly MapperCaptacion _mapperCaptacion;
        private readonly MapperModelo _mapperModelo;
        private readonly PersonaService _personaService;
        private readonly BitacoraService _bitacora;

        public CaptacionService()
        {
            _mapperBusqueda = new MapperBusquedaCliente();
            _mapperCaptacion = new MapperCaptacion();
            _mapperModelo = new MapperModelo();
            _personaService = new PersonaService();
            _bitacora = new BitacoraService();
        }

        // ------------------------------------------------------------
        // CU-CAP-01 — Registrar entrevista
        // ------------------------------------------------------------

        public int RegistrarEntrevista(int idPersona, List<int> idsMarca, List<int> idsModelo, List<TipoCarroceria> carrocerias,
                                       int? anioDesde, int? anioHasta, decimal? precioMaximo,
                                       string observaciones, bool aceptaAvisos)
        {
            RequerirAdminOPermiso("REGISTRAR_ENTREVISTA", "RegistrarEntrevista");

            var cliente = idPersona > 0 ? _personaService.ObtenerPorId(idPersona) : null;
            if (cliente == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná el cliente.");

            var marcas = (idsMarca ?? new List<int>()).Distinct().Select(idMarca => new Marca { Id = idMarca }).ToList();
            carrocerias = (carrocerias ?? new List<TipoCarroceria>()).Distinct().ToList();

            // RN-N03-02: los modelos tienen que existir y pertenecer a una de las marcas elegidas
            // (restringen esa marca a esos modelos).
            var modelos = new List<Modelo>();
            if (idsModelo != null && idsModelo.Count > 0)
            {
                var catalogo = _mapperModelo.Listar();
                foreach (int idModelo in idsModelo.Distinct())
                {
                    var modelo = catalogo.FirstOrDefault(m => m.Id == idModelo);
                    if (modelo == null)
                        throw new BLLException("ERR_MODELO_NO_EXISTE", "No se encontró el modelo seleccionado.");
                    if (!marcas.Any(ma => ma.Id == modelo.Marca.Id))
                        marcas.Add(modelo.Marca);
                    modelos.Add(modelo);
                }
            }

            if (marcas.Count == 0 && carrocerias.Count == 0)
                throw new BLLException("ERR_BUSQUEDA_SIN_CRITERIO",
                    "Tildá al menos una marca o un tipo de carrocería que busque el cliente.");

            int anioMaximo = DateTime.Now.Year + 1;
            if ((anioDesde.HasValue && (anioDesde < ANIO_MINIMO || anioDesde > anioMaximo))
                || (anioHasta.HasValue && (anioHasta < ANIO_MINIMO || anioHasta > anioMaximo))
                || (anioDesde.HasValue && anioHasta.HasValue && anioDesde > anioHasta))
                throw new BLLException("ERR_BUSQUEDA_RANGO_ANIOS",
                    "El rango de años no es válido (entre {0} y {1}, y el año desde no puede ser mayor al año hasta).",
                    ANIO_MINIMO, anioMaximo);

            if (precioMaximo.HasValue && precioMaximo <= 0)
                throw new BLLException("ERR_PRECIO_INVALIDO", "El precio máximo debe ser mayor a cero.");

            // RN-N03-04: para recibir avisos el cliente necesita un mail registrado.
            if (aceptaAvisos && string.IsNullOrWhiteSpace(cliente.Email))
                throw new BLLException("ERR_CLIENTE_SIN_EMAIL",
                    "El cliente no tiene un correo electrónico registrado. Cargalo en la ficha de la persona o registrá la búsqueda sin avisos.");

            var parametros = _mapperCaptacion.ObtenerParametros();
            var usuario = SesionUsuario.GetInstancia().Usuario;
            var busqueda = new BusquedaCliente
            {
                Cliente          = cliente,
                Entrevistador    = usuario,
                FechaVencimiento = DateTime.Now.AddDays(parametros.DiasVigenciaBusqueda),
                Marcas           = marcas,
                Modelos          = modelos,
                Carrocerias      = carrocerias,
                AnioDesde        = anioDesde,
                AnioHasta        = anioHasta,
                PrecioMaximo     = precioMaximo,
                Observaciones    = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim(),
                AceptaAvisos     = aceptaAvisos
            };
            int id = _mapperBusqueda.Insertar(busqueda);

            _bitacora.Insertar(usuario, TipoBitacora.EntrevistaCliente,
                $"Entrevista registrada: búsqueda Id {id} de {cliente.Nombre} ({cliente.Documento})"
                + (aceptaAvisos ? ", acepta avisos." : ", sin avisos."));

            // Manda la última entrevista: si acepta avisos se levanta la suspensión; si no, se suspenden.
            bool suspendido = _mapperCaptacion.ObtenerSuspension(cliente.Id) != null;
            if (aceptaAvisos && suspendido)
            {
                _mapperCaptacion.ReanudarAvisos(cliente.Id);
                _bitacora.Insertar(usuario, TipoBitacora.GestionAvisosCliente,
                    $"Avisos reanudados a {cliente.Nombre} ({cliente.Documento}): aceptó avisos en la entrevista.");
            }
            else if (!aceptaAvisos && !suspendido)
            {
                _mapperCaptacion.SuspenderAvisos(cliente.Id, usuario.Id, MOTIVO_NO_ACEPTA_AVISOS);
                _bitacora.Insertar(usuario, TipoBitacora.GestionAvisosCliente,
                    $"Avisos suspendidos a {cliente.Nombre} ({cliente.Documento}): no aceptó avisos en la entrevista.");
            }
            return id;
        }

        public List<UnidadEnVenta> ListarUnidadesCoincidentes(int idBusqueda)
        {
            var busqueda = _mapperBusqueda.Listar(null, null).FirstOrDefault(b => b.Id == idBusqueda);
            if (busqueda == null) return new List<UnidadEnVenta>();
            return _mapperCaptacion.ListarUnidadesEnVenta().Where(u => Coincide(busqueda, u)).ToList();
        }

        public List<UnidadEnVenta> ListarUnidadesCoincidentes(List<int> idsMarca, List<int> idsModelo, List<TipoCarroceria> carrocerias,
                                                              int? anioDesde, int? anioHasta, decimal? precioMaximo)
        {
            var marcas = (idsMarca ?? new List<int>()).Distinct().Select(idMarca => new Marca { Id = idMarca }).ToList();
            var modelos = new List<Modelo>();
            if (idsModelo != null && idsModelo.Count > 0)
            {
                var ids = new HashSet<int>(idsModelo);
                modelos = _mapperModelo.Listar().Where(m => ids.Contains(m.Id)).ToList();
                foreach (var mo in modelos)
                    if (!marcas.Any(ma => ma.Id == mo.Marca.Id)) marcas.Add(mo.Marca);
            }
            carrocerias = (carrocerias ?? new List<TipoCarroceria>()).Distinct().ToList();

            // Mismo requisito que al registrar: sin marca ni carrocería no hay búsqueda.
            if (marcas.Count == 0 && carrocerias.Count == 0) return new List<UnidadEnVenta>();

            var busqueda = new BusquedaCliente
            {
                Marcas       = marcas,
                Modelos      = modelos,
                Carrocerias  = carrocerias,
                AnioDesde    = anioDesde,
                AnioHasta    = anioHasta,
                PrecioMaximo = precioMaximo
            };
            return _mapperCaptacion.ListarUnidadesEnVenta().Where(u => Coincide(busqueda, u)).ToList();
        }

        // ------------------------------------------------------------
        // CU-CAP-02 — Tablero de captación
        // ------------------------------------------------------------

        public TableroCaptacion ObtenerTablero(DateTime desde, DateTime hasta)
        {
            RequerirAdminOPermiso("CONSULTAR_TABLERO_CAPTACION", "ObtenerTablero");

            VencerBusquedas();
            var parametros = _mapperCaptacion.ObtenerParametros();
            var unidades = _mapperCaptacion.ListarUnidadesEnVenta();
            var busquedas = _mapperBusqueda.Listar(EstadoBusqueda.Activa, null);
            DateTime hastaExclusivo = hasta.Date.AddDays(1);

            var tablero = new TableroCaptacion { Desde = desde.Date, Hasta = hasta.Date };

            foreach (var u in unidades.OrderByDescending(x => x.FechaEnVenta))
            {
                int interesados = busquedas.Where(b => Coincide(b, u)).Select(b => b.Cliente.Id).Distinct().Count();
                int dias = Math.Max(0, (DateTime.Now - u.FechaEnVenta).Days);
                tablero.Unidades.Add(new FilaUnidadTablero
                {
                    Unidad = u,
                    CantidadInteresados = interesados,
                    DiasPublicada = dias,
                    Alerta = interesados == 0 && dias >= parametros.DiasAlertaSinInteresados
                });
            }

            // Posibles compradores: clientes con búsqueda activa que coincide con al menos una unidad en venta.
            foreach (var grupo in busquedas.GroupBy(b => b.Cliente.Id))
            {
                var coincidencias = unidades.Where(u => grupo.Any(b => Coincide(b, u))).ToList();
                if (coincidencias.Count == 0)
                {
                    // Ninguna de sus búsquedas activas coincide con lo que hay en venta: espera stock.
                    tablero.InteresadosSinUnidades.Add(new InteresadoTablero
                    {
                        Cliente = grupo.First().Cliente,
                        Busquedas = grupo.ToList(),
                        AvisosSuspendidos = grupo.First().AvisosSuspendidos,
                        FechaUltimaEntrevista = grupo.Max(b => b.FechaRegistro)
                    });
                    continue;
                }
                tablero.Interesados.Add(new InteresadoTablero
                {
                    Cliente = grupo.First().Cliente,
                    Busquedas = grupo.ToList(),
                    Coincidencias = coincidencias,
                    AvisosSuspendidos = grupo.First().AvisosSuspendidos,
                    FechaUltimaEntrevista = grupo.Max(b => b.FechaRegistro)
                });
            }
            tablero.Interesados = tablero.Interesados
                .OrderByDescending(i => i.Coincidencias.Count)
                .ThenByDescending(i => i.FechaUltimaEntrevista)
                .ToList();
            tablero.InteresadosSinUnidades = tablero.InteresadosSinUnidades
                .OrderByDescending(i => i.FechaUltimaEntrevista)
                .ToList();

            tablero.Avisos =_mapperCaptacion.ListarNotificaciones(desde.Date, hastaExclusivo, null);
            var idsEnVenta = new HashSet<int>(unidades.Select(u => u.IdUnidad));
            tablero.AvisosUnidadesEnVenta = _mapperCaptacion.ListarNotificaciones(null, null, null)
                .Where(a => idsEnVenta.Contains(a.IdUnidad)).ToList();
            tablero.ClientesAvisados = _mapperCaptacion.ContarClientesAvisados(desde.Date, hastaExclusivo);
            tablero.UnidadesPorSalirAVenta = _mapperCaptacion.ContarUnidadesPorSalirAVenta();
            tablero.Conversiones = _mapperCaptacion.ListarConversiones(desde.Date, hastaExclusivo);
            return tablero;
        }

        // ------------------------------------------------------------
        // CU-CAP-05 — Suspender / reanudar avisos, cerrar búsqueda
        // ------------------------------------------------------------

        public List<Persona> BuscarClientes(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return new List<Persona>();
            return _mapperBusqueda.BuscarPersonasConBusquedas(texto.Trim());
        }

        public List<BusquedaCliente> ListarBusquedasDeCliente(int idPersona)
        {
            VencerBusquedas();
            return _mapperBusqueda.Listar(null, idPersona);
        }

        public List<NotificacionEnviada> ListarAvisosDeCliente(int idPersona) =>
            _mapperCaptacion.ListarNotificaciones(null, null, idPersona);

        public SuspensionAvisos ObtenerSuspension(int idPersona) => _mapperCaptacion.ObtenerSuspension(idPersona);

        public void SuspenderAvisos(int idPersona, string motivo)
        {
            RequerirAdminOPermiso("GESTIONAR_AVISOS_CLIENTE", "SuspenderAvisos");
            var cliente = ObtenerClienteOFallar(idPersona);
            if (_mapperCaptacion.ObtenerSuspension(idPersona) != null)
                throw new BLLException("ERR_AVISOS_YA_SUSPENDIDOS", "Los avisos de este cliente ya están suspendidos.");

            var usuario = SesionUsuario.GetInstancia().Usuario;
            _mapperCaptacion.SuspenderAvisos(idPersona, usuario.Id, string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim());
            _bitacora.Insertar(usuario, TipoBitacora.GestionAvisosCliente,
                $"Avisos suspendidos a {cliente.Nombre} ({cliente.Documento})"
                + (string.IsNullOrWhiteSpace(motivo) ? "." : $". Motivo: {motivo.Trim()}"));
        }

        public void ReanudarAvisos(int idPersona)
        {
            RequerirAdminOPermiso("GESTIONAR_AVISOS_CLIENTE", "ReanudarAvisos");
            var cliente = ObtenerClienteOFallar(idPersona);
            if (_mapperCaptacion.ObtenerSuspension(idPersona) == null)
                throw new BLLException("ERR_AVISOS_NO_SUSPENDIDOS", "Los avisos de este cliente no están suspendidos.");

            _mapperCaptacion.ReanudarAvisos(idPersona);
            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.GestionAvisosCliente,
                $"Avisos reanudados a {cliente.Nombre} ({cliente.Documento}).");
        }

        public void CerrarBusqueda(int idBusqueda, string motivo)
        {
            RequerirAdminOPermiso("GESTIONAR_AVISOS_CLIENTE", "CerrarBusqueda");
            var busqueda = _mapperBusqueda.Listar(null, null).FirstOrDefault(b => b.Id == idBusqueda);
            if (busqueda == null)
                throw new BLLException("ERR_BUSQUEDA_NO_ENCONTRADA", "No se encontró la búsqueda.");
            if (busqueda.Estado != EstadoBusqueda.Activa)
                throw new BLLException("ERR_BUSQUEDA_NO_ACTIVA", "Solo se puede cerrar una búsqueda activa.");

            string m = string.IsNullOrWhiteSpace(motivo) ? "El cliente indicó que ya no busca." : motivo.Trim();
            _mapperBusqueda.Cerrar(idBusqueda, m);
            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.GestionAvisosCliente,
                $"Búsqueda Id {idBusqueda} de {busqueda.Cliente.Nombre} cerrada. Motivo: {m}");
        }

        // ------------------------------------------------------------
        // Parámetros
        // ------------------------------------------------------------

        public ParametrosCaptacion ObtenerParametros() => _mapperCaptacion.ObtenerParametros();

        public void ActualizarParametros(bool notificacionesActivas, int diasVigenciaBusqueda, int diasAlertaSinInteresados)
        {
            RequerirAdminOPermiso("GESTIONAR_PARAMETROS_CAPTACION", "ActualizarParametros");
            if (diasVigenciaBusqueda < 1 || diasVigenciaBusqueda > 3650 || diasAlertaSinInteresados < 1 || diasAlertaSinInteresados > 3650)
                throw new BLLException("ERR_PARAMETROS_CAPTACION_INVALIDOS",
                    "Los días de vigencia y de alerta deben estar entre 1 y 3650.");

            _mapperCaptacion.ActualizarParametros(new ParametrosCaptacion
            {
                NotificacionesActivas    = notificacionesActivas,
                DiasVigenciaBusqueda     = diasVigenciaBusqueda,
                DiasAlertaSinInteresados = diasAlertaSinInteresados
            });
            _bitacora.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.ParametrosCaptacion,
                $"Parámetros de captación: avisos {(notificacionesActivas ? "activados" : "desactivados")}, "
                + $"vigencia {diasVigenciaBusqueda} días, alerta {diasAlertaSinInteresados} días.");
        }

        // ------------------------------------------------------------
        // Uso interno de otros services (N02 y CU-CAP-03)
        // ------------------------------------------------------------

        // RN-N03-03: la unidad coincide si cumple todos los criterios informados en la búsqueda.
        // - Marca: si hay marcas elegidas, la de la unidad tiene que estar entre ellas.
        // - Modelo: si para la marca de la unidad se eligieron modelos, tiene que ser uno de ellos;
        //   si no, cualquier modelo de esa marca sirve.
        // - Carrocería: si hay carrocerías elegidas, la de la unidad tiene que estar entre ellas.
        // Si la unidad no tiene precio de publicación, el criterio de precio no se evalúa.
        internal static bool Coincide(BusquedaCliente b, UnidadEnVenta u)
        {
            if (b.Marcas.Count > 0 && !b.Marcas.Any(m => m.Id == u.IdMarca)) return false;
            var modelosDeLaMarca = b.Modelos.Where(m => m.Marca != null && m.Marca.Id == u.IdMarca).ToList();
            if (modelosDeLaMarca.Count > 0 && !modelosDeLaMarca.Any(m => m.Id == u.IdModelo)) return false;
            if (b.Carrocerias.Count > 0 && !b.Carrocerias.Contains(u.TipoCarroceria)) return false;
            if (b.AnioDesde.HasValue && u.Anio < b.AnioDesde.Value) return false;
            if (b.AnioHasta.HasValue && u.Anio > b.AnioHasta.Value) return false;
            if (b.PrecioMaximo.HasValue && u.PrecioPublicacion.HasValue && u.PrecioPublicacion.Value > b.PrecioMaximo.Value) return false;
            return true;
        }

        // RN-N03-08
        internal void VencerBusquedas()
        {
            int vencidas = _mapperBusqueda.VencerBusquedas();
            if (vencidas > 0)
                _bitacora.Insertar(null, TipoBitacora.GestionAvisosCliente,
                    $"{vencidas} búsqueda(s) de clientes vencieron por plazo de vigencia.");
        }

        internal List<BusquedaCliente> ListarBusquedasActivas() => _mapperBusqueda.Listar(EstadoBusqueda.Activa, null);

        private Persona ObtenerClienteOFallar(int idPersona)
        {
            var p = idPersona > 0 ? _personaService.ObtenerPorId(idPersona) : null;
            if (p == null) throw new BLLException("ERR_PERSONA_NO_EXISTE", "Seleccioná el cliente.");
            return p;
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;
            if (new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;
            _bitacora.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
