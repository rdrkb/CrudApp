using Business.Accounts.Commands;
using Contracts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                Token result = await _mediator.Send(new UserRegisterCommand(registerModel));
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
                Token result = await _mediator.Send(new UserLoginCommand(loginModel));

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
                bool isDeleted = await _mediator.Send(new UserDeleteCommand(username));

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
