using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittenUrl.Models
{
    [BsonIgnoreExtraElements]
    public class KittenUrlModel
    {
        //[BsonId]
       // public int Id { get; set; }

        [BsonElement("ts")] // use the abbreviation in serialization
        public BsonTimestamp TimeStamp { get; set; }

        [BsonElement]
        public string LongUrl { get; set; }

        [BsonElement]
        public string ShortUrl { get; set; }

        [BsonDateTimeOptions]
        public DateTime DateCreated { get; set; }
    }
}
