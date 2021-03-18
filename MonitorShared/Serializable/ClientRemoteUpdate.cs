#region Descripción
/*
    Implementa los datos para modificar la configuración del clientes de forma remota, mediante
   el programa Supervisor.
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion


namespace Monitor.Shared
{
    /// <summary>
    /// Implementa los datos para modificar la configuración del clientes de forma remota, mediante
    /// el programa Supervisor.
    /// El atributo  [Serializable] es necesario para Zyan Frx y serializacion a disco.
    /// </summary>
    [Serializable]
    public class ClientRemoteUpdate
    {

        public Guid ClientId { get; set; }

        public string Name { get; set; }
        public ClientIdType IdType { get; set; }
        public string Id { get; set; }
        public string AppFilePath { get; set; }
        public string LogFilePath { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public bool MailEnabled { get; set; }
        public bool LogAttachEnabled { get; set; }
        public int QueueSize { get; set; }
    }
}
