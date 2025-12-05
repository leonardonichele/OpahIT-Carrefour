using Domain.Commands.Requests;

namespace Domain.Entities;
public class Launch : AggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public decimal Valor { get; private set; }
    public Tipos Tipo { get; private set; } 
    public DateTime Dt { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Launch(decimal valor, Tipos tipo, DateTime dt)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor inválido");

        Valor = valor;
        Tipo = tipo;
        Dt = dt;
        
        AddEvent(new LaunchCreatedEvent(Id, Valor, Tipo, Dt));
    }
}