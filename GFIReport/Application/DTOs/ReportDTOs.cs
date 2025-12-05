namespace GFIReport.Application.DTOs;

public class ReportResponse
{
    public decimal TotalCredito { get; set; }
    public decimal TotalDebito { get; set; }
    public decimal Saldo { get; set; }
    public List<LaunchItem> Items { get; set; } = new();
}

public class LaunchItem
{
    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = "";
    public DateTime Dt { get; set; }
}