using CicekSepeti.Entities.Infrastructure;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CicekSepeti.Entities.Models
{
    public class Basket:EntityBase
    {
        [BsonElement("CustomerIdx")]
        public string CustomerIdx { get; set; }

        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}
