using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monitor.Shared.Tests
{
    /// <summary>
    /// Summary description for ClientDataManagerTest
    /// </summary>
    [TestClass]
    public class ClientDataManagerTest
    {
        public ClientDataManagerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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

        [TestMethod]
        public void CreationTest()
        {
            ClientDataManager cdm = new ClientDataManager();
            Assert.IsInstanceOfType(cdm, typeof(ClientDataManager));
            Assert.IsNotNull(cdm.List);
        }

        [TestMethod]
        public void ClientKeyIdCreation()
        {
            string id = "ER$%^667";
            string name = "TestClt";
            string appPath = "TestClt\\qewqew \\qwerq\\qwer";
            string logPath = "TestClt\\245\\vfd erw5\\wregerw";
            int timeout = 345;
            int port = 0;
            bool mail = false;
            bool logAttach = true;
            int queueSize = 56;

            ClientDataManager cdm = new ClientDataManager();

            Assert.IsFalse(cdm.ContainsId(id));
            Assert.IsFalse(cdm.ContainsName(name));

            ClientData clt = cdm.CreateClient(ClientIdType.KeyByIdString,
                                              id,
                                              port,
                                              name,
                                              appPath,
                                              logPath,
                                              timeout,
                                              mail,
                                              logAttach,
                                              queueSize);

            Assert.IsTrue(cdm.ContainsId(id));
            Assert.IsFalse(cdm.ContainsPort(port));
            Assert.IsTrue(cdm.ContainsName(name));
            Assert.AreEqual(id, clt.Id);
            Assert.AreEqual(name, clt.Name);
            Assert.AreEqual(appPath, clt.AppFilePath);
            Assert.AreEqual(logPath, clt.LogFilePath);
            Assert.AreEqual(timeout, clt.Timeout);
            Assert.AreEqual(mail, clt.MailEnabled);
            Assert.AreEqual(logAttach, clt.LogAttachEnabled);
            Assert.AreEqual(queueSize, clt.QueueSize);

            ClientData newclt = cdm.GetClient(id);
            Assert.AreEqual(clt, newclt);

            Assert.AreEqual(1, cdm.List.Length);
            cdm.Delete(newclt.ClientId);
            Assert.AreEqual(0, cdm.List.Length);
        }

        [TestMethod]
        public void ClientKeyPortCreation()
        {
            int port = 56800;
            string id = null;
            string name = "TestClt";
            string appPath = "TestClt\\qewqew \\qwerq\\qwer";
            string logPath = "TestClt\\245\\vfd erw5\\wregerw";
            int timeout = 345;
            bool mail = false;
            bool logAttach = true;
            int queueSize = 56;

            ClientDataManager cdm = new ClientDataManager();

            Assert.IsFalse(cdm.ContainsPort(port));
            Assert.IsFalse(cdm.ContainsName(name));

            ClientData clt = cdm.CreateClient(ClientIdType.KeyByUdpPort,
                                              id,
                                              port,
                                              name,
                                              appPath,
                                              logPath,
                                              timeout,
                                              mail,
                                              logAttach,
                                              queueSize);

            Assert.IsTrue(cdm.ContainsPort(port));
            Assert.IsTrue(cdm.ContainsName(name));
            Assert.IsFalse(cdm.ContainsId(id));
            Assert.AreEqual(port, clt.Port);
            Assert.AreEqual(name, clt.Name);
            Assert.AreEqual(appPath, clt.AppFilePath);
            Assert.AreEqual(logPath, clt.LogFilePath);
            Assert.AreEqual(timeout, clt.Timeout);
            Assert.AreEqual(mail, clt.MailEnabled);
            Assert.AreEqual(logAttach, clt.LogAttachEnabled);
            Assert.AreEqual(queueSize, clt.QueueSize);

            ClientData newclt = cdm.GetClient(port);
            Assert.AreEqual(clt, newclt);

            Assert.AreEqual(1, cdm.List.Length);
            cdm.Delete(newclt.ClientId);
            Assert.AreEqual(0, cdm.List.Length);
        }
    }
}
