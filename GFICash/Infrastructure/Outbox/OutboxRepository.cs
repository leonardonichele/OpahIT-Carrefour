using GFICash.Infrastructure.Context;
using GFICash.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

public class OutboxRepository : IOutboxRepository
{
    private readonly AppDbContext _context;

    public OutboxRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveEventAsync(string eventType, string payload)
    {
        var msg = new OutboxMessage
        {
            EventType = eventType,
            Payload = payload
        };

        await _context.Outbox.AddAsync(msg);
    }

    public Task<List<OutboxMessage>> GetUnprocessedAsync()
    {
        return _context.Outbox
            .Where(x => !x.Processed)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsProcessedAsync(Guid id)
    {
        var msg = await _context.Outbox.FindAsync(id);
        if (msg != null)
        {
            msg.Processed = true;
            msg.ProcessedAt = DateTime.UtcNow;
        }
    }
}