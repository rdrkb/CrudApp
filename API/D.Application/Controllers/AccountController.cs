using A.Contracts.DTOs;
using C.Business.AccountLogic;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountLogic _accountLogic;

        public AccountController(IAccountLogic accountLogic)
        {
            _accountLogic = accountLogic;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                if (await _accountLogic.UserExists(registerModel.Username))
                {
                    return BadRequest("Username is taken");
                }

                Token result = await _accountLogic.Register(registerModel);
                return Ok(new { Token = result.token });
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred during registration: " + ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                Token result = await _accountLogic.Login(loginModel);

                if (!string.IsNullOrEmpty(result.token))
                {
                    
                    return Ok(new { Token = result.token });
                }
                else
                {
                    return BadRequest(new { ErrorMessage = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }
        }

        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteAccount(string username)
        {
            try
            {
                bool isDeleted = await _accountLogic.DeleteAccount(username);

                if (isDeleted)
                {
                    return Ok(new { Message = "Account deleted successfully." });
                }
                else
                {
                    return NotFound(new { ErrorMessage = "Account not found." });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { ErrorMessage = e.Message });
            }
        }
    }
}
