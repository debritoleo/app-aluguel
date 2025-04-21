using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Commands.Rental;
using RentIt.Application.Messaging;
using RentIt.Application.Queries.Motorcycle;
using RentIt.Application.Queries.Rental;
using RentIt.Application.Requests.Deliveryman;
using RentIt.Application.Services;
using RentIt.Application.Services.Interfaces;
using RentIt.Application.Settings;
using RentIt.Application.Validators;
using RentIt.Domain.Repositories;
using RentIt.Infrastructure;
using RentIt.Infrastructure.Messaging;
using RentIt.Infrastructure.Messaging.Consumers;
using RentIt.Infrastructure.Messaging.Handlers;
using RentIt.Infrastructure.Persistence.Repositories;
using RentIt.Infrastructure.Queries.Motorcycle;
using RentIt.Infrastructure.Queries.Rental;
using RentIt.IntegrationEvents.Motorcycle;

namespace RentIt.API.Extensions.DI;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        AddRepositories(services);
        AddServices(services);
        AddValidators(services);
        AddQueries(services);
        AddMessaging(services);
        AddSettings(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddSingleton(TimeProvider.System);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IDeliverymanRepository, DeliverymanRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IDeliverymanService, DeliverymanService>();
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<RentalCreationValidator>();
        services.AddScoped<ICnhStorageService, LocalCnhStorageService>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateDeliverymanRequest>, CreateDeliverymanRequestValidator>();
        services.AddScoped<IValidator<CreateMotorcycleRequest>, CreateMotorcycleRequestValidator>();
        services.AddScoped<IValidator<CreateRentalRequest>, CreateRentalRequestValidator>();
    }

    private static void AddQueries(IServiceCollection services)
    {
        services.AddScoped<IMotorcycleQueries, MotorcycleQueries>();
        services.AddScoped<IRentalQueries, RentalQueries>();
    }

    private static void AddMessaging(IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

        services.AddScoped<IIntegrationEventHandler<MotorcycleCreatedEvent>, MotorcycleCreatedEventHandler>();

        services.AddHostedService(provider =>
            new RabbitMqBackgroundConsumer<MotorcycleCreatedEvent>(
                provider,
                queueName: "motorcycles"
            )
        );
    }

    private static void AddSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CnhStorageSettings>(
            configuration.GetSection("CnhStorage"));
    }
}
