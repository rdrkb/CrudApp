using Business.Students.Services;
using MassTransit;
using MediatR;
using NotificationApi.Contracts.Events;

namespace Business.Students.Commands
{
    public class UpdateStudentCommandConsumer : IRequestHandler<UpdateStudentCommand>
    {
        private readonly IStudentService _studentService;
        private readonly IBus _bus;

        public UpdateStudentCommandConsumer(IStudentService studentService, IBus bus)
        {
            _studentService = studentService;
            _bus = bus;
        }

        public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            List<UpdatedField> updatedFields = await _studentService.UpdateStudent(request.Username, request.Student);

            await _bus.Publish(new UserInfoUpdatedEvent
            {
                Username = request.Username,
                UpdatedFields = updatedFields
            });

            return;
        }
    }
}
