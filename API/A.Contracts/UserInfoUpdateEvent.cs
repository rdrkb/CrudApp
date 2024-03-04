namespace Contracts
{
    public class UserInfoUpdateEvent
    {
        public string UserName { get; set; }
        public string UserInfoField { get; set; }
        public string PreviousUserInfoFieldValue { get; set; }
        public string CurrentUserInfoFieldValue { get; set; }
    }
}
