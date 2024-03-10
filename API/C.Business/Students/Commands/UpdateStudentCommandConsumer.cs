using Business.Students.Services;
using MassTransit;
using MediatR;
using NotificationApi.Contracts.Events;
using NotificationApi.Contracts.Models;
using SchoolManagement.Shared.CQRS;

namespace Business.Students.Commands
{
    public class UpdateStudentCommandConsumer : ACommandConsumer<UpdateStudentCommand, UserNotification>
    {
        private readonly IStudentService _studentService;
        private readonly IBus _bus;

        public UpdateStudentCommandConsumer(IStudentService studentService, IBus bus)
        {
            _studentService = studentService;
            _bus = bus;
        }

        protected override async Task<UserNotification> ExecuteAsync(UpdateStudentCommand command, ConsumeContext<UpdateStudentCommand> context = null)
        {
            List<UpdatedField> updatedFields = await _studentService.UpdateStudent(command.Username, command.Student);

            UserNotification notification = new UserNotification
            {
                Id = Guid.NewGuid().ToString(),
                Title = "User Info Updated",
                Username = command.Username,
                Description = GetDescription(updatedFields, command.Username),
                Receiver = "@admin",
                Content = updatedFields,
                CreatedAt = DateTime.UtcNow
            };

            var userInfoUpdatedEvent = new UserInfoUpdatedEvent
            {
                Id = notification.Id,
                Title = notification.Title,
                Username = notification.Username,
                Description = notification.Description,
                Receiver = notification.Receiver,
                Content = notification.Content,
                CreatedAt = notification.CreatedAt
            };

            if (context is not null)
            {
                await context.Publish(userInfoUpdatedEvent);
            }
            else
            {
                await _bus.Publish(userInfoUpdatedEvent);
            }

            return notification;
        }

        private string GetDescription(List<UpdatedField> updatedFields, string username)
        {
            if (updatedFields.Count == 1)
            {
                return $"{username} has updated his {updatedFields[0].FieldName}";
            }
            if (updatedFields.Count == 2)
            {
                return $"{username} has updated his {updatedFields[0].FieldName} and {updatedFields[1].FieldName}";
            }

            return $"{username} has updated his {updatedFields[0].FieldName} , {updatedFields[1].FieldName} and {updatedFields.Count - 2} others fields";
        }
    }
}
