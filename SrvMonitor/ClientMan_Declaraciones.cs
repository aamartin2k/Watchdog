#region Descripción
/*
    Implementa el monitoreo de clientes mediante un conjunto de colas que
    representan diferentes estados del cliente.
    Se almacenan los ultimos heartbeats de los clientes. Solicita la salva periodica de datos.
    Parcial Declaraciones.
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
        #region Declaraciones

        // Hilo para ejecutar proceso de monitoreo de clientes/colas.
        private Thread _queueManager;
        // Bool flag para mantener la ejecución del thread loop.
        private bool _continueThread;

        // Para uso de Log.
        private const string ClassName = "ClientManager";

        // Datos de configuracion necesarios.
        // Puerto de escucha para el servidor UDP.
        private int _udpServerPort;
        private string _udpServerIP;
        // Tiempo de espera por clientes iniciados/reiniciados.
        private int _systemTimeout;
        // Limite de reinicios
        private int _restCount;

        // Gestor de clientes.
        private ClientDataManager _clientList;

        // Objeto de sincronizacion
        static private AutoResetEvent _areClientUpdate = new AutoResetEvent(false);

        // Listas para gestionar monitoreo de clientes.
        private List<ClientData> _startList;
        private List<ClientData> _workList;
        private List<ClientData> _timeOutList;
        private List<ClientData> _recoverList;
        private List<ClientData> _deadList;
        private List<ClientData> _pausedList;

        // Contador de ciclos
        private int _count = 0;
        // debug, dejar en 240 para produccion;
        // Salvar la informacion de clientes a disco aproximadamente cada dos minutos
        // El thread se ejecuta cada 500 milisegundos,  120 cuentas aproxima a un minuto, 240 a dos...
#if DEBUG
        private const int _loops = 120;
#else
        private const int _loops = 240;
#endif

        #endregion
    }
}