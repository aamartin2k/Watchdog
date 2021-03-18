using System;
using System.Collections.Generic;
using System.Linq;
using Monitor.Shared;

namespace ConMonitor
{
    

    // Delegates para eventos
    // Config de sistema
    internal delegate void SystemConfigEventHandler(object sender, SystemConfigEventArgs e);
    // Lista de config de clientes
    internal delegate void ClientConfigListEventHandler(object sender, ClientConfigListEventArgs e);
    
    internal delegate void ClientTextEventHandler(object sender, ClientTextEventArgs e);

    
   
    public delegate void UniversalHandler();


    // Derivadas de EventArgs
    internal class SystemConfigEventArgs : EventArgs
    {
        public SystemConfigData Data;

        public SystemConfigEventArgs(SystemConfigData data)
        { Data = data; }
    }

    //internal class ClientStatusListEventArgs : EventArgs
    //{
    //    public Dictionary<string, ClientStatusData> List { get; set; }

    //    public ClientStatusListEventArgs(Dictionary<string, ClientStatusData> list)
    //    { List = list; }
    //}

    internal class ClientConfigListEventArgs : EventArgs
    {
        public Dictionary<string, ClientData> List;

        public ClientConfigListEventArgs(Dictionary<string, ClientData> list)
        { List = list; }
    }


    //internal class ClientConfigEventArgs : EventArgs
    //{
    //    public ClientData Data;

    //    public ClientConfigEventArgs(ClientData data)
    //    { Data = data; }
    //}


    internal class ClientTextEventArgs : System.EventArgs
    {
        public string Text { get; private set; }

        public ClientTextEventArgs(string text)
        { Text = text; }
    }
}
