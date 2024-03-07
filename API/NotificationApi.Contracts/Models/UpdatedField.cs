
namespace NotificationApi.Contracts.Models
{
    public class UpdatedField
    {
        public string FieldName { get; set; }
        public string CurrentValue { get; set; }
        public string PreviousValue { get; set; }
    }
}
