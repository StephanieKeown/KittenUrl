using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
        [BsonIgnoreExtraElements] //maybe dont need BsonTypes here?? Keep it lightwieght -- else front end mpdel does not need

        public class UniqueKeyDto
        {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonElement]
            public string UniqueKey { get; set; } //should i override the mongodb id?

            [BsonElement]
            public bool InUse { get; set; }
        }
    }

