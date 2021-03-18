#region Descripción
/*
    Implementa el gestor del sistema.
    Parcial Consola
    Implementa metodos para ejecutar servicio en modo consola.
*/
#endregion

#region Using
using Monitor.Shared;
using System;

#endregion

namespace Monitor.Service
{
    static partial class Builder
    {

        #region Modo Consola
        static private void CargarTodosEjecutarConsole()
        {
            writeBaseLine = true;

            Builder.Output("Ejecutando servicio en modo consola.");

            if (!ConfigurarComponentes())
                return;

            if (!IniciarComponentes())
                return;

            if (!RegistrarMensajes())
                return;

            NotificarInicioSistema();
            
            Builder.Output("Simulando ejecucion ininterrumpida del servicio. Parada en Console.ReadLine... ");
            Console.ReadLine();

            DetenerComponentes();

        }

        static private bool ConfigurarComponentes()
        {
            try
            {
                Builder.Output("Creando componentes.");
                // Crear referencias
                _clientManager = new ClientManager();
                _notifier = new Notifier();
                _zyanServer = new ZyanServer();

                Builder.Output("Configurando componentes.");
                //  ClientManager -> all Config
                MessageBus.Register<SendSystemConfig>(_clientManager.ReceiveSystemConfig);
                MessageBus.Register<SendClientConfig>(_clientManager.ReceiveClientConfig);

                // Notifier y ZyanServer -> system config
                MessageBus.Register<SendSystemConfig>(_notifier.ReceiveSystemConfig);
                MessageBus.Register<SendSystemConfig>(_zyanServer.ReceiveSystemConfig);

                // Enviando config
                MessageBus.Send(new SendClientConfig(_dbHandler.ClientList));
                MessageBus.Send(new SendSystemConfig(_dbHandler.SystemData));

                // Pausa para que los request handlers de los componentes terminen
                System.Threading.Thread.Sleep(100);

                return true;
            }
            catch (Exception ex)
            {
                Builder.Output(string.Format("{0}.ConfigurarComponentes: ocurrio una excepcion: {1}", ClassName, ex.Message), System.Diagnostics.TraceEventType.Error);
                return false;
            }
        }

        static private bool IniciarComponentes()
        {
            try
            {
                // Iniciar componentes
                //TODO  Iniciar componentes como tareas que devuelven Result.
                //TODO  Usar Barrera para esperar inicio de todos los componentes.
                Builder.Output("Iniciando componentes.");

                // Notifier Inicio
                _areEsperaOperacion.Reset();
                MessageBus.Register<RequestStart>(_notifier.Start);
                MessageBus.Register<RequestStop>(_notifier.Stop);
                MessageBus.Register<ReplyOK>(NotifierStartOK);
                MessageBus.Register<ReplyError>(ModuleStartError);
                MessageBus.Send(new RequestStart());

                Detenerse = false;
                // Esperar por respuesta de Notifier            
                _areEsperaOperacion.WaitOne();
                if (Detenerse)
                {  // ocurrio un error
                    TerminarNotifier(_errMsg);
                    return false;
                }
                // Registrando mensaje de envio de email
                MessageBus.Register<RequestSendEmail>(_notifier.DoSendEmail);
                // Notifier Fin

                //  ClientManager Inicio
                _areEsperaOperacion.Reset();
                MessageBus.Register<RequestStart>(_clientManager.Start);
                MessageBus.Register<RequestStop>(_clientManager.Stop);
                MessageBus.Register<ReplyOK>(ClientManagerStartOK);
                MessageBus.Register<ReplyError>(ModuleStartError);
                MessageBus.Send(new RequestStart());

                Detenerse = false;
                // Esperar por respuesta de ClientManager            
                _areEsperaOperacion.WaitOne();

                if (Detenerse)
                {  // ocurrio un error
                    TerminarClientManager(_errMsg);
                    return false;
                }
                //  ClientManager Fin

                // ZyanServer Inicio

                // Remote Monitor Proxy
                _remoteMonitor = new RemoteMonitor();
                // Registrando mensajes del proxy
                MessageBus.Register<SendClientConfig>(_remoteMonitor.DoReplyClientConfig);
                MessageBus.Register<SendSystemConfig>(_remoteMonitor.DoReplySystemConfig);
                MessageBus.Register<SendQueueInfo>(_remoteMonitor.DoReplyQueueInfo);

                // Asignando Proxy a Zyan server
                _zyanServer.Proxy = _remoteMonitor;

                _areEsperaOperacion.Reset();
                MessageBus.Register<RequestStart>(_zyanServer.Start);
                MessageBus.Register<RequestStop>(_zyanServer.Stop);
                MessageBus.Register<ReplyOK>(ZyanServerStartOK);
                MessageBus.Register<ReplyError>(ModuleStartError);
                MessageBus.Send(new RequestStart());

                Detenerse = false;
                _areEsperaOperacion.WaitOne();
                if (Detenerse)
                {  // ocurrio un error
                    TerminarZyanServer(_errMsg);
                    return false;
                }
                // ZyanServer Fin
                Builder.Output("Componentes iniciados.");

                return true;
            }
            catch (Exception ex)
            {
                Builder.Output(string.Format("{0}.IniciarComponentes: ocurrio una excepcion: {1}", ClassName, ex.Message), System.Diagnostics.TraceEventType.Error);
                return false;
            }
        }

