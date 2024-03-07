using Microsoft.AspNetCore.Mvc;
using NotificationApi.Business.Notification;
using NotificationApi.Contracts.Models;

namespace NotificationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("CreateNotification")]
        public async Task<IActionResult> CreateNotification([FromBody] UserNotification userNotification)
        {
            try
            {
                await _notificationService.CreateNotification(userNotification);
                return StatusCode(201, new { success = true, message = "Notification created successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { success = false, message = e.Message });
            }
        }

        [HttpGet("totalNumberOfNotification")]
        public async Task<IActionResult> GetTotalNumberOfNotification()
        {
            long totalNumberOfNotification = await _notificationService.GetNumberOfNotification();

            return Ok(totalNumberOfNotification);
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications(int pageNumber = 1, int pageSize = 10)
        {
            List<UserNotification> userNotifications = new List<UserNotification>();

            try
            {
                userNotifications = await _notificationService.GetNotifications(pageNumber, pageSize);
                return Ok(userNotifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
