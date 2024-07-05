using System.Data;
using Ardalis.GuardClauses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ASM.Application.Infrastructure.Persistence;

public sealed class DatabaseFactory : IDatabaseFactory
{
    private readonly string _connectionString;

    public DatabaseFactory(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");
        _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection() => new SqlConnection(_connectionString);
}
