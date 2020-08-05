#region Descripción
/*
    Implementa el monitoreo de clientes mediante servidor UDP y un conjunto de colas que
    representan diferentes estados del cliente.
    Se almacenan los ultimos heartbeats de los clientes. Solicita la salva periodica de datos.
    Parcial Gestion de las pruebas.
*/
#endregion

#region Using

using AMGS.Application.Utils.Log;
using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Threading;

#endregion

namespace Monitor.Service
{
    public partial class ClientManager
    {

        #region Testing

        /*  La realizacion de los test requiere ejecucion directa de los metodos. Esto va 
           en contra de la arquitectura del sistema, basada en intercambio de mensajes entre componentes.
           Para realizar los test se implementan metodos que exponen de modo publico otros metodos 
           privados del componente. Se emplea compilacion condicional para limitar la generación de
           estos metodos.
           Se requiere emplear la configuracion de proyecto denominada Test, donde se define el
           simbolo TEST.
        */

#if TEST
        
        // Gestion del componente.
        public void Start()
        { StartComponent(); }

        public void Stop()
        { StopComponent(); }

        // Entrada de la configuracion.
       
        public void SetSystemConfig(SendSystemConfig data)
        {
            ReceiveSystemConfig(data);
        }

        public void SetClientConfig(SendClientConfig data)
        {
            ReceiveClientConfig(data);
        }

        // Entrada de Heartbeats.
        public void ReceiveHB(SendHeartbeat req)
        {
            ReceiveHearbeat(req);
        }

        // Acceso a gestor interno de clientes
        public ClientDataManager ClientList { get { return _clientList; } }
        // Acceso a las colas.
        public List<ClientData> StartList { get { return _startList; } }
        public List<ClientData> WorkList { get { return _workList; } }
        public List<ClientData> TimeOutList { get { return _timeOutList; } }
        public List<ClientData> RecoverList { get { return _recoverList; } }
        public List<ClientData> DeadList { get { return _deadList; } }
        public List<ClientData> PausedList { get { return _pausedList; } }

        // Comprobacion de timeout.
        public void CheckOpTimeout()
        { CheckOperationTimeout(); }

        public void CheckInitTimeout()
        { CheckInitialTimeout(); }

        public void TryToRestart()
        { ClientTryToRestart(); }

        // Acceso al Thread y flag.
        public Thread QManagerTh { get { return _queueManager; } }
        public bool ContinueTh { get { return _continueThread; } }

        // Pausar cliente
        public void PauseClient(RequestPauseClient req)
        { DoPauseClient(req); }

        public void ResumeClient(RequestResumeClient req)
        { DoResumeClient(req); }

#endif
        #endregion


    }
}