using A.Contracts.Update_Models;
using Contracts;
using MassTransit;
using Newtonsoft.Json;
using NotificationApi.Business.Notification;

namespace NotificationApi.Business.Consumers
{
    public class StudentInfoConsumer : IConsumer<UpdateStudentMessage>
    {
        private readonly INotificationRepository _notificationRepository;
        public StudentInfoConsumer(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task Consume(ConsumeContext<UpdateStudentMessage> context)
        {
            var message = context.Message;

            var studentInfo = JsonConvert.DeserializeObject<StudentInfo>(message.MessageData);

            string username = studentInfo.Username;
            var student = new UpdateStudentModel
            {
                Name = studentInfo.Name,
                University = studentInfo.University,
                Student_id = studentInfo.Student_id,
                Department = studentInfo.Department,
                Gender = studentInfo.Gender,
                Year_of_graduation = studentInfo.Year_of_graduation,
                Blood_group = studentInfo.Blood_group
            };

            //await _studentDataAccess.UpdateStudent(username, student);
        }
    }
}
