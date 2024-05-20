namespace NotificationService.Models;

public class NotificationRequest
{
    public string UserId { get; set; }
    public string Message { get; set; }
    public bool SendInApp { get; set; }
}