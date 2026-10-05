using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class PersonaService : IPersonaService
    {
        private readonly MapperPersona _mapperPersona;
        private readonly BitacoraService _bitacoraService;

        public PersonaService()
        {
            _mapperPersona = new MapperPersona();
            _bitacoraService = new BitacoraService();
        }

        public List<Persona> Listar() => _mapperPersona.Listar();

        public Persona ObtenerPorId(int id) => _mapperPersona.Obtener(id);

        public Persona BuscarPorDocumento(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento)) return null;
            return _mapperPersona.ObtenerPorDocumento(documento.Trim());
        }

        public List<Persona> BuscarPorDocumentoParcial(string fragmento)
        {
            if (string.IsNullOrWhiteSpace(fragmento)) return new List<Persona>();
            return _mapperPersona.BuscarPorDocumentoLike(fragmento.Trim());
        }

        public int Registrar(char tipoPersona, string documento, string nombre, string domicilio,
                             string telefono, string email, EstadoCivil estadoCivil, DateTime fechaNacimiento)
        {
            RequerirAdminOPermiso("GESTIONAR_PERSONAS", "RegistrarPersona");
            ValidarCampos(tipoPersona, documento, nombre, domicilio, email, fechaNacimiento);

            if (_mapperPersona.ObtenerPorDocumento(documento.Trim()) != null)
                throw new BLLException("ERR_PERSONA_DOCUMENTO_DUPLICADO",
                    "Ya existe una persona con el documento '{0}'.", documento);

            var persona = new Persona
            {
                TipoPersona     = tipoPersona,
                Documento       = documento.Trim(),
                Nombre          = nombre.Trim(),
                Domicilio       = domicilio.Trim(),
                Telefono        = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
                Email           = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                EstadoCivil     = estadoCivil,
                FechaNacimiento = fechaNacimiento.Date
            };

            int id = _mapperPersona.Insertar(persona);
            persona.Id = id;

            new VerificadorPersona().RecalcularDVs();

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaPersona,
                $"Persona creada: '{persona.Nombre}' (Doc {persona.Documento}, Id {id}).");

            return id;
        }

        public void Editar(int id, char tipoPersona, string documento, string nombre, string domicilio,
                           string telefono, string email, EstadoCivil estadoCivil, DateTime fechaNacimiento)
        {
            RequerirAdminOPermiso("GESTIONAR_PERSONAS", "EditarPersona");
            ValidarCampos(tipoPersona, documento, nombre, domicilio, email, fechaNacimiento);

            var persona = _mapperPersona.Obtener(id);
            if (persona == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "La persona no existe.");

            var dup = _mapperPersona.ObtenerPorDocumento(documento.Trim());
            if (dup != null && dup.Id != id)
                throw new BLLException("ERR_PERSONA_DOCUMENTO_DUPLICADO",
                    "Ya existe una persona con el documento '{0}'.", documento);

            persona.TipoPersona     = tipoPersona;
            persona.Documento       = documento.Trim();
            persona.Nombre          = nombre.Trim();
            persona.Domicilio       = domicilio.Trim();
            persona.Telefono        = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim();
            persona.Email           = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
            persona.EstadoCivil     = estadoCivil;
            persona.FechaNacimiento = fechaNacimiento.Date;

            _mapperPersona.Editar(persona);

            new VerificadorPersona().RecalcularDVs();

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaPersona,
                $"Persona editada: Id {id} (Doc {persona.Documento}).");
        }

        public void Eliminar(int id)
        {
            RequerirAdminOPermiso("GESTIONAR_PERSONAS", "EliminarPersona");

            var persona = _mapperPersona.Obtener(id);
            if (persona == null)
                throw new BLLException("ERR_PERSONA_NO_EXISTE", "La persona no existe.");

            int unidadesAsociadas = _mapperPersona.ContarUnidadesConPersona(id);
            if (unidadesAsociadas > 0)
                throw new BLLException("ERR_PERSONA_EN_USO",
                    "No se puede eliminar la persona '{0}' porque hay {1} unidad(es) asociada(s).",
                    persona.Nombre, unidadesAsociadas);

            _mapperPersona.Eliminar(persona);

            new VerificadorPersona().RecalcularDVs();

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.AltaPersona,
                $"Persona eliminada: '{persona.Nombre}' (Id {id}).");
        }

        // Patrón para validar emails: parte local + @ + dominio con al menos un punto.
        private static readonly Regex RegexEmail = new Regex(
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
            RegexOptions.Compiled);

        private static void ValidarCampos(char tipoPersona, string documento, string nombre, string domicilio,
                                          string email, DateTime fechaNacimiento)
        {
            if (tipoPersona != 'F' && tipoPersona != 'J')
                throw new BLLException("ERR_PERSONA_TIPO_INVALIDO", "Tipo de persona inválido (esperado 'F' o 'J').");
            if (string.IsNullOrWhiteSpace(documento))
                throw new BLLException("ERR_PERSONA_DATOS_INCOMPLETOS", "Faltan datos obligatorios de la persona.");
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BLLException("ERR_PERSONA_DATOS_INCOMPLETOS", "Faltan datos obligatorios de la persona.");
            if (string.IsNullOrWhiteSpace(domicilio))
                throw new BLLException("ERR_PERSONA_DATOS_INCOMPLETOS", "Faltan datos obligatorios de la persona.");
            if (!string.IsNullOrWhiteSpace(email) && !RegexEmail.IsMatch(email.Trim()))
                throw new BLLException("ERR_PERSONA_EMAIL_INVALIDO",
                    "El email '{0}' no tiene un formato válido.", email);
            if (fechaNacimiento == default(DateTime) || fechaNacimiento > DateTime.Today)
                throw new BLLException("ERR_PERSONA_FECHA_NACIMIENTO_INVALIDA",
                    "La fecha de nacimiento es inválida.");
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
