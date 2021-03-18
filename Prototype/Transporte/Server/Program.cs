using System;
using System.Text;
using Monitor.Shared.Server;
using Monitor.Shared.Interfaces;

namespace Server
{
    class Program
    {
        static Encoding _enc = new UTF8Encoding();

        static void Main(string[] args)
        {
            if (args.Length > 0)
                Pipeserver();
            else
                UdpServer();
        }

       
        //  Pipeserver
        static void Pipeserver()
        {
            string[] pipes = new string[]  
            {
                "Client1",
                "Client2",
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

        // UdpServer
        static void UdpServer()
        {
 
            ICommunicationServer  server = new HeartbeatServer("127.0.0.1", 8888, _enc);

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

        private static void Server_ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine(e.ClientId + " Disconnected.");
        }

        private static void Server_ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine(e.ClientId + " Connected.");
        }


    }
}
