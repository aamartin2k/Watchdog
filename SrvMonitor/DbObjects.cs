using System;
using System.Collections.Generic;
using System.Linq;
using Volante;
using Monitor.Shared;


namespace ConMonitor
{
    internal class DatabaseRoot : Persistent
    {
        // Index para Configuracion del sistema
        // Contiene un unico record
        public IIndex<string, SystemConfigData> SystemConfigIndex;
        // Index para Configuracion de los clientes
        public IIndex<string, ClientConfigData> ClientConfigIndex;
        // Index para Estado de los clientes
        public IIndex<string, ClientStatusData> ClientStatusIndex;
    }
}
