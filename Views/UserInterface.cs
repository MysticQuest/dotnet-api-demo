using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Views
{
    public class UserInterface
    {
        private readonly ApiService _apiService;

        public UserInterface(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1: Create an item");
                Console.WriteLine("2: Delete an item");
                Console.WriteLine("3: Print an item");
                Console.WriteLine("4: Print all items");
                Console.WriteLine("5: Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await TriggerCreateItemAsync();
                        break;
                    case "2":
                        await DeleteItemAsync();
                        break;
                    case "3":
                        await PrintItemAsync();
                        break;
                    case "4":
                        await PrintAllItemsAsync();
                        break;
                    case "5":
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

        private async Task TriggerCreateItemAsync()
        {
            Console.WriteLine("Triggering item creation...");
            await _apiService.TriggerCreateItemAsync();
            Console.WriteLine("Item creation triggered. Check the database for the new item.");
        }

        private async Task DeleteItemAsync()
        {
            Console.Write("Enter the ID of the item to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await _apiService.DeleteItemAsync(id);
                Console.WriteLine($"Item with ID {id} has been deleted.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        private async Task PrintItemAsync()
        {
            Console.Write("Enter the ID of the item to print: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Item item = await _apiService.GetItemByIdAsync(id);
                if (item != null)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Url}");
                }
                else
                {
                    Console.WriteLine("Item not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        private async Task PrintAllItemsAsync()
        {
            IEnumerable<Item> items = await _apiService.GetAllItemsAsync();
            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.Id}, Url: {item.Url}");
            }
        }
    }
}
