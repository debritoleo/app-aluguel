using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using RentIt.Application.Messaging;

namespace RentIt.Infrastructure.Messaging.Consumers;

public class RabbitMqBackgroundConsumer<TEvent> : BackgroundService where TEvent : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _queueName;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqBackgroundConsumer(IServiceProvider serviceProvider, string queueName)
    {
        _serviceProvider = serviceProvider;
        _queueName = queueName;

        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest",
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var message = JsonSerializer.Deserialize<TEvent>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (message != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<TEvent>>();
                    await handler.HandleAsync(message, stoppingToken);
                }

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem RabbitMQ: {ex.Message}");
                // Aqui em um cenário real, seria enviado para uma dead letter, retries, etc...
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
        base.Dispose();
    }
}
