using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Aggregates.MotorcycleDenormalizedAggregate;
using RentIt.Domain.Aggregates.RentalAggregate;

namespace RentIt.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<Deliveryman> Deliverymen => Set<Deliveryman>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<MotorcycleDenormalized> MotorcycleDenormalizeds => Set<MotorcycleDenormalized>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ApplyUtcDateTimeConverter(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private static void ApplyUtcDateTimeConverter(ModelBuilder modelBuilder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            toDb => toDb.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(toDb, DateTimeKind.Utc) : toDb.ToUniversalTime(),
            fromDb => DateTime.SpecifyKind(fromDb, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            toDb => toDb.HasValue ?
                (toDb.Value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(toDb.Value, DateTimeKind.Utc) : toDb.Value.ToUniversalTime())
                : null,
            fromDb => fromDb.HasValue ? DateTime.SpecifyKind(fromDb.Value, DateTimeKind.Utc) : null);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }
}
