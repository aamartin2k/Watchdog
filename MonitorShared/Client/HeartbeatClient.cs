#region Descripción
/*
    Implementa un emisor de heartbeat que emplea UdpClient ó PipeClient
    como cliente de comunicación mediante la interfaz ICommunicationClient.
*/
#endregion

#region Using
using Monitor.Shared.Client.Pipe;
using Monitor.Shared.Client.Udp;
using Monitor.Shared.Heartbeat;
using Monitor.Shared.Interfaces;
using System;
using System.Net;
using System.Text;
using System.Timers;
#endregion


namespace Monitor.Shared.Client
{
    public class HeartbeatClient : IHeartbeatSender
    {
        #region Declaraciones
        private readonly HeartBeatGenerator _hbgen;
        private readonly ICommunicationClient _client;
        private readonly int _interval;
        private Timer _timer;
        #endregion

        #region Constructor
        #region Constructores para Clientes Pipe
        public HeartbeatClient(string pipe) : this(".",   // servidor local
           pipe,
           Constants.DefaultPipeTimeout,       
           new UTF8Encoding(),
           Constants.DefaultInterval,
           new HeartBeatGenerator())
        { }

        public HeartbeatClient(string server, string pipe) : this(server, pipe,
            Constants.DefaultPipeTimeout,       //  3 minutos
            new UTF8Encoding(),
            Constants.DefaultInterval,
            new HeartBeatGenerator())
        { }

        public HeartbeatClient(string server, string pipe, int timeout) : this(server, pipe, timeout,
            new UTF8Encoding(),
            Constants.DefaultInterval,
            new HeartBeatGenerator())
        { }

        public HeartbeatClient(string server, string pipe, int timeout, Encoding encoder) : this(server, pipe, timeout, encoder,
            Constants.DefaultInterval,
            new HeartBeatGenerator())
        { }

        public HeartbeatClient(string server, string pipe, int timeout, Encoding encoder, int interval) : this(server, pipe, timeout, encoder, interval,
            new HeartBeatGenerator())
        { }

        /// <summary>
        /// Constructor for PipeClient.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="pipe"></param>
        /// <param name="timeout"></param>
        /// <param name="encoder"></param>
        /// <param name="interval"></param>
        /// <param name="hbgen"></param>
        public HeartbeatClient(string server, string pipe, int timeout, 
                              Encoding encoder, int interval, HeartBeatGenerator hbgen)
        {
            // Check parameters
            if (server == null)
            {
                throw new ArgumentNullException("server", "Se necesita un nombre de servidor.");
            }
            if (pipe == null)
            {
                throw new ArgumentNullException("pipe", "Se necesita un nombre de Pipe.");
            }
            // Timeout
            if (timeout == 0)
            {
                throw new ArgumentOutOfRangeException("timeout", " El  valor debe ser mayor que cero.");
            }
            //  Encoder
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder", "Se necesita un codificador de caracteres.");
            }
            // Intervalo
            if (interval == 0)
            {
                throw new ArgumentOutOfRangeException("interval", " El  valor debe ser mayor que cero.");
            }
            // HeartBeatGenerator
            if (hbgen == null)
            {
                throw new ArgumentNullException("hbgen", "Se necesita un generador de heartbeat.");
            }
            // Check parameters

            _interval = interval * 1000;        // convertir segundos recibidos a milisegundos
            _hbgen = hbgen;
            timeout = timeout * 60  * 1000;     // convertir minutos recibidos a milisegundos
            _client = new PipeClient(server, pipe, timeout, encoder); ;
            
        }
        #endregion
        #region Constructores para Clientes UDP

        public HeartbeatClient(string ipad, int remPort, int locPort) : this(ipad, remPort, locPort,
            new UTF8Encoding(),
            60,
            new HeartBeatGenerator())
        { }

        public HeartbeatClient(string ipad, int remPort, int locPort, Encoding encoder) : this(ipad, remPort, locPort, encoder,
            60,
            new HeartBeatGenerator())
        { }

        public HeartbeatClient(string ipad, int remPort, int locPort, Encoding encoder, int interval) : this(ipad, remPort, locPort, encoder, interval,
           new HeartBeatGenerator())
        { }

        /// <summary> 
        /// Constructor for UdpClient
        /// </summary>
        /// <param name="ipad"></param>
        /// <param name="remPort">A valid port value from 1024 to 65535</param>
        /// <param name="locPort">A valid port value from 1024 to 65535</param>
        /// <param name="encoder"></param>
        /// <param name="interval"></param>
        /// <param name="hbgen"></param>
        public HeartbeatClient(string ipad, int remPort, int locPort, Encoding encoder, 
                               int interval, HeartBeatGenerator hbgen)
        {
            // Check paramters
            // IP Address
            if (ipad == null)
            {
                throw new ArgumentNullException("ipad", "Se necesita una dirección IP.");
            }

            IPAddress tempIP = null;
            if (!IPAddress.TryParse(ipad, out tempIP))
            {
                throw new ArgumentException("La dirección IP no es válida.", "ipad");
            }
            // Puerto Remoto
            if (remPort < 1024 || remPort > 65535)
            {
                throw new ArgumentOutOfRangeException("remPort", " El  valor debe estar entre 1024 y 65535.");
            }
            // Puerto Local
            if (locPort != 0 & (locPort < 1024 || remPort > 65535))
            {
                throw new ArgumentOutOfRangeException("locPort", " El  valor debe estar entre 1024 y 65535.");
            }
            //  Encoder
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder", "Se necesita un codificador de caracteres.");
            }
            // Intervalo
            if (interval == 0)
            {
                throw new ArgumentOutOfRangeException("interval", " El  valor debe ser mayor que cero.");
            }
            // HeartBeatGenerator
            if (hbgen == null)
            {
                throw new ArgumentNullException("hbgen", "Se necesita un generador de heartbeat.");
            }
            //End Check paramters

            _interval = interval * 1000;
            _hbgen = hbgen;

            _client = new UdpClient(tempIP, remPort, locPort, encoder);

        }
        #endregion
        #endregion

        #region IHeartbeatSender implementation

       
        public void StartTimer()
        {
            try
            {
                _client.Start();

                _timer = new Timer();
                _timer.Interval = _interval;
                _timer.AutoReset = true;
                _timer.Elapsed += new ElapsedEventHandler(SendHeartbeat);
                _timer.Start();
            }
            catch (Exception ex)
            {
                throw;
            }

           
        }

        public void StopTimer()
        {
            _client.Stop();

            _timer.Stop();
            _timer.Dispose();
        }

        #endregion

        #region Métodos Privados
        private void SendHeartbeat(object sender, ElapsedEventArgs e)
        {
            string datahb = _hbgen.GeneraHeartbeatString();

            _client.SendMessage(datahb);
        }



        #endregion

    }
}
