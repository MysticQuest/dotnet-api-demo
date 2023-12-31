﻿using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Views
{
    public class ApiClient<T> where T : class, IEntity, new()
    {
        private readonly HttpClient _httpClient;
        private readonly string _typeName;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _typeName = typeof(T).Name;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"Generic?typeName={_typeName}");
            response.EnsureSuccessStatusCode();
            var entities = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            if (entities == null)
                throw new InvalidOperationException("Item not found or deserialization returned null.");
            return entities;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Generic/{id}?typeName={_typeName}");
            response.EnsureSuccessStatusCode();
            var entity = await response.Content.ReadFromJsonAsync<T>();
            if (entity == null)
                throw new InvalidOperationException("Item not found or deserialization returned null.");
            return entity;
        }

        public async Task TriggerCreateAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"Generic?typeName={_typeName}", new { });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"Generic/{id}?typeName={_typeName}", entity);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Generic/{id}?typeName={_typeName}");
            response.EnsureSuccessStatusCode();
        }
    }

}
