using GFICash.Infrastructure.Repositories;
using GFICash.Infrastructure.Outbox;

namespace GFICash.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ILaunchRepository LaunchRepository { get; }
    IOutboxRepository OutboxRepository { get; }
    Task<int> CommitAsync();
}