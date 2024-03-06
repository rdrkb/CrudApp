using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Students.Commands
{
    public class DeleteAllStudentCommand : IRequest<bool>
    {
    }
}
