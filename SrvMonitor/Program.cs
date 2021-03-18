#region Descripción
/*
    Implementa el punto de entrada del sistema.
*/
#endregion

#region Using
using System;
using System.Threading;
using System.Windows.Forms;
#endregion

namespace Monitor.Service
{
    static class Program
    {
        private static Mutex mutex;
        
         [STAThread]
        static void Main(string[] args)
        {

            #region Chequeo de Mutex

            // Garantizar ejecucion de una unica instancia con un mutex.
            const string mess = "El Monitor ya se encuentra en ejecución";
            bool onlyInstance = false;

            mutex = new Mutex(true, "MonitorWatchDog", out onlyInstance);
            if (!onlyInstance)
            {
                if (Environment.UserInteractive)
                {
                    MessageBox.Show(mess);
                }
                else
                {
                    Console.WriteLine(mess);
                }

                return;
            }
            #endregion

            // Where It All Begins...
            if (Environment.UserInteractive)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                GC.KeepAlive(mutex);
                Builder.ModoInteractivo(args);
            }
            else
            {
                Builder.ModoServicio();
            }
            
        } 
    }
}
