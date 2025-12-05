using Domain.Entities;

namespace GFICash.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILaunchRepository
{
    Task AddAsync(Launch launch);
    Task<Launch?> GetByIdAsync(Guid id);
    Task<List<Launch>> GetAllAsync();
}