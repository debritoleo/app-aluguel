using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.RentalAggregate;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("rentals");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.MotorcycleId).HasMaxLength(36).IsRequired();
        builder.Property(r => r.DeliverymanId).HasMaxLength(36).IsRequired();

        builder.OwnsOne(r => r.Period, p =>
        {
            p.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .IsRequired();

            p.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .IsRequired();

            p.Property(x => x.ExpectedEndDate)
                .HasColumnName("expected_end_date")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .IsRequired();
        });

        builder.OwnsOne(r => r.Plan, p =>
        {
            p.Property(x => x.Days).HasColumnName("plan_days").IsRequired();
            p.Property(x => x.DailyRate).HasColumnName("daily_rate").HasPrecision(10, 2).IsRequired();
            p.Property(x => x.EarlyReturnPenaltyPercentage).HasColumnName("early_penalty").HasPrecision(5, 2).IsRequired();
            p.Property(x => x.LateFeePerDay).HasColumnName("late_fee").HasPrecision(10, 2).IsRequired();
        });
    }
}
