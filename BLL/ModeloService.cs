using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ModeloService : IModeloService
    {
        private readonly MapperModelo _mapperModelo;
        private readonly BitacoraService _bitacoraService;

        public ModeloService()
        {
            _mapperModelo = new MapperModelo();
            _bitacoraService = new BitacoraService();
        }

        public List<Modelo> Listar() => _mapperModelo.Listar();

        public List<Modelo> ListarPorMarca(int idMarca) => _mapperModelo.ListarPorMarca(idMarca);

        private Modelo ObtenerPorId(int id)
        {
            foreach (Modelo m in _mapperModelo.Listar())
                if (m.Id == id) return m;
            return null;
        }

        public int Registrar(string nombre, int idMarca, TipoCarroceria tipoCarroceria)
        {
            RequerirAdminOPermiso("GESTIONAR_MODELOS", "RegistrarModelo");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new BLLException("ERR_MODELO_NOMBRE_OBLIGATORIO", "El nombre del modelo es obligatorio.");
            if (idMarca <= 0)
                throw new BLLException("ERR_MODELO_MARCA_OBLIGATORIA", "Debés seleccionar una marca para el modelo.");

            nombre = nombre.Trim();

            if (_mapperModelo.ObtenerPorNombreYMarca(nombre, idMarca) != null)
                throw new BLLException("ERR_MODELO_NOMBRE_DUPLICADO",
                    "Ya existe un modelo con el nombre '{0}' para esta marca.", nombre);

            int id = _mapperModelo.Insertar(new Modelo
            {
                Nombre = nombre,
                IdMarca = idMarca,
                TipoCarroceria = tipoCarroceria
            });

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaModelo,
                $"Modelo creado: '{nombre}' (IdMarca {idMarca}, carrocería {tipoCarroceria}).");

            return id;
        }

        public void Editar(int id, string nuevoNombre, int idMarca, TipoCarroceria tipoCarroceria)
        {
            RequerirAdminOPermiso("GESTIONAR_MODELOS", "EditarModelo");

            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new BLLException("ERR_MODELO_NOMBRE_OBLIGATORIO", "El nombre del modelo es obligatorio.");
            if (idMarca <= 0)
                throw new BLLException("ERR_MODELO_MARCA_OBLIGATORIA", "Debés seleccionar una marca para el modelo.");

            nuevoNombre = nuevoNombre.Trim();

            var modelo = ObtenerPorId(id);
            if (modelo == null)
                throw new BLLException("ERR_MODELO_NO_EXISTE", "El modelo no existe.");

            bool cambioNombre = !string.Equals(modelo.Nombre, nuevoNombre, StringComparison.OrdinalIgnoreCase);
            bool cambioMarca  = modelo.IdMarca != idMarca;
            if (cambioNombre || cambioMarca)
            {
                var dup = _mapperModelo.ObtenerPorNombreYMarca(nuevoNombre, idMarca);
                if (dup != null && dup.Id != id)
                    throw new BLLException("ERR_MODELO_NOMBRE_DUPLICADO",
                        "Ya existe un modelo con el nombre '{0}' para esta marca.", nuevoNombre);
            }

            string nombreAnterior = modelo.Nombre;
            modelo.Nombre = nuevoNombre;
            modelo.IdMarca = idMarca;
            modelo.TipoCarroceria = tipoCarroceria;
            _mapperModelo.Editar(modelo);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaModelo,
                $"Modelo editado: '{nombreAnterior}' → '{nuevoNombre}' (Id {id}).");
        }

        public void Eliminar(int id)
        {
            RequerirAdminOPermiso("GESTIONAR_MODELOS", "EliminarModelo");

            var modelo = ObtenerPorId(id);
            if (modelo == null)
                throw new BLLException("ERR_MODELO_NO_EXISTE", "El modelo no existe.");

            int unidadesConElModelo = _mapperModelo.ContarUnidadesConModelo(modelo.Nombre);
            if (unidadesConElModelo > 0)
                throw new BLLException("ERR_MODELO_EN_USO",
                    "No se puede eliminar el modelo '{0}' porque hay {1} unidad(es) asociada(s).",
                    modelo.Nombre, unidadesConElModelo);

            _mapperModelo.Eliminar(modelo);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaModelo,
                $"Modelo eliminado: '{modelo.Nombre}' (Id {id}).");
        }

        private void RequerirAdminOPermiso(string codigoPermiso, string accion)
        {
            var usuario = SesionUsuario.GetInstancia().Usuario;
            bool esAdmin = usuario != null && usuario.Username != null
                && usuario.Username.ToLower() == "admin";
            if (esAdmin) return;

            if (new PermisoService().UsuarioTienePermiso(usuario, codigoPermiso)) return;

            _bitacoraService.Insertar(usuario, TipoBitacora.Error,
                $"Acceso denegado a '{accion}': se requiere admin o permiso '{codigoPermiso}'.");
            throw new BLLException("ERR_SIN_PERMISO_ACCION",
                "No tenés permiso para ejecutar esta acción ({0}).", codigoPermiso);
        }
    }
}
