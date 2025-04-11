using RentIt.Application.Commands.Deliveryman;
using RentIt.Application.Common;

namespace RentIt.Application.Services.Interfaces;

public interface IDeliverymanService
{
    Task<Result<string>> CreateAsync(CreateDeliverymanRequest request, CancellationToken cancellationToken = default);
}