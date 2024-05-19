using MessagePublisher.Configuration;
using MessagePublisher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddTransient<MessageConsumer>();
        var serviceProvider = services.BuildServiceProvider();

        var rabbitMqConnectionService = serviceProvider.GetRequiredService<RabbitMqConnectionService>();
        string queueName = rabbitMqConnectionService.GetQueueName();

        if (rabbitMqConnectionService == null)
        {
            Console.WriteLine("RabbitMqConnectionService is null");
            return;
        }
        
        var messageConsumer = serviceProvider.GetRequiredService<MessageConsumer>();
        
        messageConsumer.StartConsuming();
        
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("Stopping consumer...");
            messageConsumer.StopConsuming();
            e.Cancel = true;
        };
        
        Thread.Sleep(Timeout.Infinite);
    }
}