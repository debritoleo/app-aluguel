using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "deliverymen",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CnhNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CnhType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliverymen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Plate = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MotorcycleId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    DeliverymanId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expected_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    plan_days = table.Column<int>(type: "integer", nullable: false),
                    daily_rate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    early_penalty = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    late_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliverymen_CnhNumber",
                table: "deliverymen",
                column: "CnhNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deliverymen_Cnpj",
                table: "deliverymen",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_motorcycles_Plate",
                table: "motorcycles",
                column: "Plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deliverymen");

            migrationBuilder.DropTable(
                name: "motorcycles");

            migrationBuilder.DropTable(
                name: "rentals");
        }
    }
}
