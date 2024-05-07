using System.Text;
using MessagePublisher.Services;
using MessagePublisher.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagePublisher;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly RabbitMqPublisher _publisher;

    public Worker(RabbitMqConnectionService connectionService, RabbitMqPublisher publisher, ILogger<Worker> logger)
    {
        _logger = logger;
        _channel = connectionService.CreateModel();
        _publisher = publisher;
        _queueName = connectionService.GetQueueName();

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var message = "Hello";
        Console.WriteLine($"Received: {message}");
        try
        {
            _logger.LogInformation("Processing message...");
            _publisher.Publish($"Message '{message}' processed successfully.");
            _logger.LogInformation("Message processed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message.");
        }
        
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            _logger.LogInformation($"Received message: {content}");
            
            Task.Delay(TimeSpan.FromMinutes(2)).ContinueWith(t =>
            {
                _logger.LogInformation($"Processed message: {content}");
                _channel.BasicAck(ea.DeliveryTag, false);
            });
        };

        _channel.BasicConsume(_queueName, false, consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
    
    public override void Dispose()
    {
        _channel?.Close();
        base.Dispose();
    }
}