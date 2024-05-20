using System.Globalization;
using System.Text.Json;
using MessagePublisher.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageClient;

public class MessageConsumer
{
    private readonly RabbitMqConnectionService _rabbitMqConnectionService;
    private readonly HubConnection _hubConnection;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly int _intervalSeconds = 20;
    
    public MessageConsumer(RabbitMqConnectionService rabbitMqConnectionService, string hubUrl)
    {
        _rabbitMqConnectionService = rabbitMqConnectionService;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .WithAutomaticReconnect()
            .Build();
        StartHubConnection().Wait();
    }

    public void StartConsuming()
    {
        var queueName = _rabbitMqConnectionService.GetQueueName();
        var channel = _rabbitMqConnectionService.CreateModel();
        
        var consumer = new EventingBasicConsumer(channel);  
        
        Task.Run(() => ProcessMessages(channel, queueName, _cancellationTokenSource.Token));
        
        // string consumerTag = channel.BasicConsume(
        //     queue: queueName,
        //     autoAck: false,
        //     consumer: consumer
        // );
    
        Console.WriteLine("Consumer is running. Press Ctrl+C to exit.");
    }
    
    public void StopConsuming()
    {
        _cancellationTokenSource.Cancel();
    }
    
    private async Task StartHubConnection()
    {
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("SignalR connection established.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
        }
    }
    
    private async Task ProcessMessages(IModel channel, string queueName, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var queueDeclareOk = channel.QueueDeclarePassive(queueName);
                var messageCount = queueDeclareOk.MessageCount;
                Console.WriteLine(
                    $"Initial number of messages in the queue '{queueName}': {queueDeclareOk.MessageCount}");
                for (var i = 0; i < messageCount; i++)
                {
                    var result = channel.BasicGet(queueName, false);
                    if (result != null)
                    {
                        var body = result.Body.ToArray();
                        var messageJson = System.Text.Encoding.UTF8.GetString(body);
                        var message = JsonSerializer.Deserialize<UserMessage>(messageJson);
                        Console.WriteLine($"Received message for user {message.UserId}: {message.Content}");
                        await _hubConnection.SendAsync("SendToAll", message.Content);
                        channel.BasicAck(result.DeliveryTag, false);
                        
                    }
                }
                await Task.Delay(_intervalSeconds * 1000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while logging queue message count: {ex.Message}");
            }
            
        }
    }
    
    
    public class UserMessage
    {
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}