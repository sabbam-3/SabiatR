namespace SabiatR.Notifications;

public interface IPublisher
{
    Task Publish(INotification notification, CancellationToken cancellationToken = default);

    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);
}
