namespace BE
{
    public class PermisoIndividual : Permiso
    {
        public override string ToString()
        {
            string desc = string.IsNullOrWhiteSpace(Descripcion) ? "" : " — " + Descripcion;
            return "[Permiso] " + Codigo + desc;
        }
    }
}
