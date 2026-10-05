using System.ComponentModel;

namespace BE.Enums
{
    public enum TipoCarroceria
    {
        [Description("Sedán")]
        Sedan = 1,

        [Description("Hatchback")]
        Hatchback = 2,

        [Description("SUV")]
        SUV = 3,

        [Description("Pickup")]
        Pickup = 4,

        [Description("Coupe")]
        Coupe = 5,
    }
}
