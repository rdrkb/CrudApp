using MassTransit;
using NotificationApi.Business.Models;
using NotificationApi.Business.Notification;
using NotificationApi.Contracts.Events;

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

            var updatedFields = message.UpdatedFields;

            var notification = new UserNotification 
            { 
                Title = "User Info Updated",
                Username = message.Username,
                Description = GetDescription(updatedFields),
                Receiver = "Admin",
                Content = updatedFields
            };

            await _notificationRepository.CreateNotification(notification);

            // send this notification to admins 
            
        }

        private string GetDescription(List<UpdatedField> updatedFields)
        {
            if (updatedFields.Count == 1)
            {
                return "";
            }
            if (updatedFields.Count == 2)
            {
                return "";
            } 

            return $"{updatedFields[0].FieldName} , {updatedFields[1].FieldName} and {updatedFields.Count - 2} others fields updated";
        }
    }
}
