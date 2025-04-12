using FluentValidation;
using RentIt.Application.Commands.Deliveryman;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Services.Interfaces;
using RentIt.Application.Services;
using RentIt.Domain.Repositories;
using RentIt.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using RentIt.Infrastructure;
using RentIt.Application.Settings;
using RentIt.Application.Commands.Rental;
using RentIt.Application.Validators;

namespace RentIt.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDeliverymanRepository, DeliverymanRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();

        services.AddScoped<IDeliverymanService, DeliverymanService>();
        services.AddScoped<IMotorcycleService, MotorcycleService>();

        services.AddScoped<IValidator<CreateDeliverymanRequest>, CreateDeliverymanRequestValidator>();
        services.AddScoped<IValidator<CreateMotorcycleRequest>, CreateMotorcycleRequestValidator>();
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<RentalCreationValidator>();
        services.AddScoped<IValidator<CreateRentalRequest>, CreateRentalRequestValidator>();

        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<CnhStorageSettings>(
            configuration.GetSection("CnhStorage"));

        services.AddScoped<ICnhStorageService, LocalCnhStorageService>();

        return services;
    }
}
