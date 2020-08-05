#region Descripción
/*
        Implementacion de mensajes serializados entre el cliente y el servidor Zyan, para
        comunicación remota.
   */
#endregion

#region Using
using System;
using System.Collections.Generic;
#endregion

namespace Monitor.Shared
{
    // Implementacion de tipos para las solicitudes generadas en el form, atendidas en el server.
    

    [Serializable]
    public class RemReqClientData{}

    [Serializable]
    public class RemReqSystemData { }

    [Serializable]
    public class RemReqQueueData { }

    [Serializable]
    public class RemReqLogFile
    {
        public string LogFile { get; set; }

        public RemReqLogFile(string file)
        { LogFile = file; }
    }

    [Serializable]
    public class RemReqConsoleText { }

    [Serializable]
    public class RemReqCreateClient
    {
        public ClientRemoteUpdate Data { get; private set; }

        public RemReqCreateClient(ClientRemoteUpdate data)
        { Data = data; }
    }

    [Serializable]
    public class RemReqUpdateClient
    {
        public ClientRemoteUpdate Data { get; private set; }

        public RemReqUpdateClient(ClientRemoteUpdate data )
        {    Data = data;    }
    }

    [Serializable]
    public class RemReqUpdateSystem
    {

        public SystemConfigData Data { get; private set; }

        public RemReqUpdateSystem(SystemConfigData data)
        { Data = data; }
    }

    [Serializable]
    public class RemReqDeleteClient
    {
        public Guid Id { get; private set; }

        public RemReqDeleteClient(Guid id)
        { Id = id; }
    }

    [Serializable]
    public class RemReqPauseClient
    {
        public Guid Id { get; private set; }

        public RemReqPauseClient(Guid id)
        { Id = id; }
    }

    [Serializable]
    public class RemReqResumeClient
    {
        public Guid Id { get; private set; }

        public RemReqResumeClient(Guid id)
        { Id = id; }
    }


    // Implementacion de tipos para las respuestas.
    // ClientDataList y SystemConfigData se encuentran en archivos propios.
    [Serializable]
    public class RemReplyClientData
    {
        //public ClientDataList Data { get; set; }
        public ClientDataManager Data { get; private set; }
        public RemReplyClientData(ClientDataManager data)
        { Data = data; }
    }

    [Serializable]
    public class RemReplySystemData
    {
        public SystemConfigData Data { get; set; }

        public RemReplySystemData(SystemConfigData data)
        { Data = data; }
    }

    [Serializable]
    public class QueueInfo
    {
        public QueueInfo()
        {
            InitialList = new List<string>();
            WorkList = new List<string>();
            PausedList = new List<string>();
            RecoverList = new List<string>();
            DeadList = new List<string>();
        }

        public List<string> InitialList
        { get; set; }

        public List<string> WorkList
        { get; set; }

        public List<string> PausedList
        { get; set; }

        public List<string> RecoverList
        { get; set; }

        public List<string> DeadList
        { get; set; }
    }

    [Serializable]
    public class RemReplyQueueInfo
    {
        public QueueInfo Data { get; private set; }

        public RemReplyQueueInfo(QueueInfo data)
        { Data = data; }
    }

    [Serializable]
    public class RemReplyConsoleText
    {
        public List<RemoteConsoleText> List { get; private set; }

        // Constructores
        // Single instance data
        public RemReplyConsoleText(RemoteConsoleText text)
        {
            List = new List<RemoteConsoleText>();
            List.Add(text);
        }

        // List of data
        public RemReplyConsoleText(List<RemoteConsoleText> list)
        { List = list; }
    }

    [Serializable]
    public class RemReplyLogFile
    {
        public System.IO.Stream File { get; private set; }

        public RemReplyLogFile(System.IO.Stream file)
        { File = file;  }
    }
}
