using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public interface IDataStore<T>
    {
        Task AddItemAsync(T item);
        Task UpdateItemAsync(T item);
        Task DeleteItemAsync(T item);
        Task<T> GetItemAsync(long item);
        Task<List<T>> GetItemsAsync();
    }
}
