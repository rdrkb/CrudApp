using Business.Students.Services;
using MediatR;

namespace Business.Students.Commands
{
    public class DeleteAllStudentCommandConsumer : IRequestHandler<DeleteAllStudentCommand, bool>
    {
        private readonly IStudentService _studentService;

        public DeleteAllStudentCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<bool> Handle(DeleteAllStudentCommand request, CancellationToken cancellationToken)
        {
            return await _studentService.DeleteAllStudent();
        }
    }
}
