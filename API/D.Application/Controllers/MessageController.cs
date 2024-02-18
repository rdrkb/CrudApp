using A.Contracts.DTOs;
using A.Contracts.Models;
using C.Business.MessageLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D.Application.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageLogic _messageLogic;

        public MessageController(IMessageLogic messageLogic)
        {
            _messageLogic = messageLogic;
        }

        [HttpPost("send")]
        public async Task<IActionResult> CreateMessage(MessageDto message)
        {
            
            try
            {
                await _messageLogic.CreateMessage(message);
                return StatusCode(201, new { success = true, message = "message send successfully" });
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

        [HttpGet("totalNumberOfMessage/{sender}/{receiver}")]
        public async Task<long> GetNumberOfMessage(string sender, string receiver)
        {
            return await _messageLogic.GetNumberOfMessage(sender, receiver);
        }

        [HttpGet("getmessages/{sender}/{receiver}")]
        public async Task<IActionResult> GetMessages(string sender, string receiver, int pageNumber = 1, int itemPerPage = 10)
        {
            List<Message> messages = new List<Message>();

            try
            {
                messages = await _messageLogic.GetMessages(sender, receiver, pageNumber, itemPerPage);
                return Ok(messages);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

        [HttpGet("totalNumberOfList/{username}")]
        public async Task<long> GetNumberOfList(string username)
        {
            return await _messageLogic.GetNumberOfList(username);
        }

        [HttpGet("getlists/{username}")]
        public async Task<IActionResult> GetLists(string username, int pageNumber = 1, int itemPerPage = 10)
        {
            List<MessageListDto> lists = new List<MessageListDto>();

            try
            {
                lists = await _messageLogic.GetList(username, pageNumber, itemPerPage);
                return Ok(lists);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }
    }
}
