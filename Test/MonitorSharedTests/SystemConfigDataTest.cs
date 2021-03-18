using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monitor.Shared.Tests
{
    /// <summary>
    /// Comprobación de la clase SystemConfigData.
    /// </summary>
    [TestClass]
    public class SystemConfigDataTest
    {
        /// <summary>
        /// Se comprueba la presencia del atributo SerializableAttribute.
        /// </summary>
        [TestMethod]
        public void SerializableAttributeTest()
        {
            // Se comprueba la presencia del atributo  [Serializable]
            SerializableAttribute att =
           (SerializableAttribute)Attribute.GetCustomAttribute(typeof(SystemConfigData), typeof(SerializableAttribute));

            Assert.IsNotNull(att);
        }

        [TestMethod]
        public void SimplePropertiesTest()
        {
            // La clase SystemConfigData es un almacen de datos, sus propiedades son
            // simples {get set}. 

            // arrange
            SystemConfigData syscfg = new SystemConfigData();

            string expectedSMtpServer = "SystemConfigData Test Value";
            string expectedPassword = "SystemConfigData Test Value password";
            int expectedPort = 56567;
            int expectedServerPort = 556666;

            syscfg.SMtpServer = expectedSMtpServer;
            syscfg.Password = expectedPassword;
            syscfg.UdpServerPort = expectedPort;
            syscfg.ZyanServerPort = expectedServerPort;

            Assert.AreEqual(expectedSMtpServer, syscfg.SMtpServer);
            Assert.AreEqual(expectedPassword, syscfg.Password);
            Assert.AreEqual(expectedPort, syscfg.UdpServerPort);
            Assert.AreEqual(expectedServerPort, syscfg.ZyanServerPort);

        }


     }
}
