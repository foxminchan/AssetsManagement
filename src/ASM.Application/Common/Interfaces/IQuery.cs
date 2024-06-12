using MediatR;

namespace ASM.Application.Common.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>;
