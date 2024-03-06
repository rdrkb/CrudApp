﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Students.Commands
{
    public class GetTotalNumberOfStudentsCommand : IRequest<long>
    {
        public int PageNumber {  get; set; }
        public int ItemPerPage { get; set; }
        public string University { get; set; }
        public string Department { get; set; }

        public GetTotalNumberOfStudentsCommand(int pageNumber, int itemPerPage, string university, string department)
        {
            PageNumber = pageNumber;
            ItemPerPage = itemPerPage;
            University = university;
            Department = department;
        }
    }
}
