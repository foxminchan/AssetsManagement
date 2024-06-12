using Ardalis.Specification;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Common.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot;
