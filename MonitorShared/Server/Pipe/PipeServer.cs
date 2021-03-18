#region Descripción
/*
    Implementa una colección concurrente de InternalPipeServer.
*/
#endregion

#region Using
using Monitor.Shared.Interfaces;
using Monitor.Shared.Utilities;
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
#endregion


namespace Monitor.Shared.Server.Pipe
{
    internal class PipeServer : ICommunicationServer
    {
        #region Declaraciones

        private readonly Encoding _encoder;
        private readonly string[] _pipeNames;
        private readonly SynchronizationContext _synchronizationContext;
        private readonly IDictionary<string, ICommunicationServer> _servers; // ConcurrentDictionary is thread safe
        private const int MaxNumberOfServerInstances = 10;

        #endregion

        #region Constructor

        public PipeServer(string[] names, Encoding encoder)
        {
            _pipeNames = names;
            _encoder = encoder;
            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            _servers = new ConcurrentDictionary<string, ICommunicationServer>();
        }

        #endregion

        #region Eventos

        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        #endregion

        #region Implementación ICommunicationServer

       
        public void Start()
        {
            foreach (var pipe in _pipeNames)
            {
                StartNamedPipeServer(pipe);
            }
            
        }

        public void Stop()
        {
            foreach (var server in _servers.Values)
            {
                try
                {
                    UnregisterFromServerEventos(server);
                    server.Stop();
                }
                catch (Exception)
                {
                    //Logger.Error("Failed to stop server");
                }
            }

            _servers.Clear();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Starts a new NamedPipeServerStream that waits for connection
        /// </summary>
        private void StartNamedPipeServer(string _pipeName)
        {
            var server = new InternalPipeServer(_pipeName, MaxNumberOfServerInstances, _encoder);
            
            _servers[_pipeName] = server;

            server.ClientConnectedEvent += ClientConnectedHandler;
            server.ClientDisconnectedEvent += ClientDisconnectedHandler;
            server.MessageReceivedEvent += MessageReceivedHandler;

            server.Start();
        }

        /// <summary>
        /// Stops the server that belongs to the given id
        /// </summary>
        /// <param name="id"></param>
        private void StopNamedPipeServer(string id)
        {
            UnregisterFromServerEventos(_servers[id]);
            _servers[id].Stop();
            _servers.Remove(id);
        }

        /// <summary>
        /// Unregisters from the given server's Eventos
        /// </summary>
        /// <param name="server"></param>
        private void UnregisterFromServerEventos(ICommunicationServer server)
        {
            server.ClientConnectedEvent -= ClientConnectedHandler;
            server.ClientDisconnectedEvent -= ClientDisconnectedHandler;
            server.MessageReceivedEvent -= MessageReceivedHandler;
        }

        /// <summary>
        /// Fires MessageReceivedEvent in the current thread
        /// </summary>
        /// <param name="eventArgs"></param>
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

        /// <summary>
        /// Handles a client connection. Fires the relevant event and prepares for new connection.
        /// </summary>
        private void ClientConnectedHandler(object sender, ClientConnectedEventArgs eventArgs)
        {
            OnClientConnected(eventArgs);

            // Disenno original, cuando un cliente se conecta, se crea un nuevo servidor
            // para Watchdog se crea un server para cada cliente a monitorear, la lista se 
            // conoce de antemano.
            //StartNamedPipeServer(); // Create a additional server as a preparation for new connection
        }

        /// <summary>
        /// Hanldes a client disconnection. Fires the relevant event ans removes its server from the pool
        /// </summary>
        private void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs eventArgs)
        {
            OnClientDisconnected(eventArgs);

            // Disenno original, termina el NamedPipeServerStream cuando el cliente se desconecta
            // para Watchdog el pipeserver solo se termina  cuando se termina el servicio
            // por lo que comienza a esperar conexiones de nuevo.
            //StopNamedPipeServer(eventArgs.ClientId);
        }

        /// <summary>
        /// Handles a message that is received from the client. Fires the relevant event.
        /// </summary>
        private void MessageReceivedHandler(object sender, MessageReceivedEventArgs eventArgs)
        {
            OnMessageReceived(eventArgs);
        }

        #endregion
    }
}
