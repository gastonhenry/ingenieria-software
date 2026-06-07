using BE;
using BE.Enums;
using System.Collections.Generic;

namespace BLL
{
    public interface IBitacoraService
    {
        void Insertar(Usuario usuario, TipoBitacora tipo, string detalle = null);
        List<Bitacora> Listar(int? tipoId = null, int? usuarioId = null);
    }
}