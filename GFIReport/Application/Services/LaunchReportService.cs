using GFIReport.Application.Interfaces;
using GFIReport.Domain.Entities;
using GFIReport.Domain.Events;
using GFIReport.Infrastructure.Persistence;

namespace GFIReport.Application.Services;

public class LaunchReportService : ILaunchReportService
{
    private readonly ILaunchReportRepository _repo;

    public LaunchReportService(ILaunchReportRepository repo)
    {
        _repo = repo;
    }

    public async Task ProcessAsync(LaunchCreatedEvent evt)
    {
        var report = new LaunchReport
        {
            Id = evt.Id,
            Valor = evt.Valor,
            Tipo = evt.Tipo,
            Dt = evt.Dt
        };

        await _repo.AddAsync(report);
        await _repo.SaveChangesAsync();

        Console.WriteLine($"[GFIReport] Saved launch report {evt.Id}");
    }
}