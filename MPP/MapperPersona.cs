using BE;
using BE.Enums;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperPersona : Mapper<Persona>
    {
        public override int Insertar(Persona p)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@TipoPersona",     p.TipoPersona.ToString()),
                db.CrearParametro("@Documento",       p.Documento),
                db.CrearParametro("@Nombre",          p.Nombre),
                db.CrearParametro("@Domicilio",       p.Domicilio),
                db.CrearParametro("@Telefono",        p.Telefono),
                db.CrearParametro("@Email",           p.Email),
                db.CrearParametro("@EstadoCivil",     (int)p.EstadoCivil),
                db.CrearParametro("@FechaNacimiento", p.FechaNacimiento)
            };
            return db.LeerEscalar("InsertarPersona", parametros);
        }

        public override int Editar(Persona p)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@PersonaId",       p.Id),
                db.CrearParametro("@TipoPersona",     p.TipoPersona.ToString()),
                db.CrearParametro("@Documento",       p.Documento),
                db.CrearParametro("@Nombre",          p.Nombre),
                db.CrearParametro("@Domicilio",       p.Domicilio),
                db.CrearParametro("@Telefono",        p.Telefono),
                db.CrearParametro("@Email",           p.Email),
                db.CrearParametro("@EstadoCivil",     (int)p.EstadoCivil),
                db.CrearParametro("@FechaNacimiento", p.FechaNacimiento)
            };
            return db.Escribir("EditarPersona", parametros);
        }

        public override int Eliminar(Persona p)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@PersonaId", p.Id) };
            return db.Escribir("EliminarPersona", parametros);
        }

        public override Persona Obtener(int id)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@PersonaId", id) };
            DataTable tabla = db.Leer("ObtenerPersonaPorId", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public Persona ObtenerPorDocumento(string documento)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@Documento", documento) };
            DataTable tabla = db.Leer("ObtenerPersonaPorDocumento", parametros);
            if (tabla.Rows.Count == 0) return null;
            return MapearFila(tabla.Rows[0]);
        }

        public List<Persona> BuscarPorDocumentoLike(string fragmento)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter> { db.CrearParametro("@Fragmento", fragmento) };
            DataTable tabla = db.Leer("BuscarPersonasPorDocumentoLike", parametros);
            var lista = new List<Persona>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public override List<Persona> Listar()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarPersonas");
            var lista = new List<Persona>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public List<Persona> ListarParaVerificacion()
        {
            AccesoDB db = AccesoDB.GetInstancia();
            DataTable tabla = db.Leer("ListarPersonasParaVerificacion");
            var lista = new List<Persona>();
            foreach (DataRow row in tabla.Rows) lista.Add(MapearFila(row));
            return lista;
        }

        public void ActualizarDVH(int personaId, string dvh)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@PersonaId", personaId),
                db.CrearParametro("@DVH",       dvh)
            };
            db.Escribir("ActualizarDVHPersona", parametros);
        }

        public int ContarUnidadesConPersona(int personaId)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var parametros = new List<SqlParameter>
            {
                db.CrearParametro("@PersonaId", personaId)
            };
            return db.LeerEscalar("ContarUnidadesConPersona", parametros);
        }

        private static Persona MapearFila(DataRow row)
        {
            string tipo = (string)row["TipoPersona"];
            return new Persona
            {
                Id              = (int)row["Id"],
                TipoPersona     = string.IsNullOrEmpty(tipo) ? 'F' : tipo[0],
                Documento       = (string)row["Documento"],
                Nombre          = (string)row["Nombre"],
                Domicilio       = row.IsNull("Domicilio") ? "" : (string)row["Domicilio"],
                Telefono        = row.IsNull("Telefono")  ? null : (string)row["Telefono"],
                Email           = row.IsNull("Email")     ? null : (string)row["Email"],
                EstadoCivil     = (EstadoCivil)(int)row["EstadoCivil"],
                FechaNacimiento = (DateTime)row["FechaNacimiento"],
                DVH             = row.IsNull("DVH")       ? null : (string)row["DVH"]
            };
        }
    }
}
