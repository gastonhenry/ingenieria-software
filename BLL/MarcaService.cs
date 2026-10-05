using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class MarcaService : IMarcaService
    {
        private readonly MapperMarca _mapperMarca;
        private readonly BitacoraService _bitacoraService;

        public MarcaService()
        {
            _mapperMarca = new MapperMarca();
            _bitacoraService = new BitacoraService();
        }

        public List<Marca> Listar() => _mapperMarca.Listar();

        private Marca ObtenerPorId(int id)
        {
            foreach (Marca m in _mapperMarca.Listar())
                if (m.Id == id) return m;
            return null;
        }

        public void Editar(int id, string nuevoNombre)
        {
            RequerirAdminOPermiso("GESTIONAR_MARCAS", "EditarMarca");

            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new BLLException("ERR_MARCA_NOMBRE_OBLIGATORIO", "El nombre de la marca es obligatorio.");

            nuevoNombre = nuevoNombre.Trim();

            var marca = ObtenerPorId(id);
            if (marca == null)
                throw new BLLException("ERR_MARCA_NO_EXISTE", "La marca no existe.");

            if (!string.Equals(marca.Nombre, nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                var dup = _mapperMarca.ObtenerPorNombre(nuevoNombre);
                if (dup != null && dup.Id != id)
                    throw new BLLException("ERR_MARCA_NOMBRE_DUPLICADO",
                        "Ya existe una marca con el nombre '{0}'.", nuevoNombre);
            }

            string nombreAnterior = marca.Nombre;
            marca.Nombre = nuevoNombre;
            _mapperMarca.Editar(marca);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaMarca,
                $"Marca editada: '{nombreAnterior}' → '{nuevoNombre}' (Id {id}).");
        }

        public void Eliminar(int id)
        {
            RequerirAdminOPermiso("GESTIONAR_MARCAS", "EliminarMarca");

            var marca = ObtenerPorId(id);
            if (marca == null)
                throw new BLLException("ERR_MARCA_NO_EXISTE", "La marca no existe.");

            int unidadesConLaMarca = _mapperMarca.ContarUnidadesConMarca(marca.Id);
            if (unidadesConLaMarca > 0)
                throw new BLLException("ERR_MARCA_EN_USO",
                    "No se puede eliminar la marca '{0}' porque hay {1} unidad(es) asociada(s).",
                    marca.Nombre, unidadesConLaMarca);

            _mapperMarca.Eliminar(marca);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaMarca,
                $"Marca eliminada: '{marca.Nombre}' (Id {id}).");
        }

        public int Registrar(string nombre)
        {
            RequerirAdminOPermiso("GESTIONAR_MARCAS", "RegistrarMarca");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new BLLException("ERR_MARCA_NOMBRE_OBLIGATORIO", "El nombre de la marca es obligatorio.");

            nombre = nombre.Trim();

            if (_mapperMarca.ObtenerPorNombre(nombre) != null)
                throw new BLLException("ERR_MARCA_NOMBRE_DUPLICADO",
                    "Ya existe una marca con el nombre '{0}'.", nombre);

            int id = _mapperMarca.Insertar(new Marca { Nombre = nombre });

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaMarca,
                $"Marca creada: {nombre}");

            return id;
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
