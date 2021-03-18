using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monitor.Shared.Tests
{
    /// <summary>
    /// Comprobación de la clase SystemPlusClientData.
    /// </summary>
    [TestClass()]
    public class SystemPlusClientDataTest
    {

        /// <summary>
        /// Se comprueba la presencia del atributo SerializableAttribute.
        /// </summary>
        [TestMethod]
        public void SerializableAttributeTest()
        {
            // Se comprueba la presencia del atributo  [Serializable]
            SerializableAttribute att =
           (SerializableAttribute)Attribute.GetCustomAttribute(typeof(SystemPlusClientData), typeof(SerializableAttribute));

            Assert.IsNotNull(att);
        }

        /// <summary>
        /// Se comprueba la inicializacion de propiedades despues de la construccion.
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            SystemPlusClientData spc = new SystemPlusClientData();

            Assert.IsNotNull(spc.ClientManager);
            Assert.IsNotNull(spc.SystemConfig);

            Assert.AreEqual(Constants.UdpServerPort, spc.SystemConfig.UdpServerPort);
            Assert.AreEqual(Constants.ZyanServerName, spc.SystemConfig.ZyanServerName);
            Assert.AreEqual(Constants.ZyanServerPort, spc.SystemConfig.ZyanServerPort);

        }
    }
}
