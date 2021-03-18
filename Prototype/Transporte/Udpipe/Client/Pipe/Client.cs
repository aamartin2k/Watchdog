#region Descripción
/*
    Implementa un cliente de comunicaciones basado en NamedPipeClientStream.
*/
#endregion

#region Using
using Monitor.Shared.Interfaces;
using Monitor.Shared.Utilities;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Monitor.Shared.Client.Pipe
{
    internal class PipeClient : ICommunicationClient, IDisposable
    {
        #region Declaraciones

        private readonly NamedPipeClientStream _pipeClient;
        private readonly int _timeout;
        private readonly Encoding _encoder;
        
        #endregion

        #region Constructor

        public PipeClient(string server, string pipe, int timeout, Encoding encoder)
        {
            _timeout = timeout;
            _encoder = encoder;
            _pipeClient = new NamedPipeClientStream(server, pipe, PipeDirection.Out, PipeOptions.Asynchronous);
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
                _pipeClient.Connect(_timeout);
            }
            catch (Exception ex)
            {

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
                _pipeClient.WaitForPipeDrain();
            }
            finally
            {
                _pipeClient.Close();
                _pipeClient.Dispose();
            }
        }

        public Task<TaskResult> SendMessage(string message)
        {
            var taskCompletionSource = new TaskCompletionSource<TaskResult>();

            if (_pipeClient.IsConnected)
            {
                
                var buffer =  _encoder.GetBytes(message);
                _pipeClient.BeginWrite(buffer, 0, buffer.Length, asyncResult =>
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
            else
            {
                //Logger.Error("Cannot send message, pipe is not connected");
                throw new IOException("pipe is not connected");
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
            _pipeClient.EndWrite(asyncResult);
            _pipeClient.Flush();

            return new TaskResult { IsSuccess = true };
        }

        #endregion

        #region Implementación IDisposable
        public void Dispose()
        {
            _pipeClient.Dispose();
        }
        #endregion

       

    }
}
