

using Contracts;
using NotificationApi.Contracts.Models;

namespace NotificationApi.Business.Notification
{
    public interface INotificationRepository
    {
        Task CreateNotification(UserNotification userNotification);
        Task<List<UserNotification>> GetNotifications(int pageNumber, int pageSize);

        Task<long> GetNumberOfNotification();
    }
}
