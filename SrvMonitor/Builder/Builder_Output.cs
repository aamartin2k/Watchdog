#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Utileria Output
*/
#endregion

#region Using
using AMGS.Application.Utils.Log;
using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
#endregion


namespace Monitor.Service
{   /// <summary>
    /// Implementa escritura de mensajes en consola manteniendo el mensaje base.
    /// Parcial.
    /// </summary>
    static partial class Builder
    {
        #region Utileria Consola Remota

        // Controla el mensaje SupervisorClientLogEvent enviado por ZyanServer 
        // Mantiene estado de conexion de cliente en bool flag, si hay cliente conectado (true)
        // se 

        static private void SendRemoteTextCache(SupervisorClientRequestConsoleText req)
        {
            // Se ha conectado un cliente, si existe texto en cache, enviar y anular el cache
            if (_texCache != null)
            {
                MessageBus.Send(new RemoteConsoleText(_texCache));
            }

            _texCache = null;
        }

        static private void ClientLogonEvent(SupervisorClientLogEvent req)
        {
            SupervisorOnLine = (req.EventType == SupervisorLoginType.Logon) ? true : false;

            if (!SupervisorOnLine)
            {
                // Se ha desconectado un cliente, crear nuevo cache para almacenar texto
                //int size = 12 * 1024;   // 12288   12 KB;
                //int maxSize = 4 * size; // 49152   50 KB;

                //_texCache = new StringBuilder(size, maxSize);

                _texCache = new List<RemoteConsoleText>();
            }
        }

        static private void SendRemoteText(string text, TraceEventType type)
        {
            try
            {
                if (SupervisorOnLine)
                {
                    // Enviar texto a ZyanServer para que lo reenvie a Supervisor conectado
                    MessageBus.Send(new RemoteConsoleText(text, type));
                }
                else
                {
                    // guardar texto en cache
                    //_texCache.AppendLine(text);
                    _texCache.Add(new RemoteConsoleText(text, type));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        #endregion

        #region Utileria Output

        internal static void Output(string newMess)
        {
            Output(newMess, TraceEventType.Information);
        }

        internal static void Output(string newMess, TraceEventType type)
        {
            _messageOutput.Write(newMess, type);
        }


        ////Utilitarios para escritura de mensajes en consola manteniendo el mensaje base
        //private static int origRow;
        //private static int origCol;
        //private static int baseRow;
        //private static object refLock = new object();

        // Diferenciar mensajes de servicio en modo consola de los demas: install, uninstall, interactive etc.
        //private static bool writeBaseLine = false;

        //private const string baseMsg = "<presione Enter para terminar el servicio...>                                                                   ";
        //private const string baseClr = "                                                                                                                ";
        /*
       
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
        */

#endregion
    }
}
