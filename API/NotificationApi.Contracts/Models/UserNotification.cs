using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationApi.Contracts.Models
{
    public class UserNotification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Receiver { get; set; }
    }
}
