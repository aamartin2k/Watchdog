using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace ProtDev
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Builder.Build();

            if (args.Length > 0 )
            {
                //GC.KeepAlive(mutex);
                Builder.LaunchForm();
            }
            else
            {
                // Modo servicio
                Builder.LaunchConsole();
                
            }

            Builder.Close();

        }
        
      
    }
}
