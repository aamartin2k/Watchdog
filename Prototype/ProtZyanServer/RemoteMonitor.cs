using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Monitor.Shared;
using System.Diagnostics;

namespace ProtZyanServer
{
    internal class RemoteMonitor : IMonitor
    {
        public Action<RemReplyConsoleText> Out_SendConsoleText
        { get; set; }

        public Action<RemReplyConsoleText> Out_SendError
        { get; set; }


        public Action<RemReplyClientData> Out_SendClientConfig 
        { get; set; }

        public Action<RemReplySystemData> Out_SendSystemConfigData
        { get; set; }

        public Action<RemReplyQueueInfo> Out_SendQueueData
        { get; set; }

        public Action<RemReplyLogFile> Out_SendLogFile
        { get; set; }

       

        public void In_RequestClientData(RemReqClientData request)
        {
            Console.WriteLine("Receive Request ClientData from Remote Client.");

            Dictionary<string, ClientData>  _cltCfgData = new Dictionary<string, ClientData>();

            ClientData clt = new ClientData();
            clt.Id = "qwer";
            clt.Name = "Name1";
            _cltCfgData.Add(clt.Id, clt);

            clt = new ClientData();
            clt.Id = "45trt";
            clt.Name = "Name2";
            _cltCfgData.Add(clt.Id, clt);

            ClientDataManager cd = new ClientDataManager();

            Out_SendClientConfig(new RemReplyClientData(cd));
        }

        public void In_RequestConsoleText(RemReqConsoleText request)
        {
            Console.WriteLine("Receive Request ConsoleText from Remote Client.");

            Out_SendConsoleText(new RemReplyConsoleText(new RemoteConsoleText("Texto de Consola: PRUEBA", TraceEventType.Information )));

            Out_SendError(new RemReplyConsoleText(new RemoteConsoleText("Texto de Error: PRUEBA", TraceEventType.Critical)) );
        }

        public void In_RequestSystemData(RemReqSystemData  request)
        {
            Console.WriteLine("Receive System Config from Remote Client.");
            
            SystemConfigData scfg = new SystemConfigData();
            scfg.ZyanServerName = "Servidor Remoto de Prueba WDAltMonitor";

            Out_SendSystemConfigData(new RemReplySystemData(scfg));
        }

        public void In_RequestLogFile(RemReqLogFile request)
        {
            Console.WriteLine("Receive Request Log File from Remote Client: " + request.LogFile);
            try
            {
                if (System.IO.File.Exists(request.LogFile))
                {
                    FileStream fs = new FileStream(request.LogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Console.WriteLine("Archivo: " + fs.Name + "  Long: " + fs.Length);

                    Out_SendLogFile(new RemReplyLogFile(fs));

                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        public void In_RequestQueueData(RemReqQueueData request)
        {
            Console.WriteLine("Receive Request QueueData from Remote Client.");
            Out_SendQueueData(new RemReplyQueueInfo(null));
        }

        public void In_RequestClientUpdate(RemReqUpdateClient request)
        {
            throw new NotImplementedException();
        }

        public void In_RequestClientDelete(RemReqDeleteClient request)
        {
            throw new NotImplementedException();
        }
    }
}
