using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IBitacoraService
    {
        void Insertar(Usuario usuario, BitacoraEnum tipo, string detalle = null);
        List<Bitacora> Listar(int? tipoId = null, int? usuarioId = null);
    }
}