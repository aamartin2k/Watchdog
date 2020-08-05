#region Descripción
/*
    Implementa funcionalidad para la instalación del servicio.
*/
#endregion

#region Using
using System.Linq;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
#endregion

namespace Monitor.Service
{
    /// <summary>
    /// Implementa funcionalidad para la instalación del servicio.
    /// </summary>
    [RunInstaller(true)]
    public class SelfInstallerHelper : Installer
    {
        #region Propiedades del servicio
        // Nombre para identificar el servicio en el Administrador de Servicios de Windows.
        private static readonly string _serviceName = "AppMonitor";
        // Nombre para mostrar el servicio en el Administrador de Servicios de Windows.
        private static readonly string _serviceDisplayName = "Servicio de monitoreo de aplicaciones";
        // descripción del servicio en el Administrador de Servicios de Windows.
        private static readonly string _serviceDescription = "Supervisa las aplicaciones que emplean el modelo Watchdog para indicar su funcionamiento. Si la aplicación deja de funcionar, se intenta su reinicio.";

        internal static string ServiceName
        { get { return _serviceName; } }

        internal static string ServiceDisplayName
        { get { return _serviceDisplayName; } }

        #endregion

        #region Instalación del servicio

        private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Indica si el servicio especificado se encuentra ejecutándose.
        /// </summary>
        /// <returns>True si el servicio está activo, de lo contrario False.</returns>
        internal static bool IsServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == ServiceName);
        }

        /// <summary>
        /// Instala el servicio.
        /// </summary>
        /// <returns>True si el servicio se instala con éxito, de lo contrario False.</returns>
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Desinstala el servicio.
        /// </summary>
        /// <returns>True si el servicio se desinstala con éxito, de lo contrario False.</returns>
        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Constructor del componente de instalación Installer

        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public SelfInstallerHelper()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // Estableciendo privilegios del servicio.
            //  Modificando estas propiedades se puede instalar el servicio con otras credenciales
            //  distintas de LocalSystem.
            processInstaller.Account = ServiceAccount.LocalSystem;
            //processInstaller.Username = "user";
            //processInstaller.Password = "password";
            //processInstaller.HelpText = "Texto de ayuda";


            serviceInstaller.ServiceName = _serviceName;
            serviceInstaller.DisplayName = _serviceDisplayName;
            serviceInstaller.Description = _serviceDescription;
            serviceInstaller.StartType = ServiceStartMode.Automatic;


            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);

        }
        #endregion

    }
}