#region Descripción
/*
    Define métodos para los servidores de comunicación.
*/
#endregion

#region Using
using System;
using Monitor.Shared.Server;
#endregion


namespace Monitor.Shared.Interfaces
{
    public interface ICommunicationServer : ICommunication
    {
        /// <summary>
        /// This event is fired when a message is received 
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;

        /// <summary>
        /// This event is fired when a client connects 
        /// </summary>
        event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;

        /// <summary>
        /// This event is fired when a client disconnects 
        /// </summary>
        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

    }
}
