using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Views
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;

        public ApiService(HttpClient httpClient, string baseAddress)
        {
            _httpClient = httpClient;
            _baseAddress = baseAddress;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/items");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Item>>();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/items/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<Item>();
        }

        public async Task CreateItemAsync(Item item)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseAddress}/items", item);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseAddress}/items/{item.Id}", item);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseAddress}/items/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
