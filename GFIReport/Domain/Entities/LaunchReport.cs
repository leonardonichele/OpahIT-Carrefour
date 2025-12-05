namespace GFIReport.Domain.Entities;

public class LaunchReport
{
    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = "";
    public DateTime Dt { get; set; }
}