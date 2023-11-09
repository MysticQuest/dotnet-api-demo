using Models;
using DataAccess;
using System.Net.Http;
using Newtonsoft.Json;

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

        public async Task CreateItemAsync(Item item)
        {
            var httpClient = _httpClientFactory.CreateClient("PostmanEcho");

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://postman-echo.com/post", item);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to receive a successful HTTP response. Status code: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadFromJsonAsync<dynamic>();
            Item responseItem = JsonConvert.DeserializeObject<Item>(Convert.ToString(responseData.data));

            await _itemRepository.CreateAsync(responseItem);
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
