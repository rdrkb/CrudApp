
using Contracts;
using NotificationApi.Contracts.Models;

namespace NotificationApi.Business.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task CreateNotification(UserNotification userNotification)
        {
            await _notificationRepository.CreateNotification(userNotification);
            return;
        }

        public async Task<List<UserNotification>> GetNotifications(int pageNumber, int pageSize)
        {
            return await _notificationRepository.GetNotifications(pageNumber, pageSize);
        }

        public async Task<long> GetNumberOfNotification()
        {
            return await _notificationRepository.GetNumberOfNotification();
        }
    }
}
