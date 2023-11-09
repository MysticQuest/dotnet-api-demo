using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Models;

namespace Views
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "http://localhost:5000";

        public ApiService(HttpClient httpClient, string baseAddress)
        {
            _httpClient = httpClient;
            _baseAddress = baseAddress;
        }

        public async Task TriggerCreateItemAsync()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseAddress}/items", new { });
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteItemAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseAddress}/items/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}/items/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Item not found.");
                return null;
            }
            return await response.Content.ReadFromJsonAsync<Item>();
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseAddress}/items");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Item>>();
        }
    }
}
