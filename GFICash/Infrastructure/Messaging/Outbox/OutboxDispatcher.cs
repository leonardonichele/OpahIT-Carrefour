using GFICash.Infrastructure.UnitOfWork;
using GFICash.Domain.Messaging;
using Microsoft.Extensions.Hosting;

namespace GFICash.Infrastructure.Outbox;

public class OutboxDispatcher : BackgroundService
{
    private readonly IServiceProvider _provider;

    public OutboxDispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var broker = scope.ServiceProvider.GetRequiredService<IMessageBroker>();

                var pending = await uow.OutboxRepository.GetUnprocessedAsync();

                foreach (var msg in pending)
                {
                    try
                    {
                        await broker.PublishAsync(msg.EventType, msg.Payload);

                        await uow.OutboxRepository.MarkAsProcessedAsync(msg.Id);
                        await uow.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[OUTBOX] Error sending: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OUTBOX] Dispatcher error: {ex.Message}");
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}