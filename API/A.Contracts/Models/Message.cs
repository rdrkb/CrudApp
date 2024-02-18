

using A.Contracts.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Models
{
    public class Message : MessageDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
