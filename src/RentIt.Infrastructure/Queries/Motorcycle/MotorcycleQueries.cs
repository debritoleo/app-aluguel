using RentIt.Application.ViewModels.Motorcycle;
using System.Text;
using Dapper;
using RentIt.Application.Queries.Motorcycle;
using Microsoft.EntityFrameworkCore;

namespace RentIt.Infrastructure.Queries.Motorcycle;
public class MotorcycleQueries : IMotorcycleQueries
{
    private readonly AppDbContext _context;

    public MotorcycleQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MotorcycleViewModel>> GetAllAsync(string? plate, CancellationToken cancellationToken = default)
    {
        var conn = _context.Database.GetDbConnection();

        var sql = new StringBuilder(@"
            SELECT id, identifier, year, model, plate
            FROM motorcycles
        ");

        if (!string.IsNullOrWhiteSpace(plate))
        {
            sql.AppendLine("WHERE plate = @plate");
        }

        return await conn.QueryAsync<MotorcycleViewModel>(sql.ToString(), new { plate });
    }

    public async Task<MotorcycleViewModel?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var conn = _context.Database.GetDbConnection();

        const string sql = @"
            SELECT id, identifier, year, model, plate
            FROM motorcycles
            WHERE id = @id
        ";

        return await conn.QueryFirstOrDefaultAsync<MotorcycleViewModel>(sql, new { id });
    }
}

