using Business.Students.Services;
using MediatR;

namespace Business.Students.Commands
{
    public class CreateStudentCommandConsumer : IRequestHandler<CreateStudentCommand>
    {
        private readonly IStudentService _studentService;

        public CreateStudentCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var studentModel = request.StudentModel;
            
            await _studentService.CreateNewStudent(studentModel);

        }
    }
}
