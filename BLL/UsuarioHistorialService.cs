using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class UsuarioHistorialService : IUsuarioHistorialService
    {
        private readonly MapperUsuarioHistorial _mapperHistorial;
        private readonly MapperUsuario _mapperUsuario;
        private readonly PermisoService _permisoService;
        private readonly BitacoraService _bitacoraService;

        public UsuarioHistorialService()
        {
            _mapperHistorial = new MapperUsuarioHistorial();
            _mapperUsuario = new MapperUsuario();
            _permisoService = new PermisoService();
            _bitacoraService = new BitacoraService();
        }

        public void RegistrarSnapshot(Usuario usuario, AccionUsuarioHistorial accion, int? modificadoPorUsuarioId, int? restauracionId = null)
        {
            if (usuario == null) return;

            var snapshot = new UsuarioHistorial
            {
                UsuarioId              = usuario.Id,
                Accion                 = accion,
                ModificadoPorUsuarioId = modificadoPorUsuarioId,
                RestauracionId         = restauracionId,
                Nombre                 = usuario.Nombre  ?? string.Empty,
                Apellido               = usuario.Apellido ?? string.Empty,
                Email                  = usuario.Email   ?? string.Empty,
                Telefono               = usuario.Telefono ?? string.Empty,
                Documento              = usuario.Documento ?? string.Empty,
                Domicilio              = usuario.Domicilio ?? string.Empty,
                Bloqueado              = usuario.Bloqueado
            };
            _mapperHistorial.Insertar(snapshot);
        }

        public List<UsuarioHistorial> ListarPorUsuario(int usuarioId)
        {
            RequerirAdminOPermiso("VER_HISTORIAL_USUARIO", "VerHistorialUsuario");
            return _mapperHistorial.ListarPorUsuario(usuarioId);
        }

        public UsuarioHistorial ObtenerPorIdHistorial(int historialId)
        {
            return _mapperHistorial.ObtenerPorIdHistorial(historialId);
        }

        public void Restaurar(int historialId)
        {
            RequerirAdminOPermiso("VER_HISTORIAL_USUARIO", "RestaurarUsuario");
            RequerirAdminOPermiso("EDITAR_USUARIO", "RestaurarUsuario");

            UsuarioHistorial snap = _mapperHistorial.ObtenerPorIdHistorial(historialId);
            if (snap == null)
                throw new BLLException("ERR_HISTORIAL_NO_EXISTE", "La versión seleccionada no existe.");

            Usuario usuario = _mapperUsuario.ObtenerPorId(snap.UsuarioId);
            if (usuario == null)
                throw new BLLException("ERR_USUARIO_NO_ENCONTRADO", "No se encontró el usuario.");

            usuario.Nombre    = snap.Nombre;
            usuario.Apellido  = snap.Apellido;
            usuario.Email     = snap.Email;
            usuario.Telefono  = snap.Telefono;
            usuario.Documento = snap.Documento;
            usuario.Domicilio = snap.Domicilio;
            _mapperUsuario.Editar(usuario);

            if (snap.Bloqueado && !usuario.Bloqueado)
                _mapperUsuario.BloquearUsuario(usuario.Id);
            else if (!snap.Bloqueado && usuario.Bloqueado)
                _mapperUsuario.DesbloquearUsuario(usuario.Id);

            usuario.Bloqueado = snap.Bloqueado;

            UsuarioService.RecalcularDVUsuarios();

            int? actuanteId = SesionUsuario.GetInstancia().Usuario?.Id;
            RegistrarSnapshot(usuario, AccionUsuarioHistorial.Restauracion, actuanteId, snap.Id);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.EdicionUsuario,
                $"Usuario '{usuario.Username}' restaurado al estado del {snap.FechaHora:dd/MM/yyyy HH:mm:ss} (HistorialId {snap.Id})");
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null
                && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;

            if (_permisoService.UsuarioTienePermiso(usuario, codigoPermiso)) return;

            _bitacoraService.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
