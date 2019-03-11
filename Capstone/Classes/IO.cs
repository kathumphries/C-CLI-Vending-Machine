using System;
using System.Collections.Generic;
using System.IO;
using Capstone.Classes;
using System.Text;

namespace Capstone.Classes
{
    class IO
    {

        public List<string[]> FetchData(string filePath, string fileName)
        {
            List<string[]> stockList = new List<string[]>();

            try
            {
                using (StreamReader reader = new StreamReader(Path.Combine(filePath, fileName)))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        string[] item = new string[3]; //SlotID, ItemName, Price

                        item = line.Split('|');

                        stockList.Add(item);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading file!  Please try again with file in proper directory.");
                Console.WriteLine(ex.Message);
            }

            return stockList;

        }

        public void WriteReport(string filePath, List<VendingMachineItem> CurrentItemsInventory, VendingMachine vendingMachineName) //override
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(filePath, $"salesReport_ {DateTime.UtcNow.ToString("MM-dd-yyyy_hh-mm-ss")}.txt"), false))
                {
                    foreach (VendingMachineItem item in CurrentItemsInventory)
                    {
                        writer.WriteLine($"{item.ItemName}|{5 - item.InventoryCount}");
                    }
                    writer.WriteLine($"\n** TOTAL SALES **  {vendingMachineName.TotalRevenue:C}");
                }
            }
            catch (IOException ex)
            {

                Console.WriteLine("File write error - call customer support at (614) 565-8382");
                Console.WriteLine(ex.Message);
                
            }


        }

        public void WriteLog(string filePath, string activity, decimal moneyPlaceHolder, decimal transactionBalance) //append
        {
            //TODO: make date and currency pretty/correct
            string writePhrase = string.Format("{0,-25} {1,-25} {2,-8:C} {3:C}", DateTime.UtcNow.ToString(), activity, moneyPlaceHolder, transactionBalance);

            try
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(filePath, "Log.txt"), true))
                {
                    writer.WriteLine(writePhrase);
                }
            }
            catch (IOException ex)
            {

                Console.WriteLine("File write error - call customer support at (614) 565-8382");
                Console.WriteLine(ex.Message);
            }
        }

    }
}
