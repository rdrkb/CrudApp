using Business.Accounts.Commands;
using Contracts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Shared.CQRS;

namespace SchoolManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICommandService _commandService;
        public AccountController(IMediator mediator, ICommandService commandService)
        {
            _mediator = mediator;
            _commandService = commandService;
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
                var userLoginCommand = new DynamicCommand
                {
                    Name = "UserLoginDynamicCommand",
                    Api = "SchoolManagementApi",
                    WaitForResponse = true,
                };

                userLoginCommand.SetValue("LoginModel", loginModel);

                var dynamicCommandResult = await _commandService.ExecuteAsync(userLoginCommand);

                //Token result = await _mediator.Send(new UserLoginCommand(loginModel));

                if (dynamicCommandResult.FieldValues.ContainsKey("Token"))
                {

                    return Ok(new Token
                    {
                        token = dynamicCommandResult.GetValue<string>("Token")!
                    });
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
