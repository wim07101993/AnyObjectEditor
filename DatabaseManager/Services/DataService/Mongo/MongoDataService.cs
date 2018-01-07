using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseManager.Services.DataService.Mongo
{
    public class MongoDataService : IDataService
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

        public async Task<IEnumerable<JObject>> GetAllAsync()
        {
            var ret = new List<JObject>();

            await _database.GetCollection<BsonDocument>(_collectionName)
                .Find(FilterDefinition<BsonDocument>.Empty)
                .ForEachAsync(x =>
                {
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    var str = x.ToString();
                    var totalMatch = new Regex(@"""_id"" : ObjectId\(""[a-z0-9]*""\)").Match(str);
                    var idMatch = new Regex(@"""[a-z0-9]*""").Match(totalMatch.Value);

                    var json = str.Substring(0, totalMatch.Index) +
                               "\"id\" : " + idMatch.Value +
                               str.Substring(totalMatch.Index + totalMatch.Length);

                    ret.Add((JObject) JsonConvert.DeserializeObject(json));
                });

            return ret;
        }


        public async Task InsertAsync(JObject item)
        {
            await _database
                .GetCollection<BsonDocument>(_collectionName)
                .InsertOneAsync(item.ToBsonDocument());
        }

        public async Task UpdateAsync(JObject item)
        {
        }

        public async Task RemoveAsync(JObject item)
        {
        }

        public async Task<Dictionary<string, Dictionary<string, object>>> GetAttributesDictionary()
        {
            var filter = Builders<BsonDocument>
                .Filter.Eq("_id", ObjectId.Parse(_attributesId));

            var bsonDoc = await _database
                .GetCollection<BsonDocument>(_attributesCollectionName)
                .Find(filter)
                .FirstOrDefaultAsync();

            var str = bsonDoc.ToString();
            var totalMatch = new Regex(@"""_id"" : ObjectId\(""[a-z0-9]*""\)").Match(str);
            var idMatch = new Regex(@"""[a-z0-9]*""").Match(totalMatch.Value);

            var json = str.Substring(0, totalMatch.Index) +
                       "\"id\" : { \"id\" : " + idMatch.Value + " }" +
                       str.Substring(totalMatch.Index + totalMatch.Length);

            var ret = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);

            return ret;
        }

        #endregion METHODS
    }
}