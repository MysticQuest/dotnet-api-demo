using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net.Http.Json;

namespace Views
{
    public class ApiClient<T> where T : class, IEntity, new()
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{typeof(T).Name}s");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{typeof(T).Name}s/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task TriggerCreateAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"{typeof(T).Name}s", new { });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, T item)
        {
            var response = await _httpClient.PutAsJsonAsync($"{typeof(T).Name}s/{id}", item);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{typeof(T).Name}s/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
