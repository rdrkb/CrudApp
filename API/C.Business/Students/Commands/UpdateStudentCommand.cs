using A.Contracts.Update_Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Students.Commands
{
    public class UpdateStudentCommand : IRequest
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
