namespace MessagePublisher.Services;

public interface IMessagePublisher
{
    void Publish(string message);
}