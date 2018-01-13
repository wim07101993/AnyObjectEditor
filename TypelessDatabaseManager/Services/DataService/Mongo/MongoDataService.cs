using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Helpers.Attributes;
using Shared.Services;
using TypelessDatabaseManager.Models;
using Object = TypelessDatabaseManager.Models.Object;

namespace TypelessDatabaseManager.Services.DataService.Mongo
{
    public class MongoDataService : IDataService<Object>
    {
        #region FIELDS

        private readonly IMongoDatabase _database;
        private readonly string _collectionName;
        private readonly string _attributesId;
        private readonly string _attributesCollectionName;
        private Dictionary<string, Dictionary<string, object>> _attributes;

        #endregion FIELDS


        #region CONSTRUCTORS

        public MongoDataService(string connectionString, string databaseName, string collectionName,
            string attributesId = null, string attributesCollectionName = null)
        {
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
            _collectionName = collectionName;
            _attributesId = attributesId;
            _attributesCollectionName = attributesCollectionName;

            Task.Factory.StartNew(async () =>
                _attributes = await GetAttributesDictionary(attributesId, attributesCollectionName));
        }

        #endregion CONSTRUCTORS


        #region METHODS

        public async Task<IEnumerable<Object>> GetAllAsync()
        {
            var objects = new List<Object>();

            await _database.GetCollection<BsonDocument>(_collectionName)
                .Find(FilterDefinition<BsonDocument>.Empty)
                .ForEachAsync(x => objects.Add(ConvertBsonDocumentToObject(x)));

            return objects;
        }


        public async Task InsertAsync(Object item)
        {
        }

        public async Task UpdateAsync(Object item)
        {
        }

        public async Task RemoveAsync(Object item)
        {
        }

        public async Task<Dictionary<string, Dictionary<string, object>>> GetAttributesDictionary(string attributesId,
            string attributesCollectionName)
        {
            var filter = Builders<BsonDocument>
                .Filter.Eq("_id", ObjectId.Parse(attributesId));

            var bsonDoc = await _database
                .GetCollection<BsonDocument>(attributesCollectionName)
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

        private Object ConvertBsonDocumentToObject(BsonDocument document)
        {
            if (_attributes == null)
                _attributes = GetAttributesDictionary(_attributesId, _attributesCollectionName).Result;

            var properties = document
                .Elements
                .Select(x => ConvertBsonElementToProperty(x, _attributes))
                .ToDictionary(x => x.Name, x => x);

            return new Object(properties);
        }

        private Property ConvertBsonElementToProperty(BsonElement element,
            Dictionary<string, Dictionary<string, object>> attributes)
        {
            var name = element.Name;

            ConvertBsonValueToTypeAndValue(element.Value, out var value);
            var obj = new Object {Value = value};

            //var propAttributes = attributes[name]?.Select(x => new Attribute(x.Key, x.Value));
            var propAttributes = new List<IAttribute>();

            if (attributes.ContainsKey(name))
                propAttributes.AddRange(attributes[name].Select(ConvertAttributeKeyValuePairToIAttribute));

            return new Property(name, true, true, propAttributes, obj);
        }

        private void ConvertBsonValueToTypeAndValue(BsonValue bsonValue, out object value)
        {
            switch (bsonValue.BsonType)
            {
                case BsonType.Double:
                    value = bsonValue.AsDouble;
                    return;
                case BsonType.String:
                    value = bsonValue.AsString;
                    return;
                case BsonType.Document:
                    value = ConvertBsonDocumentToObject(bsonValue.AsBsonDocument);
                    return;
                case BsonType.Array:
                    // TODO
                    value = null;
                    return;
                case BsonType.Binary:
                    value = bsonValue.AsBsonBinaryData.Bytes;
                    return;
                case BsonType.Boolean:
                    value = bsonValue.AsBoolean;
                    return;
                case BsonType.DateTime:
                    value = bsonValue.ToUniversalTime();
                    return;
                case BsonType.RegularExpression:
                    value = bsonValue.AsRegex;
                    return;
                case BsonType.Int32:
                    value = bsonValue.AsInt32;
                    return;
                case BsonType.Int64:
                    value = bsonValue.AsInt64;
                    return;
                case BsonType.Decimal128:
                    value = bsonValue.AsDecimal128;
                    return;
                case BsonType.ObjectId:
                    value = bsonValue.AsObjectId.ToString();
                    return;
                case BsonType.JavaScript:
                    value = bsonValue.AsBsonJavaScript.ToString();
                    return;
                case BsonType.Symbol:
                    value = bsonValue.AsBsonSymbol.ToString();
                    return;
                case BsonType.JavaScriptWithScope:
                    value = bsonValue.AsBsonJavaScriptWithScope.ToString();
                    return;
                case BsonType.Timestamp:
                    value = bsonValue.AsBsonTimestamp.Timestamp;
                    return;
                case BsonType.MinKey:
                    value = bsonValue.AsBsonMinKey.ToString();
                    return;
                case BsonType.MaxKey:
                    value = bsonValue.AsBsonMaxKey.ToString();
                    return;
                default:
                    value = null;
                    return;
            }
        }

        private IAttribute ConvertAttributeKeyValuePairToIAttribute(KeyValuePair<string, object> pair)
        {
            switch (pair.Key)
            {
                case nameof(DescriptionAttribute):
                    return new DescriptionAttribute((bool) pair.Value);
                case nameof(IdAttribute):
                    return new IdAttribute((bool) pair.Value);
                case nameof(PictureAttribute):
                    return new PictureAttribute((bool) pair.Value);
                case nameof(SubtitleAttribute):
                    return new SubtitleAttribute((bool) pair.Value);
                case nameof(TitleAttribute):
                    return new TitleAttribute((bool)pair.Value);
                case nameof(BrowsableAttribute):
                    return new BrowsableAttribute((bool)pair.Value);
                case nameof(DisplayNameAttribute):
                    return new DisplayNameAttribute((string)pair.Value);

                default:
                    return null;
            }
        }

        #endregion METHODS
    }
}