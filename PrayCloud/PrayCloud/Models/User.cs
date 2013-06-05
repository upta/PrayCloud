using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PrayCloud
{
    public class User
    {
        [Key]
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string Id { get; set; }

        public DateTime LastAssigned { get; set; }

        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}