using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace MessageListener.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IDistributedCache _connectedClientsCache;
    private readonly ILogger<NotificationHub> _logger;
    private readonly MessageConsumer.MessageConsumer _messageConsumer;

    public NotificationHub(MessageConsumer.MessageConsumer messageConsumer, IDistributedCache cache,
        ILogger<NotificationHub> logger)
    {
        _messageConsumer = messageConsumer;
        _connectedClientsCache = cache;
        _logger = logger;
    }

    //Todo add a parameter userId guid from message in the future

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
        _logger.LogInformation("Client connected: " + Context.ConnectionId);
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await _connectedClientsCache.SetStringAsync(Context.ConnectionId, userId);
            await _connectedClientsCache.SetStringAsync(userId, Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("Client disconnected: " + Context.ConnectionId);
        await _connectedClientsCache.RemoveAsync(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendToAll(string message)
    {
        _logger.LogInformation("Sending message to all clients: " + message);
        await Clients.All.SendAsync("Retrieve", message);
    }

    public async Task SendMessageToStudent(string studentId, string message)
    {
        await Clients.User(studentId).SendAsync("Retrieve", message);
    }

    public async Task NotifyConnectedUsers()
    {
        var userId = Context.UserIdentifier;
        // if(!string.IsNullOrEmpty(userId))
        // {
        //     var messages = await _messageConsumer.GetPendingMessagesFromRabbitMqAsync(userId);
        //     foreach (var message in messages)
        //     {
        //         await Clients.User(userId).SendAsync("Retrieve", message);
        //     }
        // }   
    }


    public async Task<bool> IsUserConnected(string userId)
    {
        var connectionId = await _connectedClientsCache.GetStringAsync(userId);
        return !string.IsNullOrEmpty(connectionId);
    }
}