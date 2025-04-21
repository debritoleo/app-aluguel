using Dapper;
using Microsoft.EntityFrameworkCore;
using RentIt.Application.Queries.Rental;
using RentIt.Application.ViewModels.Rental;

namespace RentIt.Infrastructure.Queries.Rental;
public class RentalQueries : IRentalQueries
{
    private readonly AppDbContext _context;

    public RentalQueries(AppDbContext context) => _context = context;

    public async Task<RentalViewModel?> GetByIdAsync(string id)
    {
        var conn = _context.Database.GetDbConnection();

        const string sql = @"
            SELECT
                ""Id"" AS ""Id"",
                ""MotorcycleId"" AS ""MotorcycleId"",
                ""DeliverymanId"" AS ""DeliverymanId"",
                ""start_date"" AS ""StartDate"",
                ""end_date"" AS ""EndDate"",
                ""expected_end_date"" AS ""ExpectedEndDate"",
                ""plan_days"" AS ""PlanDays"",
                ""daily_rate"" AS ""DailyRate""
            FROM ""rentals""
            WHERE ""Id"" = @id
        ";

        var result = await conn.QueryFirstOrDefaultAsync<RentalViewModel>(sql, new { id });
        return result;
    }
}