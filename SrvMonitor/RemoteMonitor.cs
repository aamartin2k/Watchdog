
#region Descripción
/*
    Implementa la interface IMonitor para servicio remoto a traves de Zyan Frx.
    Recibe las solicitudes remotas del formulario Supervisor, las procesa y envia respuestas.
*/
#endregion

#region Using
using System;
using System.IO;
using Monitor.Shared;
using System.Diagnostics;
using Monitor.Shared.Interfaces;
#endregion

namespace Monitor.Service
{
    internal class RemoteMonitor : IMonitor
    {
        #region Declaraciones 
        private const string ClassName = "RemoteMonitor";

        #endregion

        #region Implementacion de IMonitor Members

        #region Salida, Respuesta a solicitudes remotas

        public Action<RemReplyConsoleText> Out_SendConsoleText
        { get; set; }

        public Action<RemReplyConsoleText> Out_SendError
        { get; set; }

        public Action<RemReplyClientData> Out_SendClientConfig
        { get; set; }

        public Action<RemReplySystemData> Out_SendSystemConfigData
        { get; set; }

        public Action<RemReplyQueueInfo> Out_SendQueueData 
        { get; set; }

        public Action<RemReplyLogFile> Out_SendLogFile 
        { get; set; }

        #endregion

        #region Entrada, Manejo de solicitudes remotas
        public void In_RequestClientPause(RemReqPauseClient request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de poner en pausa un cliente.", TraceEventType.Verbose);
            try
            {
                MessageBus.Send(new RequestPauseClient(request.Id));
            }
            catch (Exception ex)
            {
                //TODO no lanzar, registar en log, enviar error msg a cliente
                Out_SendError(new RemReplyConsoleText(new RemoteConsoleText(ex.Message, TraceEventType.Error)));

            }
        }
        public void In_RequestClientResume(RemReqResumeClient request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de terminar la pausa a un cliente.", TraceEventType.Verbose);
            try
            {
                MessageBus.Send(new RequestResumeClient(request.Id));
            }
            catch (Exception ex)
            {
                //TODO no lanzar, registar en log, enviar error msg a cliente
                Out_SendError(new RemReplyConsoleText(new RemoteConsoleText(ex.Message, TraceEventType.Error)));
            }
        }

        public void In_RequestSystemUpdate(RemReqUpdateSystem request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de actualizar configuracion de sistema.", TraceEventType.Verbose);
            try
            {
                MessageBus.Send(new RequestUpdateSystem(request.Data));
            }
            catch (Exception ex)
            {
                //TODO no lanzar, registar en log, enviar error msg a cliente
                //throw;
                Out_SendError(new RemReplyConsoleText(new RemoteConsoleText(ex.Message, TraceEventType.Error)));
            }
        }

        public void In_RequestClientCreate(RemReqCreateClient request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de crear nuevo cliente.", TraceEventType.Verbose);
            MessageBus.Send(new RequestCreateClient(request.Data));

        }

        public void In_RequestClientDelete(RemReqDeleteClient request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de eliminar cliente.", TraceEventType.Verbose);
            MessageBus.Send(new RequestDeleteClient(request.Id));
        }

        public void In_RequestClientUpdate(RemReqUpdateClient request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de actualizar cliente.", TraceEventType.Verbose);
            MessageBus.Send(new RequestUpdateClient(request.Data));
        }

        public void In_RequestClientData(RemReqClientData request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de datos de cliente.", TraceEventType.Verbose);
            MessageBus.Send(new RequestClientConfig());
        }

        public void In_RequestSystemData(RemReqSystemData request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de datos de sistema.", TraceEventType.Verbose);
            MessageBus.Send(new RequestSystemConfig());
        }

        public void In_RequestQueueData(RemReqQueueData request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de estado de las colas.", TraceEventType.Verbose);
            MessageBus.Send(new RequestQueueInfo());
        }

        public void In_RequestConsoleText(RemReqConsoleText request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de texto de consola.", TraceEventType.Verbose);
            MessageBus.Send(new SupervisorClientRequestConsoleText() );
        }

        public void In_RequestLogFile(RemReqLogFile request)
        {
            Builder.Output(ClassName + ": recibe solicitud remota de archivo log de cliente: " + request.LogFile, TraceEventType.Verbose);

            try
            {
                if (System.IO.File.Exists(request.LogFile))
                {
                    FileStream fs = new FileStream(request.LogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Out_SendLogFile(new RemReplyLogFile(fs));
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //throw;
                //TODO no lanzar, registar en log, enviar error msg a cliente

                Out_SendError(new RemReplyConsoleText(new RemoteConsoleText(ex.Message, TraceEventType.Error)));
            }
        }


        #endregion

        #endregion

        #region Request Handlers

        internal void SendRemoteConsoleText(RemoteConsoleText req)
        {
            //  !!!!!       *******                    !!!!!!!!!       *********               !!!!!!!!!
            // Importante: No realizar llamadas a Builder.Output desde este metodo.
            // Se genera un circulo vicioso.
            RemReplyConsoleText msg;

            if (null == req.List)
            {
                msg = new RemReplyConsoleText(req);
            }
            else
            {
                msg = new RemReplyConsoleText(req.List);
            }

            Out_SendConsoleText(msg);
        }

        internal void DoReplyQueueInfo(SendQueueInfo req)
        {
            Builder.Output(ClassName + ": envia estado de las colas a cliente remoto.", TraceEventType.Verbose);
            Out_SendQueueData(new RemReplyQueueInfo(req.Data));
        }

        // Recibe respuesta de DbHandler con los datos de config
        // se envian al cliente remoto.
        internal void DoReplySystemConfig(SendSystemConfig req)
        {
            Builder.Output(ClassName + ": envia datos de sistema a cliente remoto.", TraceEventType.Verbose);
            Out_SendSystemConfigData(new RemReplySystemData(req.Data));
        }
        internal void DoReplyClientConfig(SendClientConfig req)
        {
            Builder.Output(ClassName + ": envia datos de cliente a cliente remoto.", TraceEventType.Verbose);
            Out_SendClientConfig(new RemReplyClientData(req.Data));
        }

        #endregion

    }

}
