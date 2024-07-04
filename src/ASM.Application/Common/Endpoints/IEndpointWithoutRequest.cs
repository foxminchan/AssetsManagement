using MediatR;

namespace ASM.Application.Common.Endpoints;

public interface IEndpointWithoutRequest<TResponse> : IEndpointBase
{
    Task<TResponse> HandleAsync(ISender sender, CancellationToken cancellationToken = default);
}
