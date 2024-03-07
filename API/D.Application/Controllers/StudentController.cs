using A.Contracts.Update_Models;
using Business.Students.Commands;
using Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.Contracts.Models;

namespace SchoolManagementApi.Controllers
{

    //[Authorize(Roles = "admin,student")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateNewStudent(StudentModel student)
        {
            try
            {
                await _mediator.Send(new CreateStudentCommand(student));

                return StatusCode(201, new { success = true, message = "Student created successfully" });
            }
            catch (FormatException e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { success = false, message = e.Message });
            }
        }


        [HttpGet("getstudents")]
        public async Task<IActionResult> GetStudents(int pageNumber = 1, int itemPerPage = 10, string university = "all", string department = "all")
        {
            List<StudentModel> studentModels = new List<StudentModel>();

            try
            {
                studentModels = await _mediator.Send(new GetStudentsCommand(pageNumber, itemPerPage, university, department));
                return Ok(studentModels);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

        [HttpGet("getstudent/{username}")]
        public async Task<IActionResult> GetStudent(string username)
        {
            StudentModel studentModel = new StudentModel();

            try
            {
                studentModel = await _mediator.Send(new GetStudentCommand(username));
                return Ok(studentModel);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }


        [HttpGet("totalstudents")]
        public async Task<long> TotalNumberOfStudents(int pageNumber = 1, int itemPerPage = 10, string university = "all", string department = "all")
        {
            return await _mediator.Send(new GetTotalNumberOfStudentsCommand(pageNumber, itemPerPage, university, department));
        }

        [HttpPut("update/{username}")]
        public async Task<IActionResult> UpdateStudent(string username, [FromBody] UpdateStudentModel student)
        {
            UserNotification userNotification = new UserNotification();
            try
            {
                userNotification = await _mediator.Send(new UpdateStudentCommand(username, student));

                return Ok(userNotification);
            }
            catch (FormatException e)
            {
                return BadRequest(new { ErrorMessage = "Invalid format for one or more input fields." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteStudent(string username)
        {
            try
            {
                bool isDeleted = await _mediator.Send(new DeleteStudentCommand(username));

                if (isDeleted)
                {
                    return Ok(new { Message = "Student deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Student not found." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAllStudent()
        {
            try
            {
                bool isDeleted = await _mediator.Send(new DeleteAllStudentCommand());

                if (isDeleted)
                {
                    return Ok(new { Message = "All students deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "No student found" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpPatch("updatepartial/{username}")]
        public async Task<IActionResult> PartiallyUpdateStudent(string username, [FromBody] JsonPatchDocument<StudentModel> patchDocument)
        {
            try
            {
                bool isStudentFound = await _mediator.Send(new PartiallyUpdateStudentCommand(username, patchDocument));

                if (isStudentFound)
                {
                    return Ok(new { Message = "Student updated successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Student not found." });
                }
            }
            catch (FormatException e)
            {
                return BadRequest(new { ErrorMessage = "Invalid format of input field." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }
    }
}
