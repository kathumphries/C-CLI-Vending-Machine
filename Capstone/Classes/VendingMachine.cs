using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        //constructor takes in filepath and creats list of content

        private List<VendingMachineItem> items = new List<VendingMachineItem>(); //do not change
        private string filePath = @"C:\VendingMachine"; //do not change  - write all files/reports here...
        private string logFileName = @"vendingmachine.csv";
        public decimal TransactionBalance { get; private set; }
        public decimal TotalRevenue { get; private set; }
        IO io = new IO();

        public VendingMachine()
        {
            TransactionBalance = 0M;
            TotalRevenue = 0M;
            List<string[]> stockList = new List<string[]>();
            stockList = io.FetchData(filePath, logFileName);

            foreach (string[] stockItemArray in stockList)
            {
                string slotId = (stockItemArray[0]);
                string itemName = (stockItemArray[1]);
                decimal price = decimal.Parse(stockItemArray[2]);//TODO: Handle Parse exception
                items.Add(new VendingMachineItem(slotId, itemName, price));
            }

        }

        //TODO refactor constructor

        public VendingMachine(decimal initialBalance)
        {
            if (initialBalance < 0)
            {
                initialBalance = 0;
            }
            TransactionBalance = initialBalance;
            TotalRevenue = 0M;
            List<string[]> stockList = new List<string[]>();
            stockList = io.FetchData(filePath, logFileName);

            foreach (string[] stockItemArray in stockList)
            {
                string slotId = (stockItemArray[0]);
                string itemName = (stockItemArray[1]);
                decimal price = decimal.Parse(stockItemArray[2]);//TODO: Handle Parse exception
                items.Add(new VendingMachineItem(slotId, itemName, price));
            }

        }
        //todo move to io?

        public void AddTender(int moneyTendered)
        {
            TransactionBalance += moneyTendered;
            io.WriteLog(filePath, "FEED MONEY:", moneyTendered, TransactionBalance);
        }

        public string Vend(string itemSelection)
        {
            const string sold = "SOLD";
            const string outOfStock = "OutOfStock";
            const string notASlotID = "DoesNotExist";
            const string cantAfford = "cantAfford";
            string result = "";

            foreach (VendingMachineItem item in items)
            {
                if (item.SlotID.ToUpper() == itemSelection.ToUpper())
                {
                    if (item.InventoryCount > 0)
                    {
                        if (TransactionBalance >= item.Price)
                        {
                            decimal initialBalance = TransactionBalance;
                            item.InventoryCount--;
                            TransactionBalance -= item.Price;
                            TotalRevenue += item.Price;
                            // pre-balance for IO call = TransactionBalance + item.Price;
                            io.WriteLog(filePath, (item.ItemName + " " + item.SlotID), (TransactionBalance + item.Price), TransactionBalance);
                            result = sold;
                            break;
                        }
                        else
                        {
                            result = cantAfford;
                            break;
                        }
                    }
                    else
                    {
                        result = outOfStock;
                        break;
                    }
                }
                else
                {
                    result = notASlotID;
                }
            }
            return result;
        }  //TODO reverse logic else = success

        public int[] DispenseChange()
        {
            // Assume all prices are divisible by 5 cents because there are no pennies.
            // quarters[0], dimes[1], nickels[2]
            decimal tempBalance = TransactionBalance;
            int[] change = new int[3];
            change[0] = (int)Math.Floor(TransactionBalance / 0.25M);
            TransactionBalance -= change[0] * 0.25M;
            change[1] = (int)(Math.Floor(TransactionBalance / 0.10M));
            TransactionBalance -= change[1] * 0.10M;
            change[2] = (int)(Math.Floor(TransactionBalance / 0.05M));
            TransactionBalance = 0;
            io.WriteLog(filePath, "GIVE CHANGE:", tempBalance, TransactionBalance);
            return change;
        }

        public List<VendingMachineItem> GetInventoryData()
        {
            return items;
        }

        public void WriteSalesReportData(VendingMachine vendingMachineName)
        {
            io.WriteReport(filePath, items, vendingMachineName);
        }

    }

}
