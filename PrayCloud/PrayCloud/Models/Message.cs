﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrayCloud
{
    public class Message
    {
        public DateTime Created { get; set; }

        [BsonRepresentation( BsonType.ObjectId )]
        public string Creator { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Body { get; set; }
        
        public List<string> Users { get; set; }
    }
}