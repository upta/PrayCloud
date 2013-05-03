using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrayCloud
{
    public class User
    {
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        public List<Message> Messages { get; set; }
    }
}