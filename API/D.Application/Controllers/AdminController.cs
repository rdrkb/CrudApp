using A.Contracts.Models;
using A.Contracts.Update_Models;
using C.Business.AdminLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminLogic _adminLogic;

        public AdminController(IAdminLogic adminLogic)
        {
            _adminLogic = adminLogic;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewAdmin(AdminModel admin)
        {
            try
            {
                await _adminLogic.CreateNewAdmin(admin);
                return StatusCode(201, new { success = true, message = "admin created successfully" });
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
        [HttpGet("getadmins")]
        public async Task<IActionResult> GetAdmins(int pageNumber = 1, int itemPerPage = 10)
        {
            List<AdminModel> adminModels = new List<AdminModel>();

            try
            {
                adminModels = await _adminLogic.GetAdmins(pageNumber, itemPerPage);
                return Ok(adminModels);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

        [HttpGet("getadmin/{username}")]
        public async Task<IActionResult> GetAdmin(string username)
        {
            AdminModel adminModel = new AdminModel();

            try
            {
                adminModel = await _adminLogic.GetAdmin(username);
                return Ok(adminModel);
            }
            catch(Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpGet("totaladmins")]
        public async Task<long> TotalNumberOfAdmins(int pageNumber = 1, int itemPerPage = 10)
        {
            return await _adminLogic.TotalNumberOfAdmins(pageNumber, itemPerPage);
        }

        [HttpPut("update/{username}")]
        public async Task<IActionResult> UpdateAdmin(string username, [FromBody] UpdateAdminModel admin)
        {
            try
            {
                await _adminLogic.UpdateAdmin(username, admin);

                return Ok(new { Message = "Admin updated successfully." });
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
        public async Task<IActionResult> DeleteAdmin(string username)
        {
            try
            {
                bool isDeleted = await _adminLogic.DeleteAdmin(username);

                if (isDeleted)
                {
                    return Ok(new { Message = "Admin deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Admin not found." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAllAdmin()
        {
            try
            {
                bool isDeleted = await _adminLogic.DeleteAllAdmin();

                if (isDeleted)
                {
                    return Ok(new { Message = "All admins deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "No admin found" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }

        [HttpPatch("updatepartial/{username}")]
        public async Task<IActionResult> PartiallyUpdateAdmin(string username, [FromBody] JsonPatchDocument<AdminModel> patchDocument)
        {
            if (string.IsNullOrEmpty(username) || patchDocument == null)
            {
                return BadRequest("Invalid username or payload");
            }

            try
            {
                bool isAdminFound = await _adminLogic.PartiallyUpdateAdmin(username, patchDocument);

                if (isAdminFound)
                {
                    return Ok(new { Message = "Admin updated successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Admin not found." });
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
