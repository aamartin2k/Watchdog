using Monitor.Shared.Client;
using Monitor.Shared.Heartbeat;
using Monitor.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static List<IHeartbeatSender> senders;
        static Encoding _enc = new UTF8Encoding();

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                //MultiPipeClient(); 
               PipeClient();
            }
            else
            {
                //MultiUdpClient();   
                UdpClient();
            }
        }

        static void UdpClient()
        {
            Console.WriteLine("Udp Client started.");

          
            HeartBeatGenerator hbgen = new HeartBeatGenerator
            {
                ClientID = "UdpCIdn02",
                UsarSerialHB = false
            };

            IHeartbeatSender sender = new HeartbeatClient("127.0.0.1", 8888, 9090, _enc, 3, hbgen);
            sender.StartTimer();

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            sender.StopTimer();
        }

        static void PipeClient()
        {
            Console.WriteLine("Pipe Client started.");

            HeartBeatGenerator hbgen = new HeartBeatGenerator
            {
                ClientID = "PClient1",  UsarSerialHB = true
            };

            IHeartbeatSender sender = new HeartbeatClient(".", "Client1", 3 * 1000, _enc, 3, hbgen);
            sender.StartTimer();

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            sender.StopTimer();
        }

        //  Multiples clientes desde el mismo proceso comparten static HeartBeatGenerator
        // y se sobrescriben las propiedades. SOLO funciona con clientes sin propiedases
        static void MultiUdpClient()
        {
            Console.WriteLine("Udp Clients started.");

            senders = new List<IHeartbeatSender>();
          
            Parallel.For(0, 9, i =>
            {

                HeartBeatGenerator hbgen = new HeartBeatGenerator();
                // dos con clientid
                if ((i == 1) | (i == 3))
                {
                    hbgen.ClientID = string.Format("CName{0}", i);
                }
                // dos con clientid + serial number
                if ((i == 2) | (i == 6))
                {
                    hbgen.ClientID = string.Format("CIdn{0}", i);
                    hbgen.UsarSerialHB = true;
                }

                IHeartbeatSender sender = new HeartbeatClient("127.0.0.1", 8888, 9090 + i, _enc, 3, hbgen);
                senders.Add(sender);
            });

            foreach (var item in senders)
            {
                item.StartTimer();
            }

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            foreach (var item in senders)
            {
                item.StopTimer();
            }
        }


        static void MultiPipeClient()
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

            Console.WriteLine("Pipe Clients started.");
            senders = new List<IHeartbeatSender>();

            Parallel.For(0, 8, i =>
            {
                HeartBeatGenerator hbgen = new HeartBeatGenerator();
                // dos con clientid
                if ((i == 1) | (i == 3))
                {
                    hbgen.ClientID = string.Format("CName{0}", i);
                }
                // dos con clientid + serial number
                if ((i == 2) | (i == 6))
                {
                    hbgen.ClientID = string.Format("CIdn{0}", i);
                    hbgen.UsarSerialHB = true;
                }

                IHeartbeatSender sender = new HeartbeatClient(".", pipes[i], 3 * 1000, _enc, 3, hbgen);
                senders.Add(sender);
            });

            Task.Delay(400).Wait();  // esperar que todos los clientes inicien

            foreach (var item in senders)
            {
                item.StartTimer();
            }

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

            foreach (var item in senders)
            {
                item.StopTimer();
            }         
        }

    }
}
