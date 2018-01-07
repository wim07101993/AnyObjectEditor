using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseManager.Models.Bases;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseManager.Services.DataService.Mongo
{
    public class MongoDataService<T> : IDataService<T> where T : IMongoModel
    {
        #region FIELDS

        private readonly IMongoDatabase _database;
        private readonly string _collectionName;
        private readonly string _attributesCollectionName;
        private readonly string _attributesId;

        #endregion FIELDS


        #region CONSTRUCTORS

        public MongoDataService(string connectionString, string databaseName, string collectionName,
            string attributesCollectionName = null, string attributesId = null)
        {
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
            _collectionName = collectionName;
            _attributesCollectionName = attributesCollectionName;
            _attributesId = attributesId;
        }

        #endregion CONSTRUCTORS


        #region METHODS

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var ret = new List<T>();

            await _database.GetCollection<T>(_collectionName)
                .Find(FilterDefinition<T>.Empty)
                .ForEachAsync(x => ret.Add(x));

            return ret;
        }

        public async Task InsertAsync(T item)
        {
            await _database.GetCollection<T>(_collectionName)
                .InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            var filter = Builders<T>.Filter
                .Eq(filterItem => filterItem.ObjectId, item.ObjectId);

            await _database
                .GetCollection<T>(_collectionName)
                .ReplaceOneAsync(filter, item);
        }

        public async Task RemoveAsync(T item)
        {
            var filter = Builders<T>.Filter
                .Eq(filterItem => filterItem.ObjectId, item.ObjectId);

            await _database
                .GetCollection<T>(_collectionName)
                .DeleteOneAsync(filter);
        }

        public async Task<Dictionary<string, Dictionary<string, object>>> GetAttributesDictionary()
        {
            var filter = Builders<Dictionary<string, Dictionary<string, object>>>
                .Filter.Eq("id", ObjectId.Parse(_attributesId));

            return await _database
                .GetCollection<Dictionary<string, Dictionary<string, object>>>(_attributesCollectionName)
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        #endregion METHODS
    }
}