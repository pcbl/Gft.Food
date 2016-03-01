using System;
using System.Collections.Generic;
using System.Text;

namespace Gft.FoodMenu.Client
{
    /// <summary>
    /// Sample Client program to use the Food Menu Application
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleKey key;
                do
                {
                    SetupUserInterface();
                    //Reads input and forward it to the MenuManager Singleton            
                    var input = Console.ReadLine();
                    Console.WriteLine(MenuManager.Instance.ProcessOrderRequest(input));
                    //Optionally the user can quit using 'ESCAPE' key
                    Console.WriteLine("Press ESC(Escape) to quit or ANY other key to continue...");
                    key = Console.ReadKey().Key;//Press Any
                } while (key!=ConsoleKey.Escape);
            }
            catch (Exception ex)
            {
                //If something completelly weird happen
                Console.Clear();
                Console.WriteLine("An unexpected exception has occurred. Application execution was aborted.");
                Console.WriteLine("Error Message:" + ex.Message);
                Console.Read();//User is supposed to see the error message...
            }
        }

        private static void SetupUserInterface()
        {
            Console.Clear();
            Console.WriteLine("Please Provide Menu selection based on the following criteria:");            
            List<string> menuOptions = new List<string>();
            List<string> dishTypeOptions = new List<string>();
            //We get the available Menus as we use MEF and could potentially support a wider variety of Menus within the Future
            foreach (var menu in MenuManager.Instance.AvailableMenus)
            {
                menuOptions.Add(menu.Value.Name);                
                var menuDishes = new StringBuilder();
                foreach (var dishOption in menu.Value.DishOptions)
                {                    
                    menuDishes.AppendLine(string.Format("\t\t\t-{0}={1}({2}){3}",
                        (int)dishOption.DishType,
                        dishOption.Name,
                        dishOption.DishType.ToString(),
                        dishOption.SupportsMultipleOrders ? "(Multiple Orders Supported)" : string.Empty));
                }
                dishTypeOptions.Add(string.Format("\t\t\t{0}:\r\n{1}", menu.Value.Name, string.Join("\r\n", menuDishes)));
            }
            //Then we print the available options for the user on a user-friendly way
            Console.Write("\t- Time Of Day Must be one of the following values: ");
            Console.WriteLine(string.Join(", ", menuOptions));
            Console.WriteLine("\t\t- Dish Options are: ");
            foreach (var dishOptioNSet in dishTypeOptions)
                Console.WriteLine(dishOptioNSet);
            Console.WriteLine();
            Console.WriteLine("\t-Enter the item number, comma separated");
            Console.WriteLine("\t-Items can be ordered only one time, except the ones marked as 'Multiple Orders Supported', which can be ordered multiple times");
            //We also provide some input samples to instruct the user on the best way
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Sample Calls:");
            Console.WriteLine("\tmorning, 1, 2, 3");
            Console.WriteLine("\tnight, 1, 2, 3, 4");
            Console.WriteLine("\tnight, 1, 2, 2, 4");
            Console.WriteLine();
            Console.WriteLine("Please, place your order");
        }
    }
}
