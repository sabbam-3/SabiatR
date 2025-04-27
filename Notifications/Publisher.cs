using Microsoft.Extensions.DependencyInjection;
using SabiatR.Exceptions;

namespace SabiatR.Notifications;

internal sealed class Publisher(IServiceProvider serviceProvider) : IPublisher
{
    public async Task Publish(INotification notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

        var handlers = serviceProvider.GetServices(handlerType);
      
        foreach(var handler in handlers)
        {
            if (handler is null)
            {
                continue;
            }

            var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest>.Handle));

            if (handleMethod == null)
            {
                throw new HandlerNotFoundException(notificationType, nameof(IRequestHandler<IRequest>.Handle));
            }

            Func<Task> handle = () =>
            {
                var result = handleMethod.Invoke(handler, [notification, cancellationToken]);
                return result as Task ?? Task.FromException(new InvalidHandlerResultException(handler.GetType(), handleMethod.Name, result));
            };

            await handle();
        }
    }

    public async Task PublishAsync(INotification notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

        var handlers = serviceProvider.GetServices(handlerType);

        IList<Task> handles = new List<Task>();

        foreach (var handler in handlers)
        {
            if(handler is null)
            {
                continue;
            }

            var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest>.Handle));

            if (handleMethod == null)
            {
                throw new HandlerNotFoundException(notificationType, nameof(IRequestHandler<IRequest>.Handle));
            }

            Func<Task> handle = () =>
            {
                var result = handleMethod.Invoke(handler, [notification, cancellationToken]);
                return result as Task ?? Task.FromException(new InvalidHandlerResultException(handler.GetType(), handleMethod.Name, result));
            };

            handles.Add(handle());
        }

        await Task.WhenAll(handles);
    }
}
