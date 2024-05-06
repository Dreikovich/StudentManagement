using Messaging.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Messaging.Services;

public class RabbitMqConnectionService
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly RabbitMqConfiguration _config;
    
    public RabbitMqConnectionService(IOptions<RabbitMqConfiguration> options)
    {
        _config = options.Value;
        _factory = new ConnectionFactory()
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password
        };
        _connection = _factory.CreateConnection();
    }
    
    public IModel CreateModel()
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(_config.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.ExchangeDeclare(_config.ExchangeName, "fanout", durable: true, autoDelete: false);
        channel.QueueBind(_config.QueueName, _config.ExchangeName, "A");
        channel.BasicQos(0, 1, false);
        return channel;
    }
    
    public string GetQueueName()
    {
        return _config.QueueName;
    }
}