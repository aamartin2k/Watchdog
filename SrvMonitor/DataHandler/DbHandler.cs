
#region Descripción
/*
    Implementa la persistencia de objetos mediante serialización.
    Se almacenan los datos de configuracion del sistema, propiedades
    y estado de los clientes.
*/
#endregion

#region Using
using System;
using System.IO;
using AMGS.Application.Utils.Serialization.Bin;
using Monitor.Shared;
using System.Collections.Generic;
#endregion

namespace Monitor.Service
{
    public class DbHandler
    {

        #region Declaraciones
        // Nombre del archivo de base de datos
        private const string DatabaseName = "Monitor.Service.Data.bsr";
        // Para uso de Log
        private const string ClassName = "DbHandler";
        // Referencia al objeto Root de la configuracion
        private  SystemPlusClientData _dbRoot;

        #endregion

        #region Propiedades

        #region Propiedades Públicas

        internal string ErrorMsg
        { get; private set; }

        //internal SystemPlusClientData DbRoot
        //{
        //    set { _dbRoot = value; }
        //}
        internal ClientDataManager ClientList
        {
            get { return _dbRoot.ClientManager; }
        }

        internal SystemConfigData SystemData
        {
            get { return _dbRoot.SystemConfig; }             
        }

        #endregion

        #region Propiedades Privadas

        #endregion

        #endregion

        #region Métodos

        #region Métodos Públicos

        static internal bool DatabaseExists()
        {
            return File.Exists(DatabaseName);
        }


        internal bool OpenDatabase()
        {
            try
            {
                SystemPlusClientData tmpDB;

                if (DatabaseExists())
                {
                    Builder.Output(ClassName + ": Archivo de datos de configuracion existente. Se abre.", System.Diagnostics.TraceEventType.Verbose);
                    tmpDB = DeSerializarDeDisco(DatabaseName);
                }
                else
                {
                    Builder.Output(ClassName + ": No existe archivo de datos de configuracion. Se crea nuevo.", System.Diagnostics.TraceEventType.Verbose);
                    tmpDB = new SystemPlusClientData();

                    SerializarADisco(tmpDB, DatabaseName);
                }
                
                _dbRoot = tmpDB;
                return true;
            }
            catch (Exception ex)
            {
                // guardar msg de error
                _dbRoot = null;
                ErrorMsg = ex.Message;
                return false;
            }
        }
        // guardar db
        private void CommitDatabase()
        {
            Builder.Output(ClassName + ": Base de datos de configuracion salvada.", System.Diagnostics.TraceEventType.Verbose);
            SerializarADisco(_dbRoot, DatabaseName);
        }
        internal void CloseDatabase()
        {
            CommitDatabase();
            Builder.Output(ClassName + ": Base de datos de configuracion cerrada.", System.Diagnostics.TraceEventType.Verbose);
        }

        //Solicitud de importar configuracion completa
        internal bool ImportConfig(string filename)
        {
            Builder.Output("Importar file: " + filename);

            try
            {
                SystemPlusClientData tmpDB = DeSerializarDeDisco(filename);
                _dbRoot = tmpDB;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return false;
            }

        }
        // exportar
        internal bool ExportConfig(string filename)
        {
            Builder.Output("Exportar file: " + filename);
            // TODO implementar chequeo de Error 

            try
            {
                SerializarADisco(_dbRoot, filename);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return false;
            }
        }


        #region Métodos Request Handlers
        //Solicitud de iniciar el componente
        internal void Start(RequestStart req)
        {
            Builder.Output("Iniciando DbHandler.");
            StartComponent();
            Builder.Output("DbHandler iniciado.");
        
        }
        //Solicitud de detener el componente
        internal void Stop(RequestStop req)
        {
            Builder.Output("Deteniendo DbHandler.");
            StopComponent();
            Builder.Output("DbHandler detenido.");
            MessageBus.Send(new ReplyStop() );
        }
       
        // Solicitud de enviar datos de clientes
        internal void SendClientConfig(RequestClientConfig req)
        {
            MessageBus.Send(new SendClientConfig(_dbRoot.ClientManager) );
        }
        // Solicitud de enviar datos de sistema
        internal void SendSystemConfig(RequestSystemConfig req)
        {
            MessageBus.Send(new SendSystemConfig(_dbRoot.SystemConfig));
        }

      
        // salvar config a disco
        internal void Save(RequestSaveConfig req)
        {
            CommitDatabase();
            Builder.Output(ClassName + ": Configuracion salvada.");
        }

