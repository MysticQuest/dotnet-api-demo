using Models;
using DataAccess;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Services
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IRepository<Item> itemRepository, IHttpClientFactory httpClientFactory, ILogger<ItemService> logger)
        {
            _itemRepository = itemRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task UpdateItemAsync(Item item)
        {
            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _itemRepository.DeleteAsync(id);
        }

        public async Task<Item> CreateItemAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("PostmanEcho");

            try
            {
                var response = await httpClient.PostAsJsonAsync("https://postman-echo.com/post", new { });
                _logger.LogInformation("Sent request to Postman Echo API.");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to receive a successful HTTP response. Status code: {response.StatusCode}");
                    throw new HttpRequestException($"Failed to receive a successful HTTP response. Status code: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Response from Postman Echo: {responseContent}");

                using var jsonDocument = JsonDocument.Parse(responseContent);
                var root = jsonDocument.RootElement;

                var url = root.GetProperty("url").GetString();
                var httpCode = root.GetProperty("headers").GetProperty("x-forwarded-port").GetString();
                var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var item = new Item
                {
                    Url = url,
                    HttpCode = httpCode,
                    DateTime = dateTime
                };

                await _itemRepository.CreateAsync(item);
                return item;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"An error occurred when sending the request: {httpEx.Message}");
                throw;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError($"An error occurred when parsing the response: {jsonEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
