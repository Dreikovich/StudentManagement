namespace MessageListener.Services;

public class MessageListeningService : BackgroundService
{
    private readonly MessageConsumer.MessageConsumer _messageConsumer;
    
    public MessageListeningService(MessageConsumer.MessageConsumer messageConsumer)
    {
        _messageConsumer = messageConsumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _messageConsumer.StartConsumingAsync();
            await Task.Delay(30000, stoppingToken);
        }
    }
}