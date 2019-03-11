using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTestClass
    {
        [TestMethod]
        public void VendingMachineInstantiationTest()
        {
            VendingMachine testVendingMachine = new VendingMachine();
            Assert.IsNotNull(testVendingMachine);
            Assert.AreEqual(0M, testVendingMachine.TotalRevenue, "Total revenue property should initialize to zero.");
            Assert.AreEqual(0M, testVendingMachine.TransactionBalance, "Transaction balance property should initialize to zero.");

        }

        [TestMethod]
        public void VendingMachineInitialBalanceConstructorTest()
        {
            VendingMachine testVendingMachine = new VendingMachine(20.50M);
            Assert.AreEqual(20.50M, testVendingMachine.TransactionBalance);
        }

        [TestMethod]
        public void VendingMachineNegativeInitialBalanceConstructorTest()
        {
            VendingMachine testVendingMachine = new VendingMachine(-20.50M);
            Assert.AreEqual(0M, testVendingMachine.TransactionBalance);
        }

        [TestMethod]
        public void VendingMachineAddTenderTest()
        {
            VendingMachine testVendingMachine = new VendingMachine();
            Assert.AreEqual(0M, testVendingMachine.TransactionBalance);
            testVendingMachine.AddTender(1);
            Assert.AreEqual(1M, testVendingMachine.TransactionBalance);
            Assert.AreEqual(1M, testVendingMachine.TransactionBalance);
            testVendingMachine.AddTender(2);
            Assert.AreEqual(3M, testVendingMachine.TransactionBalance);
            testVendingMachine.AddTender(5);
            Assert.AreEqual(8M, testVendingMachine.TransactionBalance);
            testVendingMachine.AddTender(10);
            Assert.AreEqual(18M, testVendingMachine.TransactionBalance);
            testVendingMachine.AddTender(20);
            Assert.AreEqual(38M, testVendingMachine.TransactionBalance);
            testVendingMachine.AddTender(3);

        }

        [TestMethod]
        public void VendingMachineVendTest()
        {
            VendingMachine testVendingMachine = new VendingMachine();
            //In Test File A1|Potato Crisps|3.05

            string actual = testVendingMachine.Vend("A1");
            string expected = "cantAfford";
            Assert.AreEqual(expected, actual);

            testVendingMachine.AddTender(10);
            Assert.AreEqual(testVendingMachine.TransactionBalance, 10);
            Assert.AreEqual(testVendingMachine.TotalRevenue, 0);

            expected = "SOLD";
            actual = testVendingMachine.Vend("A1");
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(6.95M, testVendingMachine.TransactionBalance);
            Assert.AreEqual(3.05M, testVendingMachine.TotalRevenue);

            testVendingMachine.AddTender(5);
            testVendingMachine.AddTender(1);
            testVendingMachine.Vend("A1");
            testVendingMachine.Vend("A1");
            testVendingMachine.Vend("A1");
            testVendingMachine.Vend("A1");

            Assert.AreEqual(0.75M, testVendingMachine.TransactionBalance);
            Assert.AreEqual(15.25M, testVendingMachine.TotalRevenue);

            expected = "OutOfStock";
            actual = testVendingMachine.Vend("A1");
            Assert.AreEqual(expected, actual);

            expected = "DoesNotExist";
            actual = testVendingMachine.Vend("A100");
            Assert.AreEqual(expected, actual);

        }

        [TestMethod] 
        public void VendingMachineDispenseChangeToZeroTest()
        {
            VendingMachine testVendingMachine = new VendingMachine(20);
            testVendingMachine.DispenseChange();
            Assert.AreEqual(0M, testVendingMachine.TransactionBalance);
        }

        [TestMethod] 
        public void VendingMachineDispenseChangeCorrectChangeTest()
        {
            VendingMachine testVendingMachine = new VendingMachine(15.40M);
            int[] result = { 61, 1, 1 };
            CollectionAssert.AreEqual(result, testVendingMachine.DispenseChange());

        }

        [TestMethod] 
        public void VendingMachineDispenseChangeOddChangeTest()
        {
            VendingMachine testVendingMachine = new VendingMachine(0.92M);
            int[] result = { 3, 1, 1 };
            CollectionAssert.AreEqual(result, testVendingMachine.DispenseChange());
        }

        [TestMethod]
        public void VendingMachineGetInventoryDataTest()
        {
            VendingMachine testVendingMachine = new VendingMachine();

            List<VendingMachineItem> actual = testVendingMachine.GetInventoryData();

            List<VendingMachineItem> expected = new List<VendingMachineItem>();
            //based on test file

            expected.Add(new VendingMachineItem("A1", "Potato Crisps", 3.05M));
            expected.Add(new VendingMachineItem("A2", "Stackers", 1.45M));
            expected.Add(new VendingMachineItem("A3", "Grain Waves", 2.75M));
            expected.Add(new VendingMachineItem("A4", "Cloud Popcorn", 3.65M));
            expected.Add(new VendingMachineItem("B1", "Moonpie", 1.80M));
            expected.Add(new VendingMachineItem("B2", "Cowtales", 1.50M));
            expected.Add(new VendingMachineItem("B3", "Wonka Bar", 1.50M));
            expected.Add(new VendingMachineItem("B4", "Crunchie", 1.75M));
            expected.Add(new VendingMachineItem("C1", "Cola", 1.25M));
            expected.Add(new VendingMachineItem("C2", "Dr.Salt", 1.50M));
            expected.Add(new VendingMachineItem("C3", "Mountain Melter", 1.50M));
            expected.Add(new VendingMachineItem("C4", "Heavy", 1.50M));
            expected.Add(new VendingMachineItem("D1", "U - Chews", 0.85M));
            expected.Add(new VendingMachineItem("D2", "Little League Chew", 0.95M));
            expected.Add(new VendingMachineItem("D3", "Chiclets", 0.75M));
            expected.Add(new VendingMachineItem("D4", "Triplemint", 0.75M));

            CollectionAssert.Equals(expected, actual);

        }

        [TestMethod]  //HOW DO WE DO THIS FOR I/O?
        public void WriteSalesReportDateTest()
        {
            //manual testing?
        }
    }
}
