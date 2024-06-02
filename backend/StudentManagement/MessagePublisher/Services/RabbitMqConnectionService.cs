using MessagePublisher.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MessagePublisher.Services;

public class RabbitMqConnectionService
{
    private readonly RabbitMqConfiguration _config;
    private readonly IConnection _connection;
    private readonly ConnectionFactory _factory;

    public RabbitMqConnectionService(IOptions<RabbitMqConfiguration> options)
    {
        _config = options.Value;
        _factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password,
            RequestedHeartbeat = TimeSpan.FromSeconds(60),
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
        _connection = _factory.CreateConnection();
    }

    public IModel CreateModel()
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(_config.QueueName, false, false, false, null);
        channel.ExchangeDeclare(_config.ExchangeName, "fanout", true, false);
        channel.QueueBind(_config.QueueName, _config.ExchangeName, "A");
        channel.BasicQos(0, 1, false);
        return channel;
    }

    public string GetQueueName()
    {
        return _config.QueueName;
    }
}