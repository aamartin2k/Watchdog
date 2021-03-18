#region Descripción
/*
    Implementa el gestor del sistema.
    Almacena referencias, crea instancias y enlaza notificaciones con controladores.
*/ 
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using System.ServiceProcess;


using Monitor.Shared;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

#endregion

namespace Monitor.Service
{
    internal class MonitorService // : ServiceBase
    {
        #region Declaraciones
        // Componentes
        private ClientManager _clientManager;
        private DbHandler _dbHandler;
        private ZyanServer _zyanServer;
        private Notifier _notifier;

        // Formularios
        private FormEditConfig _editForm;

        // Flags
       
        // Locks/Monitors
        private static object _created = new object();
        #endregion

        private ModuleId ModuleId
        { get { return ModuleId.Builder; } }

        #region  Metodos Basicos de Inicio

        internal bool IniciarServicio()
        {

            try
            {
                Builder.Output("Starting Monitor");

                // Crear todo
                Build();

                // Registrar Mensajes Mover  al final
                RegisterMessages();

                StartModules();
               
                // Iniciar UDP server para recibir datos de clientes
                MessageBus.Send(new RequestStartUdpServer());

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }
        }

        internal void DetenerServicio()
        {
            Builder.Output("Stopping Monitor");

            StopModules();

            // Going down anyway

            try
            {
                _clientManager = null;
                _dbHandler = null;
                _zyanServer = null;
                _notifier = null;
                _editForm = null;
           
            }
            catch (Exception )
            {
            // ignorar excepciones  en terminacion
            }
        }

