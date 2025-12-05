using System.Text.Json;
using Domain.Commands.Requests;
using Domain.Commands.Response;
using Domain.Entities;
using GFICash.Infrastructure.UnitOfWork;
using MediatR;

public class CreateLaunchHandler :
    IRequestHandler<CreateLaunchRequest, CreateLaunchResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;

    public CreateLaunchHandler(IUnitOfWork uow, IMediator mediator)
    {
        _uow = uow;
        _mediator = mediator;
    }

    public async Task<CreateLaunchResponse> Handle(CreateLaunchRequest request, CancellationToken cancellationToken)
    {
        var launch = new Launch(request.Valor, request.Tipo, request.Dt);

        await _uow.LaunchRepository.AddAsync(launch);
        
        var payloadJson = JsonSerializer.Serialize(new
        {
            launch.Id,
            launch.Valor,
            Tipo = launch.Tipo.ToString(),
            launch.Dt
        });
        
        foreach (var domainEvent in launch.Events)
        {
            await _uow.OutboxRepository.SaveEventAsync("LaunchCreated", payloadJson);

        }

        await _uow.CommitAsync();
        
        foreach (var domainEvent in launch.Events)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        launch.ClearEvents();

        return new CreateLaunchResponse
        {
            Id = launch.Id,
            Valor = launch.Valor,
            Tipo = launch.Tipo.ToString(),
            Dt = launch.Dt,
            CreatedAt = launch.CreatedAt
        };
    }
}