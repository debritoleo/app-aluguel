namespace RentIt.Application.Messaging;
public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string queue, CancellationToken cancellationToken = default);
}