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
            };

            int id = db.LeerEscalar("InsertarBitacora", parametros);
            db.Cerrar();
            return id;
        }

        public override int Editar(Bitacora obj)
        {
            int filas;
            try
            {
                AccesoDB db = AccesoDB.GetInstancia();
                db.Abrir();

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    db.CrearParametro("@BitacoraId", obj.Id),
                };

                if (obj.FechaHoraFin.HasValue)
                {
                    parametros.Add(db.CrearParametro("@FechaHoraFin", obj.FechaHoraFin.Value));
                }

                filas = db.Escribir("EditarBitacora", parametros);
                db.Cerrar();
            }
            catch (Exception)
            {
                filas = 1;
            }
            return filas;
        }

        public override int Eliminar(Bitacora obj)
        {
            throw new NotImplementedException();
        }

        public override List<Bitacora> Listar()
        {
            throw new NotImplementedException();
        }

        public override Bitacora Obtener(int id)
        {
            throw new NotImplementedException();
        }
    }
}