using System;
using System.Text;
using System.IO.Pipes;
using Monitor.Shared.Interfaces;
using Monitor.Shared.Server;

/*  
    Este servidor realiza una implementación del receptor de Heartbeat.
    El proyecto contiene una referencia a la dll Monitor.Shared.

    Simula el servidor real que se implementa en SrvMonitor. Se usa como herramienta
    para la depuracion y prueba. Se emplea el tipo de aplicación consola para automatizar 
    su ejecucion desde scripts.

 */
namespace ConServerPipe
{
    class Program
    {
        /*  Argumentos de la linea de comando.
        *  Uso:  svpip  [Name  MaxConn]
        *  
        *  Argumento           Función
        *  1  Name            Nombre del pipe
        *  2  MaxConn         Cantidad maxima de conexiones

        *  
        *   Uso:  svudp  [*] | [D] | [d] | [default] 
        *  
        *   Emplea la configuración por defecto, hardcoded.
        */

        // declaraciones
        // Pipe server
        static NamedPipeServerStream _pipeServer;

        const int bufferSize = 52;  
        static byte[] buffer;
        const string messg = "Presione CTRL+C para terminar.";
        static string name;
        static int maxconn;
        //static ASCIIEncoding Encd = new ASCIIEncoding() ;
        static UTF8Encoding _enc = new UTF8Encoding();
        static bool keepLoop;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Simulador de Monitor. Implementa un Servidor Pipe.\n\n");

            // Discriminando casos posibles
            // Con argumento default
            if (args.Length == 1 && (args[0] == "*" | args[0] == "d" | args[0] == "D" | args[0] == "default"))
            {
                name = "wdpipe";
                maxconn = 12;

                //IniciaServidorInterno();
                IniciaServidorShared();
            }
            // Con argumentos completos, parsear 
            else if (args != null && args.Length >= 2 )
            {
                name = args[0];
                maxconn = int.Parse(args[1]);
                
                //IniciaServidorInterno();
                IniciaServidorShared();
            }
            else
                ShowHelp();
        }

        #region Servidor Interno
        static private void IniciaServidorInterno()
        {
            Console.WriteLine("Simulador iniciado. Servidor Pipe atiende: " + name);
            Console.WriteLine(messg);
            Console.CancelKeyPress += Console_CancelKeyPress;

            _pipeServer = new NamedPipeServerStream(name, PipeDirection.In, maxconn, PipeTransmissionMode.Message, PipeOptions.Asynchronous, bufferSize, bufferSize);

            int numBytes = 0;
            keepLoop = true;

            while (keepLoop)
            {
                string message = String.Empty;
                buffer = new byte[bufferSize];

                _pipeServer.WaitForConnection();
               
                do
                {
                    numBytes = _pipeServer.Read(buffer, 0, bufferSize);
                    //Console.WriteLine("LEidos: " + numBytes.ToString());
                } while (numBytes != 0);

                message = _enc.GetString(buffer);
                Console.WriteLine(message);

                //HeartBeat hb = HeartBeat.CreateHeartBeat(buffer);
                //message = string.Format("Cliente: {0}  Pipe: {1}  TS: {2}  Serial: {3}", hb.ClientID, name, hb.Timestamp, hb.Serial);
                //Console.WriteLine(message);

                _pipeServer.Disconnect();
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            keepLoop = false;
        }

        #endregion

        #region Servidor Shared
        static void IniciaServidorShared()
        {
            string[] pipes = new string[]
           {
                "wdpipe",      //  pipe por defecto de clientes consola
                "wdpipewf",    //  pipe por defecto de clientes winform
                "Client3",
                "Client4",
                "Client5",
                "Client6",
                "Client7",
                "Client8"
           };

            ICommunicationServer server = new HeartbeatServer(pipes, _enc);

            server.ClientConnectedEvent += Server_ClientConnectedEvent;
            server.ClientDisconnectedEvent += Server_ClientDisconnectedEvent;
            server.MessageReceivedEvent += Server_MessageReceivedEvent;

            server.Start();

            Console.WriteLine("Pipe Server started.");
            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            server.Stop();
        }


        private static void Server_MessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ClientId + " message: " + e.Message);
        }

        private static void Server_ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine(e.ClientId + " Disconnected.");
        }

        private static void Server_ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine(e.ClientId + " Connected.");
        }


        #endregion

        static private void ShowHelp()
        {
            string bigMsg = "  Uso:  svpip  [Name  MaxConn]  \n\n" +
                            " Argumento       Función  \n" +
                            "  Name           Nombre del pipe. \n" +
                            "  MaxConn        Maximo de conexiones. \n\n" +
                            "  Uso:  svpip  *  |  D  |  d  |  default \n\n" +
                            "   Emplear configuración por defecto: \n" +
                            "      Name = wdpipe \n" +
                            "   MaxConn = 12 \n\n";

            Console.Write(bigMsg);
            //Console.WriteLine(messg);
            //Console.ReadLine();
        }
    }
}
