using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseManager.Models.Bases;
using MongoDB.Driver;

namespace DatabaseManager.Services.DataService.Mongo
{
    public class MongoDataService<T> : IDataService<T> where T : IMongoModel
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

        #endregion METHODS
    }
}