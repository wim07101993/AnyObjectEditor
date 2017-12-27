using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.Services.DataService
{
    public interface IDataService
    {
        Task<IEnumerable<JObject>> GetAllAsync();
        Task InsertAsync(object item);
        Task UpdateAsync(object item);
        Task RemoveAsync(object item);
    }
}