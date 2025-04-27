namespace SabiatR.Notifications;

public interface IPublisher
{
    Task Publish(INotification notification, CancellationToken cancellationToken);

    Task PublishAsync(INotification notification, CancellationToken cancellationToken);
}
