#region Descripción
/*
    Implementa el monitoreo de clientes mediante servidor UDP y un conjunto de colas que
    representan diferentes estados del cliente.
    Se almacenan los ultimos heartbeats de los clientes. Solicita la salva periodica de datos.
    Parcial Manejo de solicitudes (Request Handlers).
*/
#endregion

#region Using

using Monitor.Shared;
using System;
using System.Diagnostics;


#endregion

namespace Monitor.Service
{
    public partial class ClientManager
    {
        #region Request Handlers

        #region Métodos Públicos
        internal void ReceiveSystemConfig(SendSystemConfig data)
        {
            // guardando datos de  config system
            _udpServerPort = data.Data.UdpServerPort;
            _udpServerIP = data.Data.ServerIpAdr;
            _systemTimeout = data.Data.TimeoutStartRestart;
            _restCount = data.Data.RestartAttemps;

            Builder.Output(ClassName + ": recibidos datos de configuracion de sistema.", TraceEventType.Information);
        }

        internal void ReceiveClientConfig(SendClientConfig data)
        {
            _clientList = data.Data;
            Builder.Output(ClassName + ": recibidos datos de configuracion de clientes.", TraceEventType.Information);
        }


        internal void DoRequestQueueInfo(RequestQueueInfo req)
        {
            SendQueueStatus();
        }

        internal void DoUpdateClient(RequestUpdateClient req)
        {
            if (!_clientList.ContainsGuid(req.Data.ClientId))
                return;

            // Comenzar cambio de propiedades cuando no se ejecute QueueManagerThreadProcess
            _areClientUpdate.WaitOne();

            // Actualizar cliente
            _clientList.UpdateClient(req.Data);

            // SAlvar datos
            MessageBus.Send(new RequestSaveConfig());

            // Enviar a Remote Monitor para envio remoto a Supervisor
            MessageBus.Send(new SendClientConfig(_clientList));
        }

        internal void DoCreateClient(RequestCreateClient req)
        {
            // Comenzar cambio de propiedades cuando no se ejecute QueueManagerThreadProcess
            _areClientUpdate.WaitOne();

            // Actualizar cliente
            _clientList.CreateClientRemote(req.Data);

            // SAlvar datos
            MessageBus.Send(new RequestSaveConfig());

            // Enviar a Remote Monitor para envio remoto a Supervisor
            MessageBus.Send(new SendClientConfig(_clientList));

        }

        internal void DoDeleteClient(RequestDeleteClient req)
        {
            if (!_clientList.ContainsGuid(req.Id))
                return;

            // Comenzar cambio de propiedades
            _areClientUpdate.WaitOne();

            // Eliminar ref a cliente en colas
            ClientData clt = _clientList.GetClient(req.Id);
            if (_startList.Contains(clt))
                _startList.Remove(clt);

            if (_workList.Contains(clt))
                _workList.Remove(clt);

            if (_timeOutList.Contains(clt))
                _timeOutList.Remove(clt);

            if (_recoverList.Contains(clt))
                _recoverList.Remove(clt);

            if (_deadList.Contains(clt))
                _deadList.Remove(clt);

            // Eliminar cliente
            _clientList.Delete(req.Id);

            // SAlvar datos
            MessageBus.Send(new RequestSaveConfig());

            // Enviar a Remote Monitor para envio remoto a Supervisor
            MessageBus.Send(new SendClientConfig(_clientList));

        }

        internal void DoRestartHbReceiver(RequestStartHbServer req)
        {
            StopHbReceiver();
            StartHbReceiver();
        }

        internal void DoPauseClient(RequestPauseClient req)
        {
            // Verificar existencia de clave.
            if (!_clientList.ContainsGuid(req.Id))
            {
                //TODO Log clave inexistente.
                return;
            }
#if !TEST
            // Comenzar cambio cuando no se ejecuta queue manager process.
            _areClientUpdate.WaitOne();
#endif
            // Obtener cliente.
            ClientData client = _clientList.GetClient(req.Id);

            // Modificar Status.
            client.Status = ClientStatus.Pausado;

            // Quitar de _workList.
            lock (_workList)
            {
                if(_workList.Contains(client))
                _workList.Remove(client);
            }
            // Poner en _pausedList.
            lock (_pausedList)
            {
                if (!_pausedList.Contains(client))
                    _pausedList.Add(client);
            }

            // Enviar actualizacion a Supervisor
            SendQueueStatus();

            // Notificar.
            MessageBus.Send(new RequestSendEmail(EMessageAction.Pause, DateTime.Now, client));
            Builder.Output(string.Format("{0} : Cliente {1} movido a cola de Pausado.", ClassName, client.Name), TraceEventType.Verbose);
        }

        internal void DoResumeClient(RequestResumeClient req)
        {
            // Verificar existencia de clave.
            if (!_clientList.ContainsGuid(req.Id))
            {
                //TODO Log clave inexistente.
                return;
            }
#if !TEST
            // Comenzar cambio cuando no se ejecuta queue manager process.
            _areClientUpdate.WaitOne();
#endif
            // Obtener cliente.
            ClientData client = _clientList.GetClient(req.Id);

            // Modificar Status.
            client.Status = ClientStatus.Operacional;

            // Quitar de _pausedList.
            lock (_pausedList)
            {
                if (_pausedList.Contains(client))
                    _pausedList.Remove(client);
            }
            // Poner en _workList.
            lock (_workList)
            {
                if (!_workList.Contains(client))
                    _workList.Add(client);
            }

            // Enviar actualizacion a Supervisor
            SendQueueStatus();

            MessageBus.Send(new RequestSendEmail(EMessageAction.Resume, DateTime.Now, client));
            Builder.Output(string.Format("{0} : Cliente {1} movido a cola de Trabajo.", ClassName, client.Name), TraceEventType.Verbose);
        }

#endregion

#region Métodos Privados

        private void SendQueueStatus()
        {

#if TEST
            return;
#endif
            QueueInfo rpq = new QueueInfo();

            // Comenzar cuando no se ejecute QueueManagerThreadProcess
            _areClientUpdate.WaitOne();

            foreach (var item in _startList)
            {
                rpq.InitialList.Add(item.Name);
            }

            foreach (var item in _workList)
            {
                rpq.WorkList.Add(item.Name);
            }

            foreach (var item in _pausedList)
            {
                rpq.PausedList.Add(item.Name);
            }

            foreach (var item in _recoverList)
            {
                rpq.RecoverList.Add(item.Name);
            }

            foreach (var item in _deadList)
            {
                rpq.DeadList.Add(item.Name);
            }

            MessageBus.Send(new SendQueueInfo(rpq));
        }


#endregion

#endregion
    }
}