        internal void CreateTestConfig()
        {

            ClientDataManager cdm = new ClientDataManager();
            /*
             * Cliente con clave Id, puerto automatico
             * 
            client.Name = "WinClt01";
            client.Id = "WC0001";
            client.AppFilePath = "c:\\Tmp\\ClientUdp\\wcudp1.exe";
            client.LogFilePath = "c:\\Tmp\\ClientUdp\\wcudp1.txt";
            client.MailEnabled = true;
            client.Timeout = 55;
            client.Port = 0;
            client.QueueSize = 12;
            */
            cdm.CreateClient(ClientIdType.KeyByIdString,
                             "WC0001", 
                             0, 
                             "WinClt01", 
                             "c:\\Tmp\\ClientUdp\\wcudp1.exe", 
                             "c:\\Tmp\\ClientUdp\\wcudp1.txt",
                             55, true, false, 12);
            /*
             Cliente con clave Port, puerto 50100

            client = new ClientData();
            client.Name = "WinClt02";
            client.Id = "WC0002";
            client.AppFilePath = "c:\\Tmp\\ClientUdp\\wcudp2.exe";
            client.LogFilePath = "c:\\Tmp\\ClientUdp\\wcudp2.txt";
            client.MailEnabled = true;
            client.LogAttachEnabled = false;
            client.Timeout = 65;
            client.Port = 50100;
            client.QueueSize = 24;
            */
            cdm.CreateClient(ClientIdType.KeyByUdpPort,
                             null, 
                             50100,
                             "WinClt02",
                             "c:\\Tmp\\ClientUdp\\wcudp2.exe",
                             "c:\\Tmp\\ClientUdp\\wcudp2.txt",
                             65, true, false, 24);
            //// ... and again

            //client = new ClientData();
            //client.Name = "CConsole1";
            //client.Id = HeartBeat.DefaultClientID;
            //client.AppFilePath = "c:\\Tmp\\ClientUdp\\cudp.cmd";
            //client.LogFilePath = "c:\\Tmp\\ClientUdp\\cudp.txt";
            //client.MailEnabled = false;
            //client.LogAttachEnabled = false;
            //client.Timeout = 70;
            //client.Port = 50200;
            //client.QueueSize = 48;

            cdm.CreateClient(ClientIdType.KeyByUdpPort,
                             null,
                             50200,
                             "CConsole1",
                             "c:\\Tmp\\ClientUdp\\cudp.cmd",
                             "c:\\Tmp\\ClientUdp\\cudp.txt",
                             70, false, false, 48);

            // config system
            SystemConfigData tmpSys = new SystemConfigData();

            tmpSys.ServerIpAdr = "127.0.0.1";
            tmpSys.UdpServerPort = 8888;
            tmpSys.ZyanServerName = Constants.ZyanServerName;
            tmpSys.ZyanServerPort = Constants.ZyanServerPort;
            tmpSys.SMtpServer = "localhost";

            tmpSys.Source = "monitor@test.home.cu";
            tmpSys.Password = "monitor";
            tmpSys.Destination = "receiver1@test.home.cu, receiver2@test.home.cu";
            tmpSys.TimeoutStartRestart = 180;
            tmpSys.RestartAttemps = 2;

            SystemPlusClientData tmpDb = new SystemPlusClientData(cdm, tmpSys);
            _dbRoot = tmpDb;

            CommitDatabase();
        }

        #endregion

        #endregion

        #region Métodos Privados

        
        private void StartComponent()
        {
            bool ret = OpenDatabase();
            if (ret)
            {
                // enviar confirmacion de inicio
                MessageBus.Send(new ReplyOK());
            }
        }

        private void StopComponent()
        {
            CloseDatabase();
            _dbRoot = null;
        }

               

        #region Serializacion
        private void SerializarADisco(SystemPlusClientData obj, string file)
        {
            if (obj != null)
                Serializer.Serialize<SystemPlusClientData>(obj, file);
            else
                Builder.Output("Intentando serializar referencia nula.", System.Diagnostics.TraceEventType.Warning);
        }

        private SystemPlusClientData DeSerializarDeDisco(string file)
        {
            return Serializer.Deserialize<SystemPlusClientData>(file);
        }


        #endregion

        #endregion

        #endregion

        #region Testing
        /*  La realizacion de los test requiere ejecucion directa de los metodos. Esto va 
           en contra de la arquitectura del sistema, basada en intercambio de mensajes entre componentes.
           Para realizar los test se implementan metodos que exponen de modo publico otros metodos 
           privados del componente. Se emplea compilacion condicional para limitar la generación de
           estos metodos.
           Se requiere emplear la configuracion de proyecto denominada Test, donde se define el
           simbolo TEST.
        */
#if TEST
        // Metodos implementados para facilitar las pruebas
        // Gestion del componente
        public void Start()
        { StartComponent(); }

        public void Stop()
        { StopComponent(); }

        public SystemPlusClientData DbRoot { get { return _dbRoot; } }

        public string DbName { get { return DatabaseName; } }

        public bool ImportCfg(string filename)
        { return ImportConfig(filename);    }

        public bool ExportCfg(string filename)
        { return ExportConfig(filename); }

#endif
        #endregion
    }
}
