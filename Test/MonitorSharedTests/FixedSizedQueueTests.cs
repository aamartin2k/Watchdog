
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace Monitor.Shared.Tests
{
    /// <summary>
    /// Comprobación de la clase FixedSizedQueue
    /// </summary>
    [TestClass()]
    public class FixedSizedQueueTests
    {
        [TestMethod()]
        public void CreationTest()
        {
            // arrange
            FixedSizedQueue<int> fsqNoItems;
            FixedSizedQueue<int> fsqWithItemsShort;
            FixedSizedQueue<int> fsqWithItemsLong;

            Int32 shortSize = 12; 
            Int32 longSize = 56;
            List<int> ListShort = new List<int>();
            for (int i = 0; i < shortSize; i++)
            {
                ListShort.Add(i+20);
            }

            List<int> ListLong = new List<int>();
            for (int i = 0; i < longSize; i++)
            {
                ListLong.Add(i * 100);
            }

            // act
            fsqNoItems = new FixedSizedQueue<int>(shortSize);
            fsqWithItemsShort = new FixedSizedQueue<int>(shortSize, ListShort);
            fsqWithItemsLong = new FixedSizedQueue<int>(shortSize, ListLong);

            // assert
            // lista sin elementos insertados
            Assert.AreEqual(shortSize, fsqNoItems.MaxSize);
            Assert.AreEqual(0, fsqNoItems.GetSnapshot().Length);
            // lista creada con elementos por debajo del limite
            Assert.AreEqual(shortSize, fsqWithItemsShort.MaxSize);
            Assert.AreEqual(shortSize, fsqWithItemsShort.GetSnapshot().Length);
            // lista creada con elementos por encima del limite
            Assert.AreEqual(shortSize, fsqWithItemsLong.MaxSize);
            Assert.AreEqual(shortSize, fsqWithItemsLong.GetSnapshot().Length);
            // comprobando elementos en los extremos
            int index = 0;
            Assert.AreEqual(ListShort[index], fsqWithItemsShort.GetSnapshot()[index]);
            index = shortSize - 1;
            Assert.AreEqual(ListShort[index], fsqWithItemsShort.GetSnapshot()[index]);
            // los indices cambian de acuerdo a la lista.
            Assert.AreEqual(ListLong[longSize - shortSize], fsqWithItemsLong.GetSnapshot()[0]);
            Assert.AreEqual(ListLong[longSize - 1], fsqWithItemsLong.GetSnapshot()[shortSize - 1]);

        }

       

       

    }
}
