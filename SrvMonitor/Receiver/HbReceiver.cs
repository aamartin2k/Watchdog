#region Descripción
/*
    Implementa el receptor de Heartbeat. Emplea los dos tipos de HeartbeatServer.
*/
#endregion

#region Using

using Monitor.Shared;
using Monitor.Shared.Interfaces;
using Monitor.Shared.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

#endregion


namespace Monitor.Service
{
    static internal class HbReceiver
    {
        #region Declaraciones  
        static private ICommunicationServer _pipeServer;
        static private ICommunicationServer _udpServer;

        static UTF8Encoding _enc = new UTF8Encoding();

        private const string ClassName = "HbReceiver";
        #endregion


        #region Métodos Públicos  
        static internal void Start(string[] pipeNames, string ipadr, int port)
        {
            Builder.Output("Iniciando HbReceiver.");
            StartComponent(pipeNames, ipadr, port);
            Builder.Output("HbReceiver iniciado.");
        }

        static internal void Stop()
        {
            Builder.Output("Deteniendo HbReceiver.");
            StopComponent();
            Builder.Output("HbReceiver detenido.");
        }

        #endregion
        #region Métodos Privados  
        static private void StartComponent(string[] pipeNames, string ipadr, int port)
        {
            if (pipeNames.Length > 0)
            {
                // Iniciando Pipe server
                _pipeServer = new HeartbeatServer(pipeNames, _enc);
                _pipeServer.MessageReceivedEvent += PipeServer_MessageReceived;
                _pipeServer.Start();
                Builder.Output("Iniciando _pipeServer.");
            }
            // Iniciando UdpServer
            _udpServer  = new HeartbeatServer(ipadr, port, _enc);
            _udpServer.MessageReceivedEvent += UdpServer_MessageReceived;
            _udpServer.Start();
            Builder.Output("Iniciando _udpServer.");
        }

        static private void StopComponent()
        {
            _pipeServer.Stop();
            _udpServer.Stop();
        }

        private static void PipeServer_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //Console.WriteLine(e.ClientId + " message: " + e.Message);

            MessageBus.Send(new SendHeartbeat { SenderType = HeartbeatSenderType.SenderPipe,
                                                HbData = e.Message,
                                                PipeName = e.ClientId} );
        }

        private static void UdpServer_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //Console.WriteLine(e.ClientId + " message: " + e.Message);
            // splitting ClientId into Ip and Port
            string[] data = e.ClientId.Split(new char[] { Constants.EndpointSeparator });

            MessageBus.Send(new SendHeartbeat { SenderType = HeartbeatSenderType.SenderUdp,
                                                HbData = e.Message,
                                                IpAddress = data[0],
                                                Port = int.Parse(data[1]) } );
        }

        #endregion
    }
}
