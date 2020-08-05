#region Descripción
/*
    Implementa el servidor Zyan para comunicacion remota con el cliente Supervisor. Soporte 
    para la arquitectura de aplicacion distribuida.
*/
#endregion

#region Using

using Monitor.Shared;
using Monitor.Shared.Interfaces;
using System;
using System.Diagnostics;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

#endregion

namespace Monitor.Service
{
    public class ZyanServer 
    {
        #region Declaraciones

        private ZyanComponentHost _znHost;
        private int _zsPort;
        private string _zsName;
        // Para uso de Log.
        private const string ClassName = "ZyanServer";
        #endregion

        #region Propiedades

        #region Propiedades Públicas
        internal RemoteMonitor Proxy { get; set; }
        #endregion

        #region Propiedades Privadas

        #endregion

        #endregion

        #region Métodos

        #region Métodos Principales Inicio/Detencion

        internal void Start(RequestStart req)
        {
            Builder.Output("Iniciando ZyanServer.");
            StartComponent();
            Builder.Output("ZyanServer iniciado.");
        }

        internal void Stop(RequestStop req)
        {
            Builder.Output("Deteniendo ZyanServer.");
            StopComponent();
            Builder.Output("ZyanServer detenido.");
            MessageBus.Send(new ReplyStop());

        }

        #endregion

        #region Métodos Request Handlers

        internal void ReceiveSystemConfig(SendSystemConfig req)
        {
           
            int port;
            string name;

            // Garantiza inicio de las propiedades con valores por defecto
            // si la configuracion no está actualizada.
            if (req.Data.ZyanServerPort == 0)
                port = Constants.ZyanServerPort;
            else
                port = req.Data.ZyanServerPort;

            if (string.IsNullOrEmpty(req.Data.ZyanServerName))
                name = Constants.ZyanServerName;
            else
                name = req.Data.ZyanServerName;

            _zsName = name;
            _zsPort = port;

            Builder.Output(ClassName + ": recibidos datos de configuracion de sistema.", TraceEventType.Information);
        }

        internal void DoRestart(RequestStartZyanServer req)
        {
            StopComponent();

            RegisterComponent(_zsPort, _zsName, Proxy);
        }

        #endregion

        #region Zyan Comms

        private void RegisterComponent(int port, string name, RemoteMonitor instance)
        {
            TcpDuplexServerProtocolSetup protocol = new TcpDuplexServerProtocolSetup(port);

            _znHost = new ZyanComponentHost(name, protocol);
            //_znHost.EnableDiscovery();

            //_znHost.RegisterComponent<IMonitor, RemoteMonitor>(ActivationType.Singleton);
            _znHost.RegisterComponent<IMonitor, RemoteMonitor>(instance);

            _znHost.ClientLoggedOn += ClientLoggedOn;
            _znHost.ClientLoggedOff += ClientLoggedOff;

            Builder.Output(string.Format(ClassName + ": registrado componente {0} en puerto {1}.", name, port), TraceEventType.Verbose);
        }

        private void ClientLoggedOn(object sender, LoginEventArgs e)
        {
            string msg = "Usuario: {0} Ip: {1} inicia sesion de supervision en: {2}";
            string name = string.IsNullOrEmpty(e.Identity.Name) ? "anonimo" : e.Identity.Name;

            Builder.Output(string.Format(msg, name, e.ClientAddress, e.Timestamp.ToString()), TraceEventType.Verbose);
           
            MessageBus.Send(new SupervisorClientLogEvent(e.ClientAddress,
                                                    SupervisorLoginType.Logon,
                                                    e.Identity.Name,
                                                    e.Timestamp));
        }

        private void ClientLoggedOff(object sender, LoginEventArgs e)
        {
            string msg = "Usuario: {0} Ip: {1} termina sesion de supervision en: {2}";
            string name = string.IsNullOrEmpty(e.Identity.Name) ? "anonimo" : e.Identity.Name;

            Builder.Output(string.Format(msg, name, e.ClientAddress, e.Timestamp.ToString()), TraceEventType.Verbose);

            

            MessageBus.Send(new SupervisorClientLogEvent(e.ClientAddress,
                                                    SupervisorLoginType.Logoff,
                                                    e.Identity.Name,
                                                    e.Timestamp));
        }

        #endregion

        #region Métodos Privados

        private void StartComponent()
        {
            // debug
            //MessageBus.Send(new ReplyError("Debug Error."));
            //return;

            try
            {
                if (Proxy == null)
                {
                    MessageBus.Send(new ReplyError("RemoteMonitor Proxy es null."));
                    return;
                }

                //Registrar componente / iniciar servidor
                RegisterComponent(_zsPort, _zsName, Proxy);

                // Fin OK sin error
                MessageBus.Send(new ReplyOK());
            }
            catch (Exception ex)
            {
                MessageBus.Send(new ReplyError(ex.Message));
            }

        }

        private void StopComponent()
        {
            _znHost.UnregisterComponent(_zsName);
            _znHost.Dispose();
            _znHost = null;

        }

        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_znHost != null)
                _znHost.Dispose();
        }

        #endregion
    }
}
