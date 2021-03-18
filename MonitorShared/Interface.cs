#region Descripción
/*
    Interfase de comunicacion entre el servidor Monitor 
    y el cliente Supervisor empleando Zyan Framework
*/
#endregion

#region Using
using System;
#endregion


namespace Monitor.Shared
{

    /// <summary>
    /// Describe metodos y eventos para comunicacion entre el servidor Monitor 
    /// y el cliente Supervisor empleando Zyan Framework
    /// </summary>
    public interface IMonitor
    {
 
        // Solicitudes generadas en el form, atendidas en el server.
        void In_RequestClientData(RemReqClientData request);

        void In_RequestSystemData(RemReqSystemData request);

        void In_RequestQueueData(RemReqQueueData request);

        void In_RequestLogFile(RemReqLogFile request);

        void In_RequestConsoleText(RemReqConsoleText request);

        void In_RequestClientCreate(RemReqCreateClient request);

        void In_RequestClientUpdate(RemReqUpdateClient request);

        void In_RequestClientDelete(RemReqDeleteClient request);

        void In_RequestSystemUpdate(RemReqUpdateSystem request);

        void In_RequestClientPause(RemReqPauseClient request);

        void In_RequestClientResume(RemReqResumeClient request);


        // Respuestas generadas en el server, consumidos por el client.

        // Enviar informacion a mostrar en Consola Remota.
        Action<RemReplyConsoleText> Out_SendConsoleText { get; set; }

        // Enviar informacion de Error generada en el servidor.
        Action<RemReplyConsoleText> Out_SendError { get; set; }

        // Enviar informacion de clientes
        Action<RemReplyClientData> Out_SendClientConfig { get; set; }
        
        // Enviar informacion del sistema
        Action<RemReplySystemData> Out_SendSystemConfigData { get; set; }

        // Enviar informacion de las colas
        Action<RemReplyQueueInfo> Out_SendQueueData { get; set; }

        // Enviar archivo de log
        //Action<System.IO.Stream> Out_SendLogFile { get; set; }
        Action<RemReplyLogFile> Out_SendLogFile { get; set; }

    }

   
    

   
}
