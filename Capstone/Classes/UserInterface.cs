using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Classes;
using System.Drawing;
using Console = Colorful.Console;
using Colorful;
using System.Threading;
using System.Linq;

namespace Capstone.Classes
{
    public class UserInterface
    {
        public VendingMachine vendingMachine = new VendingMachine();
        public void RunInterface()
        {
            bool done = false;
            while (!done)
            {
                done = Menu();
            }
        }//RunInterface
        public bool Menu()
        {
            
            MasterPage();
            StyleSheet styleSheetMM = new StyleSheet(Color.White);
            styleSheetMM.AddStyle("MAIN MENU", (Color.LimeGreen));
            Console.WriteLine("MAIN MENU", Color.LimeGreen);
            Console.WriteLine("\n(1) Display Vending Machine Items\n(2) Make A Purchase\n(3) End Program\n");
            string menuInput = Console.ReadLine();

           
            const bool StopMenu = false;
            const bool ContinueMenu = true;
            const bool Incomplete = false;
            bool isPurchaseTransactionComplete = Incomplete;
            const string DisplaySelction = "1";
            const string PurchaseSelection = "2";
            const string SecretReport = "9";
            const string EndProgram = "3";

            if (menuInput == DisplaySelction)
            {
                Display();
                Console.WriteLineStyled("\n\nPress enter to return to the MAIN MENU...", styleSheetMM);
                Console.ReadLine();
                return StopMenu;
            }
            else if (menuInput == PurchaseSelection)
            {
                while (isPurchaseTransactionComplete == Incomplete)
                {
                    isPurchaseTransactionComplete = Purchase();
                }
                return StopMenu;
            }
            else if (menuInput == EndProgram)
            {
                return ContinueMenu;
            }
            else if (menuInput == SecretReport)
            {
                Console.WriteLine("Generating sales report...", Color.LimeGreen);
                InitiateSalesReport();
                Console.ReadLine();
                Console.Clear();
                return StopMenu;
            }
            else
            {
                ErrorMessage();                    
                Console.WriteLineStyled("SELECTION NOT VALID! Press enter to return to the MAIN MENU.", styleSheetMM);
                Console.ReadLine();
                return StopMenu;
            }
        }//Menu
        public void Display()
        {
            MasterPage();
            Console.WriteLine("VENDING MACHINE ITEMS\n", Color.LimeGreen);
            List<VendingMachineItem> items = vendingMachine.GetInventoryData();
            List<string> itemsDisplay = new List<string>();
            foreach (VendingMachineItem item in items)
            {
                itemsDisplay.Add(item.ToString());
            }
            itemsDisplay.Sort();
            Console.WriteLine(string.Format("{0, 10} {1, 20} {2, 8:C} {3, 11}", "Product", "Item", "Price", "Available"));
            Console.WriteLine(string.Format("{0, 7} {1, 20} {2, 6:C} {3, 16}", "Code", "", "", "Inventory"));
            Console.WriteLine("-----------------------------------------------------", (Color.LimeGreen));
            //TODO moving this to vending machine class in order of display --state assumption
            foreach (string item in itemsDisplay)
            {
                Console.WriteLine(item);
            }
        }//Display
        public bool Purchase()
        {
            MasterPage();
            const bool PurchaseComplete = true;
            const bool PurchaseIncomplete = false;


            string userSelection = "";
            const string FeedMoneyOption = "1";
            const string SelectProductOption = "2";
            const string FinishTransactionOption = "3";

            // might want to consider separate method for testing valid user selection
            while (!(userSelection == FeedMoneyOption || userSelection == SelectProductOption || userSelection == FinishTransactionOption)) // Magic Constant test
            {

                Console.WriteLine("PURCHASE MENU", Color.LimeGreen);
                Console.WriteLine("\n(1) Insert Payment\n(2) Select Product\n(3) Complete Transaction\n");
                Console.WriteLine("Current Money Provided: {0:C}\n", vendingMachine.TransactionBalance, (Color.LimeGreen));
                userSelection = Console.ReadLine();
            }
            if (userSelection == FeedMoneyOption)
            {
                FeedMoney();
            }

            else if (userSelection == SelectProductOption)
            {
                SelectProduct();
            }
            else
            {
                FinishTransaction();
                return PurchaseComplete;
            }//finished
            return PurchaseIncomplete;
        } //Purchase
        public void InitiateSalesReport()
        {
            vendingMachine.WriteSalesReportData(vendingMachine);
        }//InitiateSalesReport

