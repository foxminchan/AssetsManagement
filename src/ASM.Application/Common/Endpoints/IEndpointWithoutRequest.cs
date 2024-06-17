namespace ASM.Application.Common.Endpoints;

public interface IEndpointWithoutRequest<TResponse> : IEndpointBase
{
    Task<TResponse> HandleAsync(CancellationToken cancellationToken = default);
}
