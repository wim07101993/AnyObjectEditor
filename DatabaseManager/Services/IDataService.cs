using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.Services
{
    public interface IDataService
    {
        Task<IEnumerable<JObject>> GetAllDocuments();
        Task<IEnumerable<T>> GetAll<T>();
    }
}
