
namespace NotificationApi.Contracts.Events
{
    public class UserInfoUpdatedEvent
    {
        public string Username { get; set; }
        public List<UpdatedField> UpdatedFields { get; set; }
        public UserInfoUpdatedEvent()
        {
            UpdatedFields = new List<UpdatedField>();
        }
    }

    public class UpdatedField
    {
        public string FieldName { get; set; }
        public string CurrentValue { get; set; }
        public string PreviousValue { get; set; }
    }
}
