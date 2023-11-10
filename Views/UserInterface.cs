using Models;

namespace Views
{
    public class UserInterface
    {
        private readonly ApiClient _apiClient;

        public UserInterface(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Select the model to manipulate:");
            Console.WriteLine("1: Item");
            Console.WriteLine("2: PingData");
            Console.Write("Enter your choice: ");
            var modelChoice = Console.ReadLine();

            switch (modelChoice)
            {
                case "1":
                    await PerformItemActionsAsync();
                    break;
                case "2":
                    await PerformPingDataActionsAsync();
                    break;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }

        private async Task PerformItemActionsAsync()
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
            Console.WriteLine("Triggering item creation on the server...");
            try
            {
                await _apiClient.TriggerCreateItemAsync(); // This should now be a simple trigger
                Console.WriteLine("Item creation was triggered. Check the database for the new item.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred when calling the API: {ex.Message}");
            }
        }

        private async Task DeleteItemAsync()
        {
            Console.Write("Enter the ID of the item to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await _apiClient.DeleteItemAsync(id);
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
                Item item = await _apiClient.GetItemByIdAsync(id);
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
            IEnumerable<Item> items = await _apiClient.GetAllItemsAsync();
            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.Id}, Url: {item.Url}");
            }
        }

        private void PrintInstruction(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void PrintAction(string message)
        {
            Console.ForegroundColor = ConsoleColor.Teal;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private string PromptForInput(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(message);
            var input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }

    }
}
