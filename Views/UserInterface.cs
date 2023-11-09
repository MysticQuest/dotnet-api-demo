using Services;
using System;
using System.Threading.Tasks;

namespace Views
{
    public class UserInterface
    {
        private readonly IItemService _itemService;

        public UserInterface(IItemService itemService)
        {
            _itemService = itemService;
        }

        public async Task RunAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("WELCOME TO THE ITEM MANAGEMENT SYSTEM");
            Console.ResetColor();

            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1: Add an item");
                Console.WriteLine("2: Delete an item");
                Console.WriteLine("3: Print an item");
                Console.WriteLine("4: Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddItemAsync();
                        break;
                    case "2":
                        await DeleteItemAsync();
                        break;
                    case "3":
                        await PrintItemAsync();
                        break;
                    case "4":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option, try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private async Task AddItemAsync()
        {
            // Implement the logic to add an item by using _itemService
            Console.WriteLine("Adding an item...");
            // Your logic here
        }

        private async Task DeleteItemAsync()
        {
            // Implement the logic to delete an item by using _itemService
            Console.WriteLine("Deleting an item...");
            // Your logic here
        }

        private async Task PrintItemAsync()
        {
            // Implement the logic to print an item by using _itemService
            Console.WriteLine("Printing an item...");
            // Your logic here
        }
    }
}
