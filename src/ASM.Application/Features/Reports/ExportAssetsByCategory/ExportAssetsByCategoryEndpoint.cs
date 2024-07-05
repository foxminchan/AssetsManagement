using System.Net.Mime;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Features.Reports.AssetsByCategory;
using ASM.Application.Infrastructure.Excel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Reports.ExportAssetsByCategory;

public sealed record ExportAssetsByCategoryRequest(string OrderBy, bool IsDescending);

public sealed class ExportAssetsByCategoryEndpoint : IEndpointBase
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/reports/assets-by-category/export", async (
                ISender sender,
                IExcelWriter<AssetsByCategoryDto> excelWriter,
                string orderBy = nameof(AssetsByCategoryDto.Category),
                bool isDescending = true) => await HandleAsync(new(orderBy, isDescending), sender, excelWriter))
            .Produces<FileStreamHttpResult>(StatusCodes.Status206PartialContent, MediaTypeNames.Application.Octet)
            .WithTags(nameof(Reports))
            .WithName("Export Assets by Category")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<FileStreamHttpResult> HandleAsync(
        ExportAssetsByCategoryRequest request,
        ISender sender,
        IExcelWriter<AssetsByCategoryDto> excelWriter,
        CancellationToken cancellationToken = default)
    {
        AssetsByCategoryQuery query = new(request.OrderBy, request.IsDescending);

        var data = await sender.Send(query, cancellationToken);

        var stream = new MemoryStream();

        excelWriter.Write(data.Value.ToList(), nameof(AssetsByCategoryDto.Id), stream);

        stream.Seek(0, SeekOrigin.Begin);

        return TypedResults.Stream(stream, MediaTypeNames.Application.Octet, "Report.xlsx");
    }
}
