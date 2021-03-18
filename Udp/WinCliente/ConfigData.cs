using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinClienteUdp
{
    [Serializable]
    class ConfigData
    {
        public string IpServidor { get; set; }
        public int PuertoServidor { get; set; }
        public int PuertoLocal { get; set; }
        public int Intervalo { get; set; }
        public string ClientID { get; set; }
        public bool UseSerial { get; set; }
        public string Format { get; set; }
        public bool UseId { get; set; }


        public ConfigData()
        {
            IpServidor = Constants.ServerIpAdr; //  "127.0.0.1";
            PuertoServidor = Constants.UdpServerPort;
            PuertoLocal = 8001;
            Intervalo = 9;

            ClientID = "CLT001";
            UseSerial = false;
            Format = null; // "dd/MMM/yyyy HH:mm:ss";
        }

    }
}