        static private bool RegistrarMensajes()
        {
            try
            {
                // Enlaces adicionales
                MessageBus.Register<RequestClientConfig>(_dbHandler.SendClientConfig);
                MessageBus.Register<RequestSystemConfig>(_dbHandler.SendSystemConfig);
                MessageBus.Register<RequestQueueInfo>(_clientManager.DoRequestQueueInfo);
                MessageBus.Register<SendHeartbeat>(_clientManager.ReceiveHearbeat);
                MessageBus.Register<SupervisorClientLogEvent>(ClientLogonEvent);
                MessageBus.Register<RemoteConsoleText>(_remoteMonitor.SendRemoteConsoleText);
                MessageBus.Register<SupervisorClientRequestConsoleText>(SendRemoteTextCache);
                MessageBus.Register<RequestSaveConfig>(_dbHandler.Save);
                MessageBus.Register<RequestUpdateClient>(_clientManager.DoUpdateClient);
                MessageBus.Register<RequestCreateClient>(_clientManager.DoCreateClient);
                MessageBus.Register<RequestDeleteClient>(_clientManager.DoDeleteClient);
                MessageBus.Register<RequestPauseClient>(_clientManager.DoPauseClient);
                MessageBus.Register<RequestResumeClient>(_clientManager.DoResumeClient);
                MessageBus.Register<RequestUpdateSystem>(DoUpdateSystem);
                MessageBus.Register<RequestStartHbServer>(_clientManager.DoRestartHbReceiver);

                return true;
            }
            catch (Exception ex)
            {
                Builder.Output(string.Format("{0}.RegistrarMensajes: ocurrio una excepcion: {1}", ClassName, ex.Message), System.Diagnostics.TraceEventType.Error);
                return false;
            }
        }

        static private void DetenerComponentes()
        {
            try
            {
                // Comienza descarga
                Builder.Output("Terminando ejecucion del servicio.");

                // Salvar datos
                MessageBus.Send(new RequestSaveConfig());

                // Terminar modulos consecutivamente esperando confirmacion.
                Builder.Output("Deteniendo componentes.");

                // ClientManager
                _areEsperaOperacion.Reset();
                MessageBus.Register<RequestStop>(_clientManager.Stop);
                MessageBus.Register<ReplyStop>(ClientManagerStopOK);
                MessageBus.Send(new RequestStop());

                _areEsperaOperacion.WaitOne();

                // ZyanServer
                _areEsperaOperacion.Reset();
                MessageBus.Register<RequestStop>(_zyanServer.Stop);
                MessageBus.Register<ReplyStop>(ZyanServerStopOK);
                MessageBus.Send(new RequestStop());

                _areEsperaOperacion.WaitOne();

                // Notificar fin de sistema antes de detener Notifier
                NotificarFinSistema();

                // Notifier
                _areEsperaOperacion.Reset();
                MessageBus.Register<RequestStop>(_notifier.Stop);
                MessageBus.Register<ReplyStop>(NotifierStopOK);
                MessageBus.Send(new RequestStop());

                _areEsperaOperacion.WaitOne();

                // Anular referencias
                _zyanServer = null;
                _notifier = null;
                _clientManager = null;

                Builder.Output("Componentes detenidos.");
            }
            catch (Exception ex)
            {
                Builder.Output(string.Format("{0}.DetenerComponentes: ocurrio una excepcion: {1}", ClassName, ex.Message), System.Diagnostics.TraceEventType.Error);
            }
        }

        static private void NotificarInicioSistema()
        {
            // Notificar Inicio del sistema
            MessageBus.Send(new RequestSendEmail(EMessageAction.SysStart, DateTime.Now));
        }

        static private void NotificarFinSistema()
        {
            // Notificar
            MessageBus.Send(new RequestSendEmail(EMessageAction.SysEnd, DateTime.Now));
        }

        #endregion

