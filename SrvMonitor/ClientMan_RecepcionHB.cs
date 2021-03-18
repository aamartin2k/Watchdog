#region Descripción
/*
    Implementa el monitoreo de clientes mediante servidor UDP y un conjunto de colas que
    representan diferentes estados del cliente.
    Se almacenan los ultimos heartbeats de los clientes. Solicita la salva periodica de datos.
    
    Parcial Recepcion de Heartbeats.

     Mediante este metodo se recibe la informacion periodica que envian los clientes, representada 
    en la clase HeartBeat. Se recibe mediante un servidor UDP, pero el metodo es agnostico
    respecto a la forma concreta en que se reciben los heartbeats. 
    Esta marcado con modo de acceso Internal, puesto que solo es referenciado dentro del 
    assembly (especificamente en Builder, donde se registra como metodo receptor del mensaje SendHeartbeat).
    Los heartbeats son recibidos en un servidor UDP implementado en la clase estatica HbReceiverUdp
    que se encuentra en la DLL MonitorShared. En el metodo ReadOp se completa la recepcion asincrona
    y se crea un objeto HeartBeat con los datos recibidos por la red. Adicionalmente se le incorpora la 
    direccion Ip y el puerto UDP del cliente, para posibles verificaciones. Finalmente, ese objeto 
    se incorpora a un objeto mensaje SendHeartbeat y se envia al MessageBus, que lo entrega a este metodo.

    Para realizar los test, se necesita acceso publico al metodo, por lo que se encapsula una llamada al mismo
    en un metodo con modo de acceso Public.

*/
#endregion

#region Using

using AMGS.Application.Utils.Log;
using Monitor.Shared;
using Monitor.Shared.Heartbeat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;

#endregion

namespace Monitor.Service
{
    public partial class ClientManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        internal void ReceiveHearbeat(SendHeartbeat req)
        {
            string message;

            HeartBeat hb = req.Data;

            // Obteniendo datos del emisor 
            string senderIP = string.Empty;
            string senderPipe = string.Empty;
            int senderPort = 0;

            // Temp. Usar if para separar dos posibles senders hasta ahora. Si se incrementan, cambiar por switch..
            if (req.SenderType == HeartbeatSenderType.SenderUdp)
            {
                senderIP = req.IpAddress;
                senderPort = req.Port;
            }

            if (req.SenderType == HeartbeatSenderType.SenderPipe)
            {
                senderPipe = req.PipeName;
            }

            ClientData client = null;

            // Chequear ClientID.
            // Chequear primero si hb.id es null
            if (null == hb.ClientID)
            {
                // usar puerto
                // TODO Separar chequeos en dependencia de tipo de emisor
                if (_clientList.ContainsPort(senderPort))
                    client = _clientList.GetClient(senderPort);
            }
            else
            {
                // usar ID
                if (_clientList.ContainsId(hb.ClientID))
                    client = _clientList.GetClient(hb.ClientID);
            }

            
            if (client == null)
            {
                // Para Debug y advertencia
                message = string.Format(ClassName + ": Cliente NO REGISTRADO. IP: {0}  Port: {1} Client: {2} Text: {3}  Serial: {4}.", senderIP, senderPort, hb.ClientID, hb.Timestamp, hb.Serial);
                Builder.Output(message, TraceEventType.Warning);
                // Debug
                return;
                //ToDo Al terminar depuracion implementar log y notificacion 
                throw new Exception("Cliente no registrado.");
            }

            // DEBUG Output Hb recibido
            string nombre = client.Name;
            string id = hb.ClientID ?? "No establecida";
            string serial = hb.Serial ?? "No establecido";

            message = string.Format(ClassName + ": Recibe Cliente: {0} con Id {1} IP: {2} Puerto: {3} TS: {4} Serial: {5}.", nombre, id, senderIP, senderPort, hb.Timestamp, serial);
            Builder.Output(message, TraceEventType.Verbose);

            // Realizando comprobacion de puerto si el cliente lo especifica. Se puede comprobar la IP, pero no esta
            // incorporada en los datos del cliente.
            if (client.IdType == ClientIdType.KeyByUdpPort)
            {
                if (client.Port != senderPort)
                {
                    //TODO implementar notificacion.
                    message = string.Format(ClassName + ": Cliente {0} tiene configurado puerto UDP: {1} pero transmite por el {2}.", client.Name, client.Port, senderPort);
                    Builder.Output(message, TraceEventType.Warning);
                }
            }

            client.HeartBeat = string.IsNullOrEmpty(hb.Serial) ? hb.Timestamp : hb.Timestamp + "\t" + hb.Serial;

            if (client.Status == ClientStatus.Inicial)
            {
                ClientMoveToWorkList(client);
            }

            if (client.Status == ClientStatus.Reiniciado)
            {
                ClientRecoverToWorkList(client);
            }

            if (client.Status == ClientStatus.Muerto)
            {
                ClientRecoverFromDeadToWorkList(client);
            }
        }


        /// <summary>
        ///  Mueve al cliente de la lista de inicio a la de trabajo. Notifica que se ha comenzado
        ///  a monitorear al cliente.
        /// </summary>
        /// <param name="client">Referencia al objeto ClientData a mover.</param>
        private void ClientMoveToWorkList(ClientData client)
        {

            client.Status = ClientStatus.Operacional;
            //client.RestartCount = 0;
            client.RestartCountVolatil = 0;

            lock (_startList)
            {
                if (_startList.Contains(client))
                    _startList.Remove(client);
            }

            lock (_workList)
            {
                if (!_workList.Contains(client))
                    _workList.Add(client);
            }

            MessageBus.Send(new RequestSendEmail(EMessageAction.Operational, DateTime.Now, client));
            Builder.Output(string.Format(ClassName + ": Cliente {0} movido a cola de Trabajo.", client.Name), TraceEventType.Verbose);
        }

        /// <summary>
        ///  Mueve al cliente de la lista de recuperados a la de trabajo. Notifica que se ha comenzado
        ///  a monitorear al cliente. Incrementa el contador de reinicios del cliente.
        /// </summary>
        /// <param name="client">Referencia al objeto ClientData a mover.</param>
        private void ClientRecoverToWorkList(ClientData client)
        {
            //NOTE Se ha reiniciado la aplicación, se actualiza la fecha StartTime de forma incondicional.
            client.StartTime = DateTime.Now;
            
            lock (_recoverList)
            {
                if (_recoverList.Contains(client))
                    _recoverList.Remove(client);
            }

            Builder.Output(string.Format(ClassName + ": Cliente {0} reiniciado. Se continua monitoreo.", client.Name));

            ClientMoveToWorkList(client);
   
        }

        private void ClientRecoverFromDeadToWorkList(ClientData client)
        {
            //NOTE Se ha reiniciado la aplicación, se actualiza la fecha StartTime de forma incondicional.
            client.StartTime = DateTime.Now;

            lock (_deadList)
            {
                if (_deadList.Contains(client))
                    _deadList.Remove(client);
            }

            Builder.Output(string.Format(ClassName + ": Cliente {0} recuperado. Se continua monitoreo.", client.Name));

            ClientMoveToWorkList(client);

        }

    }
}