namespace NotificationApi.Contracts.Models
{
    public class UserNotification
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public List<UpdatedField> Content { get; set; }
        public string Receiver { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
