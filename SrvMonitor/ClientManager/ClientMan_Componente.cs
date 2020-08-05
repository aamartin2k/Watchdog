#region Descripción
/*
    Implementa el monitoreo de clientes mediante servidor UDP y un conjunto de colas que
    representan diferentes estados del cliente.
    Se almacenan los ultimos heartbeats de los clientes. Solicita la salva periodica de datos.
    Parcial Gestion del Componente.
*/
#endregion

#region Using

using Monitor.Shared;
using Monitor.Shared.Heartbeat;
using System;
using System.Threading;
using System.Collections.Generic;
#endregion

namespace Monitor.Service
{
    public partial class ClientManager
    {
        #region Gestion del Componente
        #region Métodos Públicos

        internal void Start(RequestStart req)
        {
            Builder.Output("Iniciando ClientManager.");
            StartComponent();
            Builder.Output("ClientManager iniciado.");
        }

        internal void Stop(RequestStop req)
        {
            Builder.Output("Deteniendo ClientManager..");
            StopComponent();
            Builder.Output("ClientManager detenido..");
        }

        #endregion

        #region Métodos Privados

        private void StartComponent()
        {
            try
            {
                CreateQueues();
                LoadClients();

#if !TEST
                // Iniciar UDP server.
                StartHbReceiver();

                // Iniciar Queue manager Thread.
                // No se inicia el Thread si se realizan Unit Tests.
                _continueThread = true;
                _queueManager = new Thread(QueueManagerThreadProcess);
                _queueManager.Name = "QueueManager";
                _queueManager.Start();
#endif
                // Notificar inicio modulo.
                MessageBus.Send(new ReplyOK());
            }
            catch (Exception ex)
            {
                MessageBus.Send(new ReplyError(ex.Message));
            }

        }

        private void StopComponent()
        {
            // Detener UDP server.
            StopHbReceiver();


            _startList = null;
            _workList = null;
            _timeOutList = null;
            _recoverList = null;
            _deadList = null;
            _pausedList = null;
            _clientList = null;

            try
            {
                // Detener Thread.
                _continueThread = false;
                _queueManager.Join();

            }
            catch (Exception)
            {
            }
            finally
            {
                // Notificar fin modulo.
                MessageBus.Send(new ReplyStop());
            }
        }

        private void StartHbReceiver()
        {
 
            HbReceiver.Start(_pipeNames, _udpServerIP, _udpServerPort);
            Builder.Output(string.Format(ClassName + ": Udp Server iniciado en Ip: {0} Puerto: {1}", _udpServerIP, _udpServerPort), System.Diagnostics.TraceEventType.Verbose);
        }

        private void StopHbReceiver()
        {
            
            HbReceiver.Stop();
            Builder.Output(ClassName + ": Udp Server detenido.", System.Diagnostics.TraceEventType.Verbose);
        }

        private void CreateQueues()
        {
            // crear lists
            _startList = new List<ClientData>();
            _workList = new List<ClientData>();
            _timeOutList = new List<ClientData>();
            _recoverList = new List<ClientData>();
            _deadList = new List<ClientData>();
            _pausedList = new List<ClientData>();
        }

        private void LoadClients()
        {

            if (_clientList.Count > 0)
            {
                List<string> names = new List<string>();
                string piport = string.Empty;

                // Creando listas de  trabajo
                foreach (var item in _clientList.List)
                {
                    ClientData client = item;

                    // Crear lista de nombres de cola para clientes Pipe
                    //TODO La lista de pipe names se genera como info de cliente, debe estar en SystemConfigData
                    if (client.IdType == ClientIdType.KeyByPipe)
                    {
                        names.Add(client.Pipe);
                        piport = string.Format(" Pipe: {0}", client.Pipe);
                    }
                    else
                    {
                        piport = string.Format(" Puerto: {0}", client.Port);
                    }

                    ClientMoveToStartList(client);
                    Builder.Output(string.Format("{0}: Cargado cliente: {1} Id: {2} {3}", ClassName, client.Name, client.Id, piport), System.Diagnostics.TraceEventType.Verbose);
                }

                //Pasando lista a arreglo
                if (names.Count > 0)
                    _pipeNames = names.ToArray();
                else
                    _pipeNames = new string[] { Constants.PipeName };
            }

        }
        private void ClientMoveToStartList(ClientData client)
        {
            client.Status = ClientStatus.Inicial;
            client.StartTime = DateTime.Now;
            // se resetea contador de reinicios al comenzar monitoreo
            client.RestartCount = 0;
            client.RestartCountVolatil = 0;

            _startList.Add(client);
        }

#endregion
#endregion
    }
}