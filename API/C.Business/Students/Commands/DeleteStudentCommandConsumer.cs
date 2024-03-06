using Business.Students.Services;
using MediatR;

namespace Business.Students.Commands
{
    internal class DeleteStudentCommandConsumer : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IStudentService _studentService;
        public DeleteStudentCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            return await _studentService.DeleteStudent(request.Username);
        }
    }
}
