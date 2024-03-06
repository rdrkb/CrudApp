using MediatR;

namespace Business.Students.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public string Username { get; }
        public DeleteStudentCommand(string username)
        {
            Username = username;
        }

    }
}
