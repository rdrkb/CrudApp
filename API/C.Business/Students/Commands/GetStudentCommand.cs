using Contracts.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
