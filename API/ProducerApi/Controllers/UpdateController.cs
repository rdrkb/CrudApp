using A.Contracts.Update_Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProducerApi.Models;

namespace ProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IBusControl _busControl;

        public UpdateController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpPut("update/{username}")]
        public async Task<IActionResult> UpdateStudent(string username, [FromBody] UpdateStudentModel student)
        {
            var messageData = JsonConvert.SerializeObject(new
            {
                Username = username,
                Name = student.Name,
                University = student.University,
                Student_id = student.Student_id,
                Department = student.Department,
                Gender = student.Gender,
                Year_of_graduation = student.Year_of_graduation,
                Blood_group = student.Blood_group
            });


            await _busControl.Publish(new UpdateStudentMessage
            {
                MessageData = messageData
            });

            return Ok(new { Message = "Message sent successfully." });
        }
    }
}
