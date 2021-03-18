#region Descripción
/*
    Implementa el gestor de la aplicación.
    Almacena referencias, crea instancias y enlaza solicitudes y notificaciones con controladores.
*/
#endregion

#region Using
using AMGS.Application.Utils.Log;
using Monitor.Shared;
using Monitor.Shared.Interfaces;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;
using Zyan.Communication.Toolbox;
#endregion


namespace Monitor.Supervisor
{
    static class Builder
    {
        #region Declaraciones
        // Para uso de Log
        private const string ClassName = "Builder";

        static private FormEditConfig _theForm ;
        static private DbHandler _dbHandler;
        static private IMonitor _proxy;
        static private ZyanConnection _zyanConn;
        #endregion

        #region Metodos 

        #region Metodos Publicos
        static internal void Inicio()
        {
  
            Log.WriteStart();

            // Comprobar y cargar datos.
            _dbHandler = new DbHandler();
            bool ret = _dbHandler.OpenDatabase();

            if (!ret)
            {
                _dbHandler = null;
                return;
            }

            Log.WriteEntry(ClassName, "Inicio", TraceEventType.Verbose, "DbHandler OK, Config Database OK");

            // Crear formulario.
            _theForm = new FormEditConfig(EditFormMode.Supervisor);

            // Registrar solicitudes del estado de la conexion.
            MessageBus.Register<FormConnected>(_theForm.ReceiveConnection);
            MessageBus.Register<RequestConnect>(RealizarConexion);

            // Iniciar conexion a servidor Zyan.   
            RealizarConexion(_dbHandler.ServerUrl);

            Log.WriteStartOK();
            // Iniciar form
            Application.Run(_theForm);

            // Limpieza final.
            Log.WriteEnd();

            try
            {
                _theForm = null;
                _dbHandler.CloseDatabase();
                _dbHandler = null;
                _zyanConn.Dispose();
                _proxy = null;

                Log.WriteEndOK();
            }
            catch (Exception)
            { 
            }
        }
        #endregion

        #region Metodos Privados
        // Atiende solicitud de conexion enviada por el formulario. 
        static private void RealizarConexion(RequestConnect req)
        {
            Log.WriteEntry(ClassName, "RealizarConexion", TraceEventType.Verbose, "Recibiendo solicitud de conexion del formulario.");
            // Desconectando conexion existente
            if (null != _zyanConn)
            {
                if (_zyanConn.IsSessionValid)
                    // Cerrando conexion previa.
                    _zyanConn.Dispose();
            }

            RealizarConexion(req.ServerUrl);
        }
        static private void RealizarConexion(string url)
        {
            FormConnected msg;
            if (ConectarZServer(url))   
            {
                // Pasando url a dbHanlder para incorporarla a la lista
                _dbHandler.ServerUrl = url;
                msg = new FormConnected(true, url, _dbHandler.ServerUrlList);
            }
            else
            {
                msg = new FormConnected(false, url);
            }

            MessageBus.Send(msg);
        }

        static private bool ConectarZServer(string url)
        {
            Log.WriteEntry(ClassName, "ConectarZServer", TraceEventType.Verbose, "Conectando al servidor Zyan remoto: " + url);

            ZyanConnection _newConn = null;
            
            try
            {
                _newConn = new ZyanConnection(url);
            }
            catch (System.Net.Sockets.SocketException)
            {
                // Indica servidor apagado o imposible de conectar, ignorar y retornar false.
                return false;
            }
            catch (Exception ex)
            {
                // Registrar otro tipo de excepcion para depurar
                Log.WriteEntry(ClassName, "ConectarZServer", TraceEventType.Error, "Exception: " + ex.Message);
                return false;
            }

            if (_newConn == null)
            {
                // No se establecio conexion.
                return false;
            }
            else
            {
                _proxy = _newConn.CreateProxy<IMonitor>();

                // Conectando input y output.
                // Las acciones de salida del form, a metodos del proxy, para su transmision remota al servidor.
                // Acciones que solicitan envio de datos.
                _theForm.Out_GetClientConfig = Asynchronizer<RemReqClientData>.WireUp(_proxy.In_RequestClientData);
                _theForm.Out_GetSystemConfig = Asynchronizer<RemReqSystemData>.WireUp(_proxy.In_RequestSystemData);
                _theForm.Out_GetQueueInfo = Asynchronizer<RemReqQueueData>.WireUp(_proxy.In_RequestQueueData);
                _theForm.Out_GetLogFile = Asynchronizer<RemReqLogFile>.WireUp(_proxy.In_RequestLogFile);
                _theForm.Out_GetConsoleText = Asynchronizer<RemReqConsoleText>.WireUp(_proxy.In_RequestConsoleText);
               
                // Acciones que solicitan modificar objetos.
                _theForm.Out_SendCreateClient = Asynchronizer<RemReqCreateClient>.WireUp(_proxy.In_RequestClientCreate);
                _theForm.Out_SendClientUpdate = Asynchronizer<RemReqUpdateClient>.WireUp(_proxy.In_RequestClientUpdate);
                _theForm.Out_SendClientDelete = Asynchronizer<RemReqDeleteClient>.WireUp(_proxy.In_RequestClientDelete);
                _theForm.Out_SendSystemUpdate = Asynchronizer<RemReqUpdateSystem>.WireUp(_proxy.In_RequestSystemUpdate);
                _theForm.Out_SendClientPause = Asynchronizer<RemReqPauseClient>.WireUp(_proxy.In_RequestClientPause);
                _theForm.Out_SendClientResume = Asynchronizer<RemReqResumeClient>.WireUp(_proxy.In_RequestClientResume);

                // La salida del servidor remoto, a controladores de solicitud del formulario.
                _proxy.Out_SendClientConfig = SyncContextSwitcher<RemReplyClientData>.WireUp(_theForm.In_ClientDataList);
                _proxy.Out_SendSystemConfigData = SyncContextSwitcher<RemReplySystemData>.WireUp(_theForm.In_SystemConfigData);
                _proxy.Out_SendQueueData = SyncContextSwitcher<RemReplyQueueInfo>.WireUp(_theForm.In_QueueInfo);
                _proxy.Out_SendLogFile = SyncContextSwitcher<RemReplyLogFile>.WireUp(_theForm.In_LogFile);
                _proxy.Out_SendError = SyncContextSwitcher<RemReplyConsoleText>.WireUp(_theForm.In_Error);
                _proxy.Out_SendConsoleText = SyncContextSwitcher<RemReplyConsoleText>.WireUp(_theForm.In_ConsoleText);

                // Guardando referencia a nueva conexion.
                _zyanConn = _newConn;

                // Estableciendo controlador de sesion
                _zyanConn.Disconnected += _zyanConn_Disconnected;
                // Estableciendo monitoreo de la conexion
                _zyanConn.PollingInterval = TimeSpan.FromSeconds(1.5);
                _zyanConn.PollingEnabled = true;

                
                return true;
            }
        }

        private static void _zyanConn_Disconnected(object sender, DisconnectedEventArgs e)
        {
            _proxy = null;
            _zyanConn.Dispose();
            _zyanConn = null;

            MessageBus.Send( new FormConnected(false));
        }
        #endregion

        #endregion


    }
}
