using System;
using System.Timers;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Monitor.Shared.Heartbeat;
using Monitor.Shared.Interfaces;
using Monitor.Shared.Client;

/*  
    Este cliente realiza una implementación del emisor de Heartbeat.
    El proyecto contiene una referencia a la dll Monitor.Shared.

    Simula un cliente real que envia Heartbeat periódicamente. Se usa como herramienta
    para la depuracion y prueba. Se emplea el tipo de aplicación consola para automatizar su ejecucion desde
    scripts.
 */

namespace ConClienteUdp
{
    class Program
    {
        /*  Argumentos de la linea de comando.
        *  Uso:  cudp  [Ip] [Rport] [Lport] [inter] 
        *  
        *  Argumento           Función
        *  1  Ip            Direccion Ip del servidor
        *  2  Rport         Puerto Remoto, servidor listen
        *  3  Lport         Puerto local,  cliente binds
        *  4  inter         Intervalo entre eventos - en segundos 
        *  
        *   Uso:  cudp  [*] | [D] | [d] | [default] 
        *  
        *   Emplea la configuracion por defecto, hardcoded.
        */

        /* 
            Primer paso de implementación:
              Los metodos IniciaClienteDebug y EnviaHeartbeatDebug no emplean sockets, se limitan a dar salida por consola. 
            se emplean en la depuracion de los argumentos de la linea de comandos.
            
            Segundo paso de implementación:
              Los metodos IniciaCliente y EnviaHeartbeat implementan el socket y el timer declarados a nivel de clase. 
              
            Tercer paso de implementación:
              El método  IniciaClienteShared() implementa el cliente definido en la biblioteca 
             Monitor.Shared.Se presenta como ejemplo de código. Con esta implementacion todos los 
             objetos declarados para implementar localmente son innecesarios. (Socket, Timer)
         */

        static Socket _skListener;
        static Timer _timer;
        const string messg = "Presione Enter para terminar.";
        static string ipad;
        static int remPort, locPort, interv;
        //static ASCIIEncoding Encd;
        static Encoding _enc = new UTF8Encoding();

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Simulador de Proceso a Monitorear. Implementa un Cliente UDP.\n\n");

            // Discriminando casos posibles
           
            // Con argumento default
            if (args.Length == 1 && (args[0] == "*" | args[0] == "d" | args[0] == "D" | args[0] == "default"))
            {
                ipad = "127.0.0.1";
                remPort = 8888;
                locPort = 9090;
                interv = 3;

                goto EjecutarCliente;
            }
            // Con argumentos completos, parsear 
            else if (args != null && args.Length >= 4 && args[0].Length > 8
                                                 && args[1].Length > 3
                                                 && args[2].Length > 3
                                                 && args[3].Length >= 1)
            {
                ipad = args[0];
                remPort = int.Parse(args[1]);
                locPort = int.Parse(args[2]);
                interv = int.Parse(args[3]);

                goto EjecutarCliente;
            }
            else
                ShowHelp();
            return;

        EjecutarCliente: ;
            IniciaClienteShared();
            //IniciaCliente();
            //IniciaClienteDebug();
        }

        #region Primer paso de implementación
        // Generar HB y mostrarlo por consola, sin transmitir.
        static private void IniciaClienteDebug()
        {
            Console.Clear();
            Console.WriteLine("Simulador de Heartbeat. Cliente UDP. \n");

            Console.WriteLine("Configuracion vigente: ");
            Console.WriteLine("  Dirección IP: " + ipad);
            Console.WriteLine(" Puerto Remoto: " + remPort);
            Console.WriteLine("  Puerto Local: " + locPort);
            Console.WriteLine("     Intervalo: " + interv);
            Console.WriteLine();

            // Iniciando Timer
            _timer = new Timer();
            _timer.Interval = interv * 1000;
            _timer.AutoReset = true;
            _timer.Elapsed += new ElapsedEventHandler(EnviaHeartbeatDebug);
            _timer.Start();

            Console.WriteLine("Simulador iniciado.");
            Console.WriteLine(messg);
            Console.ReadLine();

        }

        static private void EnviaHeartbeatDebug(object sender, ElapsedEventArgs e)
        {
            var text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("Send... " + text);
        }

        #endregion

        #region Segundo paso de implementación
        // Iniciar cliente y timer para enviar Heartbeat
        static private void IniciaCliente()
        {
            try
            {
                 // Iniciando Socket
                _skListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _skListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                EndPoint epLocal = new IPEndPoint(IPAddress.Any, locPort);
                _skListener.Bind(epLocal);

                IPAddress remIp = IPAddress.Parse(ipad);
                EndPoint epRemote = new IPEndPoint(remIp, remPort);
                _skListener.Connect(epRemote);

                // Iniciando Timer
                _timer = new Timer();
                _timer.Interval = interv * 1000;
                _timer.AutoReset = true;
                _timer.Elapsed += new ElapsedEventHandler(EnviaHeartbeat);
                _timer.Start();

               
                Console.WriteLine("Simulador iniciado.");
                Console.WriteLine(messg);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                 Console.WriteLine("Ha ocurrido una excepcion: " + ex.Message );
                throw;
            }
        }

        // Ejecuta periodicamente el envío de heartbeat
        static private void EnviaHeartbeat(object sender, ElapsedEventArgs e)
        {
            var text = _enc.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            _skListener.Send(text);
        }
        #endregion

        #region Tercer paso de implementación
        // El método  IniciaClienteShared() implementa el cliente definido en la biblioteca 
        // Monitor.Shared.Se presenta como ejemplo de código.
        static private void IniciaClienteShared()
        {
            Console.WriteLine("Udp Client started.");


            HeartBeatGenerator hbgen = new HeartBeatGenerator
            {
                ClientID = "UdpCIdn02",
                UsarSerialHB = false
            };

            IHeartbeatSender sender = new HeartbeatClient(ipad, remPort, locPort, _enc, interv, hbgen);
            sender.StartTimer();

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            sender.StopTimer();
        }

        #endregion
        // Muesta ayuda en modo Consola
        static private void ShowHelp()
        {
            string bigMsg = "  Uso:  cudp  [Ip  Rport  Lport  Inter]  \n\n" +
                            " Argumento       Función  \n" +
                            "  Ip             Direccion Ip del servidor\n" +
                            "  Rport          Puerto Remoto, servidor listen\n\n" +
                            "  Lport          Puerto local,  cliente binds \n" +
                            "  Inter          Intervalo entre eventos - en segundos\n\n" +
                            "  Uso:  cudp  *  |  D  |  d  |  default \n\n" +
                            "   Emplear configuracion por defecto: \n" + 
                            "      ipad = 127.0.0.1 \n" +
                            "   remPort = 8888 \n" +
                            "   locPort = 8003  \n " + 
                            "    interv = 12\n\n";
                           
            Console.Clear();
            Console.WriteLine("Simulador de Heartbeat. Cliente UDP.\n\n");
            Console.Write(bigMsg);
            //Console.WriteLine(messg);
            //Console.ReadLine();
        }

    }
}
