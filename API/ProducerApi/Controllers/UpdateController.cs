using A.Contracts;
using A.Contracts.Update_Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Controllers
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
                student.Name,
                student.University,
                student.Student_id,
                student.Department,
                student.Gender,
                student.Year_of_graduation,
                student.Blood_group
            });


            await _busControl.Publish(new UpdateStudentMessage
            {
                MessageData = messageData
            });

            return Ok(new { Message = "Message sent successfully." });
        }
    }
}
