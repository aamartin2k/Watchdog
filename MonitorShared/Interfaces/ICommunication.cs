#region Descripción
/*
    Define métodos comunes del canal de comunicación.
*/
#endregion

#region Using

#endregion

namespace Monitor.Shared.Interfaces
{
    public interface ICommunication
    {
        /// <summary>
        /// Starts the communication channel
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the communication channel
        /// </summary>
        void Stop();
    }
}
