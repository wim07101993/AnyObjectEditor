using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.Services
{
    public class MongoDataService : IDataService
    {
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public MongoDataService(string connectionString, string databaseName, string collectionName)
        {
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
            _collectionName = collectionName;
        }


        public async Task<IEnumerable<JObject>> GetAllDocuments()
        {
            var ret = new List<JObject>();

            await _database.GetCollection<BsonDocument>(_collectionName)
                .Find(FilterDefinition<BsonDocument>.Empty)
                .ForEachAsync(x => ret.Add((JObject) JsonConvert.DeserializeObject(x.ToString()
                    .Replace("ObjectId(", "")
                    .Replace(")", "")
                    .Replace("_", ""))));

            return ret;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            var ret = new List<T>();

            await _database.GetCollection<T>(_collectionName)
                .Find(FilterDefinition<T>.Empty)
                .ForEachAsync(x => ret.Add(x));

            return ret;
        }
    }
}