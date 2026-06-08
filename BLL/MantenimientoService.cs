using BE;
using BE.Enums;
using HELPERS;
using MPP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BLL
{
    public class MantenimientoService : IMantenimientoService
    {
        private readonly List<IVerificadorEntidad> _verificadores;
        private readonly MapperMantenimiento _mapperMantenimiento;
        private readonly BitacoraService _bitacoraService;

        public MantenimientoService()
        {
            _verificadores = new List<IVerificadorEntidad>
            {
                new VerificadorUsuario(),
                new VerificadorPermiso()
            };
            _mapperMantenimiento = new MapperMantenimiento();
            _bitacoraService = new BitacoraService();
        }

        public string CarpetaBackups
        {
            get
            {
                string carpeta = ConfigurationManager.AppSettings["BackupFolder"];
                if (string.IsNullOrWhiteSpace(carpeta))
                    carpeta = Path.Combine(Path.GetTempPath(), "IngenieriaBackups");
                return carpeta;
            }
        }

        public ResultadoIntegridadGlobal VerificarTodo()
        {
            var global = new ResultadoIntegridadGlobal { Ok = true };
            foreach (var verificador in _verificadores)
            {
                ResultadoIntegridad r = verificador.Verificar();
                if (r.RequiereInicializacion)
                {
                    verificador.RecalcularDVs();
                    r = verificador.Verificar();
                }
                global.PorTabla[verificador.NombreTabla] = r;
                if (!r.Ok) global.Ok = false;
            }

            if (!global.Ok)
                RegistrarErrorIntegridad(global);

            return global;
        }

        public void RecalcularTodo()
        {
            foreach (var verificador in _verificadores)
                verificador.RecalcularDVs();

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Mantenimiento,
                "Recálculo manual de dígitos verificadores ejecutado desde mantenimiento.");
        }

        public string Backup()
        {
            string carpeta = CarpetaBackups;
            Directory.CreateDirectory(carpeta);
            string nombreArchivo = $"ingenieria_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string ruta = Path.Combine(carpeta, nombreArchivo);

            _mapperMantenimiento.Backup(ruta);

            _bitacoraService.Insertar(SesionUsuario.GetInstancia().Usuario, TipoBitacora.Mantenimiento,
                $"Backup creado en mantenimiento: {ruta}");

            return ruta;
        }

        public void Restore(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
                throw new BLLException("ERR_BACKUP_NO_EXISTE", "El archivo de backup '{0}' no existe.", rutaArchivo);

            _mapperMantenimiento.Restore(rutaArchivo);

            _bitacoraService.Insertar(null, TipoBitacora.Mantenimiento,
                $"Base de datos restaurada desde backup: {rutaArchivo}");
        }

        public List<string> ListarBackupsDisponibles()
        {
            string carpeta = CarpetaBackups;
            if (!Directory.Exists(carpeta)) return new List<string>();

            var archivos = Directory.GetFiles(carpeta, "*.bak");
            var lista = new List<string>(archivos);
            lista.Sort((a, b) => File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)));
            return lista;
        }

        public bool AutenticarMantenimiento(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                return false;

            string userEsperado = ConfigurationManager.AppSettings["MaintenanceAdminUser"];
            string hashEsperado = ConfigurationManager.AppSettings["MaintenanceAdminHash"];
            string saltEsperado = ConfigurationManager.AppSettings["MaintenanceAdminSalt"];

            if (string.IsNullOrEmpty(userEsperado) || string.IsNullOrEmpty(hashEsperado) || string.IsNullOrEmpty(saltEsperado))
                return false;

            if (!string.Equals(usuario, userEsperado, StringComparison.OrdinalIgnoreCase))
                return false;

            string hashCalculado = PasswordHasher.HashPassword(password, saltEsperado);
            return string.Equals(hashCalculado, hashEsperado, StringComparison.OrdinalIgnoreCase);
        }

        private void RegistrarErrorIntegridad(ResultadoIntegridadGlobal global)
        {
            try
            {
                var detalles = new List<string>();
                foreach (var par in global.PorTabla)
                {
                    if (par.Value.Ok) continue;
                    var msg = $"Tabla '{par.Key}': ";
                    if (par.Value.FilasInvalidas.Count > 0)
                        msg += $"DVH inválido en filas {string.Join(",", par.Value.FilasInvalidas)}. ";
                    if (par.Value.DVVInvalido)
                        msg += "DVV inválido. ";
                    detalles.Add(msg.Trim());
                }
                _bitacoraService.Insertar(null, TipoBitacora.Error,
                    "Verificación de integridad fallida. " + string.Join(" | ", detalles));
            }
            catch
            {
                // Si la bitácora falla por el mismo problema de integridad/conexión, no la propagamos.
            }
        }
    }
}
