using System.Data;

namespace ASM.Application.Infrastructure.Persistence;

public interface IDatabaseFactory
{
    IDbConnection GetOpenConnection();
}
