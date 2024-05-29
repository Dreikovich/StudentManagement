using MessagePublisher.Configuration;
using MessagePublisher.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MessageClient;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));
        services.AddSingleton<RabbitMqConnectionService>();
        services.AddSingleton<MessageConsumer>(provided =>
        {
            var rabbitMqConnectionService = provided.GetRequiredService<RabbitMqConnectionService>();
            var hubUrl = configuration.GetValue<string>("SignalRUrl");
            var hubContext = provided.GetRequiredService<IHubContext<Hub>>();
            var logger = provided.GetRequiredService<ILogger<MessageConsumer>>();
            var cache = provided.GetRequiredService<IDistributedCache>();
            
            return new MessageConsumer(rabbitMqConnectionService, hubUrl, hubContext, logger, cache);
        });
        var serviceProvider = services.BuildServiceProvider();

        var rabbitMqConnectionService = serviceProvider.GetService<RabbitMqConnectionService>();

        if (rabbitMqConnectionService == null)
        {
            Console.WriteLine("RabbitMqConnectionService is null");
            return;
        }
        
        var messageConsumer = serviceProvider.GetRequiredService<MessageConsumer>();
        
        // messageConsumer.StartConsuming();
        
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("Stopping consumer...");
            messageConsumer.StopConsuming();
            e.Cancel = true;
        };
        
        Thread.Sleep(Timeout.Infinite);
    }
}