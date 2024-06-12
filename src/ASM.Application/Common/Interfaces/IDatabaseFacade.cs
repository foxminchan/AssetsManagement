using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ASM.Application.Common.Interfaces;

public interface IDatabaseFacade
{
    public DatabaseFacade Database { get; }
}