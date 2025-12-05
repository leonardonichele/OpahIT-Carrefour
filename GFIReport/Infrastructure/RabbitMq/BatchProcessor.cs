using System.Text.Json;
using System.Threading.Channels;
using GFIReport.Application.Interfaces;
using GFIReport.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GFIReport.Infrastructure.RabbitMq;

public class BatchProcessor : BackgroundService
{
    private readonly Channel<LaunchCreatedEvent> _channel = Channel.CreateUnbounded<LaunchCreatedEvent>();
    private readonly IServiceScopeFactory _scopeFactory;

    private const int BatchSize = 20;
    private const int Parallelism = 4;

    public BatchProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task EnqueueAsync(LaunchCreatedEvent evt)
    {
        await _channel.Writer.WriteAsync(evt);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[BatchProcessor] Iniciado...");

        var workers = new List<Task>();

        for (int i = 0; i < Parallelism; i++)
        {
            workers.Add(Task.Run(() => WorkerAsync(stoppingToken), stoppingToken));
        }

        await Task.WhenAll(workers);
    }

    private async Task WorkerAsync(CancellationToken token)
    {
        var buffer = new List<LaunchCreatedEvent>();

        while (!token.IsCancellationRequested)
        {
            var evt = await _channel.Reader.ReadAsync(token);
            buffer.Add(evt);

            if (buffer.Count >= BatchSize)
            {
                await ProcessBatchAsync(buffer);
                buffer.Clear();
            }
        }
    }

    private async Task ProcessBatchAsync(List<LaunchCreatedEvent> batch)
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ILaunchReportService>();

        foreach (var item in batch)
            await service.ProcessAsync(item);

        Console.WriteLine($"[BatchProcessor] Processado batch de {batch.Count} itens.");
    }
}
