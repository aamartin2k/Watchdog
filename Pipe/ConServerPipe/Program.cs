using System;
using System.Text;
using System.IO.Pipes;
using Monitor.Shared.Heartbeat;


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
        static ASCIIEncoding Encd;
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
                IniciaServidor();
            }
            // Con argumentos completos, parsear 
            else if (args != null && args.Length >= 2 )
            {
                name = args[0];
                maxconn = int.Parse(args[1]);
                IniciaServidor();
            }
            else
                ShowHelp();
        }


        static private void IniciaServidor()
        {
            Console.WriteLine("Simulador iniciado. Servidor Pipe atiende: " + name);
            Console.WriteLine(messg);
            Console.CancelKeyPress += Console_CancelKeyPress;

            Encd = new ASCIIEncoding();
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

                HeartBeat hb = HeartBeat.CreateHeartBeat(buffer);
                message = string.Format("Cliente: {0}  Pipe: {1}  TS: {2}  Serial: {3}", hb.ClientID, name, hb.Timestamp, hb.Serial);
                Console.WriteLine(message);

                _pipeServer.Disconnect();
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            keepLoop = false;
        }

       
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
