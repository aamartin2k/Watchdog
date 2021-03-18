using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monitor.Shared;
using Zyan.Communication; 
using Zyan.Communication.Protocols.Tcp;


namespace ProtZyanServer
{
    class Program
    {
        static private RemoteMonitor _remoteMonitor;
        static private ZyanComponentHost _znHost;

        private const int _zsPort = 9096;
        private const string _zsName = "WDAltMonitor";
        //  tcpex://localhost:9096/WDAltMonitor


        static void Main(string[] args)
        {
            StartComponent();

            Console.WriteLine("Press Enter to terminate Zyan server.");

            Console.ReadLine();

            Console.WriteLine("Terminating Zyan server...");

            _znHost.UnregisterComponent(_zsName);
            _znHost.Dispose();

            Console.WriteLine("Zyan server terminated.");

        }


        static private void StartComponent()
        {
            try
            {
                _remoteMonitor = new RemoteMonitor();

                //Registrar componente / iniciar servidor

                TcpDuplexServerProtocolSetup protocol = new TcpDuplexServerProtocolSetup(_zsPort); 
                //TcpDuplexServerProtocolSetup protocol = new TcpDuplexServerProtocolSetup(Constants.ServerPort, new BasicWindowsAuthProvider());

                _znHost = new ZyanComponentHost(_zsName, protocol);

                // Enable 
                //_znHost.PollingEventTracingEnabled = true;

                //_znHost.RegisterComponent<IMonitor, RemoteMonitor>(ActivationType.Singleton);
                _znHost.RegisterComponent<IMonitor, RemoteMonitor>(_remoteMonitor);

                _znHost.ClientLoggedOn += new EventHandler<LoginEventArgs>((sender, e) =>
                {
                    Console.WriteLine(string.Format("{0}: User '{1}' with IP {2} logged on.", e.Timestamp.ToString(), e.Identity.Name, e.ClientAddress));
                });

                _znHost.ClientLoggedOff += new EventHandler<LoginEventArgs>((sender, e) =>
                {
                    Console.WriteLine(string.Format("{0}: User '{1}' with IP {2} logged off.", e.Timestamp.ToString(), e.Identity.Name, e.ClientAddress));
                });


                // Fin OK sin error
                Console.WriteLine("ReplyOK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " +  ex.Message);
            }

        }
    }
}
