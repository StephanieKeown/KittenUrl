using DataAccess.Interfaces;
using DTO;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class KittenUrlRepository : IKittenUrlRepository
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<KittenUrlDto> _urlCollection;
        private IMongoCollection<UniqueKeyDto> _keyCollection;

        public KittenUrlRepository()
        {
            _mongoClient = new MongoClient("mongodb+srv://Steph:StephPassword@kittenurlcluster.myohq.azure.mongodb.net/");
            _mongoDatabase = _mongoClient.GetDatabase("KittenUrlDb");
            _urlCollection = _mongoDatabase.GetCollection<KittenUrlDto>("KittenUrlDb");
        }


        public bool testmethod()
        {
            HandleShortenUrl("ppp");
            return true;
        }

        public string HandleShortenUrl(string url) //should this not be a tasl?
        {
            //get id
            var id = CreateRecord();

            var _keyDB = _mongoClient.GetDatabase("KittenKeyDb");
            _keyCollection = _keyDB.GetCollection<UniqueKeyDto>("KittenKeyDb");

            var urlKeyRecord = _keyCollection.Find(x => x.InUse == false).FirstOrDefault(); //this will take longer and longer as more keys are used

            var shortenedUrl = urlKeyRecord.UniqueKey;

            //update record
            var RecordUpdated = UpdateRecord(id.Result, url, shortenedUrl);

            urlKeyRecord.InUse = true;
            try
            {
                var replaceOneResult = _keyCollection.ReplaceOneAsync(   //await async static??
                        doc => doc.Id == urlKeyRecord.Id,
                        urlKeyRecord);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            var myupdatedkeyrecord = _keyCollection.Find(k => k.InUse == true).ToList();

            return shortenedUrl;
        }


        public string FindUrl(string url)
        {
            _mongoDatabase = _mongoClient.GetDatabase("KittenUrlDb");
            _urlCollection = _mongoDatabase.GetCollection<KittenUrlDto>("KittenUrlDb");

            var dbList = _mongoClient.ListDatabases().ToList();

            var y = _urlCollection.Find(s => s.DateCreated < DateTime.Now).ToList();

            var path = "https://localhost:44314/";
            var completeURl = path + url;

            var x = _urlCollection.Find(s => s.ShortUrl == url).ToList();

            if (x.Count > 0)
            {
                return x.FirstOrDefault().LongUrl;
            }
            else
            {
                return null; //translate to not found
            }
        }

        public async Task<ObjectId> CreateRecord() //should this be static
        {
            var client = new MongoClient("mongodb+srv://Steph:StephPassword@kittenurlcluster.myohq.azure.mongodb.net/sample_geospatial?retryWrites=true&w=majority");
            var database = client.GetDatabase("KittenUrlDb"); //this will create if not exists

            //var dbList = client.ListDatabases().ToList();

            var collection = database.GetCollection<KittenUrlDto>("KittenUrlDb"); //if does not exist it creates

            var model = new KittenUrlDto();
            await collection.InsertOneAsync(model);
            return model.Id;
        }

        public async Task<bool> UpdateRecord(ObjectId id, string longUrl, string shortUrl)
        {
            var myShortUrl = new KittenUrlDto() { Id = id, ShortUrl = shortUrl, LongUrl = longUrl, DateCreated = DateTime.UtcNow };

            try
            {
                var replaceOneResult = await _urlCollection.ReplaceOneAsync(
                        doc => doc.Id == myShortUrl.Id,
                        myShortUrl);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return true;
        }
    }
}

