using Models;

namespace Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task<Item> CreateItemAsync();
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int id);
    }
}
