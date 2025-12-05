using MediatR;

namespace Domain.Entities;

public abstract class AggregateRoot
{
    private readonly List<INotification> _events = new();

    public IReadOnlyCollection<INotification> Events => _events.AsReadOnly();

    protected void AddEvent(INotification @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();
}