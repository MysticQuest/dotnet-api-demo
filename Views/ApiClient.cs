using Models;
using System.Net.Http.Json;

namespace Views
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;

        public ApiService(HttpClient httpClient, string baseAddress)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
        }

        public async Task TriggerCreateItemAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseAddress}/items", new { });
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error creating item: {response.ReasonPhrase}");
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseAddress}/items/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error deleting item with id {id}: {response.ReasonPhrase}");
            }
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/items/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Item with id {id} not found: {response.ReasonPhrase}");
            }
            return await response.Content.ReadFromJsonAsync<Item>();
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/items");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error retrieving items: {response.ReasonPhrase}");
            }
            return await response.Content.ReadFromJsonAsync<IEnumerable<Item>>();
        }
    }
}
