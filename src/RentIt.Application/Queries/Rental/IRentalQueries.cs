using RentIt.Application.ViewModels.Rental;

namespace RentIt.Application.Queries.Rental;

public interface IRentalQueries
{
    Task<RentalViewModel?> GetByIdAsync(string id);
}
