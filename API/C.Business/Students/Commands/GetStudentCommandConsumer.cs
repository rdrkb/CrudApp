using Business.Students.Services;
using Contracts.Models;
using MediatR;

namespace Business.Students.Commands
{
    public class GetStudentCommandConsumer : IRequestHandler<GetStudentCommand, StudentModel>
    {
        private readonly IStudentService _studentService;
        public GetStudentCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<StudentModel> Handle(GetStudentCommand request, CancellationToken cancellationToken)
        {
            var studentModel = await _studentService.GetStudent(request.Username);

            return studentModel;
        }
    }
}
