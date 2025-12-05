using MediatR;

public class LaunchCreatedEventHandler : INotificationHandler<LaunchCreatedEvent>
{
    public Task Handle(LaunchCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Tudo que for interno ao GFICash entra aqui

        Console.WriteLine($"EVENTO DE DOMÍNIO: Lançamento criado {notification.LaunchId}");

        return Task.CompletedTask;
    }
}