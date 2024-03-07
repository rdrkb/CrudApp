using MassTransit;
using NotificationApi.Business.Notification;
using NotificationApi.Contracts.Events;
using NotificationApi.Contracts.Models;

namespace NotificationApi.Business.Consumers
{
    public class UserInfoUpdatedEventConsumer : IConsumer<UserInfoUpdatedEvent>
    {
        private readonly INotificationRepository _notificationRepository;

        public UserInfoUpdatedEventConsumer(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Consume(ConsumeContext<UserInfoUpdatedEvent> context)
        {
            var message = context.Message;

            var notification = new UserNotification
            {
                Id = message.Id,
                Title = message.Title,
                Username = message.Username,
                Description = message.Description,
                Receiver = message.Receiver,
                Content = message.Content
            };

            await _notificationRepository.CreateNotification(notification);
        }
    }
}
