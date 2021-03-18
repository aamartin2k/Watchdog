using System;
using System.Collections.Generic;
using System.Timers;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Monitor.Shared.Heartbeat;

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
         *  Primer paso de implementación:
         *    Los metodos IniciaClienteDebug y EnviaHeartbeatDebug no emplean sockets, se limitan a dar salida por consola. 
         *  se emplean en la depuracion de los argumentos de la linea de comandos.
         *  
         *  Segundo paso de implementación:
         *    Los metodos IniciaCliente y EnviaHeartbeat implementan el socket y el timer declarados a nivel de clase. 
         *    
         *  Tercer paso de implementación:
         *    El metodo IniciaClienteCodigoFuente emplea una implementacion externa en codigo fuente, dada en
         *   la clase HbSenderUdp.
         */
        static Socket _skListener;
        static Timer _timer;
        const string messg = "Presione Enter para terminar.";
        static string ipad;
        static int remPort, locPort, interv;
        static ASCIIEncoding Encd;


        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Simulador de Proceso a Monitorear. Implementa un Cliente UDP.\n\n");

            // Discriminando casos posibles
            // Sin argumentos, ejecucion automatica con valores por defecto
            //if (args.Length == 0 )
            //{
            //    ipad = "127.0.0.1";
            //    remPort = 8888;
            //    locPort = 50200 ;
            //    interv = 25;

            //    goto EjecutarCliente;
            //}

            // Con argumento default
            if (args.Length == 1 && (args[0] == "*" | args[0] == "d" | args[0] == "D" | args[0] == "default"))
            {
                ipad = "127.0.0.1";
                remPort = 8888;
                locPort = 8003;
                interv = 17;

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
            IniciaClienteCodigoFuente();
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

                Encd = new ASCIIEncoding();

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
            var text = Encd.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            _skListener.Send(text);
        }
        #endregion

        #region Tercer paso de implementación

        static private void IniciaClienteCodigoFuente()
        {
            HeartBeatGenerator.UsarSerialHB = false;
            HeartBeatGenerator.ClientID = null;
            HbSenderUdp.IniciarHeartbeat(ipad, remPort, locPort, interv);

            Console.WriteLine("Simulador iniciado.");
            Console.WriteLine(messg);
            Console.ReadLine();

            HbSenderUdp.DetenerHeartbeat();
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
