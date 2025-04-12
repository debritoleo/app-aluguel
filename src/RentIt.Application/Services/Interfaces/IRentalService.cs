using RentIt.Application.Commands.Rental;
using RentIt.Application.Common;
using RentIt.Application.Requests.Rental;

namespace RentIt.Application.Services.Interfaces;
public interface IRentalService
{
    Task<Result<string>> CreateAsync(CreateRentalRequest request, CancellationToken cancellationToken = default);
    Task<Result<decimal>> ReturnAsync(string rentalId, ReturnRentalRequest request, CancellationToken cancellationToken = default);
}
