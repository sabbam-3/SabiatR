namespace SabiatR;

public interface IPipelineBehavior<in TRequest>
    where TRequest : notnull
{
    Task Handle(TRequest request, Func<Task> next, CancellationToken cancellationToken = default);
}

public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : notnull
{
    Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default);
}