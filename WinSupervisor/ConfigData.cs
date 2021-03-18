#region Descripción
/*
    Implementa la persistencia de objetos mediante serialización.
    Contiene los datos de configuracion del sistema.
*/
#endregion

#region Using
using Monitor.Shared;
using System;
using System.Linq;
#endregion

namespace Monitor.Supervisor
{
    //  Contiene los datos de configuracion del sistema. Es Serializable para almacenarse en archivo.
    [Serializable]
    public class SupervisorData
    {

        // Constructor
        public SupervisorData()
        {
            _hbq = new FixedSizedQueue<string>(QueueSize);
            ConnectionAttempts = Constants.ConnectionAttempts;
            ServerUrl = Constants.ZyanServerUrl;
        }


        public int ConnectionAttempts
        { get; set; }


        private const int _qSizeDefault = 12;
        private int _qSize;

        public int QueueSize
        {
            get
            {
                if (_qSize == 0)
                    _qSize = _qSizeDefault;

                return _qSize;

            }
            set
            {
                _qSize = value;
                _hbq.MaxSize = value;
            }
        }

        private string _lastServerUrl;
        private FixedSizedQueue<string> _hbq;
        public string ServerUrl
        {
            get
            {
                string url = string.Empty;

                if (null != _lastServerUrl)
                    url = _lastServerUrl;
                else if (ServerUrlList.Count() > 0)
                        url = ServerUrlList[0];

                return url;
            }
            set
            {
                _lastServerUrl = value;

                if (!ServerUrlList.Contains(value))
                    _hbq.Enqueue(value);
            }
        }

        public string[] ServerUrlList
        {
            get
            {
                return _hbq.GetSnapshot();
            }
        }

    }

    

    // Mensaje que contiene Configuracion del Supervisor
    public class SendSupervisorData
    {
        // Propiedad
        public SupervisorData Data { get; private set; }

        // Constructor
        public SendSupervisorData(SupervisorData data)
        { Data = data; }
    }
}
