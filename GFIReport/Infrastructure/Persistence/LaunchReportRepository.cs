using GFIReport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFIReport.Infrastructure.Persistence;

public class LaunchReportRepository : ILaunchReportRepository
{
    private readonly ReportDbContext _context;

    public LaunchReportRepository(ReportDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LaunchReport report)
    {
        await _context.LaunchReports.AddAsync(report);
    }

    public Task<List<LaunchReport>> GetAllAsync()
    {
        return _context.LaunchReports.OrderBy(x => x.Dt).ToListAsync();
    }

    public Task<List<LaunchReport>> GetDailyAsync(DateTime date)
    {
        var day = date.Date;

        return _context.LaunchReports
            .Where(x => x.Dt.Date == day)
            .OrderBy(x => x.Dt)
            .ToListAsync();
    }

    public Task<List<LaunchReport>> GetMonthlyAsync(int year, int month)
    {
        return _context.LaunchReports
            .Where(x => x.Dt.Year == year && x.Dt.Month == month)
            .OrderBy(x => x.Dt)
            .ToListAsync();
    }

    public Task<List<LaunchReport>> GetRangeAsync(DateTime start, DateTime end)
    {
        return _context.LaunchReports
            .Where(x => x.Dt >= start && x.Dt <= end)
            .OrderBy(x => x.Dt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalAsync()
    {
        var creditos = await _context.LaunchReports
            .Where(x => x.Tipo == "Credito")
            .SumAsync(x => x.Valor);

        var debitos = await _context.LaunchReports
            .Where(x => x.Tipo == "Debito")
            .SumAsync(x => x.Valor);

        return creditos + debitos;
    }


    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}