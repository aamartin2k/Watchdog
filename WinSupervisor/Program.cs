#region Descripción
/*
    Implementa el punto de entrada de la aplicación.
*/
#endregion

#region Using
using System;
using System.Windows.Forms;
#endregion

namespace Monitor.Supervisor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Where It All Begins...
            Builder.Inicio();
           
        }  
    }
}
