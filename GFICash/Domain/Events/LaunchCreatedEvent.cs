using Domain.Commands.Requests;
using MediatR;

public record LaunchCreatedEvent(Guid LaunchId, decimal Valor, Tipos Tipo, DateTime Data)
    : INotification;