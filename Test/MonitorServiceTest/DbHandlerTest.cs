using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor.Shared;


namespace Monitor.Service.Test
{
    /// <summary>
    /// Summary description for DbHandlerTest
    /// </summary>
    [TestClass]
    public class DbHandlerTest
    {
       
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Comprueba el inicio del componente.
        /// </summary>
        [TestMethod]
        public void StartModuleTest()
        {
            DbHandler _dbh = new DbHandler(); 
            // Eliminar archivo de datos serializados creado en prueba anteriores
            if (System.IO.File.Exists(_dbh.DbName))
                System.IO.File.Delete(_dbh.DbName);

            Assert.IsNull(_dbh.DbRoot);

            _dbh.Start();

            Assert.IsNotNull(_dbh.DbRoot);
            Assert.IsNotNull(_dbh.DbRoot.ClientManager);
            Assert.IsNotNull(_dbh.DbRoot.SystemConfig);

            _dbh.Stop();
        }

        [TestMethod]
        public void SystemConfigurationSaveTest()
        {
            DbHandler _dbh = new DbHandler();

            // Eliminar archivo de datos serializados creado en prueba anteriores
            if (System.IO.File.Exists(_dbh.DbName))
                System.IO.File.Delete(_dbh.DbName);

            _dbh.Start();

            SystemConfigData _sysCfgData = _dbh.DbRoot.SystemConfig;

            int expectedPort = 8888;
            string expectedSMTPserver = "localhost";
            string expectedSource = "monitor@test.home.cu";
            string expectedPwd = "monitor";
            int expectedTimeout = 3;

            _sysCfgData.UdpServerPort = expectedPort;
            _sysCfgData.ZyanServerName = Constants.ZyanServerName;
            _sysCfgData.SMtpServer = expectedSMTPserver;
            _sysCfgData.Source = expectedSource;
            _sysCfgData.Password = expectedPwd;     
            _sysCfgData.TimeoutStartRestart = expectedTimeout;

            _dbh.Stop();
            _dbh.Start();

            Assert.IsNotNull(_dbh.DbRoot);

            _sysCfgData = _dbh.DbRoot.SystemConfig;

            Assert.AreEqual(expectedPort, _sysCfgData.UdpServerPort);
            Assert.AreEqual(Constants.ZyanServerName, _sysCfgData.ZyanServerName);
            Assert.AreEqual(expectedSMTPserver, _sysCfgData.SMtpServer);
            Assert.AreEqual(expectedSource, _sysCfgData.Source);
            Assert.AreEqual(expectedPwd, _sysCfgData.Password);
            Assert.AreEqual(expectedTimeout, _sysCfgData.TimeoutStartRestart);

            _dbh.Stop();
        }

        [TestMethod]
        public void ClientConfigurationSaveTest()
        {
            DbHandler _dbh = new DbHandler();

            // Eliminar archivo de datos serializados creado en prueba anteriores
            if (System.IO.File.Exists(_dbh.DbName))
                System.IO.File.Delete(_dbh.DbName);

            _dbh.Start();

            ClientDataManager cdm = _dbh.DbRoot.ClientManager;

            string id = "ER$%^667";
            string name = "TestClt";
            string appPath = "TestClt\\qewqew \\qwerq\\qwer";
            string logPath = "TestClt\\245\\vfd erw5\\wregerw";
            int timeout = 345;
            int port = 0;
            bool mail = false;
            bool logAttach = true;
            int queueSize = 56;

            ClientData client = cdm.CreateClient(ClientIdType.KeyByIdString,
                                              id,
                                              port,
                                              name,
                                              appPath,
                                              logPath,
                                              timeout,
                                              mail,
                                              logAttach,
                                              queueSize);

            _dbh.Stop();
            _dbh.Start();

            Assert.IsNotNull(_dbh.DbRoot);
            cdm = _dbh.DbRoot.ClientManager;

            Assert.AreEqual(1, cdm.Count);

            client = cdm.GetClient(id);

            Assert.AreEqual(port, client.Port);
            Assert.AreEqual(name, client.Name);
            Assert.AreEqual(appPath, client.AppFilePath);
            Assert.AreEqual(logPath, client.LogFilePath);
            Assert.AreEqual(timeout, client.Timeout);
            Assert.AreEqual(mail, client.MailEnabled);
            Assert.AreEqual(logAttach, client.LogAttachEnabled);
            Assert.AreEqual(queueSize, client.QueueSize);
        }


