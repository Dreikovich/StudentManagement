using MessagePublisher.Configuration;
using MessagePublisher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Threading;

namespace MessageClient;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        // Set up Dependency Injection
        var services = new ServiceCollection();
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));
        services.AddSingleton<RabbitMqConnectionService>();
        var serviceProvider = services.BuildServiceProvider();

        var rabbitMqConnectionService = serviceProvider.GetRequiredService<RabbitMqConnectionService>();

        if (rabbitMqConnectionService == null)
        {
            Console.WriteLine("RabbitMqConnectionService is null");
            return;
        }

        var channel = rabbitMqConnectionService.CreateModel();
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (ch, ea) =>
        {
            var body = ea.Body.ToArray(); // Convert body to array
            channel.BasicAck(ea.DeliveryTag, false); // Acknowledge receipt of the message
            Console.WriteLine($"Received message: {System.Text.Encoding.UTF8.GetString(body)}");
            await Task.Yield(); // Yield to keep UI responsive or manage asynchronous flows
        };

        string consumerTag = channel.BasicConsume(
            queue: rabbitMqConnectionService.GetQueueName(),
            autoAck: false,
            consumer: consumer
        );

        Console.WriteLine("Consumer is running. Press Ctrl+C to exit.");

        // Keep the application running until manually stopped
        while (true)
        {
            Thread.Sleep(Timeout.Infinite);
        }
    }
}