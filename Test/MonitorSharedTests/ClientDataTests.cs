using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Monitor.Shared.Tests
{
    /// <summary>
    /// Comprobación de la clase ClientData.
    /// </summary>
    [TestClass()]
    public class ClientDataTests
    {

        /// <summary>
        /// Se comprueba la presencia del atributo SerializableAttribute
        /// </summary>
        [TestMethod()]
        public void SerializableAttributeTest()
        {
            
            SerializableAttribute att =
           (SerializableAttribute)Attribute.GetCustomAttribute(typeof(ClientData), typeof(SerializableAttribute));

            Assert.IsNotNull(att);
        }


        [TestMethod()]
        public void SimplePropertiesTest()
        {
            // La clase ClientData es principalmente un almacen de datos, tiene muchas propiedades
            // simples {get set}. Se comprueban las que tienen una funcionalidad mas compleja.

            // arrange
            ClientData cdt = new ClientData();

            string expectedName = "ClientData Test Instance";
            string expectedFileNameWithExtension = "H:\\Ruta\\de\\prueba\\Nombre_aplicacion.extension";
            string expectedFileNameNoExtension = "Nombre_aplicacion";

            int defaultQueueSize = 32;
            int expectedQueueSize = 64;

            // act
            cdt.Name = expectedName;
            cdt.AppFilePath = expectedFileNameWithExtension;

            // se insertan 5 cadenas en lista de hb
            string expectedHB = string.Empty;
            int longitudLista = 5;
            for (int i = 0; i < longitudLista; i++)
            {
                expectedHB = "Cadena de ejemplo" + i;
                cdt.HeartBeat = expectedHB;
            }

            // assert
            // propiedad Name y metodo ToString deben coincidir
            Assert.AreEqual(expectedName, cdt.Name);
            Assert.AreEqual(expectedName, cdt.ToString() );
            // propiedad AppFilePath retorna toda la ruta
            Assert.AreEqual(expectedFileNameWithExtension, cdt.AppFilePath);
            // propiedad AppName retorna nombre SIN la extension del archivo y SIN la ruta
            Assert.AreEqual(expectedFileNameNoExtension, cdt.AppName);
            // Si StartTime == null, UptimeTS retorna TimeSpan.Zero
            Assert.AreEqual(TimeSpan.Zero, cdt.UptimeTS);

            cdt.StartTime = DateTime.Now;
            // esperar unos segundos para que exista diferencia de tiempo
            Thread.SpinWait(2000000);
            Assert.AreNotEqual(TimeSpan.Zero, cdt.UptimeTS);

            // propiedad QueueSize tiene valor default
            Assert.AreEqual(defaultQueueSize, cdt.QueueSize);
            // cambio valor 
            cdt.QueueSize = expectedQueueSize;
            Assert.AreNotEqual(defaultQueueSize, cdt.QueueSize);
            Assert.AreEqual(expectedQueueSize, cdt.QueueSize);

            // propiedad HB retorna
            Assert.AreEqual(expectedHB, cdt.HeartBeat);
            // lista contiene 5 elementos
            Assert.AreEqual(longitudLista, cdt.HeartBeatList.Length);
        }

       
    }
}
