using GFIReport.Domain.Entities;

namespace GFIReport.Infrastructure.Persistence;

public interface ILaunchReportRepository
{
    Task AddAsync(LaunchReport report);
    Task<List<LaunchReport>> GetAllAsync();
    Task<List<LaunchReport>> GetDailyAsync(DateTime date);
    Task<List<LaunchReport>> GetMonthlyAsync(int year, int month);
    Task<List<LaunchReport>> GetRangeAsync(DateTime start, DateTime end);
    Task<decimal> GetTotalAsync();
    Task SaveChangesAsync();
}