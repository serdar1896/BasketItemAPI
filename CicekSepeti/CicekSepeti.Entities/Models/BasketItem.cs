using CicekSepeti.Entities.Infrastructure;
using MongoDB.Bson.Serialization.Attributes;

namespace CicekSepeti.Entities.Models
{
    public class BasketItem : EntityBase
    {

        [BsonElement("BasketIdx")]
        public string BasketIdx { get; set; }

        [BsonElement("ProductIdx")]
        public string ProductIdx { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("SalePrice")]
        public decimal SalePrice { get; set; }
    }
}