        public void FeedMoney()
        {
            List<string> allowedBills = new List<string> { "1", "2", "5", "10", "20" };
            //const bool Counterfeit = false;
            //bool isFeedMoneyAccepted = Counterfeit;

            MasterPage();
            Console.WriteLine("FEED MONEY\n", Color.LimeGreen);
            Console.WriteLine("Please enter whole dollar amount (1, 2, 5, 10, 20):");
            string dollarFeedInput = Console.ReadLine();

            if (allowedBills.Contains(dollarFeedInput))
            {

                int dollarFeed = 0;
                int.TryParse(dollarFeedInput, out dollarFeed);
                vendingMachine.AddTender(dollarFeed);

            }
            else
            {
                ErrorMessage();
                Console.WriteLine("Valid U.S. currency ONLY\n");
                Console.ReadLine();

            }


        }//FeedMoney
        public void SelectProduct()
        {
            MasterPage();
            StyleSheet styleSheetPM = new StyleSheet(Color.White);
            styleSheetPM.AddStyle("PURCHASE MENU", (Color.LimeGreen));
            Console.WriteLine("MAKE A PURCHASE\n", Color.LimeGreen);
            Display();
            Console.WriteLine("\nEnter product code to purchase:", Color.LimeGreen);
            string productSelector = Console.ReadLine(); 

            string transactionResult = vendingMachine.Vend(productSelector);

            if (transactionResult == "SOLD")
            {
                List<VendingMachineItem> products = new List<VendingMachineItem>(vendingMachine.GetInventoryData());
                foreach (VendingMachineItem item in products)
                {
                    if (item.SlotID.ToUpper() == productSelector.ToUpper())
                    {
                        Console.WriteAscii(item.Message, FigletFont.Default, Color.LawnGreen);
                        
                    }
                }

            }
            else if (transactionResult == "") // Magic Constant test num instead of strng a+ switch case?
            {
                ErrorMessage();
                Console.WriteLine("OOPS - Nothing was entered! Press enter to return to the PURCHASE MENU.", styleSheetPM);
                
            }
            else if (transactionResult == "DoesNotExist") // Magic Constant test
            {
                ErrorMessage();
                Console.WriteLine("Item does not exist! Please enter a valid product code! Press enter to return to the PURCHASE MENU.", styleSheetPM);
            }
            else if (transactionResult == "OutOfStock") // Magic Constant test
            {
                ErrorMessage();
                Console.WriteLine("OUT OF STOCK! Please make another selection. Press enter to return to the PURCHASE MENU.", styleSheetPM);
            }
            else //cant afford
            {
                ErrorMessage();
                Console.WriteLineStyled("INSUFFICIENT FUNDS! Please make an alternative selection. Press enter to return to the PURCHASE MENU.", styleSheetPM);
            }
            Console.ReadLine();


        }//SelectProduct
        public void FinishTransaction()
        {
            MasterPage();
            Console.WriteLine("ENJOY YOUR SNACK!\n", Color.LimeGreen);
            Console.WriteLine($"Your change is {vendingMachine.TransactionBalance.ToString("C")}\n");
            int[] change = vendingMachine.DispenseChange();  //display change from decimal [] 
            Console.WriteLine($"{change[0]} quarters, {change[1]} dimes, {change[2]} nickels", Color.LimeGreen);
            Console.ReadLine();
        }//FinishTransaction

