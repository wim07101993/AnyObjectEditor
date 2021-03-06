﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Helpers.Attributes;
using Shared.Services;
using Shared.Services.Converter;
using TypelessDatabaseManager.Helpers;
using TypelessDatabaseManager.Models;
using Object = TypelessDatabaseManager.Models.Object;

namespace TypelessDatabaseManager.Services.DataService.Mongo
{
    public class MongoDataService : IDataService<Object>, IObjectConverter, IObjectConverter<BsonDocument>
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

        #region data service

        public async Task<IEnumerable<Object>> GetAllAsync()
        {
            var objects = new List<Object>();

            await _database.GetCollection<BsonDocument>(_collectionName)
                .Find(FilterDefinition<BsonDocument>.Empty)
                .ForEachAsync(x => objects.Add(ConvertToObject(x)));

            return objects;
        }

        public async Task InsertAsync(Object item)
        {
            await _database.GetCollection<BsonDocument>(_collectionName)
                .InsertOneAsync(ConvertBack(item));
        }

        public async Task UpdateAsync(Object item)
        {
            var document = ConvertBack(item);
            var filter = Builders<BsonDocument>.Filter
                .Eq(filterItem => filterItem["ObjectID"], document["ObjectID"]);

            await _database
                .GetCollection<BsonDocument>(_collectionName)
                .ReplaceOneAsync(filter, document);
        }

        public async Task RemoveAsync(Object item)
        {
            var document = ConvertBack(item);
            var filter = Builders<BsonDocument>.Filter
                .Eq(filterItem => filterItem["ObjectID"], document["ObjectID"]);

            await _database
                .GetCollection<BsonDocument>(_collectionName)
                .DeleteOneAsync(filter);
        }

        #endregion data service

        #region object converter

        public Object ConvertToObject(object obj)
            => ConvertToObject(obj as BsonDocument);

        public Object ConvertToObject(BsonDocument document)
        {
            if (_attributes == null)
                _attributes = GetAttributesDictionary(_attributesId, _attributesCollectionName).Result;

            var properties = document
                .Elements
                .Select(x => ConvertBsonElementToProperty(x, _attributes))
                .ToDictionary(x => x.Name, x => x);

            return new Object(properties);
        }

        public BsonDocument ConvertBack(Object obj)
            => new BsonDocument(obj.ToDictionary(x => x.Key, x => x.Value.Value));

        object IObjectConverter.ConvertBack(Object obj)
            => ConvertBack(obj);

        #endregion object converter

        
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
        
        private Property ConvertBsonElementToProperty(BsonElement element,
            IReadOnlyDictionary<string, Dictionary<string, object>> attributes)
        {
            var name = element.Name;

            var propAttributes = new List<IAttribute>();
            if (attributes.ContainsKey(name))
                propAttributes.AddRange(attributes[name].Select(ConvertAttributeKeyValuePairToIAttribute));

            ConvertBsonValueToTypeAndValue(element.Value, out var value, propAttributes);
            var obj = new Object {Value = value};


            return new Property(name, true, true, propAttributes, obj);
        }

        private void ConvertBsonValueToTypeAndValue(BsonValue bsonValue, out object value,
            IEnumerable<IAttribute> attributes)
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
                    value = ConvertToObject(bsonValue.AsBsonDocument);
                    return;
                case BsonType.Array:
                    // TODO
                    value = null;
                    return;
                case BsonType.Binary:
                    var bytes = bsonValue.AsBsonBinaryData.Bytes;

                    if (attributes?.Any(x => x?.Name == PictureAttribute.NAME && x.Value as bool? == true ||
                                             x?.Name == ImageAttribute.NAME && x.Value as bool? == true) == true)
                        value = DatabaseValueConverter.ConvertToImage(bytes);
                    else if (attributes?.Any(x => x?.Name == ColorAttribute.NAME && x.Value as bool? == true) == true)
                        value = DatabaseValueConverter.ConvertToColor(bytes);
                    else
                        value = bytes;

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

        private static IAttribute ConvertAttributeKeyValuePairToIAttribute(KeyValuePair<string, object> pair)
        {
            switch (pair.Key)
            {
                case BrowsableAttribute.NAME:
                    return new BrowsableAttribute((bool) pair.Value);
                case ColorAttribute.NAME:
                    return new ColorAttribute((bool) pair.Value);
                case DescriptionAttribute.NAME:
                    return new DescriptionAttribute((bool) pair.Value);
                case DisplayNameAttribute.NAME:
                    return new DisplayNameAttribute((string) pair.Value);
                case IdAttribute.NAME:
                    return new IdAttribute((bool) pair.Value);
                case ImageAttribute.NAME:
                    return new ImageAttribute((bool) pair.Value);
                case PictureAttribute.NAME:
                    return new PictureAttribute((bool) pair.Value);
                case SubtitleAttribute.NAME:
                    return new SubtitleAttribute((bool) pair.Value);
                case TitleAttribute.NAME:
                    return new TitleAttribute((bool) pair.Value);
                default:
                    return null;
            }
        }

        #endregion METHODS
    }
}