using A.Contracts.Update_Models;
using Business.Students;
using Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApi.Controllers
{

    //[Authorize(Roles = "admin,student")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentLogic;

        public StudentController(IStudentService studentLogic)
        {
            _studentLogic = studentLogic;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateNewStudent(StudentModel student)
        {
            try
            {
                await _studentLogic.CreateNewStudent(student);
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
                studentModels = await _studentLogic.GetStudents(pageNumber, itemPerPage, university, department);
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
                studentModel = await _studentLogic.GetStudent(username);
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
            return await _studentLogic.TotalNumberOfStudents(pageNumber, itemPerPage, university, department);
        }

        [HttpPut("update/{username}")]
        public async Task<IActionResult> UpdateStudent(string username, [FromBody] UpdateStudentModel student)
        {
            try
            {
                await _studentLogic.UpdateStudent(username, student);

                return Ok(new { Message = "Student updated successfully." });
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
                bool isDeleted = await _studentLogic.DeleteStudent(username);

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
                bool isDeleted = await _studentLogic.DeleteAllStudent();

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
            if (string.IsNullOrEmpty(username) || patchDocument == null)
            {
                return BadRequest("Invalid username or payload");
            }

            try
            {
                bool isStudentFound = await _studentLogic.PartiallyUpdateStudent(username, patchDocument);

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
