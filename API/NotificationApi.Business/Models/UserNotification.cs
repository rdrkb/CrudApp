using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NotificationApi.Contracts.Events;

namespace NotificationApi.Business.Models
{
    public class UserNotification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public List<UpdatedField> Content { get; set; }
        public string Receiver { get; set; }
    }
}
