namespace Services
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
