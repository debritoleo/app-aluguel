using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Infrastructure.Persistence.Configurations;
public class DeliverymanConfiguration : IEntityTypeConfiguration<Deliveryman>
{
    public void Configure(EntityTypeBuilder<Deliveryman> builder)
    {
        builder.ToTable("deliverymen");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Identifier)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.CnhType)
            .IsRequired();

        builder.OwnsOne(x => x.Cnpj, cnpj =>
        {
            cnpj.Property(c => c.Value)
                .HasColumnName("Cnpj")
                .IsRequired()
                .HasMaxLength(14);

            cnpj.HasIndex(c => c.Value).IsUnique();
        });

        builder.OwnsOne(x => x.CnhNumber, cnh =>
        {
            cnh.Property(c => c.Value)
                .HasColumnName("CnhNumber")
                .IsRequired()
                .HasMaxLength(20);

            cnh.HasIndex(c => c.Value).IsUnique();
        });

        builder.OwnsOne(x => x.BirthDate, bd =>
        {
            bd.Property(b => b.Value)
              .HasColumnName("BirthDate")
              .IsRequired();
        });
    }
}