using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{

    public class VendingMachineItem
    {
        
        public int InventoryCount { get; set; } //should this be a private setter?
        public decimal Price { get; }
        public string SlotID { get; }
        public string ItemName { get; }
        public string Message
        {
            get
            {
                string message = "";

                switch (SlotID[0])
                {
                    case 'A':
                    case 'a':
                        message = "Crunch Crunch, Yum!";
                        break;
                    case 'B':
                    case 'b':
                        message = "Munch Munch, Yum!";
                        break;
                    case 'C':
                    case 'c':
                        message = "Glug Glug, Yum!";
                        break;
                    case 'D':
                    case 'd':
                        message = "Chew Chew, Yum!";
                        break;
                    default:
                        message = "Salad Dodger!";
                        break;
                }

                return message;
            }
        }

        //constructor
        public VendingMachineItem(string slotID, string itemName, decimal price)
        {   
            SlotID = slotID;
            ItemName = itemName;
            Price = price;
            InventoryCount = 5; //initial inventory set to maximum which is 5; property of vending machine then reference here...
        }

        public override string ToString()
        {
            string result = (string.Format("{0, 5} {1, 25} {2, 8:C}", SlotID, ItemName, Price));
            if (InventoryCount == 0)
            {
                result += "SOLD OUT".PadLeft(10);
            }
            else
            {
                result += InventoryCount.ToString().PadLeft(10);
            }
            return result;

        }


    }





}
