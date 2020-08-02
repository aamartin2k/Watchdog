#region Descripción
/*
    Implementa la union de los datos de configuración para su persistencia.
*/
#endregion

#region Using
using System;
using System.Collections.Generic;
#endregion

namespace Monitor.Shared
{

    /// <summary>
    /// Implementa la union de los datos del Monitor y los clientes.
    /// </summary>
    [Serializable]
    public class SystemPlusClientData
    {
        // Propiedades
        public ClientDataManager ClientManager { get; private set; }
      
        public SystemConfigData SystemConfig { get; private set; }


        // Constructores
        public SystemPlusClientData()
        {
            ClientManager = new ClientDataManager();
            SystemConfig = new SystemConfigData();

            // creando valores default para System
            SystemConfig.UdpServerPort = Constants.UdpServerPort;
            SystemConfig.ZyanServerName = Constants.ZyanServerName;
            SystemConfig.ZyanServerPort = Constants.ZyanServerPort;
            SystemConfig.ServerIpAdr = Constants.ServerIpAdr;

            SystemConfig.TimeoutStartRestart = Constants.TimeoutStartRestart;
            SystemConfig.RestartAttemps = Constants.RestartAttemps;


        }

        public SystemPlusClientData(ClientDataManager clientMan, SystemConfigData sysconf)
        {
            ClientManager = clientMan;
            SystemConfig = sysconf;
        }

    }

   
}
