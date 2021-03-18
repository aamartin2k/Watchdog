#region Descripción
/*
    Implementa un receptor de Heartbeat mediante protocolo Pipe.

    El metodo ReadOp convierte el arreglo de bytes recibidos de la conexión en una instancia
    de la clase HeartBeat, le incorpora datos del Endpoint remoto ( direccion Ip y puerto local)
    y lo envia al MessageBus para ser recibido por un receptor registrado.
*/
#endregion

#region Using
using System;
using System.IO.Pipes;
#endregion

namespace Monitor.Shared.Heartbeat
{
    /// <summary>
    /// Implementa un receptor de Heartbeat mediante protocolo Pipe.  Versión estática.
    /// </summary>
    static public class HbReceiverPipe
    {
        // Declaraciones
        static private NamedPipeServerStream _pipeServer;

        private const int maxConn = 20;
        private const int bufferSize = 50;
        static private byte[] buffer = new byte[bufferSize];
        static bool keepLoop;


        static public void StopPipeServer()
        {
            keepLoop = false;

            if (_pipeServer != null)
            {
                _pipeServer.Close();
                _pipeServer = null;
            }
        }

        static public void StartPipeServe(string name)
        {
            _pipeServer = new NamedPipeServerStream(name, PipeDirection.In, maxConn, PipeTransmissionMode.Message, PipeOptions.Asynchronous, bufferSize, bufferSize);

            int numBytes = 0;
            keepLoop = true;

            while (keepLoop)
            {
                string message = String.Empty;
                buffer = new byte[bufferSize];

                _pipeServer.WaitForConnection();

                do
                {
                    numBytes = _pipeServer.Read(buffer, 0, bufferSize);
                    //Console.WriteLine("LEidos: " + numBytes.ToString());
                } while (numBytes != 0);

                HeartBeat hb = HeartBeat.CreateHeartBeat(buffer);
                message = string.Format("Cliente: {0}  Pipe: {1}  TS: {2}  Serial: {3}", hb.ClientID, name, hb.Timestamp, hb.Serial);
                Console.WriteLine(message);

                _pipeServer.Disconnect();

                
                SendHeartbeat hbMessage = new SendHeartbeat(hb);

                hbMessage.SenderType = HeartbeatSenderType.SenderPipe;
                hbMessage.PipeName = name;

                // Enviar Heartbeat 
                MessageBus.Send(hbMessage);
            }
        }

    }
}
