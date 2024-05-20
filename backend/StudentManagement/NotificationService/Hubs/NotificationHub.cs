using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Hubs;

public class NotificationHub : Hub
{
    public Task SendToAll(string message)
    {
        return Clients.All.SendAsync("Retrieve", message);
    }
}