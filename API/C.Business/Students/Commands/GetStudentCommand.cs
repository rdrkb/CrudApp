using Contracts.Models;
using MediatR;

namespace Business.Students.Commands
{
    public class GetStudentCommand : IRequest<StudentModel>
    {
        public string Username { get; set; }
        public GetStudentCommand(string username)
        {
            Username = username;
        }
        
    }
}
