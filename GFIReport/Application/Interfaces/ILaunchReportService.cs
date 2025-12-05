using GFIReport.Domain.Events;

namespace GFIReport.Application.Interfaces;

public interface ILaunchReportService
{
    Task ProcessAsync(LaunchCreatedEvent evt);
}