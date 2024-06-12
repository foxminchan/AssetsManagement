using Ardalis.Specification;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Common.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot;
