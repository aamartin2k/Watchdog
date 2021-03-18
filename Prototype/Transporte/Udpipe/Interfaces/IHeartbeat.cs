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
        bool UsarSerialHB { set; }
        string ClientID { set; }
        string TimestampFormat { set; }

        void StartTimer();

        void StopTimer();

  
    }
}
