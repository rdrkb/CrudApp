namespace NotificationApi.Events
{
    public class UserInfoUpdateEvent
    {
        public string UserId { get; set;}
        public string UserInfoField { get; set;}
        public string PreviousUserInfoFieldValue { get; set;}
        public string CurrentUserInfoFieldValue { get; set;}
    }
}
