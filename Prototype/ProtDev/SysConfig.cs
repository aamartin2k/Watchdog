using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ProtDev
{
    public enum ClientStatus { Inicial, Operacional, Reiniciado, Detenido, Muerto }

    [Serializable]
    public class SysConfig 
    {

        // Datos de correo electronico
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Password { get; set; }
        public string SMtpServer { get; set; }

        // datos del serv Zyan
        public string ZyanServerName { get; set; }
        public int ZyanServerPort { get; set; }

        // datos del serv Udp
        public int UdpServerPort { get; set; }

        // timeout de clientes despues de reiniciar (default 1 - 1.5 min)
        // debe ser mayor que el timeout de operacion (30 - 45 segs)
        public int TimeoutStartRestart { get; set; }
    }

    // Se usan varios
    [Serializable]
    public class ClientConfig 
    {
        /// <summary>
        /// Nombre para designar y diferenciar los registros.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nombre del archivo ejecutable, sin la extensión. Se emplea para terminar
        /// y reiniciar la aplicación mediante  System.Diagnostics.Process.
        /// </summary>
        public string AppName { get; set; }

        // campo de apoyo
        private string _appFilePath;
        /// <summary>
        /// Nombre y ruta al archivo de la aplicación.
        /// </summary>
        public string AppFilePath
        {
            get
            {
                return _appFilePath;
            }
            set
            {
                _appFilePath = value;
                AppName = System.IO.Path.GetFileNameWithoutExtension(_appFilePath);
            }
        }
        /// <summary>
        /// Nombre y ruta al archivo log.
        /// </summary>
        public string LogFilePath { get; set; }
        /// <summary>
        /// Puerto udp asignado para la comunicación entre la aplicación y el monitor.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Intervalo de tiempo que la aplicación debe cumplir, antes de declararla inactiva
        /// y proceder a su reinicio por el monitor. Valor en segundos.
        /// </summary>
        public double Timeout { get; set; }
        /// <summary>
        /// Valor booleano que indica si las notificaciones se realizan por correo electrónico.
        /// </summary>
        public bool MailEnabled { get; set; }
        /// <summary>
        /// Valor booleano que indica si se envia el archivo log adjunto en la notificacion por correo electrónico.
        /// </summary>
        public bool LogAttachEnabled { get; set; }

        // Datos de estado
        /// <summary>
        /// Enumeracion que describe los estados de la aplicación mientras es monitoreada.
        /// </summary>
        public ClientStatus Status { get; set; }
        /// <summary>
        /// Registro de la cantidad de veces que se ha reiniciado la aplicación mientras es monitoreada.
        /// El valor persiste entre sesiones de ejecución del monitor.
        /// </summary>
        public int RestartCount { get; set; }
        /// <summary>
        /// Registro de la hora en que el cliente entra a las colas y recibe HeartBeats,
        /// se emplea para calcular timeouts.
        /// </summary>
        public DateTime EnterTime { get; set; }
        // unir enter y receive
        //public DateTime ReceiveTime { get; set; }
        /// <summary>
        /// Fecha/Hora en que la aplicacion comienza a ser monitoreada. Es actualizada 
        /// cuando se reinicia la aplicación. El valor persiste entre sesiones de ejecución del monitor. 
        /// Se toma como base para calcular el tiempo de actividad ininterrumpida de la aplicación.
        /// </summary>
        public Nullable<DateTime> StartTime { get; set; }
       

        /// <summary>
        /// Tiempo que la aplicación monitoreada se mantiene en comunicación con el monitor. 
        /// </summary>
        /// <remarks>Se puede interpretar como el tiempo de actividad ininterrupida de la aplicación, 
        /// pero no exactamente. Si la aplicación se detiene o reinicia mientras el monitor esta apagado, no
        /// se tomará en cuenta ese tiempo.</remarks>
        public TimeSpan UptimeTS
        {
            get
            {
                if (StartTime != null)
                    return DateTime.Now.Subtract(StartTime.Value);
                else
                    return TimeSpan.Zero;
            }
        }

        public string Uptime
        {
            get
            {
                TimeSpan lapso = UptimeTS;
                // Devolver cadena formateada en dependencia del valor
                if (lapso.Days > 0)
                {
                    object[] args = { lapso.Days, lapso.Hours, lapso.Minutes, lapso.Seconds };
                    return String.Format("{0} d {1} h {2} m {3} s.", args);
                }

                if (lapso.Hours > 0)
                    return String.Format("{0} h {1} m {2} s.", lapso.Hours, lapso.Minutes, lapso.Seconds);

                return String.Format("{0} m {1} s.", lapso.Minutes, lapso.Seconds);
            }
        }


        private const int _qSizeDefault = 32;
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
                // _hbq.MaxSize = value;
            }
        }

        public string HeartBeat { get; set; }

        public string[] HeartBeatList { get; set; }
    }

    [Serializable]
    public class ConfigRoot
    { 
        // Index para almacenamiento y recuperacio
        public SysConfig  SysConfigIndex;
        public List<ClientConfig> ClientConfigNameIndex;


        public ConfigRoot()
        {
            // Construyendo ref por defecto
            SysConfigIndex = new SysConfig();
            ClientConfigNameIndex = new List<ClientConfig>();

        }

    }

}
