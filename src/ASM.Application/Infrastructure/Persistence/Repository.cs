using Ardalis.Specification.EntityFrameworkCore;
using ASM.Application.Common.Interfaces;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Infrastructure.Persistence;

public sealed class Repository<T>(ApplicationDbContext dbContext)
    : RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot;
