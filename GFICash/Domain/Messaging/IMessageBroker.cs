namespace GFICash.Domain.Messaging;

public interface IMessageBroker
{
    Task PublishAsync(string eventType, string payload);
}