using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using MessagePublisher.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageClient;

public class MessageConsumer
{
    private readonly RabbitMqConnectionService _rabbitMqConnectionService;
    private readonly string _hubUrl; 
    // private readonly HubConnection _hubConnection;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly int _intervalSeconds = 20;
    
    public MessageConsumer(RabbitMqConnectionService rabbitMqConnectionService, string hubUrl)
    {
        _rabbitMqConnectionService = rabbitMqConnectionService;
        _hubUrl = hubUrl;
        
    }

    public void StartConsuming()
    {
        var queueName = _rabbitMqConnectionService.GetQueueName();
        var channel = _rabbitMqConnectionService.CreateModel();
        
        Task.Run(() => ProcessMessages(channel, queueName, _cancellationTokenSource.Token));
    
        Console.WriteLine("Consumer is running. Press Ctrl+C to exit.");
    }
    
    public void StopConsuming()
    {
        _cancellationTokenSource.Cancel();
    }
    
    private async Task ProcessMessages(IModel channel, string queueName, CancellationToken cancellationToken)
    {
        int retryCount = 0;
        const int maxRetries = 5;
        const int delayBetweenRetries = 2000;
        
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
                        await SendMessageToHub(message.Content);
                        channel.BasicAck(result.DeliveryTag, false);
                        
                    }
                }
                await Task.Delay(_intervalSeconds * 1000, cancellationToken);
                retryCount = 0;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (RabbitMQ.Client.Exceptions.AlreadyClosedException ex)
            {
                Console.WriteLine($"RabbitMQ connection closed: {ex.Message}");
                if (++retryCount <= maxRetries)
                {
                    Console.WriteLine($"Retrying in {delayBetweenRetries / 1000} seconds...");
                    await Task.Delay(delayBetweenRetries, cancellationToken);
                }
                else
                {
                    Console.WriteLine("Max retry attempts reached. Exiting...");
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while logging queue message count: {ex.Message}");
            }
            
        }
    }
    
    private async Task SendMessageToHub(string message)
    {
        
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .Build();
        try
        {
            await hubConnection.StartAsync();
            Console.WriteLine("SignalR connection established.");
            await hubConnection.SendAsync("SendToAll", message);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
        }
        finally
        {
            await hubConnection.StopAsync();
            Console.WriteLine("SignalR connection stopped.");
        }
    }
    
    
    public class UserMessage
    {
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}