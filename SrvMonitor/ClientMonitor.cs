
#region Descripción
/*
    

*/
#endregion

#region Using
using System;

using Monitor.Shared;
using System.Timers;

#endregion


namespace ConMonitor
{
    /// <summary>
    /// Realiza el ciclo de monitoreo, detencion y reinicio de la app
    /// monitoreada.  Primera variante. Instanciar un monitor para cada cliente
    /// </summary>
    internal class ClientMonitor
    {
        #region Declaraciones

        //private Timer _timer;
        private ClientConfigData _cfg;
        private string _hb;

        internal DateTime IniTime = DateTime.MinValue;

        #endregion
        
        #region Propiedades

        #region Propiedades Públicas
        // Referencia a datos de configuracion del cliente
        public ClientConfigData Data
        {
            get
            {
                return _cfg;
            }
            set
            {
                _cfg = value;
                Start();
            }
        }
        // Referencia a datos de estado del cliente
        public ClientStatusData Status { get; set; }
        
        // Datos consultables del cliente
        public string Heartbeat 
        {
            get 
            {
                return _hb;
            } 
            set 
            {
                _hb = value;
                IniTime = DateTime.Now;

                //_timer.Start();

                // DEbug
                //Program.WriteAtBase(string.Format("CM: {0}  HB {1}" , _cfg.Name , _hb));
            } 
        }


      
        #endregion

        #region Propiedades Privadas

        #endregion

        #endregion


        #region Métodos

        #region Métodos Públicos

        internal void Start()
        { 
            // inicializacion

            // Crear timeout timer
            //_timer = new Timer();
            //_timer.Interval = Data.Timeout * 1000 ;  //  llevar a milisegundos
            //_timer.AutoReset = true;
            //_timer.Elapsed += _timer_Elapsed;
            // Proc timedEvent

        }


        #region Envio de eventos Out

        #endregion

        #region Manejo de eventos In

        #endregion


        #endregion

        #region Métodos Privados

        // Ciclo basico. Si se ejecuta, el cliente monitoreado no se conecta en el tiempo timeout
        // Se detendra y reinciara.
         void _timer_Elapsed(object sender, ElapsedEventArgs e)
         {
             //lock (_timer)
             //{
             //    _timer.Stop();

             //    // log

             //    // kill

             //    // restart

             //    // log
             //    Program.WriteAtBase(string.Format("CM: {0}  Timed Out!!", _cfg.Name));
             //    _timer.Start();
             //}
         }

        #endregion

        #endregion

    }
}
