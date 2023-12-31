﻿using Microsoft.Extensions.DependencyInjection;
using Models;
using System.Threading.Tasks;

namespace Views
{
    public class UserInterface
    {
        private readonly IServiceProvider _serviceProvider;
        private Type? _currentModelType;
        private dynamic? _apiClient;
        private dynamic ApiClient
        {
            get
            {
                if (_apiClient == null)
                {
                    throw new InvalidOperationException("The API client has not been initialized.");
                }
                return _apiClient;
            }
        }

        public UserInterface(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task RunAsync()
        {
            while (true)
            {
                try
                {
                    PrintInstruction("Select the model to manipulate:");
                    PrintAction("1: Item");
                    PrintAction("2: PingData");
                    PrintAction("3: Exit");
                    var modelChoice = PromptForInput("Enter your choice: ");

                    switch (modelChoice)
                    {
                        case "1":
                            _currentModelType = typeof(Item);
                            _apiClient = _serviceProvider.GetRequiredService<ApiClient<Item>>();
                            break;
                        case "2":
                            _currentModelType = typeof(PingData);
                            _apiClient = _serviceProvider.GetRequiredService<ApiClient<PingData>>();
                            break;
                        case "3":
                            PrintSuccess("Exiting...");
                            return;
                        default:
                            PrintError("Invalid option, try again.");
                            break;
                    }

                    if (_currentModelType != null)
                    {
                        await PerformActionsAsync();
                    }
                }
                catch (Exception ex)
                {
                    PrintError($"An error occurred: {ex.Message}");
                }
            }
        }

        private async Task PerformActionsAsync()
        {
            PrintInstruction("Select an action:");
            PrintAction("1: Create");
            PrintAction("2: Delete");
            PrintAction("3: Print one");
            PrintAction("4: Print all");
            PrintAction("5: Return to main menu");
            var actionChoice = PromptForInput("Enter your choice: ");

            switch (actionChoice)
            {
                case "1":
                    await TriggerCreateAsync();
                    break;
                case "2":
                    await DeleteAsync();
                    break;
                case "3":
                    await PrintAsync();
                    break;
                case "4":
                    await PrintAllAsync();
                    break;
                case "5":
                    return;
                default:
                    PrintError("Invalid action. Please try again.");
                    break;
            }
        }

        private async Task TriggerCreateAsync()
        {
            PrintAction("Triggering item creation on the server...");
            try
            {
                await ApiClient.TriggerCreateAsync();
                PrintSuccess("Item creation was triggered. Check the database for the new item.");
            }
            catch (HttpRequestException ex)
            {
                PrintError($"An error occurred when calling the API: {ex.Message}");
            }
            catch (Exception ex)
            {
                PrintError($"An unexpected error occurred: {ex.Message}");
            }
        }

        private async Task DeleteAsync()
        {
            PrintInstruction("Enter the ID of the item to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    await ApiClient.DeleteAsync(id);
                    PrintSuccess($"Item with ID {id} has been deleted.");
                }
                catch (HttpRequestException ex)
                {
                    PrintError($"An HTTP error occurred when calling the API: {ex.Message}");
                }
                catch (Exception ex)
                {
                    PrintError($"An unexpected error occurred: {ex.Message}");
                }
            }
            else
            {
                PrintError("Invalid ID format.");
            }
        }


        private async Task PrintAsync()
        {
            PrintInstruction("Enter the ID of the item to print: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var item = await ApiClient.GetByIdAsync(id);
                    if (item != null)
                    {
                        PrintSuccess($"ID: {item.Id}, Url: {item.Url}");
                    }
                    else
                    {
                        PrintError("Item not found.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    PrintError($"An HTTP error occurred when calling the API: {ex.Message}");
                }
                catch (Exception ex)
                {
                    PrintError($"An unexpected error occurred: {ex.Message}");
                }
            }
            else
            {
                PrintError("Invalid ID format.");
            }
        }


        private async Task PrintAllAsync()
        {
            try
            {
                var entities = await ApiClient.GetAllAsync();
                foreach (var entity in entities)
                {
                    PrintEntityDetails(entity);
                }
            }
            catch (HttpRequestException ex)
            {
                PrintError($"An HTTP error occurred when calling the API: {ex.Message}");
            }
            catch (Exception ex)
            {
                PrintError($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void PrintInstruction(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + message + "\n");
            Console.ResetColor();
        }

        private void PrintAction(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
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
            Console.Write("\n" + message);
            var input = Console.ReadLine();
            Console.ResetColor();
            return input ?? string.Empty;
        }

        private void PrintEntityDetails(IEntity entity)
        {
            if (entity == null)
            {
                PrintError("Entity is null.");
                return;
            }

            Type type = entity.GetType();
            PrintInstruction($"{type.Name} properties:");
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(entity);
                PrintSuccess($"{prop.Name}: {value}");
            }
        }
    }
}
