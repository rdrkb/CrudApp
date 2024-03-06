using Business.Students.Services;
using Contracts.Models;
using MediatR;

namespace Business.Students.Commands
{
    public class GetStudentsCommandConsumer : IRequestHandler<GetStudentsCommand, List<StudentModel>>
    {
        private readonly IStudentService _studentService;

        public GetStudentsCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<List<StudentModel>> Handle(GetStudentsCommand request, CancellationToken cancellationToken)
        {
            var result = await _studentService.GetStudents(request.PageNumber, request.ItemPerPage, request.University, request.Department);
            
            return result;
        }
    }
}
