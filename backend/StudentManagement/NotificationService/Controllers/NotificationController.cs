    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using NotificationService.Hubs;
    using NotificationService.Models;

    namespace NotificationService.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController: ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        
        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest notificationRequest)
        {
            try
            {
                await _hubContext.Clients.User(notificationRequest.UserId).SendAsync(notificationRequest.UserId, notificationRequest.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        
        [HttpGet("testsend")]
        public async Task<IActionResult> TestSendNotification(string userId, string message)
        {
            try
            {
                await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        
    }