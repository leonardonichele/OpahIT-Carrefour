using GFIReport.Application.DTOs;
using GFIReport.Application.Interfaces;
using GFIReport.Infrastructure.Persistence;

namespace GFIReport.Application.Services;

public class ReportQueryService : IReportQueryService
{
    private readonly ILaunchReportRepository _repo;

    public ReportQueryService(ILaunchReportRepository repo)
    {
        _repo = repo;
    }

    private ReportResponse Build(List<Domain.Entities.LaunchReport> list)
    {
        var credit = list.Where(x => x.Tipo == "Credito").Sum(x => x.Valor);
        var debit = list.Where(x => x.Tipo == "Debito").Sum(x => x.Valor);

        return new ReportResponse
        {
            TotalCredito = credit,
            TotalDebito = debit,
            Saldo = credit + debit,
            Items = list.Select(x => new LaunchItem
            {
                Id = x.Id,
                Valor = x.Valor,
                Tipo = x.Tipo,
                Dt = x.Dt
            }).ToList()
        };
    }

    public async Task<ReportResponse> DailyAsync(DateTime day)
    {
        var list = await _repo.GetDailyAsync(day);
        return Build(list);
    }

    public async Task<ReportResponse> MonthlyAsync(int year, int month)
    {
        var list = await _repo.GetMonthlyAsync(year, month);
        return Build(list);
    }

    public async Task<ReportResponse> RangeAsync(DateTime start, DateTime end)
    {
        var list = await _repo.GetRangeAsync(start, end);
        return Build(list);
    }

    public Task<decimal> SummaryAsync()
    {
        return _repo.GetTotalAsync();
    }
}