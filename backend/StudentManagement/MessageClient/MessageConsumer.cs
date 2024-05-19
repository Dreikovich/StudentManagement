using System.Globalization;
using MessagePublisher.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace MessageClient;

public class MessageConsumer
{
    private readonly RabbitMqConnectionService _rabbitMqConnectionService;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly int _intervalSeconds = 20;
    
    public MessageConsumer(RabbitMqConnectionService rabbitMqConnectionService)
    {
        _rabbitMqConnectionService = rabbitMqConnectionService;
    }

    public void StartConsuming()
    {
        var queueName = _rabbitMqConnectionService.GetQueueName();
        var channel = _rabbitMqConnectionService.CreateModel();
        
        var consumer = new EventingBasicConsumer(channel);  
        
        Task.Run(() => ProcessMessages(channel, queueName, _cancellationTokenSource.Token));

        Task.Run((() => LogInitialQueueMessageCount(channel, queueName, _cancellationTokenSource.Token)));


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
                        Console.WriteLine($"Received message: {System.Text.Encoding.UTF8.GetString(body)}");
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
    
    private void LogInitialQueueMessageCount(IModel channel, string queueName, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var queueDeclareOk = channel.QueueDeclarePassive(queueName);
                Console.WriteLine(
                    $"Initial number of messages in the queue '{queueName}': {queueDeclareOk.MessageCount}");
                Task.Delay(_intervalSeconds * 1000, cancellationToken).Wait();
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
}