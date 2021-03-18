
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using Monitor.Shared;

namespace Monitor.Service.Test
{
    [TestClass]
    public class ClientManagerTest
    {
        // Declaraciones
        private string Client1_ID = "WC0001";
        private string Client1_Name = "WinClt01";
        private Guid Client1_Guid;

        /// <summary>
        /// Crea configuracion de prueba para los tests.
        /// Configuracion de sistema
        /// </summary>
        private SystemConfigData CreateSystemConfig()
        {
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

            return tmpSys;

        }

        /// <summary>
        /// Crea configuracion de prueba para los tests.
        /// Configuracion de cliente
        /// </summary>
        private ClientDataManager CreateClientConfig()
        {
            ClientDataManager cdm = new ClientDataManager();

            ClientData client;

            client = cdm.CreateClient(ClientIdType.KeyByIdString,
                            Client1_ID,
                            0,
                            Client1_Name,
                            "c:\\Tmp\\ClientUdp\\wcudp1.exe",
                            "c:\\Tmp\\ClientUdp\\wcudp1.txt",
                            55, true, false, 12);

            // salvar ref a Guid
            Client1_Guid = client.ClientId;


            return cdm;

        }

        /// <summary>
        /// Comprueba el inicio del componente.
        /// </summary>
        [TestMethod]
        public void StartModuleTest()
        {
            ClientManager cmn = new ClientManager();

            Assert.IsNull(cmn.ClientList);

            // Pasando configuracion.
            cmn.SetClientConfig(new SendClientConfig(CreateClientConfig()));
            cmn.SetSystemConfig(new SendSystemConfig(CreateSystemConfig()));
            cmn.Start();

            // Comprobar objetos creados.
            Assert.IsNotNull(cmn.ClientList);
            // Colas
            Assert.IsNotNull(cmn.StartList);
            Assert.IsNotNull(cmn.WorkList);
            Assert.IsNotNull(cmn.TimeOutList);
            Assert.IsNotNull(cmn.RecoverList);
            Assert.IsNotNull(cmn.DeadList);
            Assert.IsNotNull(cmn.PausedList);

            // Comprobar cliente cargado
            Assert.AreEqual(1, cmn.ClientList.Count);
            Assert.AreEqual(1, cmn.StartList.Count);

            // Fin
            cmn.Stop();

            // Comprobar objetos destruidos.
            Assert.IsNull(cmn.ClientList);
            // Colas
            Assert.IsNull(cmn.StartList);
            Assert.IsNull(cmn.WorkList);
            Assert.IsNull(cmn.TimeOutList);
            Assert.IsNull(cmn.RecoverList);
            Assert.IsNull(cmn.DeadList);
            Assert.IsNull(cmn.PausedList);
        }

        /// <summary>
        /// Comprueba el inicio de monitoreo de un cliente.
        /// </summary>
        [TestMethod]
        public void StartClientTest()
        {
            ClientManager cmn = new ClientManager();

            // Pasando configuracion.
            cmn.SetClientConfig(new SendClientConfig(CreateClientConfig()));
            cmn.SetSystemConfig(new SendSystemConfig(CreateSystemConfig()));
            cmn.Start();

            // Generando HB para el cliente.
            HeartBeat hb = HeartBeat.CreateHeartBeat(Client1_ID);
            cmn.ReceiveHB(new SendHeartbeat(hb));

            // Comprobar cliente en lista de trabajo
            Assert.AreEqual(1, cmn.ClientList.Count);
            Assert.AreEqual(1, cmn.WorkList.Count);

            // Fin
            cmn.Stop();
        }

        /// <summary>
        /// Comprueba el pausado y continuacion de monitoreo de un cliente.
        /// </summary>
        [TestMethod]
        public void PauseResumeClientTest()
        {
            ClientManager cmn = new ClientManager();
              
            // Pasando configuracion.
            cmn.SetClientConfig(new SendClientConfig(CreateClientConfig()));
            cmn.SetSystemConfig(new SendSystemConfig(CreateSystemConfig()));
            cmn.Start();

            // Generando HB para el cliente.
            HeartBeat hb = HeartBeat.CreateHeartBeat(Client1_ID);
            cmn.ReceiveHB(new SendHeartbeat(hb));

            // Comprobar cliente en lista de trabajo
            Assert.AreEqual(1, cmn.ClientList.Count);
            Assert.AreEqual(1, cmn.WorkList.Count);

            // Pausar cliente
            cmn.PauseClient(new RequestPauseClient(Client1_Guid) );

            // Comprobar cliente en lista de pausa
            Assert.AreEqual(0, cmn.WorkList.Count);
            Assert.AreEqual(1, cmn.PausedList.Count);

            // Continuar cliente
            cmn.ResumeClient(new RequestResumeClient(Client1_Guid));

            // Comprobar cliente en lista de trabajo
            Assert.AreEqual(1, cmn.WorkList.Count);
            Assert.AreEqual(0, cmn.PausedList.Count);

            // Fin
            cmn.Stop();
        }



        }

}
