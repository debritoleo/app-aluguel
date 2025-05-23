﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RentIt.Infrastructure;

#nullable disable

namespace RentIt.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RentIt.Domain.Aggregates.DeliverymanAggregate.Deliveryman", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("CnhType")
                        .HasColumnType("integer");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("deliverymen", (string)null);
                });

            modelBuilder.Entity("RentIt.Domain.Aggregates.MotorcycleAggregate.Motorcycle", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("motorcycles", (string)null);
                });

            modelBuilder.Entity("RentIt.Domain.Aggregates.MotorcycleDenormalizedAggregate.MotorcycleDenormalized", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("motorcycles_denormalized", (string)null);
                });

            modelBuilder.Entity("RentIt.Domain.Aggregates.RentalAggregate.Rental", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DeliverymanId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("MotorcycleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.HasKey("Id");

                    b.ToTable("rentals", (string)null);
                });

            modelBuilder.Entity("RentIt.Domain.Aggregates.DeliverymanAggregate.Deliveryman", b =>
                {
                    b.OwnsOne("RentIt.Domain.Aggregates.DeliverymanAggregate.BirthDate", "BirthDate", b1 =>
                        {
                            b1.Property<string>("DeliverymanId")
                                .HasColumnType("text");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("BirthDate");

                            b1.HasKey("DeliverymanId");

                            b1.ToTable("deliverymen");

                            b1.WithOwner()
                                .HasForeignKey("DeliverymanId");
                        });

                    b.OwnsOne("RentIt.Domain.Aggregates.DeliverymanAggregate.CnhNumber", "CnhNumber", b1 =>
                        {
                            b1.Property<string>("DeliverymanId")
                                .HasColumnType("text");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("CnhNumber");

                            b1.HasKey("DeliverymanId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("deliverymen");

                            b1.WithOwner()
                                .HasForeignKey("DeliverymanId");
                        });

                    b.OwnsOne("RentIt.Domain.Aggregates.DeliverymanAggregate.Cnpj", "Cnpj", b1 =>
                        {
                            b1.Property<string>("DeliverymanId")
                                .HasColumnType("text");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("Cnpj");

                            b1.HasKey("DeliverymanId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("deliverymen");

                            b1.WithOwner()
                                .HasForeignKey("DeliverymanId");
                        });

                    b.Navigation("BirthDate")
                        .IsRequired();

                    b.Navigation("CnhNumber")
                        .IsRequired();

                    b.Navigation("Cnpj")
                        .IsRequired();
                });

            modelBuilder.Entity("RentIt.Domain.Aggregates.MotorcycleAggregate.Motorcycle", b =>
                {
                    b.OwnsOne("RentIt.Domain.Aggregates.MotorcycleAggregate.Plate", "Plate", b1 =>
                        {
                            b1.Property<string>("MotorcycleId")
                                .HasColumnType("text");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)")
                                .HasColumnName("Plate");

                            b1.HasKey("MotorcycleId");

                            b1.HasIndex("Value")
                                .IsUnique();

                            b1.ToTable("motorcycles");

                            b1.WithOwner()
                                .HasForeignKey("MotorcycleId");
                        });

                    b.Navigation("Plate")
                        .IsRequired();
                });

            modelBuilder.Entity("RentIt.Domain.Aggregates.RentalAggregate.Rental", b =>
                {
                    b.OwnsOne("RentIt.Domain.Aggregates.RentalAggregate.RentalPeriod", "Period", b1 =>
                        {
                            b1.Property<string>("RentalId")
                                .HasColumnType("text");

                            b1.Property<DateTime>("EndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("end_date");

                            b1.Property<DateTime>("ExpectedEndDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("expected_end_date");

                            b1.Property<DateTime>("StartDate")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("start_date");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId");
                        });

                    b.OwnsOne("RentIt.Domain.Aggregates.RentalAggregate.RentalPlan", "Plan", b1 =>
                        {
                            b1.Property<string>("RentalId")
                                .HasColumnType("text");

                            b1.Property<decimal>("DailyRate")
                                .HasPrecision(10, 2)
                                .HasColumnType("numeric(10,2)")
                                .HasColumnName("daily_rate");

                            b1.Property<int>("Days")
                                .HasColumnType("integer")
                                .HasColumnName("plan_days");

                            b1.Property<decimal>("EarlyReturnPenaltyPercentage")
                                .HasPrecision(5, 2)
                                .HasColumnType("numeric(5,2)")
                                .HasColumnName("early_penalty");

                            b1.Property<decimal>("LateFeePerDay")
                                .HasPrecision(10, 2)
                                .HasColumnType("numeric(10,2)")
                                .HasColumnName("late_fee");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId");
                        });

                    b.Navigation("Period")
                        .IsRequired();

                    b.Navigation("Plan")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
