#region Descripción
/*
    Implementación de mensajes para comunicación inter componentes.
*/
#endregion

#region Using
using Monitor.Shared.Heartbeat;
using Monitor.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
#endregion


namespace Monitor.Shared
{

    #region Mensajes Simples

    #region Manejo de Sesion del Cliente Supervisor

    public class RequestDeleteClient
    {
        public Guid Id { get; private set; }

        public RequestDeleteClient(Guid id)
        { Id = id; }
    }

    public class RequestPauseClient
    {
        public Guid Id { get; private set; }

        public RequestPauseClient(Guid id)
        { Id = id; }
    }

    public class RequestResumeClient
    {
        public Guid Id { get; private set; }

        public RequestResumeClient(Guid id)
        { Id = id; }
    }


    public class RequestUpdateSystem
    {
        public SystemConfigData Data { get; private set; }

        public RequestUpdateSystem(SystemConfigData data)
        { Data = data; }
    }

    public class RequestCreateClient
    {
        public ClientRemoteUpdate Data { get; private set; }

        public RequestCreateClient(ClientRemoteUpdate data)
        { Data = data;  }
    }

    public class RequestUpdateClient
    {
        public ClientRemoteUpdate Data { get; private set; }

        public RequestUpdateClient(ClientRemoteUpdate data)
        { Data = data; }
    }

    public class SupervisorClientRequestConsoleText { }

    [Serializable]
    public class RemoteConsoleText
    {
        public string Text { get; private set; }

        public TraceEventType Type { get; private set; }

        public List<RemoteConsoleText> List { get; private set; }


        public RemoteConsoleText(List<RemoteConsoleText> list)
        {
            List = list;
            Text = null;
            
        }

        public RemoteConsoleText(string text, TraceEventType type)
        {
            Text = text;
            Type = type;
            List = null;
        }
    }

    public class SupervisorClientLogEvent
    {
        public string IpAddress { get; private set; }
        public SupervisorLoginType EventType { get; private set; }
        public string Name { get; private set; }
        public DateTime Timestamp { get; private set; }

        public SupervisorClientLogEvent(string ipadr, SupervisorLoginType type, string name, DateTime time)
        {
            IpAddress = ipadr;
            EventType = type;
            Name = name;
            Timestamp = time;
        }

        public SupervisorClientLogEvent(SupervisorLoginType type)
        {
            EventType = type;
        }
    }

    #endregion

    #region Manejo de Componentes
    // Solicitud de inicio de componente.
    public class RequestStart { }

    public class RequestStop { }

    public class ReplyStop { }

    // Respuesta simple de operacion terminada correctamente
    public class ReplyOK { }

    public class RequestStartHbServer { }
    public class RequestStartZyanServer { }

    #endregion

    #region Manejo de Configuracion
    // Solicitud de configuracion del sistema y los clientes
    public class RequestConfig { }
    // Solicitud de configuracion del sistema
    public class RequestSystemConfig { }
    // Solicitud de configuracion de los clientes
    public class RequestClientConfig { }
    
    //salvar datos config a disco
    public class RequestSaveConfig { }

    public class RequestQueueInfo { }

    public class ReplyError
    {
        public string Message { get; private set; }

        public ReplyError(string msg)
        { Message = msg; }
    }

    public class SendSystemConfig
    {
        // Propiedad
        public SystemConfigData Data { get; private set; }
        // Constructor
        public SendSystemConfig(SystemConfigData data)
        { Data = data; }
    }

    public class SendClientConfig
    {
        public ClientDataManager Data { get; private set; }

        public SendClientConfig(ClientDataManager data)
        { Data = data; }
    }

    public class SendConfig
    {
        // propiedad
        public SystemPlusClientData Data { get; private set; }
        // Constructor
        public SendConfig(SystemPlusClientData data)
        { Data = data; }
    }

    public class SendQueueInfo
    {
        public QueueInfo Data { get; private set; }

        public SendQueueInfo(QueueInfo data)
        { Data = data; }
    }


    #endregion


    public class SendHeartbeat
    {
        // Propiedades
        /// <summary>
        /// Carga del mensaje. Clase HeartBeat contenida.
        /// </summary>
        public HeartBeat Data { get; private set; }
        public string HbData { get; set; }
        /// <summary>
        /// Tipo de emisor del mensaje.
        /// </summary>
        public TransportType SenderType { get; set; }

        /// <summary>
        /// Puerto UDP de emisor del mensaje.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Direccion IP de emisor del mensaje.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Nombre del Pipe de emisor del mensaje.
        /// </summary>
        public string PipeName { get; set; }


        // Constructor
        //public SendHeartbeat(HeartBeat data)
        //{ Data = data; }
    }

    #region Import/Export

    public class RequestImport
    {
        // propiedad
        public string Filename { get; private set; }
        // constructor
        public RequestImport(string filename)
        { Filename = filename; }
    }

    public class RequestExport
    {
        // propiedad
        public string Filename { get; private set; }
        // constructor
        public RequestExport(string filename)
        { Filename = filename; }
    }

    #endregion

    #region Manejo de Formulario

    public class FormEndEdit
    {
        public bool AcceptChanges { get; private set; }

        public FormEndEdit(bool accept)
        { AcceptChanges = accept; }
    }

    // No se usa
    public class SendFormMode
    {
        // propiedad
        public EditFormMode Mode { get; private set; }
        // constructor
        public SendFormMode(EditFormMode mode)
        { Mode = mode; }
    }

    public class RequestConnect
    {
        public string ServerUrl { get; set; }

        public RequestConnect(string url)
        {
            ServerUrl = url;
        }
    }
    public class ZyanClientConnected
    {
        public IMonitor Proxy { get; set; }

        public string ServerUrl { get; set; }

        public bool Connected { get; set; }

        public ZyanClientConnected(bool connected)
        {
            Proxy = null;
            ServerUrl = null;
            Connected = connected;
        }

        public ZyanClientConnected(IMonitor proxy, string url)
        {
            Proxy = proxy;
            ServerUrl = url;
            Connected = true;
        }
    }

    /// <summary>
    /// Datos de la conexión.
    /// </summary>
    public class FormConnected
    {
        public string ServerUrl { get; set; }
        public bool Connected { get; set; }

        public string[] ServerUrlList { get; set; }

        public FormConnected(bool connected)
        {
            ServerUrl = null;
            Connected = connected;
            ServerUrlList = null;
        }

        public FormConnected(bool connected, string url)
        { 
            ServerUrl = url;
            Connected = connected;
            ServerUrlList = null;
        }

        public FormConnected(bool connected, string url, string[] list)
        {
            ServerUrl = url;
            Connected = connected;
            ServerUrlList = list;
        }

        public class FormEditMode
        {

            public EditFormMode Mode { get; private set; }

            public FormEditMode(EditFormMode mode)
            { Mode = mode; }
        }

    }

    #endregion


    public class RequestSendEmail
    {
        public RequestSendEmail(EMessageAction action, DateTime date)
        {
            Action = action;
            Date = date;
            Client = null;
        }

        public RequestSendEmail(EMessageAction action, DateTime date, ClientData client)
        {
            Action = action;
            Date = date;
            Client = client;
        }

        public EMessageAction Action
        { get; private set; }

        public DateTime Date
        { get; private set; }

        public ClientData Client
        { get; private set; }
    }


    #endregion
}
