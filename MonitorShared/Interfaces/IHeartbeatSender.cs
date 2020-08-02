#region Descripción
/*
    Define métodos comunes del emisor de heartbeat.
*/
#endregion

#region Using

#endregion

namespace Monitor.Shared.Interfaces
{
    public interface IHeartbeatSender
    {
       
        void StartTimer();

        void StopTimer();

  
    }
}
