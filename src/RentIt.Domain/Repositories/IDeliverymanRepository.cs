﻿using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Domain.Repositories;

public interface IDeliverymanRepository
{
    Task<Deliveryman?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(Deliveryman deliveryman, CancellationToken cancellationToken = default);
    Task<bool> CnpjExistsAsync(string cnpj, CancellationToken cancellationToken = default);
    Task<bool> CnhNumberExistsAsync(string cnhNumber, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
