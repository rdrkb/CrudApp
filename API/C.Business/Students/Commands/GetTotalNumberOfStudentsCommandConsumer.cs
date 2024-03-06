using Business.Students.Services;
using MediatR;

namespace Business.Students.Commands
{
    public class GetTotalNumberOfStudentsCommandConsumer : IRequestHandler<GetTotalNumberOfStudentsCommand, long>
    {
        private readonly IStudentService _studentService;
        public GetTotalNumberOfStudentsCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<long> Handle(GetTotalNumberOfStudentsCommand request, CancellationToken cancellationToken)
        {
            var result = await _studentService.TotalNumberOfStudents(request.PageNumber, request.ItemPerPage, request.University, request.Department);
           
            return result;
        }
    }
}