        [TestMethod]
        public void ConfigurationImportExportTest()
        {
            DbHandler _dbh = new DbHandler();
            // Eliminar archivo de datos serializados creado en prueba anteriores
            if (System.IO.File.Exists(_dbh.DbName))
                System.IO.File.Delete(_dbh.DbName);

            string cfgFile = "impexptest.bsr";

            _dbh.Start();

            // System config.
            SystemConfigData _sysCfgData = _dbh.DbRoot.SystemConfig;

            int expectedPort = 8888;
            string expectedSMTPserver = "localhost";
            string expectedSource = "monitor@test.home.cu";
            string expectedPwd = "monitor";
            int expectedTimeout = 3;

            _sysCfgData.UdpServerPort = expectedPort;
            _sysCfgData.ZyanServerName = Constants.ZyanServerName;
            _sysCfgData.SMtpServer = expectedSMTPserver;
            _sysCfgData.Source = expectedSource;
            _sysCfgData.Password = expectedPwd;
            _sysCfgData.TimeoutStartRestart = expectedTimeout;

            // Client config.
            ClientDataManager cdm = _dbh.DbRoot.ClientManager;

            string id = "ER$%^667";
            string name = "TestClt";
            string appPath = "TestClt\\qewqew \\qwerq\\qwer";
            string logPath = "TestClt\\245\\vfd erw5\\wregerw";
            int timeout = 345;
            int port = 0;
            bool mail = false;
            bool logAttach = true;
            int queueSize = 56;

            ClientData client = cdm.CreateClient(ClientIdType.KeyByIdString,
                                              id,
                                              port,
                                              name,
                                              appPath,
                                              logPath,
                                              timeout,
                                              mail,
                                              logAttach,
                                              queueSize);

            // export
            _dbh.ExportCfg(cfgFile);

            _dbh.Stop();
            // borrar salva de config creada al detener componente
            if (System.IO.File.Exists(_dbh.DbName))
                System.IO.File.Delete(_dbh.DbName);

            // se crea config por defecto al abrir
            _dbh.Start();

            Assert.IsNotNull(_dbh.DbRoot);
            Assert.IsNotNull(_dbh.DbRoot.ClientManager);
            Assert.IsNotNull(_dbh.DbRoot.SystemConfig);
            // Sin clientes
            Assert.AreEqual(0, _dbh.DbRoot.ClientManager.Count);

            // import
            _dbh.ImportCfg(cfgFile);

            Assert.IsNotNull(_dbh.DbRoot);
            Assert.IsNotNull(_dbh.DbRoot.ClientManager);
            Assert.IsNotNull(_dbh.DbRoot.SystemConfig);

            // Comprobaciones
            // System
            _sysCfgData = _dbh.DbRoot.SystemConfig;

            Assert.AreEqual(expectedPort, _sysCfgData.UdpServerPort);
            Assert.AreEqual(Constants.ZyanServerName, _sysCfgData.ZyanServerName);
            Assert.AreEqual(expectedSMTPserver, _sysCfgData.SMtpServer);
            Assert.AreEqual(expectedSource, _sysCfgData.Source);
            Assert.AreEqual(expectedPwd, _sysCfgData.Password);
            Assert.AreEqual(expectedTimeout, _sysCfgData.TimeoutStartRestart);

            // Clientes
            cdm = _dbh.DbRoot.ClientManager; ;

            Assert.AreEqual(1, cdm.Count);

            client = cdm.GetClient(id);

            Assert.AreEqual(name, client.Name);
            Assert.AreEqual(appPath, client.AppFilePath);
            Assert.AreEqual(logPath, client.LogFilePath);
            Assert.AreEqual(timeout, client.Timeout);
            Assert.AreEqual(port, client.Port);
            Assert.AreEqual(mail, client.MailEnabled);
            Assert.AreEqual(logAttach, client.LogAttachEnabled);
            Assert.AreEqual(queueSize, client.QueueSize);

            _dbh.Stop();

            if (System.IO.File.Exists(cfgFile))
                System.IO.File.Delete(cfgFile);
        }



    }
}
