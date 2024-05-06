namespace Messaging.Services;

public interface IMessagePublisher
{
    void Publish(string message);
}