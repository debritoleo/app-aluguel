using RentIt.Application.Common;
using RentIt.Application.Requests.Deliveryman;

namespace RentIt.Application.Services.Interfaces;

public interface IDeliverymanService
{
    Task<Result<string>> CreateAsync(CreateDeliverymanRequest request, CancellationToken cancellationToken = default);
}