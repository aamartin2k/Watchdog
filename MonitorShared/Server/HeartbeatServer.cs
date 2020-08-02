#region Descripción
/*
    Implementa un receptor de heartbeat que emplea UdpServer ó PipeServer
    como servidor de comunicación mediante la interfaz ICommunicationServer.
*/
#endregion

#region Using
using Monitor.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monitor.Shared.Utilities;
using System.ComponentModel;
using Monitor.Shared.Server.Udp;
using Monitor.Shared.Server.Pipe;
using System.Net;
#endregion


namespace Monitor.Shared.Server
{
    public class HeartbeatServer : ICommunicationServer
    {
        #region Declaraciones
        private readonly SynchronizationContext _synchronizationContext;
        private readonly ICommunicationServer _server;

        #endregion

        #region Eventos

        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        #endregion

        #region Constructor
        // Constructor for Pipe Server.
        public HeartbeatServer(string[] names, Encoding encoder)
        {
            //TODO Check paramters
            //  Encoder
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder", "Se necesita un codificador de caracteres.");
            }

            _synchronizationContext = AsyncOperationManager.SynchronizationContext;

            
            _server = new PipeServer(names, encoder);

            RegisterEventHandler();
        }

        // Constructor for Udp Server.
        public HeartbeatServer(string ipadr, int port, Encoding encoder)
        {
            // Direccion IpIp 
            if (ipadr == null)
            {
                throw new ArgumentNullException("ipadr", "Se necesita una dirección IP.");
            }
            IPAddress tempIP = null;
            if (ipadr.ToUpper() != "ANY" && !IPAddress.TryParse(ipadr, out tempIP))
            {
                throw new ArgumentException("La dirección IP no es válida.", "ipadrv ");
            }
            if (tempIP == null)
            {
                tempIP = IPAddress.Any;
            }
            // Puerto Local
            if (port != 0 & (port < 1024 || port > 65535))
            {
                throw new ArgumentOutOfRangeException("port", " El  valor debe estar entre 1024 y 65535.");
            }
            //  Encoder
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder", "Se necesita un codificador de caracteres.");
            }

            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            _server = new UdpServer(ipadr, port, encoder);

            RegisterEventHandler();
        }

        #endregion

        #region Implementación ICommunicationServer


        public void Start()
        {
            _server.Start();

        }

        public void Stop()
        {
            _server.Stop();
        }

        #endregion

        #region Controladores de Eventos
        private void ClientConnectedHandler(object sender, ClientConnectedEventArgs eventArgs)
        {
            OnClientConnected(eventArgs);
        }

        private void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs eventArgs)
        {
            OnClientDisconnected(eventArgs);
        }

        private void MessageReceivedHandler(object sender, MessageReceivedEventArgs eventArgs)
        {
            OnMessageReceived(eventArgs);
        }

        #endregion

        #region Métodos Privados

        private void RegisterEventHandler()
        {
            _server.ClientConnectedEvent += ClientConnectedHandler;
            _server.ClientDisconnectedEvent += ClientDisconnectedHandler;
            _server.MessageReceivedEvent += MessageReceivedHandler;
        }

        private void OnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
            _synchronizationContext.Post(e => MessageReceivedEvent.SafeInvoke(this, (MessageReceivedEventArgs)e),
                eventArgs);
        }

        /// <summary>
        /// Fires ClientConnectedEvent in the current thread
        /// </summary>
        /// <param name="eventArgs"></param>
        private void OnClientConnected(ClientConnectedEventArgs eventArgs)
        {
            _synchronizationContext.Post(e => ClientConnectedEvent.SafeInvoke(this, (ClientConnectedEventArgs)e),
                eventArgs);
        }

        /// <summary>
        /// Fires ClientDisconnectedEvent in the current thread
        /// </summary>
        /// <param name="eventArgs"></param>
        private void OnClientDisconnected(ClientDisconnectedEventArgs eventArgs)
        {
            _synchronizationContext.Post(
                e => ClientDisconnectedEvent.SafeInvoke(this, (ClientDisconnectedEventArgs)e), eventArgs);
        }

        #endregion
    }
}
