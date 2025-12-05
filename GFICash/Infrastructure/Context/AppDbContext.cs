using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using GFICash.Infrastructure.Outbox;

namespace GFICash.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Launch> Launches { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }
}