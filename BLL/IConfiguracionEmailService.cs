using BE;

namespace BLL
{
    // Cuenta de email saliente de los avisos a clientes (Mantenimiento → Configurar cuenta email).
    public interface IConfiguracionEmailService
    {
        ConfiguracionEmail Obtener();
        void Actualizar(ConfiguracionEmail configuracion);
        // Manda un correo de prueba con los datos indicados (sin guardarlos).
        void EnviarPrueba(ConfiguracionEmail configuracion, string destinatario);
    }
}
