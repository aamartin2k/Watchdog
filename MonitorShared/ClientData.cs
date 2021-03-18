#region Descripción
/*
    Implementa los datos de configuración y estado de los clientes a monitorear.
*/
#endregion

#region Using
using System;
using System.Drawing;
using System.Collections.Generic;
#endregion

namespace Monitor.Shared
{
    /// <summary>
    /// Implementa los datos de configuración y estado de los programas (clientes) a monitorear.
    /// El atributo  [Serializable] es necesario para Zyan Frx y serializacion a disco.
    /// </summary>
    [Serializable]
    public class ClientData
    {
        // Datos de configuracion
        /// <summary>
        /// Clave unica para identificacion interna de los clientes.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Enum para establecer el elemento clave en la identificacion externa de los clientes.
        /// </summary>
        public ClientIdType IdType { get; set; }
        /// <summary>
        /// Clave opcional para diferenciar los clientes.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nombre para identificar el cliente.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nombre del archivo ejecutable, sin la extensión. Se emplea para terminar
        /// y reiniciar la aplicación mediante  System.Diagnostics.Process.
        /// </summary>
        public string AppName { get;  private set; }
        
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
        /// Puerto udp asignado para la comunicación entre la aplicación y el monitor. Tiene
        /// funcion de clave para identificacion externa en la primera implementacion de los clientes.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Intervalo de tiempo que la aplicación debe cumplir, antes de declararla inactiva
        /// y proceder a su reinicio por el monitor. Valor en segundos.
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Valor booleano que indica si las notificaciones se realizan por correo electrónico.
        /// </summary>
        public bool MailEnabled { get; set; }
        /// <summary>
        /// Valor booleano que indica si se envia el archivo log adjunto en la notificacion por correo electrónico.
        /// </summary>
        public bool LogAttachEnabled { get; set; }

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
        /// Registro de la cantidad de veces que se ha reiniciado la aplicación mientras es monitoreada.
        /// El valor persiste entre sesiones de ejecución del monitor.
        /// </summary>
        public int RestartCountVolatil { get; set; }
        /// <summary>
        /// Registro de la hora en que el cliente recibe HeartBeats,
        /// se emplea para calcular timeouts.
        /// </summary>
        public DateTime EnterTime { get; set; }
        
        /// <summary>
        /// Fecha/Hora en que la aplicacion comienza a ser monitoreada. Es actualizada 
        /// cuando se reinicia la aplicación. Se toma como base para calcular el tiempo de actividad
        /// ininterrumpida de la aplicación.
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
        private int _qSize ;

        private string _lastHB;
        private FixedSizedQueue<string> _hbq ;
        public string HeartBeat 
        {
            get
            {
                return _lastHB;
            }
            set 
            {
                _lastHB = value;
                
                _hbq.Enqueue(value);
         
                // Establecer tiempo de recepcion.
                EnterTime = DateTime.Now;
            }
        }

        public string[] HeartBeatList
        { get 
            {
                return _hbq.GetSnapshot();
            } 
        }

        /// <summary>
        /// Establece color para mostrar en formulario GUI.
        /// </summary>
        public Color Color
        {
            get
            {
                Color btColor;

                switch (Status)
                {
                    case ClientStatus.Inicial:
                        btColor = Color.White;
                        break;

                    case ClientStatus.Operacional:
                        btColor = Color.Green;
                        break;

                    case ClientStatus.Pausado:
                        btColor = Color.Yellow;
                        break;

                    case ClientStatus.Reiniciado:
                        btColor = Color.Gold;
                        break;

                    case ClientStatus.Atrasado:
                        btColor = Color.Red;
                        break;

                    case ClientStatus.Muerto:
                    default:
                        btColor = Color.Black;
                        break;
                }

                return btColor;
            }
        }

        // Constructor
        public ClientData()
        {
            _hbq = new FixedSizedQueue<string>(QueueSize);
        }

        public override string ToString()
        {
            return Name;
        }
    }









}
