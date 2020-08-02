#region Descripción
/*
    Implementa un emisor de Heartbeat mediante protocolo UDP.

    Esta clase puede emplearse de forma compilada, dentro de la dll Monitor.Shared o mediante codigo fuente. 
    Para esta ultima variante, se debe tener en cuenta que se necesita el codigo fuente de la clase HeartBeat.

   La forma de empleo prevista:

   No se requiere de crear instancias, ya que es una clase estática.
   Si se requiere enviar número consecutivo, asignar TRUE a  la propiedad UsarSerialHB.
   Si se requiere un formato especifico para el timestamp, asignarlo a la propiedad TimestampFormat.
   Si se requiere enviar identificador de cliente, asignar valor a la propiedad ClientID.

   Iniciar el envío de información mediante el método público IniciarHeartbeat. Se requiere conocer 
   la dirección Ip y el puerto UDP del servidor receptor. Si se requier establecer 
   el puerto local del cliente UDP, se puede asignar un valor real al argumento, de lo contrario 
   se pasa valor cero y se emplea un puerto dinámico.

   La periodización del envío se realiza mediante un componente System.Timers.Timer. 
*/
#endregion

#region Using
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
#endregion

namespace Monitor.Shared.Heartbeat
{
    /// <summary>
    /// Implementa un emisor de Heartbeat mediante protocolo UDP.  Versión estática.
    /// </summary>
    static public class HbSenderUdp
    {
        #region Declaraciones

        static private Socket _skUdpClient;
        static private Timer _timer;
        //static private ASCIIEncoding _encoding;

        // Contador para enviar numero consecutivo en el HeartBeat.
        //static private ulong _serialCount = 0;
        #endregion

        #region Propiedades
        ///// <summary>
        ///// Controla el envío de número consecutivo en el heartbeat. Si es true se envía el número, 
        ///// si es false no se envía. La acción por defecto es no enviar.
        ///// </summary>
        //static public bool UsarSerialHB { get; set; }

        ///// <summary>
        ///// Establece la identificación del cliente.
        ///// </summary>
        //static public string ClientID { get; set; }

        ///// <summary>
        ///// Establece el formato alternativo para el timestamp. Es opcional, de no establecerse, 
        ///// se usa el valor por defecto definido en la clase HearBeat.
        ///// </summary>
        //static public string TimestampFormat { get; set; }

        #endregion

        #region Metodos publicos
        /// <summary>
        /// Inicia el envío periódico de los datos para monitoreo. Emplea un cliente UDP.
        /// </summary>
        /// <param name="ipad">Dirección IP del servidor que recibe los datos.</param>
        /// <param name="remPort">Puerto UDP del servidor que recibe los datos.</param>
        /// <param name="locPort">Puerto UDP local desde donde se envía. Puede ser cero para emplear un puerto dinámico.</param>
        /// <param name="interv">Intervalo en segundos para repetir el envío.</param>
        static public void IniciarHeartbeat(string ipad, int remPort, int locPort, int interv)
        {
            // Validacion de argumentos
            //if (ClientID == null)
            //{
            //    throw new ArgumentNullException("ClientID", "Se necesita una ID.");
            //}

            if (ipad == null)
            {
                throw new ArgumentNullException("ipad", "Se necesita una dirección IP.");
            }

            IPAddress tempIP = null;
            if (ipad.ToUpper() != "ANY" && !IPAddress.TryParse(ipad, out tempIP))
            {
                throw new ArgumentException("La dirección IP no es válida.", "ipad");
            }
            if (tempIP == null)
            {
                tempIP = IPAddress.Any;
            }

            if (remPort < 1024 || remPort > 65535)
            {
                throw new ArgumentOutOfRangeException("remPort", " El  valor debe estar entre 1024 y 65535.");
            }

            if (locPort != 0 & (locPort < 1024 || remPort > 65535))
            {
                throw new ArgumentOutOfRangeException("locPort", " El  valor debe estar entre 1024 y 65535.");
            }

            if (interv == 0 )
            {
                throw new ArgumentOutOfRangeException("interv", " El  valor debe ser mayor que cero.");
            }

            // Inicio de cliente
            IniciaClienteUdp(tempIP, remPort, locPort, interv);
        }

        /// <summary>
        /// Detiene el envío periódico de los datos y libera los recursos asociados.
        /// </summary>
        static public void DetenerHeartbeat()
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;

            if (_skUdpClient != null)
            {
                _skUdpClient.Shutdown(SocketShutdown.Send);
                _skUdpClient.Close();
            }

            _skUdpClient = null;
            //_encoding = null;
        }

        #endregion

        #region Metodos privados
        // Iniciar cliente y timer para enviar Heartbeat
        static private void IniciaClienteUdp(IPAddress remIp, int remPort, int locPort, int interv)
        {
            try
            {
                // Iniciando Socket
                _skUdpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _skUdpClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                EndPoint epLocal = new IPEndPoint(IPAddress.Any, locPort);
                _skUdpClient.Bind(epLocal);

                //IPAddress remIp = IPAddress.Parse(ipad);
                EndPoint epRemote = new IPEndPoint(remIp, remPort);
                _skUdpClient.Connect(epRemote);

                //_encoding = new ASCIIEncoding();

                // Enviando primer HB antes de que transcurra el intervalo,
                // util durante depuracion para esperar menos...
                EnviaHeartbeat(null, null);

                // Iniciando Timer
                _timer = new Timer();
                _timer.Interval = interv * 1000;
                _timer.AutoReset = true;
                _timer.Elapsed += new ElapsedEventHandler(EnviaHeartbeat);
                _timer.Start();
            }
            catch (Exception )
            {
                throw;
            }
        }

        // Ejecuta periodicamente el envío de heartbeat
        static private void EnviaHeartbeat(object sender, ElapsedEventArgs e)
        {
            var hbBytes = HeartBeatGenerator.GeneraHeartbeat();
            _skUdpClient.Send(hbBytes);
        }

        //static private byte[] GeneraHeartbeat()
        //{
        //    HeartBeat hb;

        //    // Incorporando numero consecutivo si esta establecida la propiedad UsarSerialHB
        //    if (UsarSerialHB)
        //    {
        //        _serialCount++;
        //        // Evitar exception por sobrepasar valor maximo del tipo ulong (System.UInt64)
        //        if (_serialCount == ulong.MaxValue)
        //            _serialCount = 1;

        //        if (string.IsNullOrEmpty(TimestampFormat))
        //            hb = HeartBeat.CreateHeartBeat(ClientID, _serialCount);
        //        else
        //            hb = HeartBeat.CreateHeartBeat(ClientID, TimestampFormat, _serialCount);
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(TimestampFormat))
        //            hb = HeartBeat.CreateHeartBeat(ClientID);
        //        else
        //            hb = HeartBeat.CreateHeartBeat(ClientID, TimestampFormat);
        //    }

        //    return hb.ByteSequence;
        //}

        #endregion
    }

}
