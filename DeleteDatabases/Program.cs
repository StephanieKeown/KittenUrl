using MongoDB.Driver;
using System;

namespace DeleteDatabases
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
            var client = new MongoClient("mongodb+srv://Steph:StephPassword@kittenurlcluster.myohq.azure.mongodb.net/");

            client.DropDatabaseAsync("KittenKeyDb");
            client.DropDatabaseAsync("KittenUrlDb");
        }
    }
}