        public void MasterPage()
        {
            Console.Clear();
            Console.WriteLine("Umbrella Corp: Vendo-Matic 7000");
            Console.WriteAscii("Snacking Refactored!", FigletFont.Default, Color.Plum);
            StyleSheet styleSheetMM = new StyleSheet(Color.White);
            styleSheetMM.AddStyle("MAIN MENU", (Color.LimeGreen));
            //StyleSheet styleSheetPM = new StyleSheet(Color.White);
            //styleSheetPM.AddStyle("PURCHASE MENU", (Color.LimeGreen));
        }
        static void ErrorMessage()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            var errorImg = new[]
            {
                @"***********************************************************************************************************************",
                "For Technical Assistance please call 614 - 565 - 8382",
                "Anything worth doing has at least three ways of doing it!" ,
                "Alas, this is not one of those ways.",
                "Please try again!",
                "***********************************************************************************************************************",
                //ascii image created with https://manytools.org/hacker-tools/convert-images-to-ascii-art/go
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@(*,,,,..,,,,*/(%&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#*,,,,,,,,,,,,,,,.,,,,,**,,,,,.,,,#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@/,,,,,,,,,,,,,,,,,,,,,,,,,,,**,,,,,,.,....../@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#,,,,,,,,,,,,,,,,,,,,,,,,,,,....,***,,,,,,,,........,.(@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@******,**,,*,,,,,,,,,,,,,,,,.......,***,*,,,.,.........,..,,/@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@,************,,,,,,/(((((((((#(,,,,,,,,***,*,,,...........,,,,,,,,*@@@@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@*,..****,,,,,,,,,,,#%#%%%&&%###%&&&&&/,,,,,**,*,,,.,.........,,,,**,****(@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@&***,.,****,,,,,,,,,(%&&&@@@@@@@@@@@@@@@@&(,,,****,,,........,,****,******%&@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@%************,*,,,,,*&&&@@@@@@@@@@@@@@@@@@@@@%,,*******,.........****,******&@@@@#%@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@%,,(***,*****,*,******#@@&&&@@@@@@@@@@@@@@@@@@@%*,*******...........,**********///****%@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@&/(,*(***********,,,,,,,#&&&&&&@@@@@@@@@@@@@@@&&&%(,***/**********.....*,,*******/****//**&@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@***,,*(***,,,,,,,,,,,,,,,%&&&&&&&@@@@@@@@@@@@@&&&&%#****//(#(,,,,,/.....,,,**********,***////@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@%%%%%#//***,,,,,,,,,,,,,,,(%##%&&&&&&&&&&((((#%%/%***/,,,,,,,,,/.....,,,********,,,****////%@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@%######(/****,,,,,,,,,,,,,,,(#####(%###(/(#%%%%&&&%&%&****,,,,,,,,,/.....,,,********,,,**//////((@@@@@@@@@@@@",
                @"@@@@@@@@@########(/**,,,,,,,,,,,,,,,,,(#///((/##%%%/&@&&&&&&%&&&&@****/((/,,,,,/.....,,,*********,,**/////((((@@@@@@@@@",
                @"@@@@@@@&%%%%#%####//*,,,,,,,,,,,,,,,,,//###%%%%%&&%%&@@&%&&&&&&&/***,,,,,,,,,*......,**********,,*//////((((#&@@@@@@@@@",
                @"@@@@@@&%%%%%%%%%%#//*,,,,,,,,,,,,,,,,,,#/####%%&&%(/,//(%&%(#%&&&*****/**,...........,,**********,,,,,*////(((((%@@@@@@",
                @"@@@@@&%%%%%%%%%%%#//*,,,,,,,,,,,,,,,,,,,####(((((((##%&%##%%@&&&&****////*.........,*,*********/*,,,,,*//((((((((@@@@@@",
                @"@@@@@%%%%%%%%%%%%%//*,,,,,,,,,,,,,,,,,,,,*####%%&&%%%&&&@@@@&&&%(,***////*,.........,,********//*,,,,,*/((((((((*,@@@@@",
                @"@@@@%%%%%%%%%%%%#%//*,,,,,,,,,,,,,,,,,,,,,*(#%%&%&&&&&@@@@@&%&%(,/(**////*,,.........,,******////,,,,,*/////((((*,,@@@@",
                @"@@@&&&&&&&&&&&&&&&(/*,,,,,,,,,,,,,,,,,,,,,,*/((#%#(((#(/(//####%.*%@(,,,,,,*,,,,.....,,*****/////,,*////////((((,,,*@@@",
                @"@@@&&&&&&&&&&&&&&&(/*,,*,,,,,,,,,,,,,,,,,,,%%((((((((///(%%%%%#%(,%&@@@*,,,,,,,,,,,,.,,*****/////,,*/////(((//((*,*,%@@",
                @"@@%&&&&&&&&&&&&&&&(/*,,**,,,,,,,,,,,,,,,. %&%%#///(((((/(#%%##%*#@&&@@@,,,,,,,,,,,,,,/****/////,,*////////**//,,,,,@@@@",
                @"@&%&&&&&&&&&&&&&&&(/*,,*,,,,,,,,,,,*,,,,,%&&&%%%/*////((((##%%&/   .%@@*.,,,,,,,.,,*****/////,,,////////*//(*,,,,#@@@@@",
                @"@%%&&&&&&&&&&&&&&/*,,,,,,,,,,*,,,,,,,,*@@@@@@@@&*/////((#%%&&% .       ..,,,,,,,.,,,****/////,,,///////**//(****,,@@@@@",
                @"@%%&&&&&&&&&&&&&&/*,,,,,,,,,,,,,,,,,,,(@@@@&*  .*/%(((((#%&&&&* ...........,,,,,,..,..,***/////,,,///////*,,*(*,,,,,&@@",
                @"&%%%%%%%%%#########/*,,,,,,,,,,,.,,,,,..#(    ..      .,#%##%&,...............,,,,,.....,,**//*//,,,////////***(*,,,,.(",
                @"%############%#####/**,,,,..,..........................,,...............,..............,..,*/////,,,///////////*,,,,,.*",
                @"%%%#%#%#%%%%%%%%%%%//*,,,,..................................,...........,................,,,*///*,.......,.............",
                @"%%%%%%%%%%%%%%%%%%#//*,.................,,.........................,..,..................,,,,,.......,,,,,,,,,,,,,,,***",
                @"%%%%%%%%%%%%%%%%%%%//................. ..,,........................,,,,...................,,,,,******,,,,,,,,,,,,,,,,,,",
                @"%%%%%%%%%%%%%%%%%%%*......... ......... ...,...........................,,,..............,...,,,,,,..,***/*****,...... .",
                @"%%%%%%%%%&&&%%&&&.......  .  ..   . .  ...,,...................................  ............,,,..**********,........  ",
                @"#%%%%%%%%%%&%%%(... .... .  .           ....,..................................  ...........,,,,,,,**.,.,**,,,.,.,. /  ",
                @"@##%%%%%%%%%%%%%%,. ..... ..   .            ...,...........,......................  . ..........,.,,,**,..,..,.,..,...&",
                @"@##%%%%%%%%%%%%%*......   ...                .....................................  ........ ........,*...............@",
                @"@%%%%%%%%%%%#.......    ....                 .................,..................    ..  ...........,****,,,,......(@@@",
                @"@@%%%%%%%%%%%*......       ..                   ......... ..........................    ..........,.,,.,***,.,.......@@",
                @"@@&%%%######,.,,.   .........                     ...,....   .......................      ...     .....,,**,,.......#@@",
                @"@@@########,,...,......                              ..,...      .................  .   .   ...  ..   ..,..........,@@@",
                @"@@@@#####(,,..... ..                                    ...,..         ............      .............. .,.........@@@@",
                @"@@@@@####,,,,....    ......                                .......         .....    ..... .......... ....*((##%%%&@@@@@",
                @"@@@@@(..    . ...................            .      ..        ......,,,,,,.,,................. ..... ..#&&&&&&&@@@@@@@@",
                @"@@@@@@&/    ...........................,(//(#%&%(/*...             ,,,,,,,,........,,...................,%%%%%%%&@@@@@@",
                @"@@@@@@@&/.....   ....................,@&@@@@@@@@@@@@@@,......  .,,,,,,,,,,,,,,,,,,,,,,,...............#%%%%%%@@@@@@@@@@",
                @"@@@@@@@@@%..  ......................,&%%%&&&&&@@@%###%&&@@@&   .,,,...............,,,,,,,,,,,,......../%%%%%%%@@@@@@@@@",
                @"@@@@@@@@@@%(,.         .............#%*/(##%%%&&@@@%/*.*/**...,,...................    .,,,,,,,,,,,,,..(%%%%%@@@@@@@@@@",
                @"@@@@@@@@@@@@&&&%%%#,        .      */(,,//((##&%&&@@@@@&%%&&&&&........................    .,,,,,,,,,,,,,,*&@@@@@@@@@@@",
                @"@@@@@@@@@@@@@%%%####/*,...          ../,...////(((((##%&&&&&%%,..........................      .,,,,,,,,,,@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@%#####(**,,,........,((.  .   (#%&@&@@@&(***(%(.... ...    ...   ............      ..,,,,&@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@#####***,,..,.....*((.....   (//(####(((((/....   .         .     ..... ....       .#@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@###(****,**,,..../(*.....  .((//(%&&@@@&....     .                  .....  ..   (@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@####/***//*,,,..,((.....    ,/(/*****, ...       .                     .. . .%@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@##(**////*,,,,.*(*.....         ,**. .          .                       ,@@@@@@@@@@@@@@@@@@@@@@@",
                @"@@@@@@@@@@@@@@@@@@@@@@@@@@(//////*,,,,,*(......             .           .                   .@@@@@@@@@@@@@@@@@@@@@@@@@@",
            };

            var maxLength = errorImg.Aggregate(0, (max, line) => Math.Max(max, line.Length));
            var x = Console.BufferWidth / 2 - maxLength / 2;
            for (int y = -errorImg.Length; y < Console.WindowHeight + -errorImg.Length; y++)
            {
                ConsoleDraw(errorImg, x, y);
                Thread.Sleep(100);
            }
            Console.ReadLine();
            Console.Clear();
        } //ErrorMessage
        static void ConsoleDraw(IEnumerable<string> lines, int x, int y) //pulled from https://stackoverflow.com/questions/2725529/how-to-create-ascii-animation-in-windows-console-application-using-c
        {
            if (x > Console.WindowWidth) return;
            if (y > Console.WindowHeight) return;

            var trimLeft = x < 0 ? -x : 0;
            int index = y;

            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            var linesToPrint =
                from line in lines
                let currentIndex = index++
                where currentIndex > 0 && currentIndex < Console.WindowHeight
                select new
                {
                    Text = new String(line.Skip(trimLeft).Take(Math.Min(Console.WindowWidth - x, line.Length - trimLeft)).ToArray()),
                    X = x,
                    Y = y++
                };

            Console.Clear();
            foreach (var line in linesToPrint)
            {
                Console.SetCursorPosition(line.X, line.Y);
                Console.Write(line.Text);
            }
        }//ConsoleDraw
    }//class
}//namespace