using Business.Students.Services;
using MassTransit;
using MediatR;
using NotificationApi.Contracts.Events;
using NotificationApi.Contracts.Models;

namespace Business.Students.Commands
{
    public class UpdateStudentCommandConsumer : IRequestHandler<UpdateStudentCommand, UserNotification>
    {
        private readonly IStudentService _studentService;
        private readonly IBus _bus;

        public UpdateStudentCommandConsumer(IStudentService studentService, IBus bus)
        {
            _studentService = studentService;
            _bus = bus;
        }

        public async Task<UserNotification> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            List<UpdatedField> updatedFields = await _studentService.UpdateStudent(request.Username, request.Student);

            UserNotification notification = new UserNotification
            {
                Id = Guid.NewGuid().ToString(),
                Title = "User Info Updated",
                Username = request.Username,
                Description = GetDescription(updatedFields, request.Username),
                Receiver = "Admin",
                Content = updatedFields,
                CreatedAt = DateTime.UtcNow
            };


            await _bus.Publish(new UserInfoUpdatedEvent
            {
                Id = notification.Id,
                Title = notification.Title,
                Username = notification.Username,
                Description = notification.Description,
                Receiver = notification.Receiver,
                Content = notification.Content,
                CreatedAt = notification.CreatedAt
            });

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
