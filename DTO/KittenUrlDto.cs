using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DTO
{
    [BsonIgnoreExtraElements]
    public class KittenUrlDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

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
