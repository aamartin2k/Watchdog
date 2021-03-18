using System;
using System.Collections.Generic;
using System.Linq;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;
using Monitor.Shared;


using System.Windows.Forms;
using Zyan.Communication.Toolbox;

namespace ProtZyanClient
{
    static class Program
    {

        static private Form1 _theForm;
        static private IMonitor _proxy;
        private const string _serverUrl = "tcpex://localhost:9096/WDAltMonitor"; 
        //private const string _serverUrl = "tcpex://localhost:9090/WDMonitor";

        static private ZyanConnection _newConn;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _theForm = new Form1();

            // indicar al form estado de la conexion
            MessageBus.Register<EstadoConexion>(_theForm.RecibirEstado);
            MessageBus.Register<Conectar>(RealizarConexion);

            RealizarConexion();

            Application.Run(_theForm);

            _newConn.Dispose();
        }

        static void RealizarConexion(Conectar req)
        { RealizarConexion(); }

        static void RealizarConexion()
        {
            EstadoConexion msg;
            if (ConectarZServer())   // RecibirEstado(EstadoConexion
            {
                msg = new EstadoConexion(true);
            }
            else
            {
                msg = new EstadoConexion(false);
            }

            MessageBus.Send(msg);
        }

        static bool ConectarZServer()
        {
            _newConn = null;
            TcpDuplexClientProtocolSetup protocol = new TcpDuplexClientProtocolSetup(true);
            try
            {
                _newConn = new ZyanConnection(_serverUrl, protocol, false);
            }
            catch (System.Net.Sockets.SocketException )
            {
                // Indica servidor apagado o imposible de conectar, ignorar y retornar false
                return false;
            }
           

            if (_newConn == null)
            {
                // No se establecio conexion
                return false;
            }
            else
            {
                _proxy = _newConn.CreateProxy<IMonitor>();

                // connections
                // conectando input y output
                // Las acciones de salida del form, a metodos del proxy, para su transmision remota al servidor.
                _theForm.Out_GetClientConfig = Asynchronizer<RemReqClientData>.WireUp(_proxy.In_RequestClientData);
                _theForm.Out_GetSystemConfig = Asynchronizer<RemReqSystemData>.WireUp(_proxy.In_RequestSystemData);
                _theForm.Out_GetQueueInfo = Asynchronizer<RemReqQueueData>.WireUp(_proxy.In_RequestQueueData);
                _theForm.Out_GetLogFile = Asynchronizer<RemReqLogFile>.WireUp(_proxy.In_RequestLogFile);
                _theForm.Out_GetConsoleText = Asynchronizer<RemReqConsoleText>.WireUp(_proxy.In_RequestConsoleText);

                // La salida del servidor remoto, a controladores de solicitud del formulario.
                _proxy.Out_SendClientConfig = SyncContextSwitcher<RemReplyClientData>.WireUp(_theForm.In_ClientDataList);
                _proxy.Out_SendSystemConfigData = SyncContextSwitcher<RemReplySystemData>.WireUp(_theForm.In_SystemConfigData);
                _proxy.Out_SendQueueData = SyncContextSwitcher<RemReplyQueueInfo>.WireUp(_theForm.In_QueueInfo);
                _proxy.Out_SendLogFile = SyncContextSwitcher<RemReplyLogFile>.WireUp(_theForm.In_LogFile);
                _proxy.Out_SendError = SyncContextSwitcher<RemReplyConsoleText>.WireUp(_theForm.In_Error);
                _proxy.Out_SendConsoleText = SyncContextSwitcher<RemReplyConsoleText>.WireUp(_theForm.In_ConsoleText);

                // estableciendo control sobre estado de la conexion
                _newConn.Disconnected += _newConn_Disconnected;

                _newConn.PollingInterval = TimeSpan.FromSeconds(1);
                _newConn.PollingEnabled = true;

                return true;
            }

        }

        private static void _newConn_Disconnected(object sender, DisconnectedEventArgs e)
        {
            _proxy = null;
            _newConn.Dispose();
            _newConn = null;

            MessageBus.Send(new EstadoConexion(false));
        }


    }

    // mensaje
    public class EstadoConexion
    {
        public bool Conectado { get; set; }

        public EstadoConexion(bool conectado)
        {
            Conectado = conectado;
        }
    }

    public class Conectar { }
}
