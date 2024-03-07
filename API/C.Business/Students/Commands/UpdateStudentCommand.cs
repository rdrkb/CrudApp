using A.Contracts.Update_Models;
using MediatR;
using NotificationApi.Contracts.Models;

namespace Business.Students.Commands
{
    public class UpdateStudentCommand : IRequest<UserNotification>
    {
        public string Username { get; }
        public UpdateStudentModel Student { get; }
        public UpdateStudentCommand(string username, UpdateStudentModel student)
        {
            Username = username;
            Student = student;
        }
    }
}
