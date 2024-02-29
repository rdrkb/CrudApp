using A.Contracts.Update_Models;
using Business.Teachers;
using Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApi.Controllers
{
    [Authorize(Roles = "admin,teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherLogic;

        public TeacherController(ITeacherService teacherLogic)
        {
            _teacherLogic = teacherLogic;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewTeacher(TeacherModel teacher)
        {
            try
            {
                await _teacherLogic.CreateNewTeacher(teacher);
                return StatusCode(201, new { success = true, message = "Teacher created successfully" });
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

        [HttpGet("getteachers")]
        public async Task<IActionResult> GetTeachers(int pageNumber = 1, int itemPerPage = 10, string university = "all", string department = "all")
        {
            List<TeacherModel> teacherModels = new List<TeacherModel>();

            try
            {
                teacherModels = await _teacherLogic.GetTeachers(pageNumber, itemPerPage, university, department);
                return Ok(teacherModels);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

        [HttpGet("getteacher/{username}")]
        public async Task<IActionResult> GetTeacher(string username)
        {
            TeacherModel teacherModel = new TeacherModel();

            try
            {
                teacherModel = await _teacherLogic.GetTeacher(username);
                return Ok(teacherModel);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpGet("totalteachers")]
        public async Task<long> TotalNumberOfTeachers(int pageNumber = 1, int itemPerPage = 10, string university = "all", string department = "all")
        {
            return await _teacherLogic.TotalNumberOfTeachers(pageNumber, itemPerPage, university, department);
        }

        [HttpPut("update/{username}")]
        public async Task<IActionResult> Teacher(string username, [FromBody] UpdateTeacherModel teacher)
        {
            try
            {
                await _teacherLogic.UpdateTeacher(username, teacher);

                return Ok(new { Message = "Teacher updated successfully." });
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
        public async Task<IActionResult> DeleteTeacher(string username)
        {
            try
            {
                bool isDeleted = await _teacherLogic.DeleteTeacher(username);

                if (isDeleted)
                {
                    return Ok(new { Message = "Teacher deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Teacher not found." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAllTeacher()
        {
            try
            {
                bool isDeleted = await _teacherLogic.DeleteAllTeacher();

                if (isDeleted)
                {
                    return Ok(new { Message = "All teachers deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "No teacher found" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpPatch("updatepartial/{username}")]
        public async Task<IActionResult> PartiallyUpdateTeacher(string username, [FromBody] JsonPatchDocument<TeacherModel> patchDocument)
        {
            if (string.IsNullOrEmpty(username) || patchDocument == null)
            {
                return BadRequest("Invalid username or payload");
            }

            try
            {
                bool isTeacherFound = await _teacherLogic.PartiallyUpdateTeacher(username, patchDocument);

                if (isTeacherFound)
                {
                    return Ok(new { Message = "Teacher updated successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Teacher not found." });
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
