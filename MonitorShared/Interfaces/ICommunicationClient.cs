#region Descripción
/*
   Define métodos para los clientes de comunicación.
*/
#endregion

#region Using

#endregion
using System.Threading.Tasks;
using Monitor.Shared.Utilities;


namespace Monitor.Shared.Interfaces
{
    public interface ICommunicationClient : ICommunication
    {
        /// <summary>
        /// This method sends the given message asynchronously over the communication channel
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A task of TaskResult</returns>
        Task<TaskResult> SendMessage(string message);
    }
}
