using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.Services.DataService
{
    public interface IDataService
    {
        Task<IEnumerable<JObject>> GetAllAsync();
        Task InsertAsync(JObject item);
        Task UpdateAsync(JObject item);
        Task RemoveAsync(JObject item);
        Task<Dictionary<string, Dictionary<string, object>>> GetAttributesDictionary();
    }
}