using NotificationApi.Contracts.Models;

namespace NotificationApi.Contracts.Events
{
    public class UserInfoUpdatedEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public List<UpdatedField> Content { get; set; }
        public string Receiver { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
