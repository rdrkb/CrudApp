using Contracts.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Students.Commands
{
    public class GetStudentsCommand : IRequest<List<StudentModel>>
    {
        public int PageNumber { get; set; }
        public int ItemPerPage { get; set; }
        public string University { get; set; }
        public string Department { get; set; }

        public GetStudentsCommand(int pageNumber, int itemPerPage, string university, string department)
        {
            PageNumber = pageNumber;
            ItemPerPage = itemPerPage;
            University = university;
            Department = department;
        }  
    }
}
