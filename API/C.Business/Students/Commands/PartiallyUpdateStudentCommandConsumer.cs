using Business.Students.Services;
using MediatR;

namespace Business.Students.Commands
{
    public class PartiallyUpdateStudentCommandConsumer : IRequestHandler<PartiallyUpdateStudentCommand, bool>
    {
        private readonly IStudentService _studentService;

        public PartiallyUpdateStudentCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<bool> Handle(PartiallyUpdateStudentCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Username) || request.PatchDocument == null)
            {
                throw new Exception("Invalid username or payload");
            }

            return await _studentService.PartiallyUpdateStudent(request.Username, request.PatchDocument);
        }
    }
}
