#region Descripción
/*
    Implementa un emisor de Heartbeat mediante protocolo Pipe.

    Esta clase puede emplearse de forma compilada, dentro de la dll Monitor.Shared o mediante codigo fuente. 
    Para esta ultima variante, se debe tener en cuenta que se necesita el codigo fuente de la clase HeartBeat.

   La forma de empleo prevista:

   No se requiere de crear instancias, ya que es una clase estática.
   Si se requiere enviar número consecutivo, asignar TRUE a  la propiedad UsarSerialHB.
   Si se requiere un formato especifico para el timestamp, asignarlo a la propiedad TimestampFormat.
   Si se requiere enviar identificador de cliente, asignar valor a la propiedad ClientID.

   Iniciar el envío de información mediante el método público IniciarHeartbeat. Se requiere conocer 
   el nombre del pipe del servidor.

   La periodización del envío se realiza mediante un componente System.Timers.Timer. 
*/
#endregion

#region Using
using System;
using System.Timers;
using System.IO.Pipes;
using System.Text;

#endregion

namespace Monitor.Shared.Heartbeat
{
    /// <summary>
    /// Implementa un emisor de Heartbeat mediante protocolo Pipe.  Versión estática.
    /// </summary>
    static public class HbSenderPipe
    {

        #region Declaraciones

        static private NamedPipeClientStream _pipeClient;
        static private Timer _timer;
        static private ASCIIEncoding _encoding;

        // Datos del servidor
        static private string _server;
        static private string _pipe;

       
        #endregion

        #region Propiedades
   

        #endregion

        #region Metodos publicos
        /// <summary>
        /// Inicia el envío periódico de los datos para monitoreo. Emplea un cliente Pipe.
        /// </summary>
        /// <param name="server">Nombre del servidor que recibe los datos.</param>
        /// <param name="pipe">Nombre del pipe que recibe los datos.</param>
        /// <param name="interv">Intervalo en segundos para repetir el envío.</param>
        static public void IniciarHeartbeat(string server, string pipe, int interv)
        {
            

            if (server == null)
            {
                throw new ArgumentNullException("server", "Se necesita un nombre de servidor.");
            }

            if (pipe == null)
            {
                throw new ArgumentNullException("pipe", "Se necesita un nombre de pipe.");
            }

            if (interv == 0)
            {
                throw new ArgumentOutOfRangeException("interv", " El  valor debe ser mayor que cero.");
            }

            // Inicio de cliente
            IniciaClientePipe(server, pipe, interv);
        }

        /// <summary>
        /// Detiene el envío periódico de los datos y libera los recursos asociados.
        /// </summary>
        static public void DetenerHeartbeat()
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;

            if (_pipeClient != null)
            {
                _pipeClient.Close();
                _pipeClient = null;
            }

            _encoding = null;
        }

        #endregion

        #region Metodos privados
        // Iniciar cliente y timer para enviar Heartbeat
        static private void IniciaClientePipe(string server, string pipe, int interv)
        {
            try
            {
                _encoding = new ASCIIEncoding();

                _server = server;
                _pipe = pipe;

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
            catch (Exception)
            {
                throw;
            }
        }

        // Ejecuta periodicamente el envío de heartbeat
        static private void EnviaHeartbeat(object sender, ElapsedEventArgs e)
        {
            var hbBytes = HeartBeatGenerator.GeneraHeartbeat();
            
            // Iniciando Pipe
            using (_pipeClient = new NamedPipeClientStream(_server, _pipe, PipeDirection.Out))
            {
                _pipeClient.Connect(2000);

                _pipeClient.Write(hbBytes, 0, hbBytes.Length);
                _pipeClient.Close();
            }
        }

        
  
        #endregion

    }
}
