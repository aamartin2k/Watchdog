#region Descripción
/*
    Constantes y defaults.
 */
#endregion


#region Using
using System;
using System.Diagnostics;
using System.Drawing;
#endregion


namespace Monitor.Shared
{
    
    public class Constants
    {
        public const int UdpServerPort = 8888;
        public const int ZyanServerPort = 9090;

        public const int TimeoutStartRestart = 90;
        public const int RestartAttemps = 2;

        public const string ServerIpAdr = "127.0.0.1";
        public const string ZyanServerName = "WDMonitor";
        public const string ZyanServerUrl = "tcpex://localhost:9090/WDMonitor";

        public const string PipeServer = ".";
        public const string PipeName = "wdpipe";

        public const char EndpointSeparator = ':';

        //public const string ServerUrl = "tcpex://dc0cfg:9090/WDMonitor";
        //public const string ServerUrl = "tcpex://alarmcenterapp:9090/WDMonitor";

        // intentos de conexion que realiza el cliente
        public const int ConnectionAttempts = 5;

        // Intervalo por defecto para enviar heartbeat, en segundos
        public const int DefaultInterval = 60;

        // Timeout por defecto para conectar el cliente pipe, en minutos
        public const int DefaultPipeTimeout = 3;
    }

    static public class Functions
    {
        static public ConsoleColor GetConsoleColor(TraceEventType type)
        {
            ConsoleColor ccolor;

            switch (type)
            {
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    ccolor = ConsoleColor.Red;
                    break;

                case TraceEventType.Warning:
                    ccolor = ConsoleColor.Yellow;
                    break;

                case TraceEventType.Verbose:
                    ccolor = ConsoleColor.Cyan;
                    break;

                case TraceEventType.Information:
                case TraceEventType.Resume:
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Transfer:
                default:
                    ccolor = ConsoleColor.Gray;
                    break;

                
            }

            return ccolor;
        }

        static public Color GetTextColor(TraceEventType type)
        {
            ConsoleColor ccolor = GetConsoleColor(type);
            Color tcolor = Color.Gray;

            switch (ccolor)
            {
                case ConsoleColor.Black:
                    tcolor = Color.Black;
                    break;

                case ConsoleColor.DarkBlue:
                    break;
                case ConsoleColor.DarkGreen:
                    break;
                case ConsoleColor.DarkCyan:
                    break;
                case ConsoleColor.DarkRed:
                    break;
                case ConsoleColor.DarkMagenta:
                    break;
                case ConsoleColor.DarkYellow:
                    break;
                case ConsoleColor.DarkGray:
                    break;
                case ConsoleColor.Blue:
                    break;
                case ConsoleColor.Green:
                    break;
                case ConsoleColor.Cyan:
                    tcolor = Color.Cyan;
                    break;

                case ConsoleColor.Red:
                    tcolor = Color.Red;
                    break;

                case ConsoleColor.Magenta:
                    break;
                case ConsoleColor.Yellow:
                    tcolor = Color.Yellow;
                    break;

                case ConsoleColor.White:
                    tcolor = Color.White;
                    break;

                case ConsoleColor.Gray:
                default:
                    tcolor = Color.Gray;
                    break;
            }

            return tcolor;
        }
    }

}
