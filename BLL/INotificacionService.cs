using BE;

namespace BLL
{
    // N03 — CU-CAP-03: avisa por mail a los clientes cuya búsqueda coincide con una unidad que pasó a En venta.
    public interface INotificacionService
    {
        ResultadoNotificacion NotificarUnidad(int idUnidad);
    }
}
