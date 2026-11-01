using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UI
{
    // N03 — textos de presentación compartidos por las pantallas de captación.
    internal static class TextosCaptacion
    {
        // Resumen legible de los criterios: "SUV, Pickup · Toyota (Hilux), Jeep · 2018–2021 · hasta $ 18.000.000".
        public static string DescribirBusqueda(BusquedaCliente b, Func<string, string, string> tr)
        {
            var partes = new List<string>();
            if (b.Carrocerias.Count > 0)
                partes.Add(string.Join(", ", b.Carrocerias.Select(c => c.GetDescripcionTraducida())));
            if (b.Marcas.Count > 0)
                partes.Add(string.Join(", ", b.Marcas.Select(m =>
                {
                    var modelos = b.Modelos.Where(mo => mo.Marca != null && mo.Marca.Id == m.Id).Select(mo => mo.Nombre).ToList();
                    return modelos.Count > 0 ? $"{m.Nombre} ({string.Join(", ", modelos)})" : m.Nombre;
                })));
            if (b.AnioDesde.HasValue && b.AnioHasta.HasValue) partes.Add($"{b.AnioDesde}–{b.AnioHasta}");
            else if (b.AnioDesde.HasValue) partes.Add(string.Format(tr("busquedaAnioDesde", "desde {0}"), b.AnioDesde));
            else if (b.AnioHasta.HasValue) partes.Add(string.Format(tr("busquedaAnioHasta", "hasta {0}"), b.AnioHasta));
            if (b.PrecioMaximo.HasValue)
                partes.Add(string.Format(tr("busquedaPrecioMax", "hasta {0}"), b.PrecioMaximo.Value.ToString("C0", CultureInfo.CurrentCulture)));
            return string.Join(" · ", partes);
        }

        public static string Precio(decimal? precio, Func<string, string, string> tr) =>
            precio.HasValue ? precio.Value.ToString("C0", CultureInfo.CurrentCulture) : tr("precioAConsultar", "Precio a consultar");
    }
}
