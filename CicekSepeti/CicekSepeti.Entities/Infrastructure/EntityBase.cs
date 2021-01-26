using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CicekSepeti.Entities.Infrastructure
{
    public class EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
