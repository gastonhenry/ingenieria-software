using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MPP
{
    public class MapperBitacora : Mapper<Bitacora>
    {
        public override int Insertar(Bitacora obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                db.CrearParametro("@Tipo", (int)obj.Tipo),
            };

            if (obj.Usuario != null)
                parametros.Add(db.CrearParametro("@UsuarioId", obj.Usuario.Id));

            if (!string.IsNullOrEmpty(obj.Detalle))
                parametros.Add(db.CrearParametro("@Detalle", obj.Detalle));

            return db.LeerEscalar("InsertarBitacora", parametros);
        }

        public List<Bitacora> Listar(int? tipoId = null, int? usuarioId = null)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            var lista = new List<Bitacora>();

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (tipoId != null)
                parametros.Add(db.CrearParametro("@Tipo", tipoId.Value));

            if (usuarioId != null)
                parametros.Add(db.CrearParametro("@UsuarioId", usuarioId.Value));

            DataTable tabla = db.Leer("ListarBitacora", parametros);

            foreach (DataRow row in tabla.Rows)
            {
                Usuario usuario = row.IsNull("UsuarioId")
                    ? new Usuario { Id = 0, Username = "Anónimo", Nombre = "Anónimo", Apellido = string.Empty, Email = "N/A" }
                    : new Usuario
                    {
                        Id = (int)row["UsuarioId"],
                        Username = (string)row["Username"],
                        Nombre = (string)row["Nombre"],
                        Apellido = (string)row["Apellido"],
                        Email = (string)row["Email"]
                    };

                var bitacora = new Bitacora(usuario, (TipoBitacora)(int)row["Tipo"])
                {
                    Id = (int)row["Id"],
                    FechaHora = (DateTime)row["FechaHora"],
                    Detalle = row.IsNull("Detalle") ? null : (string)row["Detalle"]
                };

                lista.Add(bitacora);
            }

            return lista;
        }
    }
}