        #region Kill Modules
        static internal void TerminarClientManager(string msg)
        {
            // log error
            Builder.Output("Error iniciando ClientManager.");
            Builder.Output("Error: " + msg, System.Diagnostics.TraceEventType.Error);
            // intentar detener modulo
            MessageBus.Send(new RequestStop());
            // eliminar registros
            MessageBus.Remove<RequestStart>(_clientManager.Start);
            MessageBus.Remove<RequestStop>(_clientManager.Stop);
            MessageBus.Remove<ReplyOK>(ClientManagerStartOK);
            MessageBus.Remove<ReplyError>(ModuleStartError);

            // anular referencia
            _clientManager = null;

            TerminarNotifier("Error en carga de ClientManager.");
        }
        static internal void TerminarNotifier(string msg)
        {
            // log error
            Builder.Output("Error iniciando Notifier.");
            Builder.Output("Error: " + msg, System.Diagnostics.TraceEventType.Error);
            // intentar detener modulo
            MessageBus.Send(new RequestStop());
            // eliminar registros
            MessageBus.Remove<RequestStart>(_notifier.Start);
            MessageBus.Remove<RequestStop>(_notifier.Stop);
            MessageBus.Remove<ReplyOK>(NotifierStartOK);
            MessageBus.Remove<ReplyError>(ModuleStartError);

            // anular referencia
            _notifier = null;

        }

        static internal void TerminarZyanServer(string msg)
        {
            // log error
            Builder.Output("Error iniciando ZyanServer.");
            Builder.Output("Error: " + msg, System.Diagnostics.TraceEventType.Error);
            // intentar detener modulo
            MessageBus.Send(new RequestStop());
            // eliminar registros
            MessageBus.Remove<RequestStart>(_zyanServer.Start);
            MessageBus.Remove<RequestStop>(_zyanServer.Stop);
            MessageBus.Remove<ReplyOK>(ZyanServerStartOK);
            MessageBus.Remove<ReplyError>(ModuleStartError);

            // anular referencia
            _zyanServer = null;

            TerminarClientManager("Error en carga de ZyanServer.");
        }

        #endregion

        #region Modo Reply Handlers
       

        static private void ZyanServerStopOK(ReplyStop reply)
        {
            Builder.Output("Confirmada detencion de ZyanServer.");
            MessageBus.Remove<RequestStop>(_zyanServer.Stop);
            MessageBus.Remove<ReplyStop>(ZyanServerStopOK);
            _areEsperaOperacion.Set();
        }

        static private void NotifierStopOK(ReplyStop reply)
        {
            Builder.Output("Confirmada detencion de Notifier.");
            MessageBus.Remove<RequestStop>(_notifier.Stop);
            MessageBus.Remove<ReplyStop>(NotifierStopOK);
            _areEsperaOperacion.Set();
        }
        static private void ClientManagerStopOK(ReplyStop reply)
        {
            Builder.Output("Confirmada detencion de ClientManager.");
            MessageBus.Remove<RequestStop>(_clientManager.Stop);
            MessageBus.Remove<ReplyStop>(ClientManagerStopOK);
            _areEsperaOperacion.Set();
        }

        static private void ClientManagerStartOK(ReplyOK reply)
        {
            Builder.Output("Confirmado inicio de ClientManager.");

            MessageBus.Remove<RequestStart>(_clientManager.Start);
            MessageBus.Remove<RequestStop>(_clientManager.Stop);
            MessageBus.Remove<ReplyOK>(ClientManagerStartOK);
            MessageBus.Remove<SendSystemConfig>(_clientManager.ReceiveSystemConfig);
            MessageBus.Remove<SendClientConfig>(_clientManager.ReceiveClientConfig);

            Detenerse = false;
            _areEsperaOperacion.Set();
        }

        static private void NotifierStartOK(ReplyOK reply)
        {
            Builder.Output("Confirmado inicio de Notifier.");

            MessageBus.Remove<RequestStart>(_notifier.Start);
            MessageBus.Remove<RequestStop>(_notifier.Stop);
            MessageBus.Remove<ReplyOK>(NotifierStartOK);
            MessageBus.Remove<SendSystemConfig>(_notifier.ReceiveSystemConfig);

            Detenerse = false;
            _areEsperaOperacion.Set();
        }

        static private void ZyanServerStartOK(ReplyOK reply)
        {
            Builder.Output("Confirmado inicio de ZyanServer.");

            MessageBus.Remove<RequestStart>(_zyanServer.Start);
            MessageBus.Remove<RequestStop>(_zyanServer.Stop);
            MessageBus.Remove<ReplyOK>(ZyanServerStartOK);
            MessageBus.Remove<SendSystemConfig>(_zyanServer.ReceiveSystemConfig);

            Detenerse = false;
            _areEsperaOperacion.Set();
        }


       

        #endregion

    }
}
