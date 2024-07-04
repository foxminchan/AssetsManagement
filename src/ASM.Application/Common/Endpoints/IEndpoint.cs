using MediatR;

namespace ASM.Application.Common.Endpoints;

public interface IEndpoint<TResponse, in TRequest> : IEndpointBase
{
    Task<TResponse> HandleAsync(TRequest request, ISender sender, CancellationToken cancellationToken = default);
}
