using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFICash.Infrastructure.Outbox;

public interface IOutboxRepository
{
    Task SaveEventAsync(string eventType, string payload);
    Task<List<OutboxMessage>> GetUnprocessedAsync();
    Task MarkAsProcessedAsync(Guid id);
}