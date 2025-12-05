using GFICash.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace GFICash.Infrastructure.Repositories;

public class LaunchRepository : ILaunchRepository
{
    private readonly AppDbContext _context;

    public LaunchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Launch launch)
    {
        await _context.Launches.AddAsync(launch);
    }

    public async Task<Launch?> GetByIdAsync(Guid id)
    {
        return await _context.Launches.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Launch>> GetAllAsync()
    {
        return await _context.Launches.OrderByDescending(x => x.CreatedAt).ToListAsync();
    }
}