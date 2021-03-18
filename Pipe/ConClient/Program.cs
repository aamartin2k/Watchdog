using System;
using System.Timers;
using System.Text;
using System.IO.Pipes;
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

namespace ConClientPipe
{
    class Program
    {
        /*  Argumentos de la linea de comando.
        *  Uso:  cpip  [Server  Pipe  Inter]
        *  
        *  Argumento           Función
        *  1  Server           Nombre del Server
        *  2  Pipe             Nombre del Pipe
        *  3  Inter            Intervalo entre eventos - en segundos 
        *  
        *   Uso:  cpip  [*] | [D] | [d] | [default] 
        *  
        *   Emplea la configuracion por defecto, hardcoded.
        */

        // Declaraciones
        static NamedPipeClientStream _pipeClient;
        static Timer _timer;
        const string messg = "Presione Enter para terminar.";
        static string server;
        static string pipe;
        static int interv;
        static Encoding _enc = new UTF8Encoding();


        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Simulador de Proceso a Monitorear. Implementa un Cliente Pipe.\n\n");

            // Discriminando casos posibles
            // Con argumento default
            if (args.Length == 1 && (args[0] == "*" | args[0] == "d" | args[0] == "D" | args[0] == "default"))
            {
                server = ".";
                pipe = "wdpipe";
                interv = 3;

                goto EjecutarCliente;
            }
            // Con argumentos completos, parsear 
            else if (args != null && args.Length >= 3 )
            {
                server = args[0];
                pipe = args[1];
                interv = int.Parse(args[2]);

                goto EjecutarCliente;
            }
            else
                ShowHelp();
            return;

            EjecutarCliente:;
            IniciaClienteShared();
            //IniciaCliente();
            //IniciaClienteDebug();
        }

        #region Primer paso de implementación
        // Generar HB y mostrarlo por consola, sin transmitir.
        static private void IniciaClienteDebug()
        {
            Console.Clear();
            Console.WriteLine("Simulador de Heartbeat. Cliente Pipe. \n");

            Console.WriteLine("Configuracion vigente: ");
            Console.WriteLine("      Server: " + server);
            Console.WriteLine(" Pipe Remoto: " + pipe);
            Console.WriteLine("   Intervalo: " + interv);
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
                
                // Iniciando Timer
                _timer = new Timer();
                _timer.Interval = interv * 1000;
                _timer.AutoReset = true;
                _timer.Elapsed += new ElapsedEventHandler(EnviaHeartbeat);
                _timer.Start();

                //Encd = new ASCIIEncoding();

                Console.WriteLine("Simulador iniciado.");
                Console.WriteLine(messg);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido una excepcion: " + ex.Message);
                throw;
            }
        }

        // Ejecuta periodicamente el envío de heartbeat
        static private void EnviaHeartbeat(object sender, ElapsedEventArgs e)
        {
            // Iniciando Pipe
            using (_pipeClient = new NamedPipeClientStream(server, pipe, PipeDirection.Out))
            {
                _pipeClient.Connect(3000);

                var text = _enc.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                _pipeClient.Write(text, 0, text.Length);

                _pipeClient.Close();
            }
        }
        #endregion

        #region Tercer paso de implementación
        
        static private void IniciaClienteShared()
        {
            HeartBeatGenerator hbgen = new HeartBeatGenerator
            {
                ClientID = "PClient1",
                UsarSerialHB = true
            };

            IHeartbeatSender sender = new HeartbeatClient(server, pipe, 3, _enc, interv, hbgen);
            sender.StartTimer();

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            sender.StopTimer();
        }
        
        #endregion

        // Muestra ayuda en modo Consola
        static private void ShowHelp()
        {
            string bigMsg = "  Uso:  cpip  [Server  Pipe  Inter]  \n\n" +
                            " Argumento       Función  \n" +
                            "  Server           Nombre del servidor\n" +
                            "  Pipe             Nombre del Pipe\n" +
                            "  Inter          Intervalo entre eventos - en segundos\n\n" +
                            "  Uso:  cudp  *  |  D  |  d  |  default \n\n" +
                            "   Emplear configuracion por defecto: \n" +
                            "    Server = . \n" +
                            "      Pipe = wdpipe \n" +
                            "    interv = 12\n\n";

            Console.Clear();
            Console.WriteLine("Simulador de Heartbeat. Cliente Pipe.\n\n");
            Console.Write(bigMsg);
            //Console.WriteLine(messg);
            //Console.ReadLine();
        }

    }
}
