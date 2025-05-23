﻿using Microsoft.Extensions.DependencyInjection;
using SabiatR.Exceptions;

namespace SabiatR;

internal sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    public async Task Send(IRequest request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

        // Resolve the handler from the service provider
        var handler = serviceProvider.GetRequiredService(handlerType);

        // Get the 'Handle' method of the handler
        var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest>.Handle));

        if (handleMethod == null)
        {
            throw new HandlerNotFoundException(requestType, nameof(IRequestHandler<IRequest>.Handle));
        }

        // Resolve the pipeline behaviors (if any)
        var behaviors = serviceProvider.GetServices(typeof(IPipelineBehavior<>).MakeGenericType(requestType))
            .Cast<object>()
            .Reverse()
            .ToList();

        // Create a Func<Task> to invoke the handler
        Func<Task> handle = () =>
        {
            var result = handleMethod.Invoke(handler, [request, cancellationToken]);
            return result as Task ?? Task.FromException(new InvalidHandlerResultException(handler.GetType(), handleMethod.Name, result));
        };

        // Loop through behaviors and chain them
        foreach (var behavior in behaviors)
        {
            // Capture the current 'next' function
            var next = handle;

            var handleBehaviourMethod = behavior.GetType().GetMethod(nameof(IPipelineBehavior<IRequest>.Handle));

            if (handleBehaviourMethod == null)
            {
                throw new HandlerNotFoundException(requestType, nameof(IRequestHandler<IRequest>.Handle));
            }

            handle = () =>
            {
                var result = handleBehaviourMethod.Invoke(behavior, [request, next, cancellationToken]);

                return result as Task ?? Task.FromException(new InvalidHandlerResultException(next.GetType(), handleBehaviourMethod.Name, result));
            };
        }

        // Execute the entire chain
        await handle();
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        // Resolve the handler from the service provider
        var handler = serviceProvider.GetRequiredService(handlerType);

        // Get the 'Handle' method of the handler
        var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest>.Handle));

        if (handleMethod == null)
        {
            throw new HandlerNotFoundException(requestType, nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle));
        }

        // Resolve the pipeline behaviors (if any)
        var behaviors = serviceProvider.GetServices(typeof(IPipelineBehavior<,>)
            .MakeGenericType(requestType, typeof(TResponse)))
            .Cast<object>()
            .Reverse()
            .ToList();

        // Create a Func<Task<TResponse>> to invoke the handler
        Func<Task<TResponse>> handle = () =>
        {
            var result = handleMethod.Invoke(handler, [request, cancellationToken]);
            return result as Task<TResponse> ?? Task.FromException<TResponse>(new InvalidHandlerResultException(handler.GetType(), handleMethod.Name, result));
        };

        // Loop through behaviors and chain them
        foreach (var behavior in behaviors)
        {
            // Capture the current 'next' function
            var next = handle;

            var handleBehaviourMethod = behavior.GetType().GetMethod(nameof(IPipelineBehavior<IRequest<TResponse>, TResponse>.Handle));

            if (handleBehaviourMethod == null)
            {
                throw new HandlerNotFoundException(requestType, nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle));
            }

            handle = () =>
            {
                var result = handleBehaviourMethod.Invoke(behavior, [request, next, cancellationToken]);

                return result as Task<TResponse> ?? Task.FromException<TResponse>(new InvalidHandlerResultException(next.GetType(), handleBehaviourMethod.Name, result));
            };
        }

        // Execute the entire chain
        return await handle();
    }
}