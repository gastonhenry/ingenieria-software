using System.ComponentModel;

namespace BE.Enums
{
    public enum EstadoCivil
    {
        [Description("Soltero/a")]
        Soltero = 1,

        [Description("Casado/a")]
        Casado = 2,

        [Description("Divorciado/a")]
        Divorciado = 3,

        [Description("Viudo/a")]
        Viudo = 4,

        [Description("Unión convivencial")]
        UnionConvivencial = 5,
    }
}
