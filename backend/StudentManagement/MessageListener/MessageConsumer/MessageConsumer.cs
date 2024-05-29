using System.Text;
using System.Text.Json;
using MessageListener.Hubs;
using MessageListener.Models;
using MessagePublisher.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;

namespace MessageListener.MessageConsumer;

public class MessageConsumer
    {
        private readonly RabbitMqConnectionService _rabbitMqConnectionService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IDistributedCache _cache;

        public MessageConsumer(
            RabbitMqConnectionService rabbitMqConnectionService,
            IHubContext<NotificationHub> hubContext,
            ILogger<MessageConsumer> logger,
            IDistributedCache cache)
        {
            _rabbitMqConnectionService = rabbitMqConnectionService;
            _hubContext = hubContext;
            _logger = logger;
            _cache = cache;
        }
        
        public async Task StartConsumingAsync() {
            var messages = await GetPendingMessagesFromRabbitMqAsync();
            foreach (var message in messages)
            {
                try
                {
                    string connectionId = await _cache.GetStringAsync(message.UserId);
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        // await _hubContext.Clients.User(userId:message.UserId).SendAsync("Retrieve", message);
                        await _hubContext.Clients.Client(connectionId).SendAsync("Retrieve", message);
                    }
                    else
                    {
                        await SendBackToQueueAsync(message);
                    }
                    
                    
                }
                catch (ObjectDisposedException ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the message: ObjectDisposedException");
                }
            }
        }

        public async Task<List<Message>> GetPendingMessagesFromRabbitMqAsync()
        {
            var messages = new List<Message>();
            var queueName = _rabbitMqConnectionService.GetQueueName(); 
            var channel = _rabbitMqConnectionService.CreateModel();

            try
            {
                var queueDeclareOk = channel.QueueDeclarePassive(queueName);
                var messageCount = queueDeclareOk.MessageCount;

                for (var i = 0; i < messageCount; i++)
                {
                    var result = channel.BasicGet(queueName, false);
                    if (result != null)
                    {
                        var body = result.Body.ToArray();
                        var messageJson = Encoding.UTF8.GetString(body);
                        var message = JsonSerializer.Deserialize<Message>(messageJson);

                        if (message != null )
                        {
                            messages.Add(message);
                            channel.BasicAck(result.DeliveryTag, false);
                        }
                        else
                        {
                            channel.BasicNack(result.DeliveryTag, false, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
            }
            finally
            {
                channel.Close();
                channel.Dispose();
            }

            return await Task.FromResult(messages);
        }
        
        private async Task SendBackToQueueAsync(Message message)
        {
            var channel = _rabbitMqConnectionService.CreateModel();
            await Task.Run(() =>
            {
                try
                {
                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true; 

                    //todo change this hard coded values
                    channel.BasicPublish(exchange: "dev_student_e", routingKey: "student_queue", basicProperties: properties, body: body);

                    _logger.LogInformation($"Message requeued for UserId: {message.UserId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to requeue message for UserId: {message.UserId}. Error: {ex.Message}");
                }
                finally
                {
                    channel.Close();
                    channel.Dispose();
                }
            });
            
        }
    }
    
    