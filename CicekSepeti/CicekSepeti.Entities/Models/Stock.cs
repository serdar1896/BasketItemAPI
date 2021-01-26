using CicekSepeti.Entities.Infrastructure;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CicekSepeti.Entities.Models
{
    public class Stock:EntityBase
    {
        [BsonElement("ProductIdx")]
        public string ProductIdx { get; set; }
        [BsonElement("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [BsonElement("Quantity")]
        public int Quantity { get; set; }
        [BsonElement("WarehouseAddress")]
        public string WarehouseAddress { get; set; }
        [BsonElement("Type")]
        public int Type { get; set; }
    }
}
