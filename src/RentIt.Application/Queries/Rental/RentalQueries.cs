using Dapper;
using Microsoft.EntityFrameworkCore;
using RentIt.Application.ViewModels.Rental;
using RentIt.Infrastructure;

namespace RentIt.Application.Queries.Rental;

public class RentalQueries : IRentalQueries
{
    private readonly AppDbContext _context;

    public RentalQueries(AppDbContext context) => _context = context;

    public async Task<RentalViewModel?> GetByIdAsync(string id)
    {
        var conn = _context.Database.GetDbConnection();

        const string sql = @"
        SELECT
            id,
            motorcycle_id AS MotorcycleId,
            deliveryman_id AS DeliverymanId,
            start_date AS StartDate,
            end_date AS EndDate,
            expected_end_date AS ExpectedEndDate,
            plan_days AS PlanDays,
            daily_rate AS DailyRate
        FROM rentals
        WHERE id = @id
        ";

        return await conn.QueryFirstOrDefaultAsync<RentalViewModel>(sql, new { id });
    }
}