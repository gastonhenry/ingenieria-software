using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ResultadoIntegridadGlobal resultado = VerificarIntegridadInicial();
            Application.Run(new FormLogin(resultado));
        }

        private static ResultadoIntegridadGlobal VerificarIntegridadInicial()
        {
            try
            {
                return new MantenimientoService().VerificarTodo();
            }
            catch
            {
                var fallback = new ResultadoIntegridadGlobal { Ok = false };
                fallback.PorTabla["__startup__"] = new ResultadoIntegridad
                {
                    Ok = false,
                    DVVInvalido = true
                };
                return fallback;
            }
        }
    }
}
