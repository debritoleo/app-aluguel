using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.MotorcycleAggregate;

namespace RentIt.Infrastructure.Persistence.Configurations;
public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Motorcycle> builder)
    {
        builder.ToTable("motorcycles");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Identifier).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Year).IsRequired();
        builder.Property(m => m.Model).IsRequired().HasMaxLength(100);

        builder.OwnsOne(m => m.Plate, plate =>
        {
            plate.Property(p => p.Value)
                .HasColumnName("Plate")
                .IsRequired()
                .HasMaxLength(10);

            plate.HasIndex(p => p.Value).IsUnique();
        });
    }
}