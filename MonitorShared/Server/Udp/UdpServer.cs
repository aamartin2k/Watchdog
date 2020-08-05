#region Descripción
/*
    Implementa un servidor de comunicaciones basado en Socket UDP.
*/
#endregion

#region Using
using Monitor.Shared.Interfaces;
using Monitor.Shared.Utilities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
#endregion

namespace Monitor.Shared.Server.Udp
{
    internal class UdpServer : ICommunicationServer
    {
        #region Declaraciones
        private const int bufferSize = 80;
        private byte[] buffer = new byte[bufferSize];

        private Socket _skListener;
        private readonly EndPoint _epLocal;
        private  EndPoint _epRemote;
        private readonly Encoding _encoder;
        private string _clientEndpoint;

        #endregion

        #region Constructor

        public UdpServer(string ipadr, int port, Encoding encoder)
        {
            _encoder = encoder;

            if (_skListener != null)
                Stop();

            _skListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _skListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPAddress locIp = IPAddress.Parse(ipadr);

            _epLocal = new IPEndPoint(locIp, port);
            _epRemote = new IPEndPoint(IPAddress.Any, 0);

           
        }

        #endregion

        #region Eventos

        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        #endregion

        #region Implementación ICommunicationServer

    
        public void Start()
        {
            _skListener.Bind(_epLocal);
            // Comenzar recepcion asíncrona.
            _skListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref _epRemote, new AsyncCallback(ReadOp), buffer);

        }

        public void Stop()
        {
            if (_skListener != null)
            {
                _skListener.Shutdown(SocketShutdown.Receive);
                _skListener.Close();
                _skListener = null;
            }
        }

        #endregion

        #region Métodos Privados

        private void ReadOp(IAsyncResult ares)
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
                    // text from bytes
                    text = _encoder.GetString((byte[])ares.AsyncState);

                    ipRemoteEp = (IPEndPoint)epRemote;
                    _clientEndpoint = ipRemoteEp.Address.ToString() + Constants.EndpointSeparator + ipRemoteEp.Port.ToString();

                    OnMessageReceived(text);
                    /*
                    HeartBeat hb = HeartBeat.CreateHeartBeat((byte[])ares.AsyncState);

                    ipRemoteEp = (IPEndPoint)epRemote;

                    SendHeartbeat message = new SendHeartbeat(hb);

                    message.SenderType = HeartbeatSenderType.SenderUdp;
                    message.Port = ipRemoteEp.Port;
                    message.IpAddress = ipRemoteEp.Address.ToString();

                    // Enviar Heartbeat 
                    MessageBus.Send(message);
                    */
                }

                // Continuar lectura
                buffer = new byte[bufferSize];

                if (_skListener != null)
                    _skListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(ReadOp), buffer);

            }
            catch (Exception)
            {
                // Ignorar excepciones y continuar la lectura
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //throw;
            }



        }

        /// <summary>
        /// This method fires MessageReceivedEvent with the given message
        /// </summary>
        private void OnMessageReceived(string message)
        {
            if (MessageReceivedEvent != null)
            {
                MessageReceivedEvent(this,
                    new MessageReceivedEventArgs
                    {
                        Message = message,
                        ClientId = _clientEndpoint
                    });
            }
        }

        
        #endregion
    }
}
