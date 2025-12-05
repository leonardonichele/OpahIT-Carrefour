using GFICash.Infrastructure.UnitOfWork;

namespace GFICash.Infrastructure.Outbox;

public class OutboxJob
{
    private readonly IUnitOfWork _uow;

    public OutboxJob(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task ProcessAsync()
    {
        var pending = await _uow.OutboxRepository.GetUnprocessedAsync();

        if (!pending.Any())
            return;

        foreach (var msg in pending)
        {
            try
            {
                // AQUI futuramente entra entrega para RabbitMQ ou Azure Service Bus
                Console.WriteLine($"[OUTBOX] Enviando evento {msg.EventType} | Payload: {msg.Payload}");

                await _uow.OutboxRepository.MarkAsProcessedAsync(msg.Id);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OUTBOX] Erro ao enviar evento: {ex.Message}");
            }
        }
    }
}