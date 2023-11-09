using Models;
using DataAccess;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public ItemService(IRepository<Item> itemRepository, IHttpClientFactory httpClientFactory)
        {
            _itemRepository = itemRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _itemRepository.GetByIdAsync(id);
        }

        public async Task<Item> CreateItemAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("PostmanEcho");
            var response = await httpClient.PostAsJsonAsync("https://postman-echo.com/post", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to receive a successful HTTP response. Status code: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadFromJsonAsync<dynamic>();
            var item = JsonConvert.DeserializeObject<Item>(Convert.ToString(responseData.data));

            await _itemRepository.CreateAsync(item);
            return item;
        }

        public async Task UpdateItemAsync(Item item)
        {
            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _itemRepository.DeleteAsync(id);
        }
    }
}
