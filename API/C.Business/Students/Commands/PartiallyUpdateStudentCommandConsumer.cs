using Business.Students.Services;
using MassTransit;
using SchoolManagement.Shared.CQRS;

namespace Business.Students.Commands
{
    public class PartiallyUpdateStudentCommandConsumer : ACommandConsumer<PartiallyUpdateStudentCommand, bool>
    {
        private readonly IStudentService _studentService;

        public PartiallyUpdateStudentCommandConsumer(IStudentService studentService)
        {
            _studentService = studentService;
        }

        protected override async Task<bool> ExecuteAsync(PartiallyUpdateStudentCommand command, ConsumeContext<PartiallyUpdateStudentCommand> context = null)
        {
            if (string.IsNullOrEmpty(command.Username) || command.PatchDocument == null)
            {
                throw new Exception("Invalid username or payload");
            }

            return await _studentService.PartiallyUpdateStudent(command.Username, command.PatchDocument);
        }
    }
}
