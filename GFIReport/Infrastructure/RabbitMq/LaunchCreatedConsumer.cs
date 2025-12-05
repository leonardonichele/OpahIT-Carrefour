using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using GFIReport.Application.Interfaces;
using GFIReport.Domain.Events;
using IModel = RabbitMQ.Client.IModel;

namespace GFIReport.Infrastructure.RabbitMq;

public class LaunchCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;

    private IConnection? _connection;
    private IModel? _channel;

    public LaunchCreatedConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _config = config;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _config["RabbitMQ:Host"] ?? "localhost",
            UserName = _config["RabbitMQ:Username"] ?? "admin",
            Password = _config["RabbitMQ:Password"] ?? "admin",
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        string exchange = _config["RabbitMQ:Exchange"] ?? "gficash.events";
        string queueName = "gfireport.launch-created";
        string dlqName = $"{queueName}.dlq";

        // ---------------------------
        // 1) Exchange Principal
        // ---------------------------
        _channel.ExchangeDeclare(
            exchange: exchange,
            type: ExchangeType.Fanout,
            durable: true
        );

        // ---------------------------
        // 2) DLQ
        // ---------------------------
        _channel.QueueDeclare(
            queue: dlqName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        // ---------------------------
        // 3) Queue Principal com DLQ configurada
        // ---------------------------
        var queueArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", "" },       // direct para fila
            { "x-dead-letter-routing-key", dlqName }
        };

        _channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs
        );

        // bind exchange -> queue
        _channel.QueueBind(queueName, exchange, "");

        // ---------------------------
        // 4) PREFETCH CONTROL (para 5% tolerância)
        //    Ex.: 10 mensagens por vez
        // ---------------------------
        var prefetch = int.Parse(_config["RabbitMQ:Prefetch"] ?? "10");
        _channel.BasicQos(0, (ushort)prefetch, global: false);

        // ---------------------------
        // 5) Consumer Assíncrono
        // ---------------------------
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (_, ea) =>
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ILaunchReportService>();

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = JsonSerializer.Deserialize<LaunchCreatedEvent>(json);
                var batch = scope.ServiceProvider.GetRequiredService<BatchProcessor>();

                if (evt != null)
                    await batch.EnqueueAsync(evt);


                // ACK se deu tudo certo
                _channel!.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // NACK -> vai para DLQ
                _channel!.BasicNack(ea.DeliveryTag, false, requeue: false);
                Console.WriteLine($"[DLQ] Mensagem rejeitada: {ex.Message}");
            }
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );

        return Task.CompletedTask;
    }
}
