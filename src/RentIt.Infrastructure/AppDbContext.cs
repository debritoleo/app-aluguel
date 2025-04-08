using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.MotorcycleAggregate;

namespace RentIt.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
