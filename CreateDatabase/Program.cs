using DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CreateDatabase
{
    class Program
    {
        private static List<string> _listUniqueStrings;
        private static IMongoClient _mongoClient;
        private static IMongoDatabase _mongoDatabase;
        private static readonly string _uniqueKeyCharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private static char[] _chars;
        private static int _counter = 0;
        private static int _amount = 1000000;

        static void Main(string[] args)
        {
            Console.WriteLine("Creating KittenUrlDb");
            var result = CreateKittenUrlDbAndCollection();
            if(result == false)
            {
                Console.Write("Failed to create KittenUrlDb");
            }
            else
            {
                Console.Write("KittenUrlDb Created.");  
            }

            Console.WriteLine("to create kitten key db press Y, else N to exit");
            var createKittenKeyDb = Console.ReadLine();
            if(createKittenKeyDb == "N") 
            { 
                Environment.Exit(0); 
            }
            else
            {
                Console.WriteLine("Creating KittenKeyDb");
            }
            
            var keyDbResult = CheckForKeyDBAndSeed();
            if(keyDbResult == false)
            {
                Console.WriteLine("Failed to create KittenKeyDb.  Exiting.");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("KittenKeyDb created. Exiting");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }
            
        public static bool CreateKittenUrlDbAndCollection()
        {
            _mongoClient = new MongoClient("mongodb+srv://Steph:StephPassword@kittenurlcluster.myohq.azure.mongodb.net/");

            //this will create if not exists
            _mongoDatabase = _mongoClient.GetDatabase("KittenUrlDb"); 

            var collection = _mongoDatabase.GetCollection<KittenUrlDto>("KittenUrlDb");

            //To create the mongodb database and collection you must insert a record.
            var testRecord = new KittenUrlDto()
            {
                LongUrl = "www.thisisatestlongurl.com/longtestpath",
                ShortUrl = "www.thisisatesturl.com/testpath",
                DateCreated = DateTime.UtcNow,
            };

            var testSavedRecord = collection.Find(u => u.ShortUrl == testRecord.ShortUrl).ToList();

            //This will create the collection if it does not exist
            if (testSavedRecord.Count == 0)
            {
                collection.InsertOne(testRecord);
            }

            var filter = new BsonDocument("name", "KittenUrlDb");
            var options = new ListCollectionNamesOptions { Filter = filter };

            var collectionExists =  _mongoDatabase.ListCollectionNames(options).Any();

            return collectionExists;
        }

        public static bool CheckForKeyDBAndSeed()
        {
            _mongoClient = new MongoClient("mongodb+srv://Steph:StephPassword@kittenurlcluster.myohq.azure.mongodb.net/");
            _mongoDatabase = _mongoClient.GetDatabase("KittenKeyDb");

            var collection = _mongoDatabase.GetCollection<UniqueKeyDto>("KittenKeyDb");

            var count = collection.CountDocumentsAsync(new BsonDocument()).Result;

            //exit if database is already present and populated
            if(count == _amount + 1)
            {
                return true;
            }
            else
            {
                //to create the mongodb database and collection you must insert a record.
                var testKey = new UniqueKeyDto()
                {
                    InUse = true,
                    UniqueKey = "testKey"
                };

                var testRecord = collection.Find(u => u.UniqueKey == testKey.UniqueKey).ToList();

                //this will create the collection
                if (testRecord.Count == 0)
                {
                    collection.InsertOne(testKey);
                }

                var filter = new BsonDocument("name", "KittenKeyDb");
                var options = new ListCollectionNamesOptions { Filter = filter };

                var collectionExists = _mongoDatabase.ListCollectionNames(options).Any();

                //exit if the program has failed
                if (!collectionExists)
                {
                    return false;
                }
                else
                {
                    _chars = _uniqueKeyCharSet.ToCharArray();
                    _listUniqueStrings = new List<string>();
                    var createPasswordsForDatabase = CreatePasswordsForKeyDb();

                    //if the number of passwords is not what we have set it to exit
                    if (!(_listUniqueStrings.Count == _amount))
                    {
                        return false;
                    }
                    else
                    {
                        var passwords = GetUniqueKeyDtoList();

                        try
                        {
                            collection.InsertMany(passwords);
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    }

                    return false;
                }
            }          
        }

        public static async Task<bool> CreatePasswordsForKeyDb()
        {
            //Passwordgenerator
            GenerateAllPasswords("", 0, 7);

            var testDuplicatesResult = CheckKeysForDuplicates();

            if (_listUniqueStrings.Count == _amount && testDuplicatesResult == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<UniqueKeyDto> GetUniqueKeyDtoList()
        {
            var list = new List<UniqueKeyDto>();
            foreach (var item in _listUniqueStrings)
            {
                var model = new UniqueKeyDto()
                {
                    UniqueKey = item,
                    InUse = false
                };
                list.Add(model);
            }
            return list;
        }    

        public static void GenerateAllPasswords(string pwd, int pos, int siz)
        {
            if (_counter < _amount)
            {
                if (pos < siz)
                {
                    foreach (char ch in _chars)
                    {
                        GenerateAllPasswords(pwd + ch, pos + 1, siz);
                    }
                }
                else
                {
                    _listUniqueStrings.Add(pwd);
                    _counter++;
                }
            }
        }

        public static bool CheckKeysForDuplicates()
        {
            var duplicatesList = _listUniqueStrings
              .Select((t, i) => new { Index = i, Text = t })
              .GroupBy(g => g.Text)
              .Where(g => g.Count() > 1).ToList();

            if(duplicatesList.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
