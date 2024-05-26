using System.Collections.Concurrent;
using MessageClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Models;

namespace NotificationService.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    
    private static readonly ConcurrentDictionary<string, string> _connectedClients = new ConcurrentDictionary<string, string>();
    private readonly MessageConsumer _messageClient;
    
    public NotificationHub(MessageConsumer messageClient)
    {
        _messageClient = messageClient;
    }
    
    public override async Task OnConnectedAsync()
    {
        // var logger = Context.GetHttpContext().RequestServices.GetRequiredService<ILogger<NotificationHub>>();
        // var claims = Context.User?.Claims;
        // if (claims != null)
        // {
        //     foreach (var claim in claims)
        //     {
        //         logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        //     }
        // }
        string userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            _connectedClients.TryAdd(Context.ConnectionId, userId);
        }
        await base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connectedClients.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }
    
    public Task SendToAll(string message)
    {
        return Clients.All.SendAsync("Retrieve", message);
    }

    public async Task SendMessageToStudent(string studentId, string message)
    {
        await Clients.User(studentId).SendAsync("Retrieve", message);
    }
    
    public async Task NotifyConnectedUsers()
    {
        string userId = Context.UserIdentifier;
        if(!string.IsNullOrEmpty(userId))
        {
            var messages = _messageClient.GetPendingMessagesFromRabbitMq(userId);
            foreach (var message in messages)
            {
                await Clients.User(userId).SendAsync("Retrieve", message);
            }
        }   
    }
    
    public bool IsClientConnected(string userId)
    {
        return _connectedClients.Values.Contains(userId);
    }
    
   
    
    
}