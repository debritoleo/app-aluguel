using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Aggregates.RentalAggregate;

namespace RentIt.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<Deliveryman> Deliverymen => Set<Deliveryman>();
    public DbSet<Rental> Rentals => Set<Rental>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
