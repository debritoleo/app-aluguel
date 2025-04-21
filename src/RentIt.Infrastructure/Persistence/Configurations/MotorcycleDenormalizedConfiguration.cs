using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.MotorcycleDenormalizedAggregate;

namespace RentIt.Infrastructure.Persistence.Configurations;

public class MotorcycleDenormalizedConfiguration : IEntityTypeConfiguration<MotorcycleDenormalized>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MotorcycleDenormalized> builder)
    {
        builder.ToTable("motorcycles_denormalized");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Identifier).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Year).IsRequired();
        builder.Property(m => m.Model).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Plate).IsRequired().HasMaxLength(10);
    }
}