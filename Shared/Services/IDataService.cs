using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T item);
        Task UpdateAsync(T item);
        Task RemoveAsync(T item);
    }
}