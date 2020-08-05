#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Acciones Directas

    Acciones que no requieren ejecutar el servicio de forma ininterrumpida
*/
#endregion

#region Using
using System;

#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        #region Acciones Sin acceso a la Configuración
        // Ejecuta instalacion del servicio, en modo Consola.
        static private void InstallService()
        {
            Console.WriteLine("Instalar servicio.");
#if DEBUG
            // Empleando compilacion condicional para evitar la instalacion del 
            // servicio mientras se depura.
            return;
#endif

            if (SelfInstallerHelper.IsServiceInstalled())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(SelfInstallerHelper.ServiceName + " ya se encuentra instalado.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Instalando " + SelfInstallerHelper.ServiceName + "\n");

            bool ret = SelfInstallerHelper.InstallMe();
            Console.WriteLine();

            if (ret)
                Console.WriteLine(SelfInstallerHelper.ServiceName + " instalado con éxito.");
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error instalando " + SelfInstallerHelper.ServiceName);
                Console.ResetColor();
            }
            Console.WriteLine();

        }

        // Ejecuta desinstalacion del servicio, en modo Consola.
        static private void UninstallService()
        {
            Console.WriteLine("Desinstalar servicio.");
#if DEBUG
            // Empleando compilacion condicional para evitar la desinstalacion del 
            // servicio mientras se depura.
            return;
#endif
            if (!SelfInstallerHelper.IsServiceInstalled())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(SelfInstallerHelper.ServiceName + " no se encuentra instalado.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Desinstalando " + SelfInstallerHelper.ServiceName + "\n");

            bool ret = SelfInstallerHelper.UninstallMe();
            Console.WriteLine();

            if (ret)
                Console.WriteLine(SelfInstallerHelper.ServiceName + " desinstalado con éxito.");
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error desinstalando " + SelfInstallerHelper.ServiceName);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        // Muestra ayuda en modo Consola
        static private void ShowHelp()
        {
            string bigMsg = " Uso:  Monitor.Service  [arg] \n\n" +
                            " Argumento           Función  \n" +
                            "  /i  /install  \n" +
                            "  -i  -install       Instalar el servicio\n" +
                            "  /u  /uninstall \n" +
                            "  -u  -uninstall     Desinstalar el servicio\n" +
                            "  -a  -add  \n" +
                            "  /a  /add           Registrar nuevo cliente\n" +
                            "  -e  -edit   \n" +
                            "  /e  /edit          Editar datos de cliente\n" +
                            "  -d  -del   \n" +
                            "  /d  /del           Eliminar cliente\n" +
                            "  -s  -sys   \n" +
                            "  /s  /sys           Configurar sistema\n" +
                            "  -c  -console  \n" +
                            "  /c  /console       Ejecutar en modo consola\n" +
                            "  /h  /help  \n" +
                            "  -h  -help          Mostrar ayuda \n\n" +
                            " Uso:  Monitor.Service  [arg] [filename]\n\n" +
                            " Argumento           Función  \n" +
                            "  /im  /import  \n" +
                            "  -im  -import       Importar configuración\n" +
                            "  /ex  /export  \n" +
                            "  -ex  -export       Exportar configuración\n";

            //ToDo Incorporar ayuda de Importar/Exportar
            Console.Clear();
            Console.Write(bigMsg);
            Console.WriteLine();
        }

        #endregion

    }
}
