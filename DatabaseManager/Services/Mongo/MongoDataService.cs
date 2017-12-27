using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseManager.Models.Bases;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.Services.Mongo
{
    public class MongoDataService<T> : IDataService, IDataService<T> where T : IMongoModel
    {
        #region FIELDS

        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        #endregion FIELDS


        #region CONSTRUCTORS

        public MongoDataService(string connectionString, string databaseName, string collectionName)
        {
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
            _collectionName = collectionName;
        }

        #endregion CONSTRUCTORS


        #region METHODS

        async Task<IEnumerable<JObject>> IDataService.GetAllAsync()
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var ret = new List<T>();

            await _database.GetCollection<T>(_collectionName)
                .Find(FilterDefinition<T>.Empty)
                .ForEachAsync(x => ret.Add(x));

            return ret;
        }

        async Task IDataService.InsertAsync(object item)
        {
            await _database
                .GetCollection<BsonDocument>(_collectionName)
                .InsertOneAsync(item.ToBsonDocument());
        }

        public async Task InsertAsync(T item)
        {
            //foreach (var propertyInfo in item.GetType().GetProperties())
            //    if (propertyInfo.HasAttribute<BsonIdAttribute>())
            //    {
            //        propertyInfo.SetValue(item, new ObjectId());
            //        break;
            //    }

            await _database.GetCollection<T>(_collectionName)
                .InsertOneAsync(item);
        }

        async Task IDataService.UpdateAsync(object item)
        {
            //var filter = Builders<BsonDocument>.Filter.Where(x => x)
            //await _database
            //    .GetCollection<BsonDocument>(_collectionName)
            //    .UpdateOneAsync()
        }

        public async Task UpdateAsync(T item)
        {
            var filter = Builders<T>.Filter
                .Eq(filterItem => filterItem.ObjectId, item.ObjectId);

            await _database
                .GetCollection<T>(_collectionName)
                .ReplaceOneAsync(filter, item);
        }

        async Task IDataService.RemoveAsync(object item)
        {
            throw new System.NotImplementedException();
        }

        public async Task RemoveAsync(T item)
        {
            var filter = Builders<T>.Filter
                .Eq(filterItem => filterItem.ObjectId, item.ObjectId);

            await _database
                .GetCollection<T>(_collectionName)
                .DeleteOneAsync(filter);
        }

        #endregion METHODS
    }
}