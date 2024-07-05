using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Infrastructure.Persistence;
using Dapper;

namespace ASM.Application.Features.Reports.AssetsByCategory;

public sealed record AssetsByCategoryQuery(string OrderBy, bool IsDescending)
    : IQuery<Result<IEnumerable<AssetsByCategoryDto>>>;

public sealed class AssetsByCategoryHandler(IDatabaseFactory factory)
    : IQueryHandler<AssetsByCategoryQuery, Result<IEnumerable<AssetsByCategoryDto>>>
{
    public async Task<Result<IEnumerable<AssetsByCategoryDto>>> Handle(AssetsByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        string orderBy = request.OrderBy switch
        {
            nameof(AssetsByCategoryDto.Category) => nameof(AssetsByCategoryDto.Category),
            nameof(AssetsByCategoryDto.Total) => nameof(AssetsByCategoryDto.Total),
            nameof(AssetsByCategoryDto.Assigned) => nameof(AssetsByCategoryDto.Assigned),
            nameof(AssetsByCategoryDto.Available) => nameof(AssetsByCategoryDto.Available),
            nameof(AssetsByCategoryDto.NotAvailable) => nameof(AssetsByCategoryDto.NotAvailable),
            nameof(AssetsByCategoryDto.WaitingForRecycling) => nameof(AssetsByCategoryDto.WaitingForRecycling),
            nameof(AssetsByCategoryDto.Recycled) => nameof(AssetsByCategoryDto.Recycled),
            _ => nameof(AssetsByCategoryDto.Category)
        };

        var builder = new SqlBuilder();

        var template = builder.AddTemplate(
            $"""
             SELECT c.Id,
                    c.Name                      AS {nameof(AssetsByCategoryDto.Category)},
                    COUNT(a.Id)                 AS {nameof(AssetsByCategoryDto.Total)},
                    SUM(IIF(a.State = 5, 1, 0)) AS {nameof(AssetsByCategoryDto.Assigned)},
                    SUM(IIF(a.State = 0, 1, 0)) AS {nameof(AssetsByCategoryDto.Available)},
                    SUM(IIF(a.State = 1, 1, 0)) AS {nameof(AssetsByCategoryDto.NotAvailable)},
                    SUM(IIF(a.State = 3, 1, 0)) AS {nameof(AssetsByCategoryDto.WaitingForRecycling)},
                    SUM(IIF(a.State = 4, 1, 0)) AS {nameof(AssetsByCategoryDto.Recycled)}
             FROM dbo.Categories c
                 LEFT JOIN dbo.Assets a ON c.Id = a.CategoryId
             GROUP BY c.Name, c.Id
             /**orderby**/
             """);

        builder.OrderBy($"{orderBy} {(request.IsDescending ? "DESC" : "ASC")}");

        using var conn = factory.GetOpenConnection();

        var result = await conn.QueryAsync<AssetsByCategoryDto>(template.RawSql, template.Parameters);

        return result.ToList();
    }
}
