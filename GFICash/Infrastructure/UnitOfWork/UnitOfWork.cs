using GFICash.Infrastructure.Context;
using GFICash.Infrastructure.Outbox;
using GFICash.Infrastructure.Repositories;

namespace GFICash.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private ILaunchRepository _launchRepository;
    private IOutboxRepository _outboxRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }       

    public ILaunchRepository LaunchRepository =>
        _launchRepository ??= new LaunchRepository(_context);

    public IOutboxRepository OutboxRepository =>
        _outboxRepository ??= new OutboxRepository(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}