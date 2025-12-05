using GFIReport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFIReport.Infrastructure.Persistence;

public class ReportDbContext : DbContext
{
    public ReportDbContext(DbContextOptions<ReportDbContext> options)
        : base(options) { }

    public DbSet<LaunchReport> LaunchReports => Set<LaunchReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LaunchReport>(e =>
        {
            e.ToTable("LaunchReports");
            e.HasKey(x => x.Id);
        });
    }
}