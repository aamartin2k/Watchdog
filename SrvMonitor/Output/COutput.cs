using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMGS.Application.Utils.Log;
using Monitor.Shared;
using System.Diagnostics;

namespace Monitor.Service.Output
{
    internal class COutput
    {
        #region Utileria Output

        //Utilitarios para escritura de mensajes en consola manteniendo el mensaje base
        private static int origRow;
        private static int origCol;
        private static int baseRow;
        private static object refLock = new object();

        // Diferenciar mensajes de servicio en modo consola de los demas: install, uninstall, interactive etc.
        private static bool writeBaseLine = false;

        private const string baseMsg = "<presione Enter para terminar el servicio...>                                                                   ";
        private const string baseClr = "                                                                                                                ";

        internal static void Output(string newMess)
        {
            Output(newMess, TraceEventType.Information);
        }

        internal static void Output(string newMess, TraceEventType type)
        {
            // Empeando compilacion condicional para obviar este metodo 
            //en caso de ejecutarse Unit Test.
#if TEST
            return;
#endif
            if (Environment.UserInteractive)
            {
                // Ignorar mensajes verbose
                if (!OutputVerbose && (type == TraceEventType.Verbose))
                    return;

                WriteAtBase(newMess, type);
                //DEBUG
                // LA ejecucion de Test da error al acceder a Log
                Log.WriteEntry(type, newMess);

                //DEBUG
                // Debe ser en modo servicio
                SendRemoteText(newMess, type);
            }
            else
            {
                // use log
                Log.WriteEntry(type, newMess);

            }
        }


        private static void WriteAtBase(string newMess, TraceEventType type)
        {

            ConsoleColor ccolor;
            lock (refLock)
            {

                ccolor = Functions.GetConsoleColor(type);

                if (baseRow > Console.BufferHeight)
                {
                    Console.Clear();
                    baseRow = 1;
                }

                if (writeBaseLine)
                {
                    WriteAt(baseClr, 0, baseRow);
                    Console.ForegroundColor = ccolor;
                    WriteAt(newMess, 1, baseRow);
                    Console.ResetColor();
                    baseRow++;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    WriteAt(baseMsg, 0, baseRow);
                    Console.ResetColor();
                }
                else
                {
                    WriteAt(baseClr, 0, baseRow);
                    Console.ForegroundColor = ccolor;  //
                    WriteAt(newMess, 1, baseRow);
                    Console.ResetColor();    //
                    baseRow++;
                }
            }
        }

        private static void WriteAt(string s, int col, int row)
        {
            try
            {

                Console.SetCursorPosition(origCol + col, origRow + row);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
