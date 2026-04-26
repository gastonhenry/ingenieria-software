using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class MapperBitacora : Mapper<Bitacora>
    {
        public override int Insertar(Bitacora obj)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Abrir();

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                db.CrearParametro("@UsuarioId", obj.Usuario.Id),
                db.CrearParametro("@Tipo", (int)obj.Tipo),
            };

            if (!string.IsNullOrEmpty(obj.Detalle))
            {
                parametros.Add(db.CrearParametro("@Detalle", obj.Detalle));
            }

            int id = db.LeerEscalar("InsertarBitacora", parametros);
            db.Cerrar();
            return id;
        }

        public override int Eliminar(Bitacora obj)
        {
            throw new NotImplementedException();
        }

        public override List<Bitacora> Listar()
        {
            throw new NotImplementedException();
        }

        public List<Bitacora> Listar(int? tipoId = null, int? usuarioId = null)
        {
            AccesoDB db = AccesoDB.GetInstancia();
            db.Abrir();
            var lista = new List<Bitacora>();

            List<SqlParameter> parametros = new List<SqlParameter>();
            if (tipoId != null)
            {
                parametros.Add(db.CrearParametro("@Tipo", tipoId.Value));
            }

            if (usuarioId != null)
            {
                parametros.Add(db.CrearParametro("@UsuarioId", usuarioId.Value));
            }

            using (SqlDataReader reader = db.Leer("ListarBitacora", parametros))
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido"))
                    };

                    var bitacora = new Bitacora(usuario, (BitacoraEnum)reader.GetInt32(reader.GetOrdinal("Tipo")))
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FechaHora = reader.GetDateTime(reader.GetOrdinal("FechaHora")),
                        Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Detalle"))
                    };

                    lista.Add(bitacora);
                }
            }
            db.Cerrar();
            return lista;
        }

        public override Bitacora Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public override int Editar(Bitacora obj)
        {
            throw new NotImplementedException();
        }
    }
}