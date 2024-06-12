using MediatR;

namespace ASM.Application.Common.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>, ITxRequest;
