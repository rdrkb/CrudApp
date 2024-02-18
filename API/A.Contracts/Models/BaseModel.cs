using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Models
{
    public class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string Username { get; set; }
    }
}
