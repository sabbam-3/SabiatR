namespace SabiatR;

public interface IPipelineBehavior<TRequest>
    where TRequest : IRequest
{
    Task Handle(TRequest request, Func<Task> next, CancellationToken cancellationToken = default);
}

public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default);
}