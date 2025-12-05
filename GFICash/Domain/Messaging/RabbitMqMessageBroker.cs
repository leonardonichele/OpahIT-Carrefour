using System.Text;
using RabbitMQ.Client;
using GFICash.Domain.Messaging;

namespace GFICash.Infrastructure.Messaging;

public class RabbitMqMessageBroker : IMessageBroker
{
    private readonly string _hostname;
    private readonly string _exchange;
    private readonly string _username;
    private readonly string _password;

    public RabbitMqMessageBroker(IConfiguration config)
    {
        _hostname = config["RabbitMQ:Host"] ?? "localhost";
        _exchange = config["RabbitMQ:Exchange"] ?? "gficash.events";
        _username = config["RabbitMQ:User"] ?? "admin";
        _password = config["RabbitMQ:Password"] ?? "admin";
    }

    public Task PublishAsync(string eventType, string payload)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password,
            DispatchConsumersAsync = true
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: _exchange,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false
        );

        var body = Encoding.UTF8.GetBytes(payload);

        var props = channel.CreateBasicProperties();
        props.Type = eventType;
        props.ContentType = "application/json";
        props.DeliveryMode = 2; // persistent

        channel.BasicPublish(
            exchange: _exchange,
            routingKey: "",
            basicProperties: props,
            body: body
        );

        return Task.CompletedTask;
    }
}