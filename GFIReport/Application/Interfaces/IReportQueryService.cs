using GFIReport.Application.DTOs;

namespace GFIReport.Application.Interfaces;

public interface IReportQueryService
{
    Task<ReportResponse> DailyAsync(DateTime day);
    Task<ReportResponse> MonthlyAsync(int year, int month);
    Task<ReportResponse> RangeAsync(DateTime start, DateTime end);
    Task<decimal> SummaryAsync();
}