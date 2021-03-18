#region Descripción
/*
    Implementa un receptor de Heartbeat mediante protocolo UDP.

    El metodo ReadOp convierte el arreglo de bytes recibidos de la conexion en una instancia
    de la clase HeartBeat, le incorpora datos del Endpoint remoto ( direccion Ip y puerto local)
    y lo envia al MessageBus para ser recibido por un receptor registrado.
*/
#endregion

#region Using
using System;
using System.Net;
using System.Net.Sockets;
#endregion

namespace Monitor.Shared.Heartbeat
{
    /// <summary>
    /// Implementa un receptor de Heartbeat mediante protocolo UDP.  Versión estática.
    /// </summary>
    static public class HbReceiverUdp
    {
        // Declaraciones
        static private Socket _skListener;

        private const int bufferSize = 60;
        static private byte[] buffer = new byte[bufferSize];


        static public void StopUdpServer()
        {
            if (_skListener != null)
            {
                _skListener.Shutdown( SocketShutdown.Receive);
                _skListener.Close();
                _skListener = null;
            }
        }

        static public void StartUdpServer(string ipadr, int port)
        {
            if (_skListener != null)
                StopUdpServer();

            _skListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _skListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPAddress locIp = IPAddress.Parse(ipadr);

            EndPoint epLocal = new IPEndPoint(locIp, port);
            EndPoint epRemote = new IPEndPoint(IPAddress.Any, 0);

            _skListener.Bind(epLocal);  
            // Comenzar recepcion asíncrona.
            _skListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(ReadOp), buffer);

        }

        static private void ReadOp(IAsyncResult ares)
        {
            if (_skListener == null)
                return;

            string text = String.Empty;

            IPEndPoint ipRemoteEp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint epRemote = (EndPoint)ipRemoteEp;

            try
            {
                if (_skListener.EndReceiveFrom(ares, ref epRemote) > 0)
                {
                    HeartBeat hb = HeartBeat.CreateHeartBeat((byte[])ares.AsyncState);

                    ipRemoteEp = (IPEndPoint)epRemote;

                    SendHeartbeat message = new SendHeartbeat(hb);

                    message.SenderType = HeartbeatSenderType.SenderUdp;
                    message.Port = ipRemoteEp.Port;
                    message.IpAddress = ipRemoteEp.Address.ToString();

                    // Enviar Heartbeat 
                    MessageBus.Send(message);
                    
                }

                // Continuar lectura
                buffer = new byte[bufferSize];

                if (_skListener != null)
                    _skListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(ReadOp), buffer);

            }
            catch (Exception )
            {
                // Ignorar excepciones y continuar la lectura
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //throw;
            }

           

        }

        
    }
}
