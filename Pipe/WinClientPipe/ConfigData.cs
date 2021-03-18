using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinClientPipe
{
    [Serializable]
    class ConfigData
    {
        public string Servidor { get; set; }
        public string Pipe { get; set; }
     
        public int Intervalo { get; set; }
        public string ClientID { get; set; }
        public bool UseSerial { get; set; }
        public string Format { get; set; }
        public bool UseId { get; set; }


        public ConfigData()
        {
            Servidor = Constants.PipeServer;
            Pipe = Constants.PipeName;
            
            Intervalo = 9;

            ClientID = "CLT001";
            UseSerial = false;
            Format =  "dd/MMM/yyyy HH:mm:ss";
        }

    }
}
