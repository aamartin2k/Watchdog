#region Descripción
/*
    Implementa el monitoreo de clientes mediante servidor UDP y un conjunto de colas que
    representan diferentes estados del cliente.
    Se almacenan los ultimos heartbeats de los clientes. Solicita la salva periodica de datos.
    Parcial Manejo de las colas, monitoreo de clientes.

     Mediante este metodo se realiza el monitoreo del estado de los clientes. En dependencia de 
    las diferencias de tiempo calculadas, los clientes se mueven de una cola a otra.
            
    Al terminar el ciclo de ejecucion, se incrementa un contador, cuando este llegua 
    al limite establecido se envia una solicitud para salvar los datos. Asi se preserva la lista de los ultimos 
    heartbeats recibidos. 
    Se ejecuta en un hilo propio.  
*/
#endregion

#region Using

using AMGS.Application.Utils.Log;
using Monitor.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#endregion

namespace Monitor.Service
{
    public partial class ClientManager
    {
        /// <summary>
        /// Realiza los subprocesos de chequeo de los clientes, en un hilo propio.
        /// </summary>
        private void QueueManagerThreadProcess()
        {
            try
            {
                while (_continueThread)
                {
                    // Resetear para impedir edicion de clientes mientras se ejecuta el ciclo de procesos
                    _areClientUpdate.Reset();

                    // Ciclo para intentar reiniciar clientes en lista de timeouts
                    // Si se reinicia, se mueve a recover list, sino, se mueve a deadlist
                    ClientTryToRestart();

                    // Ciclo para detectar clientes que no envian hearbeat despues de reiniciar la aplicacion 
                    //
                    //  !!!!!       *******                    !!!!!!!!!       *********               !!!!!!!!!
                    // Importante: 
                    //  Este metodo debe invocarse siempre antes que CheckRecoverTimeout. De lo contrario
                    // los clientes sin enviar en mucho tiempo HB se mueven a la lista de Detenidos y se reinician
                    // y regresan a esta lista en circulo vicioso, sin ser detectados.
                    CheckRecoverDead();

                    // Ciclo para detectar timeouts durante la operacion
                    //  mueve de  _workList a _timeOutList si el intervalo es mayor que el timeout configurado en el cliente
                    CheckOperationTimeout();

                    // Ciclo para detectar  timeouts posteriores al reinicio de la aplicacion  
                    CheckRecoverTimeout();

                    // Ciclo para detectar timeouts al iniciar operacion
                    // mueve de _startList a _timeOutList si el intervalo es mayor que el valor configurado
                    // en el sistema como Timeout al Iniciar/Reiniciar.
                    CheckInitialTimeout();


                    // Contador de ciclos, indica salva periodica de los datos HB de clientes
                    if (_count++ > _loops)
                    {
                        _count = 0;
                        MessageBus.Send(new RequestSaveConfig());
                    }

                    // Seteando para permitir acceso a colas.
                    _areClientUpdate.Set();

                    Thread.Sleep(450);
                }

            }
            catch (Exception ex)
            {
                Builder.Output(ClassName + ": QueueManagerThreadProcess Error: " + ex.Message, TraceEventType.Error);
                Log.WriteEntry(ClassName, "QueueManagerThreadProcess", TraceEventType.Error, ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Detecta demoras en la respuesta del cliente, al iniciar su monitoreo. Si demora mas de 
        /// (valor configurable) en enviar heartbeats se elimina de la lista de inicio (_startList)
        /// y se coloca en la lista de detenidos (_timeOutList).
        /// El tiempo a tener en cuenta se lee de la propiedad StartTime, que se actualiza cuando el cliente
        /// se lleva a la lista de inicio.
        /// </summary>
        private void CheckInitialTimeout()
        {
            TimeSpan dif, timeout;
            try
            {
                List<ClientData> buffer = new List<ClientData>(_startList);
                foreach (var client in buffer)
                {
                    dif = DateTime.Now.Subtract(client.StartTime.Value);
                    timeout = TimeSpan.FromSeconds(_systemTimeout);

                    if (dif > timeout)
                    {
                        client.Status = ClientStatus.Atrasado;

                        // remove from _startList
                        lock (_startList)
                        {
                            _startList.Remove(client);
                        }

                        // add to _timeOutList
                        lock (_timeOutList)
                        {
                            if (!_timeOutList.Contains(client))
                                _timeOutList.Add(client);
                        }
                        MessageBus.Send(new RequestSendEmail(EMessageAction.Timeout, DateTime.Now, client));
                        Builder.Output(string.Format("Hilo de ejecucion: {0}.", Thread.CurrentThread.Name), TraceEventType.Verbose);
                        Builder.Output(string.Format(ClassName + ": Cliente: {0} sin iniciar reporte dentro de intervalo previsto, se mueve a cola de Detenido.", client.Name), TraceEventType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Log.WriteEntry(ClassName, "CheckInitialTimeout", TraceEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Detecta demoras en la respuesta del cliente, durante el ciclo normal de monitoreo. Si demora (mas 
        /// del valor establecido en la configuracion del cliente) en enviar heartbeats se elimina de la lista 
        /// de trabajo (_workList) y se coloca en la lista de detenidos (_timeOutList)
        /// </summary>
        private void CheckOperationTimeout()
        {
            TimeSpan dif, timeout;

            try
            {
                List<ClientData> buffer = new List<ClientData>(_workList);
                foreach (var client in buffer)
                {

                    dif = DateTime.Now.Subtract(client.EnterTime);
                    timeout = TimeSpan.FromSeconds(client.Timeout);

                    if (dif > timeout)  // si esta pasado de tiempo (dif)
                    {
                        client.Status = ClientStatus.Atrasado;

                        // remove from _workList
                        lock (_workList)
                        {
                            _workList.Remove(client);
                        }

                        // add to _timeOutList
                        lock (_timeOutList)
                        {
                            if (!_timeOutList.Contains(client))
                                _timeOutList.Add(client);
                        }

                        MessageBus.Send(new RequestSendEmail(EMessageAction.Timeout, DateTime.Now, client));
                        
                        Builder.Output(string.Format(ClassName + ": Cliente: {0} sin reportar dentro de intervalo previsto, se mueve a cola de Detenido.", client.Name), TraceEventType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Log.WriteEntry(ClassName, "CheckInitialTimeout", TraceEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Detecta demoras en la respuesta del cliente, despues de haber sido reiniciado. Si demora mas de un minuto
        /// (valor configurable) en enviar heartbeats se elimina de la lista de recuperacion (_recoverList)
        /// y se coloca en la lista de detenidos (_timeOutList)
        ///  El tiempo a tener en cuenta se lee de la propiedad EnterTime, que se actualiza cuando se recibe
        /// heartbeat del cliente.
        /// </summary>
        private void CheckRecoverTimeout()
        {
            TimeSpan dif, timeout;

            List<ClientData> buffer = new List<ClientData>(_recoverList);
            foreach (var client in buffer)
            {

                dif = DateTime.Now.Subtract(client.EnterTime);
                timeout = TimeSpan.FromSeconds(_systemTimeout);

                if (dif > timeout)
                {
                    client.Status = ClientStatus.Atrasado;

                    // remove from _recoverList
                    lock (_recoverList)
                    {
                        _recoverList.Remove(client);
                    }
                    // add to _timeOutList
                    lock (_timeOutList)
                    {
                        if (!_timeOutList.Contains(client))
                            _timeOutList.Add(client);
                    }

                    Builder.Output(string.Format(ClassName + ": Cliente: {0} sin iniciar reporte despues de reiniciado, se mueve a cola de Detenido.", client.Name), TraceEventType.Warning);
                }
            }

        }
        /// <summary>
        /// Detecta clientes reiniciados varias veces que no envian hearbeat. Si se ha reiniciado 
        /// mas del limite configurado y sigue en la lista de recuperacion (_recoverList) se declara fuera de control.
        /// De lo contrario oscilaria indefinidamente entre _recoverList y _timeOutList.
        /// </summary>
        private void CheckRecoverDead()
        {
            TimeSpan dif, timeout;

            List<ClientData> buffer = new List<ClientData>(_recoverList);
            foreach (var client in buffer)
            {

                dif = DateTime.Now.Subtract(client.EnterTime);
                timeout = TimeSpan.FromSeconds(_systemTimeout);

                if ((dif > timeout) && (client.RestartCountVolatil == _restCount))
                {
                    client.Status = ClientStatus.Muerto;

                    // remove from _recoverList
                    lock (_recoverList)
                    {
                        _recoverList.Remove(client);
                    }
                    // add to _timeOutList
                    lock (_deadList)
                    {
                        if (!_deadList.Contains(client))
                            _deadList.Add(client);
                    }

                    MessageBus.Send(new RequestSendEmail(EMessageAction.Dead, DateTime.Now, client));

                    Builder.Output(string.Format(ClassName + ": Cliente: {0} sin iniciar reporte despues de reiniciado, se mueve a cola de Detenido.", client.Name), TraceEventType.Critical);
                }
            }
        }

        /// <summary>
        /// Intenta reiniciar la aplicacion cliente. Si lo logra, el cliente se mueve 
        /// de _timeOutList a _recoverList. Si no, el cliente se mueve a _deadList y no se monitorea mas.
        /// </summary>
        private void ClientTryToRestart()
        {
            bool flag;

            List<ClientData> buffer = new List<ClientData>(_timeOutList);
            foreach (var client in buffer)
            {

                // Log
                Builder.Output(string.Format(ClassName + ": Intentando reiniciar Cliente: {0} ejecutable: {1}.", client.Name, client.AppFilePath), TraceEventType.Information);

                // Comprobar si esta activo
                flag = GetProcessByName(client.AppName);

                // kill
                if (flag)
                    KillProcessByName(client.AppName);

                // "WerFault"
                const string pname = "WerFault";
                flag = GetProcessByName(pname);
                if (flag)
                    KillProcessByName(pname);
                // Realizar pausa mientras app termina
                System.Threading.Thread.Sleep(40);

                // Restart
                flag = StartProcessByName(client.AppFilePath);
                // Realizar pausa mientras sistema inicia
                System.Threading.Thread.Sleep(40);

                // Comprobar si se inicio, mover a donde corresponda y sacar de la cola
                if (flag)
                    flag = GetProcessByName(client.AppName);

                // Remover de la lista incondicionalmente.
                _timeOutList.Remove(client);

                // Mover en dependencia de si se reinicio o no.
                if (flag)
                {
                    client.Status = ClientStatus.Reiniciado;
                    client.RestartCount++;
                    client.RestartCountVolatil++;
                    client.EnterTime = DateTime.Now;

                    lock (_recoverList)
                    {
                        if (!_recoverList.Contains(client))
                            _recoverList.Add(client);
                    }
                    MessageBus.Send(new RequestSendEmail(EMessageAction.Restart, DateTime.Now, client));
                
                    Builder.Output(string.Format(ClassName + ": Reiniciado Cliente: {0} ejecutable: {1}.", client.Name, client.AppFilePath), TraceEventType.Information);
                }
                else
                {
                    client.Status = ClientStatus.Muerto;

                    lock (_deadList)
                    {
                        if (!_deadList.Contains(client))
                            _deadList.Add(client);
                    }

                    MessageBus.Send(new RequestSendEmail(EMessageAction.Dead, DateTime.Now, client));
                    
                    Builder.Output(string.Format(ClassName + ": Imposible reiniciar Cliente: {0} ejecutable: {1}, movido a cola de Fuera de Control.", client.Name, client.AppFilePath), TraceEventType.Critical);
                }
            }
        }

        /// <summary>
        /// Comprueba si una aplicacion esta activa accediendo a su proceso.
        /// </summary>
        /// <param name="name">Nombre de la aplicacion a verificar.</param>
        /// <returns>True si existen procesos asociados al nombre, de lo contrario false.</returns>
        private bool GetProcessByName(string name)
        {
            //Builder.Output(" Getting :" + name);
            try
            {
                Process[] pList = Process.GetProcessesByName(name);
                if (pList.Length != 0)
                    return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ClassName, "GetProcessByName", TraceEventType.Error, ex.Message);
            }
                return false;
        }

        /// <summary>
        /// Termina una aplicacion.
        /// </summary>
        /// <param name="name">Nombre de la aplicacion a terminar.</param>
        private void KillProcessByName(string name)
        {
            Builder.Output(string.Format(ClassName + ": Terminando proceso: {0}.", name), TraceEventType.Information);
            Process[] pList;

            try
            {
                pList = Process.GetProcessesByName(name);
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ClassName, "KillProcessByName", TraceEventType.Error, ex.Message);
                return;
            }

            foreach (var item in pList)
            {
                try
                {
                    item.Kill();
                    while (!item.HasExited)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                    //Builder.Output("  Killed :" + name);
                    Builder.Output(string.Format(ClassName + ": Proceso terminado: {0}.", item.ProcessName), TraceEventType.Information);
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    //throw;
                    Log.WriteEntry(ClassName, "KillProcessByName", TraceEventType.Error, ex.Message);
                }

            }
        }

        /// <summary>
        /// Inicia una aplicacion.
        /// </summary>
        /// <param name="name">Nombre de la aplicacion a iniciar.</param>
        /// <returns>True si se inicia la aplicacion, de lo contrario false.</returns>
        private bool StartProcessByName(string name)
        {
             Builder.Output(" Starting :" + name, TraceEventType.Verbose);
            try
            {
                Process newp = new Process();
                newp.StartInfo.FileName = name;
                newp.StartInfo.UseShellExecute = true;
                newp.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(name);
                newp.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                newp.Start();

                Builder.Output("  Started :" + name , TraceEventType.Verbose);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ClassName, "StartProcessByName", TraceEventType.Error, ex.Message);
                return false;
            }

        }


    }
}
