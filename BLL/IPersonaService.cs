using BE;
using BE.Enums;
using System;
using System.Collections.Generic;

namespace BLL
{
    public interface IPersonaService
    {
        List<Persona> Listar();
        Persona ObtenerPorId(int id);
        Persona BuscarPorDocumento(string documento);
        List<Persona> BuscarPorDocumentoParcial(string fragmento);
        int Registrar(char tipoPersona, string documento, string nombre, string domicilio,
                      string telefono, string email, EstadoCivil estadoCivil, DateTime fechaNacimiento);
        void Editar(int id, char tipoPersona, string documento, string nombre, string domicilio,
                    string telefono, string email, EstadoCivil estadoCivil, DateTime fechaNacimiento);
        void Eliminar(int id);
    }
}
