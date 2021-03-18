#region Descripción
/*
    Implementa información de eventos de cliente. Derivadas de EventArgs.
*/
#endregion

#region Using
using System;
#endregion

namespace Monitor.Shared.Server
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; }
    }

    public class ClientDisconnectedEventArgs : EventArgs
    {
        public string ClientId { get; set; }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string ClientId { get; set; }
    }
}
