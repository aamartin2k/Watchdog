#region Descripción
/*
     Implementa el gestor del sistema.
     Implementa la creación de una configuración de prueba.
     Parcial Configuración de prueba
    */
#endregion

#region Using
using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
#endregion

namespace Monitor.Service
{
    static partial class Builder
    {
        static private void DoUpdateSystem(RequestUpdateSystem req)
        {
            Builder.Output(ClassName + ": Realizando actualizacion de la configuracion del sistema", TraceEventType.Information);

            bool RestartZyanServer = false;
            bool RestartHbReceiver = false;

            SystemConfigData newCfg = req.Data;
            // Config que afecta a HbReceiver.
            if (newCfg.UdpServerPort != _dbHandler.SystemData.UdpServerPort)
            {
                _dbHandler.SystemData.UdpServerPort = newCfg.UdpServerPort;
                RestartHbReceiver = true;
            }
            if (newCfg.ServerIpAdr != _dbHandler.SystemData.ServerIpAdr)
            {
                _dbHandler.SystemData.ServerIpAdr = newCfg.ServerIpAdr;
                RestartHbReceiver = true;
            }
            // Config que afecta a ZyanServer.
            if (newCfg.ZyanServerName != _dbHandler.SystemData.ZyanServerName)
            {
                _dbHandler.SystemData.ZyanServerName = newCfg.ZyanServerName;
                RestartZyanServer = true;
            }
            if (newCfg.ZyanServerPort != _dbHandler.SystemData.ZyanServerPort)
            {
                _dbHandler.SystemData.ZyanServerPort = newCfg.ZyanServerPort;
                RestartZyanServer = true;
            }
            // Config que afecta a Notifier.
            if (newCfg.Source != _dbHandler.SystemData.Source)
            {
                _dbHandler.SystemData.Source = newCfg.Source;

            }
            if (newCfg.Destination != _dbHandler.SystemData.Destination)
            {
                _dbHandler.SystemData.Destination = newCfg.Destination;

            }
            if (newCfg.Password != _dbHandler.SystemData.Password)
            {
                _dbHandler.SystemData.Password = newCfg.Password;

            }
            if (newCfg.SMtpServer != _dbHandler.SystemData.SMtpServer)
            {
                _dbHandler.SystemData.SMtpServer = newCfg.SMtpServer;

            }

            // Resto de la configuracion
            if (newCfg.TimeoutStartRestart != _dbHandler.SystemData.TimeoutStartRestart)
            {
                _dbHandler.SystemData.TimeoutStartRestart = newCfg.TimeoutStartRestart;

            }
            if (newCfg.RestartAttemps != _dbHandler.SystemData.RestartAttemps)
            {
                _dbHandler.SystemData.RestartAttemps = newCfg.RestartAttemps;

            }

            // guardar cambios
            MessageBus.Send(new RequestSaveConfig());

            // Registrando mensaje y controlador.
            MessageBus.Register<SendSystemConfig>(_clientManager.ReceiveSystemConfig);
            MessageBus.Register<SendSystemConfig>(_notifier.ReceiveSystemConfig);
            MessageBus.Register<SendSystemConfig>(_zyanServer.ReceiveSystemConfig);

            // Enviando msg.
            MessageBus.Send(new SendSystemConfig(_dbHandler.SystemData));

            // Quitando registro.
            MessageBus.Remove<SendSystemConfig>(_clientManager.ReceiveSystemConfig);
            MessageBus.Remove<SendSystemConfig>(_notifier.ReceiveSystemConfig);
            MessageBus.Remove<SendSystemConfig>(_zyanServer.ReceiveSystemConfig);

            // Actualizar componentes afectados:
            //  posibles implicados Notifier, ZyanServer, ClientManager (HB REceiver y timeouts)
            //  De ellos solo HB REceiver (pertenece a ClientManager) y ZyanServer se deben reiniciar si hay cambios
            //  los demas solo leen los valores de la configuracion.

            if (RestartHbReceiver)
            {
                MessageBus.Register<RequestStartHbServer>(_clientManager.DoRestartHbReceiver);
                MessageBus.Send(new RequestStartHbServer());
                MessageBus.Remove<RequestStartHbServer>(_clientManager.DoRestartHbReceiver);
            }

            if (RestartZyanServer)
            {
                MessageBus.Register<RequestStartZyanServer>(_zyanServer.DoRestart);
                MessageBus.Send(new RequestStartZyanServer());
                MessageBus.Remove<RequestStartZyanServer>(_zyanServer.DoRestart);
            }

            Builder.Output(ClassName + ": Terminada actualizacion de la configuracion del sistema", TraceEventType.Information);
        }

    }
}
