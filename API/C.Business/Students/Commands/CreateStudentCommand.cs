using Contracts.Models;
using MediatR;

namespace Business.Students.Commands
{
    public class CreateStudentCommand : IRequest
    {
        public StudentModel StudentModel { get; set; }

        public CreateStudentCommand(StudentModel studentModel)
        {
            StudentModel = studentModel;
        }
    }
}
