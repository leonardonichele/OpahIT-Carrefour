using GFIReport.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFIReport.Controllers;

[ApiController]
[Route("v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportQueryService _service;

    public ReportsController(IReportQueryService service)
    {
        _service = service;
    }

    [HttpGet("daily")]
    public async Task<IActionResult> Daily([FromQuery] DateTime? date)
    {
        var d = date ?? DateTime.UtcNow.Date;
        return Ok(await _service.DailyAsync(d));
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> Monthly([FromQuery] int? year, [FromQuery] int? month)
    {
        var y = year ?? DateTime.UtcNow.Year;
        var m = month ?? DateTime.UtcNow.Month;

        return Ok(await _service.MonthlyAsync(y, m));
    }

    [HttpGet("range")]
    public async Task<IActionResult> Range(DateTime start, DateTime end)
    {
        return Ok(await _service.RangeAsync(start, end));
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        return Ok(new { total = await _service.SummaryAsync() });
    }
}