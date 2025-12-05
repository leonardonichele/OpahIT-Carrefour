using Domain.Commands.Response;
using MediatR;

namespace Domain.Commands.Requests
{
    public class CreateLaunchRequest : IRequest<CreateLaunchResponse>
    {
        public decimal Valor { get;  set; }
        public Tipos Tipo { get;  set; }
        public DateTime Dt { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow.Date;
    }

    public enum Tipos
    {
        Credito = 0,
        Debito = 1
    }
}