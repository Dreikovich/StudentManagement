using System.Text;
using System.Text.Json;
using MessagePublisher.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NotificationService.Models;

namespace MessageClient;

public class MessageConsumer
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly IDistributedCache _connectedClientsCache;

    // private readonly HubConnection _hubConnection;
    private readonly IHubContext<Hub> _hubContext;
    private readonly string _hubUrl;
    private readonly int _intervalSeconds = 20;
    private readonly ILogger _logger;
    private readonly RabbitMqConnectionService _rabbitMqConnectionService;

    public MessageConsumer(RabbitMqConnectionService rabbitMqConnectionService, string hubUrl,
        IHubContext<Hub> hubContext, ILogger logger, IDistributedCache cache)
    {
        _rabbitMqConnectionService = rabbitMqConnectionService;
        _hubUrl = hubUrl;
        _hubContext = hubContext;
        _logger = logger;
        _connectedClientsCache = cache;
    }

    // public async Task StartConsuming()
    // {
    //     var queueName = _rabbitMqConnectionService.GetQueueName();
    //     var channel = _rabbitMqConnectionService.CreateModel();
    //     
    //     await Task.Run(() => ProcessMessages(channel, queueName, _cancellationTokenSource.Token));
    //
    //     Console.WriteLine("Consumer is running. Press Ctrl+C to exit.");
    // }

    // public async Task StartConsumingAsync(){
    //     var messages = await GetPendingMessagesFromRabbitMqAsync();
    //     foreach (var message in messages)
    //     {
    //         try
    //         {
    //             string connectionId = await _connectedClientsCache.GetStringAsync(message.UserId);
    //             if (!string.IsNullOrEmpty(connectionId))
    //             {
    //                 await _hubContext.Clients.Client(connectionId).SendAsync("Retrieve", message.Content);
    //             }
    //                 
    //             
    //         }
    //         catch (ObjectDisposedException ex)
    //         {
    //             _logger.LogError(ex, "An error occurred while processing the message: ObjectDisposedException");
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "An error occurred while processing the message.");
    //         }
    //     }
    // }

    public void StopConsuming()
    {
        _cancellationTokenSource.Cancel();
    }


    public async Task<List<Message>> GetPendingMessagesFromRabbitMqAsync(string userId)
    {
        var messages = new List<Message>();

        var queueName = _rabbitMqConnectionService.GetQueueName();
        var channel = _rabbitMqConnectionService.CreateModel();

        var queueDeclareOk = channel.QueueDeclarePassive(queueName);
        var messageCount = queueDeclareOk.MessageCount;
        for (var i = 0; i < messageCount; i++)
        {
            var result = channel.BasicGet(queueName, false);
            if (result != null)
            {
                var body = result.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<Message>(messageJson);

                try
                {
                    if (userId == message.UserId)
                    {
                        messages.Add(message);
                        channel.BasicAck(result.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(result.DeliveryTag, false, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    if (result.Redelivered)
                    {
                        Console.WriteLine("Message has already been redelivered. Acknowledging...");
                        channel.BasicAck(result.DeliveryTag, false);
                    }
                    else
                    {
                        Console.WriteLine("Message has not been redelivered. Nacking...");
                        channel.BasicNack(result.DeliveryTag, false, true);
                    }
                }
            }
        }

        return await Task.FromResult(messages);
    }


    // private async Task ProcessMessages(IModel channel, string queueName, CancellationToken cancellationToken)
    // {
    //     int retryCount = 0;
    //     const int maxRetries = 5;
    //     const int delayBetweenRetries = 2000;
    //     
    //     while (!cancellationToken.IsCancellationRequested)
    //     {
    //         try
    //         {
    //             var queueDeclareOk = channel.QueueDeclarePassive(queueName);
    //             var messageCount = queueDeclareOk.MessageCount;
    //             Console.WriteLine(
    //                 $"Initial number of messages in the queue '{queueName}': {queueDeclareOk.MessageCount}");
    //             for (var i = 0; i < messageCount; i++)
    //             {
    //                 var result = channel.BasicGet(queueName, false);
    //                 if (result != null)
    //                 {
    //                     var body = result.Body.ToArray();
    //                     var messageJson = System.Text.Encoding.UTF8.GetString(body);
    //                     var message = JsonSerializer.Deserialize<Message>(messageJson);
    //                     Console.WriteLine($"Received message for user {message.UserId}: {message.Content}");
    //                     // await SendMessageToHub(message.UserId, message.Content);
    //                     channel.BasicAck(result.DeliveryTag, false);
    //                     
    //                 }
    //             }
    //             await Task.Delay(_intervalSeconds * 1000, cancellationToken);
    //             retryCount = 0;
    //         }
    //         catch (OperationCanceledException)
    //         {
    //             break;
    //         }
    //         catch (RabbitMQ.Client.Exceptions.AlreadyClosedException ex)
    //         {
    //             Console.WriteLine($"RabbitMQ connection closed: {ex.Message}");
    //             if (++retryCount <= maxRetries)
    //             {
    //                 Console.WriteLine($"Retrying in {delayBetweenRetries / 1000} seconds...");
    //                 await Task.Delay(delayBetweenRetries, cancellationToken);
    //             }
    //             else
    //             {
    //                 Console.WriteLine("Max retry attempts reached. Exiting...");
    //                 break;
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error while logging queue message count: {ex.Message}");
    //         }
    //         
    //     }
    // }
    //
    // private async Task SendMessageToHub(string userId, string message)
    // {
    //     //
    //     // var hubConnection = new HubConnectionBuilder()
    //     //     .WithUrl(_hubUrl)
    //     //     .WithAutomaticReconnect()
    //     //     .ConfigureLogging(logging =>
    //     //     {
    //     //         logging.AddConsole();
    //     //     })
    //     //     .AddJsonProtocol(options =>
    //     //     {
    //     //         options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //     //     })
    //     //     .Build();
    //     try
    //     {
    //         // await hubConnection.StartAsync();
    //         Console.WriteLine("SignalR connection established.");
    //         // await hubConnection.SendAsync("SendMessageToStudent", userId, message);
    //         await _hubContext.Clients.User(userId).SendAsync("Retrieve", message);
    //
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
    //     }
    //     finally
    //     {
    //         // await hubConnection.StopAsync();
    //         Console.WriteLine("SignalR connection stopped.");
    //     }
    // }

    // private async Task SendMessageToHub(string userId, string message)
    // {
    //     try
    //     {
    //         await _hubContext.Clients.User(userId).SendAsync("Retrieve", message);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error sending message to hub: {ex.Message}");
    //     }
    // }
}