using System;
using System.Windows.Forms;

/*  
    Este cliente realiza una implementación del emisor de Heartbeat.
    El proyecto contiene una referencia a la dll Monitor.Shared.

    Simula un cliente real que envia Heartbeat periódicamente. Se usa como herramienta
    para la depuracion y prueba. Se emplea el tipo de aplicación winform para su ejecucion manual.

 */
namespace WinClienteUdp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
