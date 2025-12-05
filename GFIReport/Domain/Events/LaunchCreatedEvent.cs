namespace GFIReport.Domain.Events;

public class LaunchCreatedEvent
{
    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = "";
    public DateTime Dt { get; set; }
}