        private void Build()
        {

            try
            {
                _clientManager = new ClientManager();
                _dbHandler = new DbHandler();
                _zyanServer = new ZyanServer();
                _notifier = new Notifier();
           
                Builder.Output("All Builded");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void StartModules()
        {
            try
            {
                //_dbHandler.Start();
                //_notifier.Start();
                //_clientManager.Start();
                //_zyanServer.Start();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }
        }

       
        private void StopModules()
        {
            try
            {
                //_zyanServer.Stop();
                //_clientManager.Stop();
                //_dbHandler.Stop();
                //_notifier.Stop();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }
        }


        #endregion

        #region Mensajes

        private void RegisterMessages()
        {
            Builder.Output("Messages Registered.");
        }



        #endregion

        #region  Metodos para Implementación de Servicio de Windows

        /// <summary>
        /// Inicia el servicio.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

            // Estableciendo directorio activo a la ubicación del ejecutable. 
            // Los servicios inician el directorio activo en Windows/system32
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            if (!IniciarServicio())
                DetenerServicio();
        }

        /// <summary>
        /// Detiene el servicio.
        /// </summary>
        protected override void OnStop()
        {
            DetenerServicio();
        }

        #endregion

        #region Metodos para accion interactiva y formularios

        #region Configuracion de Clientes

        #region Crear nuevo cliente

        internal void AddClientGui()
        {
            RegisterEditFormNewClientMessages();
            StartDbHandler();

            // crear form
            _editForm = new FormEditConfig(EditFormMode.CreateClient);
            _editForm.StartPosition = FormStartPosition.CenterScreen;

            Application.Run(_editForm);
        }

       
        private void DoEditFormAcceptNewClient(EditFormAccept req)
        {
            _dbHandler.SaveAllData();
            EndEdit();
            Builder.Output("Cliente registrado.");
        }

        private void DoEditFormCancelNewClient(EditFormCancel req)
        {
            EndEdit();
            Builder.Output("Registro cancelado.");
        }

        private void EndEdit()
        {
            StopDbHandler();
            _editForm.Close();
            _editForm = null;
        }

       
        #endregion

        #region Editar clientes

        internal void EditClientGui()
        {
            RegisterEditFormEditClientsMessages();
            StartDbHandler();

            if (CheckNoClients())
            {
                Builder.Output("No hay clientes registrados.");
                return;
            }

            _editForm = new FormEditConfig(EditFormMode.EditClient);
            _editForm.StartPosition = FormStartPosition.CenterScreen;

            Application.Run(_editForm);
        }

        // Si no hay clientes en la bd, no mostrar form
        private bool CheckNoClients()
        { 
            
            int count = _dbHandler.ClientConfigDict.Count;
            if (count == 0)
                return true;
            else
                return false;
        }
        private void DoEditFormAcceptEditClients(EditFormAccept req)
        {
            _dbHandler.SaveAllData();
            EndEdit();
            Builder.Output("Edición aceptada.");
        }
        private void DoEditFormCancelEditClients(EditFormCancel req)
        {
            EndEdit();
            Builder.Output("Edición cancelada.");
        }

        #endregion

        #region Eliminar clientes

        internal void DelClientGui()
        {
            RegisterEditFormDelClientMessages();
            StartDbHandler();

            if (CheckNoClients())
            {
                Builder.Output("No hay clientes registrados.");
                return;
            }

            _editForm = new FormEditConfig(EditFormMode.DeleteClient);
            _editForm.StartPosition = FormStartPosition.CenterScreen;

            Application.Run(_editForm);
        }

        private void DoEditFormAcceptDelClient(EditFormAccept req)
        {
            _dbHandler.SaveAllData();
            EndEdit();
            Builder.Output("Cliente eliminado.");
        }

        private void DoEditFormCancelDelClient(EditFormCancel req)
        {
            EndEdit();
            Builder.Output("Eliminación cancelada.");
        }

 
        #endregion

        #endregion

        #region Configuracion de Sistema

        internal void ConfigSysGui()
        {
            RegisterEditFormSystemMessages();
            StartDbHandler();

            _editForm = new FormEditConfig(EditFormMode.EditSystem);
            _editForm.StartPosition = FormStartPosition.CenterScreen;

            Application.Run(_editForm);
        }

        private void DoEditFormAcceptSystem(EditFormAccept req)
        {
            _dbHandler.SaveAllData();
            EndEdit();
            Builder.Output("Configuración realizada.");
        }

        private void DoEditFormCancelSystem(EditFormCancel req)
        {
            EndEdit();
            Builder.Output("Configuración cancelada.");
        }
      
  
        #endregion

        #region Crear Configuracion y Base de datos de prueba

        internal void CreateConfig()
        {
            _dbHandler = new DbHandler();

            // Crear nueva base de datos  
            _dbHandler.CreateNewDb();

            ClientData client = new ClientData();
            /*
                Win1
                c:\Tmp\ClientUdp\wcudp1.exe
                20
                8001
             */
            client.Name = "Win1";
            client.AppFilePath = "c:\\Tmp\\ClientUdp\\wcudp1.exe";
            client.LogFilePath = "c:\\Tmp\\ClientUdp\\wcudp1.txt";
            client.MailEnabled = true;
            client.Timeout = 15;
            client.Port = 8001;
            client.QueueSize = 12;
            _dbHandler.ClientConfigDict.Add(client.Name, client);

            // Do it again...
            client = new ClientData();
            client.Name = "Win2";
            client.AppFilePath = "c:\\Tmp\\ClientUdp\\wcudp2.exe";
            client.LogFilePath = "c:\\Tmp\\ClientUdp\\wcudp2.txt";
            client.MailEnabled = true;
            client.LogAttachEnabled = true;
            client.Timeout = 15;
            client.Port = 8002;
            client.QueueSize = 24;
            _dbHandler.ClientConfigDict.Add(client.Name, client);

            //// ... and again
            client = new ClientData();
            client.Name = "Con1";
            client.AppFilePath = "c:\\Tmp\\ClientUdp\\cudp.exe";
            client.LogFilePath = "c:\\Tmp\\ClientUdp\\cudp";
            client.MailEnabled = true;
            client.LogAttachEnabled = true;
            client.Timeout = 18;
            client.Port = 8003;
            client.QueueSize = 48;
            _dbHandler.ClientConfigDict.Add(client.Name, client);

            // config system
            _dbHandler.SystemConfig.UdpServerPort = 8888;
            _dbHandler.SystemConfig.ZyanServerName =  Constants.ServerName;
            _dbHandler.SystemConfig.ZyanServerPort = Constants.ServerPort;
            _dbHandler.SystemConfig.SMtpServer = "localhost";
           
            _dbHandler.SystemConfig.Source = "monitor@test.home.cu";
            _dbHandler.SystemConfig.Password = "monitor";
            _dbHandler.SystemConfig.Destination = "receiver1@test.home.cu, receiver2@test.home.cu";
            _dbHandler.SystemConfig.TimeoutStartRestart = 1;
            
            //notificando objetos a la base de datos
            _dbHandler.SaveAllData();

            StopDbHandler();

            Console.WriteLine("Configuracion de prueba creada.");
        }


        internal void ImportData(string file)  
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(SystemPlusClientToSerialize));
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            SystemPlusClientToSerialize myObject = (SystemPlusClientToSerialize)xmlSer.Deserialize(stream);
            stream.Close();

            SystemPlusClientData obj = SystemPlusClientToSerialize.FromSerial(myObject);

            StartDbHandler();

            _dbHandler.CreateNewDb();

            _dbHandler.ClientConfigDict = obj.ClientList;
            _dbHandler.SystemConfig = obj.SystemConfig;

            _dbHandler.SaveAllData();
            StopDbHandler();
        }

        internal void ExportData(string file)
        {
            StartDbHandler();

            Dictionary<string, ClientData> clDict = _dbHandler.ClientConfigDict;
            SystemConfigData sysConfig = _dbHandler.SystemConfig;

            SystemPlusClientData localData = new SystemPlusClientData(clDict, sysConfig) ;
            SystemPlusClientToSerialize toSer = SystemPlusClientToSerialize.ToSerial(localData);

            XmlSerializer xmlSer = new XmlSerializer(typeof(SystemPlusClientToSerialize));

            StreamWriter myWriter = new StreamWriter(file);
            xmlSer.Serialize(myWriter, toSer);
            myWriter.Close();

            StopDbHandler();
        }

        #endregion



        #region Métodos Privados 

        private void RegisterEditFormNewClientMessages()
        {
            MessageBus.Register<EditFormAccept>(DoEditFormAcceptNewClient);
            MessageBus.Register<EditFormCancel>(DoEditFormCancelNewClient);
        }

        private void RegisterEditFormEditClientsMessages()
        {
            MessageBus.Register<EditFormAccept>(DoEditFormAcceptEditClients);
            MessageBus.Register<EditFormCancel>(DoEditFormCancelEditClients);
        }

        private void RegisterEditFormDelClientMessages()
        {
            MessageBus.Register<EditFormAccept>(DoEditFormAcceptDelClient);
            MessageBus.Register<EditFormCancel>(DoEditFormCancelDelClient);
        }

        private void RegisterEditFormSystemMessages()
        {
            MessageBus.Register<EditFormAccept>(DoEditFormAcceptSystem);
            MessageBus.Register<EditFormCancel>(DoEditFormCancelSystem);
        }

        private void StartDbHandler()
        {
            _dbHandler = new DbHandler();

            //_dbHandler.Start();
        }

      
        private void StopDbHandler()
        {
            //_dbHandler.Stop();
        }

        #endregion
        #endregion

        //  Requiere modificar accesor de metodos en Notifier, se declararon internal para depuracion
        // se cambiaron a private despues de la implementacion con MessageBus
        internal void CreateTest()
        {
            StartDbHandler();

            _notifier = new Notifier();
            //_notifier.Start();

            //ClientData clt = new ClientData();
            //clt.Name = "Clt de Prueba";
            //string cliente = "Test Client";
            // Prueba de mensajes de texto plano
            // Inicio y parada del servicio
            _notifier.SendMail(EMessageAction.SysStart,  DateTime.Now ) ;
            _notifier.SendMail(EMessageAction.SysEnd, DateTime.Now);
            // Eventos de cliente de prueba
            // Info
            //_notifier.SendMail(EMessageAction.Operational, cliente, DateTime.Now);
            //_notifier.SendMail(EMessageAction.OperationalPast24h, cliente, DateTime.Now);
            // Alert
            //_notifier.SendMail(EMessageAction.Timeout, cliente, DateTime.Now);
            //_notifier.SendMail(EMessageAction.Restart, cliente, DateTime.Now);
            // Alarm
            //_notifier.SendMail(EMessageAction.Dead, cliente, DateTime.Now);


            StopDbHandler();
            //_notifier.Stop();

            //_dbHandler = new DbHandler();
            //_zyanServer = new ZyanServer();
            //_clientManager = new ClientManager();


            //this.RequestDbHandlerStart += _dbHandler.DoRequestStart;
            //this.RequestZyanServerStart += _zyanServer.DoRequestStart;
            //this.RequestClientManagerStart += _clientManager.DoRequestStart;

            //this.RequestDbHandlerStop += _dbHandler.DoRequestStop;
            //this.RequestZyanServerStop += _zyanServer.DoRequestStop;
            //this.RequestClientManagerStop += _clientManager.DoRequestStop;

            //_dbHandler.SendClientConfig2ClientManager += _clientManager.ReceiveClientConfig;
            //_dbHandler.SendSystemConfig2ClientManager += _clientManager.ReceiveSystemConfig;

            //_clientManager.RequestClientConfig += _dbHandler.DoSendClientConfig;
            //_clientManager.RequestSystemConfig += _dbHandler.DoSendSystemConfig;

            //_zyanServer.RequestClientConfig += _dbHandler.DoSendClientConfigToZyanServer;
            //_zyanServer.RequestSystemConfig += _dbHandler.DoSendSystemConfigToZyanServer;

            //_dbHandler.SendClientConfig2ZyanServer += _zyanServer.ReceiveClientConfig;
            //_dbHandler.SendSystemConfig2ZyanServer += _zyanServer.ReceiveSystemConfig;
            ///prueba
            
            //OnRequestDbHandlerStart();
            //OnRequestZyanServerStart();
            //OnRequestClientManagerStart();

            //Builder.Output("_zyanServer OnRequest");
            ////_zyanServer.OnRequestClientConfig();
            ////_zyanServer.OnRequestSystemConfig();

            //Console.ReadLine();
            //Console.ReadLine();

            //OnRequestDbHandlerStop();
            //OnRequestZyanServerStop();
            //OnRequestClientManagerStop();
        }

    }
}
