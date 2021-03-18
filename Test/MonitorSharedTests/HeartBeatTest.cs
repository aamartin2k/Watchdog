using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monitor.Shared.Tests
{
    [TestClass()]
    public class HeartBeatTest
    {
        /// <summary>
        /// Se prueba el constructor public HeartBeat(string id) para generar HB con Timestamp.
        /// ClientID es null
        /// </summary>
        [TestMethod()]
        public void TimestampCreationTest()
        {
            string ExpectedClientID = null;
            string ExpectedTimestamp = DateTime.Now.ToString(HeartBeat.DefaultFormat);
            string ExpectedSerial = null;
            ulong ExpectedSerialNumber = 0;

            HeartBeat TxHB = HeartBeat.CreateHeartBeat(ExpectedClientID);
            HeartBeat RxHB = HeartBeat.CreateHeartBeat(TxHB.ByteSequence);

            string ActualClientID = RxHB.ClientID;
            string ActualTimestamp = RxHB.Timestamp;
            string ActualSerial = RxHB.Serial;
            ulong ActualSerialNumber = RxHB.SerialNumber;

            Assert.AreEqual(ExpectedClientID, ActualClientID);
            Assert.AreEqual(ExpectedSerial, ActualSerial);
            Assert.AreEqual(ExpectedSerialNumber, ActualSerialNumber);
            Assert.AreEqual(ExpectedTimestamp, ActualTimestamp);
        }
        /// <summary>
        /// Se prueba el constructor public HeartBeat(string id) para generar HB con ID.
        /// </summary>
        [TestMethod()]
        public void IdCreationTest()
        {
            string ExpectedClientID = "TEST01";
            string ExpectedTimestamp = DateTime.Now.ToString(HeartBeat.DefaultFormat);
            string ExpectedSerial = null;
            ulong ExpectedSerialNumber = 0;

            HeartBeat TxHB = HeartBeat.CreateHeartBeat(ExpectedClientID);
            HeartBeat RxHB = HeartBeat.CreateHeartBeat(TxHB.ByteSequence);

            string ActualClientID = RxHB.ClientID;
            string ActualTimestamp = RxHB.Timestamp;
            string ActualSerial = RxHB.Serial;
            ulong ActualSerialNumber = RxHB.SerialNumber;

            Assert.AreEqual(ExpectedClientID, ActualClientID);
            Assert.AreEqual(ExpectedSerial, ActualSerial);
            Assert.AreEqual(ExpectedSerialNumber, ActualSerialNumber);
            Assert.AreEqual(ExpectedTimestamp, ActualTimestamp);
           
        }

        /// <summary>
        /// Se prueba el constructor  public HeartBeat(string id, string format)
        /// </summary>
        [TestMethod()]
        public void IdTimestampCreationTest()
        {
            string format = "dd/MM/yyyy HH:mm";
            string ExpectedClientID = "CTEST0001";
            string ExpectedSerial = null;
            ulong ExpectedSerialNumber = 0;
            string ExpectedTimestamp = DateTime.Now.ToString(format);
            
            HeartBeat TxHB = HeartBeat.CreateHeartBeat(ExpectedClientID, format);
            HeartBeat RxHB = HeartBeat.CreateHeartBeat(TxHB.ByteSequence);

            string ActualClientID = RxHB.ClientID;
            string ActualTimestamp = RxHB.Timestamp;
            string ActualSerial = RxHB.Serial;
            ulong ActualSerialNumber = RxHB.SerialNumber;

            Assert.AreEqual(ExpectedClientID, ActualClientID);
            Assert.AreEqual(ExpectedSerial, ActualSerial);
            Assert.AreEqual(ExpectedSerialNumber, ActualSerialNumber);
            Assert.AreEqual(ExpectedTimestamp, ActualTimestamp);
        }


        /// <summary>
        /// Se prueba el constructor  public HeartBeat(string id, string format)
        /// ClientID es null
        /// </summary>
        [TestMethod()]
        public void TimestampFormatCreationTest()
        {
            string format = "dd/MM/yyyy HH:mm";
            string ExpectedClientID = null;
            string ExpectedSerial = null;
            ulong ExpectedSerialNumber = 0;
            string ExpectedTimestamp = DateTime.Now.ToString(format);

            HeartBeat TxHB = HeartBeat.CreateHeartBeat(ExpectedClientID, format);
            HeartBeat RxHB = HeartBeat.CreateHeartBeat(TxHB.ByteSequence);

            string ActualClientID = RxHB.ClientID;
            string ActualTimestamp = RxHB.Timestamp;
            string ActualSerial = RxHB.Serial;
            ulong ActualSerialNumber = RxHB.SerialNumber;

            Assert.AreEqual(ExpectedClientID, ActualClientID);
            Assert.AreEqual(ExpectedSerial, ActualSerial);
            Assert.AreEqual(ExpectedSerialNumber, ActualSerialNumber);
            Assert.AreEqual(ExpectedTimestamp, ActualTimestamp);
        }


        /// <summary>
        /// Se prueba el constructor public HeartBeat(string id, ulong serial).
        /// </summary>
        [TestMethod()]
        public void IdSerialCreationTest()
        {

            string ExpectedClientID = "TEST089";
            string ExpectedTimestamp = DateTime.Now.ToString(HeartBeat.DefaultFormat);
            string ExpectedSerial = "3856345";
            ulong ExpectedSerialNumber = 3856345;

            HeartBeat TxHB = HeartBeat.CreateHeartBeat(ExpectedClientID, ExpectedSerialNumber);
            HeartBeat RxHB = HeartBeat.CreateHeartBeat(TxHB.ByteSequence);

            string ActualClientID = RxHB.ClientID;
            string ActualTimestamp = RxHB.Timestamp;
            string ActualSerial = RxHB.Serial;
            ulong ActualSerialNumber = RxHB.SerialNumber;

            Assert.AreEqual(ExpectedClientID, ActualClientID);
            Assert.AreEqual(ExpectedSerial, ActualSerial);
            Assert.AreEqual(ExpectedSerialNumber, ActualSerialNumber);
            Assert.AreEqual(ExpectedTimestamp, ActualTimestamp);

        }

        /// <summary>
        /// Se prueba el constructor public HeartBeat(string id, string timestamp, ulong serial)
        /// </summary>
        [TestMethod()]
        public void IdTimestampSerialCreationTest()
        {
            string format = "dd/MM/yyyy HH:mm";

            string ExpectedClientID = "TEST089";
            string ExpectedTimestamp = DateTime.Now.ToString(format);
            string ExpectedSerial = "3856345";
            ulong ExpectedSerialNumber = 3856345;

            HeartBeat TxHB = HeartBeat.CreateHeartBeat(ExpectedClientID, format, ExpectedSerialNumber);
            HeartBeat RxHB = HeartBeat.CreateHeartBeat(TxHB.ByteSequence);

            string ActualClientID = RxHB.ClientID;
            string ActualTimestamp = RxHB.Timestamp;
            string ActualSerial = RxHB.Serial;
            ulong ActualSerialNumber = RxHB.SerialNumber;

            Assert.AreEqual(ExpectedClientID, ActualClientID);
            Assert.AreEqual(ExpectedSerial, ActualSerial);
            Assert.AreEqual(ExpectedSerialNumber, ActualSerialNumber);
            Assert.AreEqual(ExpectedTimestamp, ActualTimestamp);
        }

        // end of class
    }
}
