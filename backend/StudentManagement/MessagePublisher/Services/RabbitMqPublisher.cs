using System;
using System.Text;
using MessagePublisher.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MessagePublisher.Services;

public class RabbitMqPublisher 
{
    private readonly IModel _channel;
    
    public RabbitMqPublisher(RabbitMqConnectionService connectionService)
    {
        _channel = connectionService.CreateModel();
    }
    
    public void Publish(string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "dev_student_e", routingKey: "student_queue", basicProperties: null, body: body);
            Console.WriteLine("Message published successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to publish message: " + ex.Message);
        }
        
    }
}