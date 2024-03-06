using Contracts.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
