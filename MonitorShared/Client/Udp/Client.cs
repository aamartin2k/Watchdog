#region Descripción
/*
    Implementa un cliente de comunicaciones basado en Socket UDP.
*/
#endregion

#region Using
using Monitor.Shared.Interfaces;
using Monitor.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Monitor.Shared.Client.Udp
{
  
    public class UdpClient : ICommunicationClient, IDisposable
    {
        #region Declaraciones
        private readonly Socket _skUdpClient;
        private readonly EndPoint _epLocal;
        private readonly EndPoint _epRemote;
        private readonly Encoding _encoder;
        #endregion

        #region Constructor
        
        public UdpClient(IPAddress remIp, int remPort, int locPort, Encoding encoder)
        {
            _encoder = encoder;

            // Iniciando Socket
            _skUdpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _skUdpClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            _epLocal = new IPEndPoint(IPAddress.Any, locPort);
            _epRemote = new IPEndPoint(remIp, remPort);
            
        }

        #endregion

        #region Implementación ICommunicationClient

        /// <summary>
        /// Starts the client. Connects to the server.
        /// </summary>
        public void Start()
        {

            try
            {
                _skUdpClient.Bind(_epLocal);
                _skUdpClient.Connect(_epRemote);

            }
            catch (Exception ex)
            {
                //TODO Set log
                throw;
            }
        }

        /// <summary>
        /// Stops the client. Waits for pipe drain, closes and disposes it.
        /// </summary>
        public void Stop()
        {
            try
            {
                
                if (_skUdpClient != null)
                {
                    _skUdpClient.Shutdown(SocketShutdown.Send);
                }
            }
            finally
            {
                _skUdpClient.Close();
                _skUdpClient.Dispose();
            }
        }

        public Task<TaskResult> SendMessage(string message)
        {
            var taskCompletionSource = new TaskCompletionSource<TaskResult>();

            if (_skUdpClient.Connected)
            {

                byte[] buffer = _encoder.GetBytes(message);

                _skUdpClient.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        taskCompletionSource.SetResult(EndWriteCallBack(asyncResult));
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.SetException(ex);
                    }

                }, null);
            }
            

            return taskCompletionSource.Task;
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// This callback is called when the BeginWrite operation is completed.
        /// It can be called whether the connection is valid or not.
        /// </summary>
        /// <param name="asyncResult"></param>
        private TaskResult EndWriteCallBack(IAsyncResult asyncResult)
        {
            _skUdpClient.EndSend(asyncResult);
            return new TaskResult { IsSuccess = true };
        }

        #endregion

        #region Implementación IDisposable
        public void Dispose()
        {
            _skUdpClient.Dispose();
        }
        #endregion
    }
}
