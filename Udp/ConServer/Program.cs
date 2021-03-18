using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Monitor.Shared.Heartbeat;
using Monitor.Shared.Interfaces;
using Monitor.Shared.Server;

/*  
    Este servidor realiza una implementación del receptor de Heartbeat.
    El proyecto contiene una referencia a la dll Monitor.Shared.

    Simula el servidor real que se implementa en SrvMonitor. Se usa como herramienta
    para la depuracion y prueba. Se emplea el tipo de aplicación consola para automatizar 
    su ejecucion desde scripts.

 */

namespace ConServerUdp
{
    class Program
    {
        /*  Argumentos de la linea de comando.
           Uso:  svudp  [Ip]  [port]
           
           Argumento           Función
           1  Ip            Direccion Ip del servidor
           2  port          Puerto local, servidor listens
         
            Uso:  svudp  [*] | [D] | [d] | [default] 
           
            Emplea la configuración por defecto, hardcoded.

            El método IniciaServidorInterno() implementa un receptor Udp en código. Se escribió 
           como prototipo. Requiere presencia del archivo HeartBeat.cs, y modificar su propiedad 
           BuildAction de None a Compile.

            El método  IniciaServidorShared() implementa el receptor definido en la biblioteca 
           Monitor.Shared. Se presenta como ejemplo de código. No requiere presencia del archivo HeartBeat.cs
        */

        // declaraciones
        // Udp Socket
        static Socket _skListener;

        const int bufferSize = 52;  //  32;
        static byte[] buffer = new byte[bufferSize];
        const string messg = "Presione Enter para terminar.";
        static string ipad;
        static int  locPort;
        //static ASCIIEncoding Encd = new ASCIIEncoding() ;
        static UTF8Encoding Encd = new UTF8Encoding();

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Simulador de Monitor. Implementa un Servidor UDP.\n\n");

            // Discriminando casos posibles
            // Con argumento default
            if (args.Length == 1 && (args[0] == "*" | args[0] == "d" | args[0] == "D" | args[0] == "default"))
            {
                ipad = "127.0.0.1";
                locPort = 8888;
                //IniciaServidorInterno();
                IniciaServidorShared();

            }   // Con argumentos completos, parsear 
            else if (args != null && args.Length >= 2 && args[0].Length > 8
                                                      && args[1].Length > 3)
            {
                ipad = args[0];
                locPort = int.Parse(args[1]);

                //IniciaServidorInterno();
                IniciaServidorShared();
            }
            else
                ShowHelp();
        }

        #region Servidor Interno
        static private void IniciaServidorInterno()
        {
            _skListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _skListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPAddress locIp = IPAddress.Parse(ipad);
            //EndPoint epLocal = new IPEndPoint(IPAddress.Any, port);
            EndPoint epLocal = new IPEndPoint(locIp, locPort);
            EndPoint epRemote = new IPEndPoint(IPAddress.Any, 0);

            _skListener.Bind(epLocal);  // atendiendo Any Ip, port 8888
            
            _skListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(ReadOp), buffer);

           
    
            Console.WriteLine("Simulador iniciado. Servidor UDP escucha por puerto: " + locPort );
            Console.WriteLine(messg);
            Console.ReadLine();
        }

        static private void ReadOp(IAsyncResult ar)
        {
            string text = String.Empty;

            IPEndPoint ipRemoteEp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint epRemote = (EndPoint)ipRemoteEp;

            try
            {
                string message, ipadr, port;

                if (_skListener.EndReceiveFrom(ar, ref epRemote) > 0)
                {
                    text = Encd.GetString((byte[])ar.AsyncState);
                    Console.WriteLine(text);
                    HeartBeat hb = HeartBeat.CreateHeartBeat(text);

                    ipRemoteEp = (IPEndPoint)epRemote;
                    port = ipRemoteEp.Port.ToString();
                    ipadr = ipRemoteEp.Address.ToString();

                    message = string.Format("Cliente: {0}  IP: {1}  Puerto: {2}  TS: {3}  Serial: {4}", hb.ClientID, ipadr, port, hb.Timestamp, hb.Serial);
                    Console.WriteLine(message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido una excepcion: " + ex.Message);
                throw;
            }

            buffer = new byte[bufferSize];

            if (_skListener != null)
                _skListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(ReadOp), buffer);
        }
        #endregion

        #region Servidor Shared
        static void IniciaServidorShared()
        {

            ICommunicationServer server = new HeartbeatServer(ipad, locPort, Encd);

            server.MessageReceivedEvent += Server_MessageReceivedEvent;
            server.Start();

            Console.WriteLine("Udp Server started.");
            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            server.Stop();
        }

        private static void Server_MessageReceivedEvent(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.ClientId + " message: " + e.Message);
        }

       
        #endregion


        // Muesta ayuda en modo Consola
        static private void ShowHelp()
        {
            string bigMsg = "  Uso:  svudp  [Ip  Port]  \n\n" +
                            " Argumento       Función  \n" +
                            "  Ip             Direccion Ip del servidor\n" +
                            "  Port           Puerto local,  servidor listens \n\n" +
                            "  Uso:  svudp  *  |  D  |  d  |  default \n\n" +
                            "   Emplear configuración por defecto: \n" +
                            "      ipad = 127.0.0.1 \n" +
                            "      port = 8888 \n\n";
                           

           
            Console.Write(bigMsg);
            //Console.WriteLine(messg);
            //Console.ReadLine();
        }
    }
}
