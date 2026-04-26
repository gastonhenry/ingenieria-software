using BE;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class BitacoraService : IBitacoraService
    {
        private readonly MapperBitacora _mapperBitacora;

        public BitacoraService()
        {
            _mapperBitacora = new MapperBitacora();
        }

        public void Insertar(Usuario usuario, BitacoraEnum tipo, string detalle = null)
        {
            var bitacora = new Bitacora(usuario, tipo, detalle);
            _mapperBitacora.Insertar(bitacora);
        }

        public List<Bitacora> Listar(int? tipoId = null, int? usuarioId = null)
        {
            List<Bitacora> bitacoras = _mapperBitacora.Listar(tipoId, usuarioId);
            return bitacoras;
        }
    }
}