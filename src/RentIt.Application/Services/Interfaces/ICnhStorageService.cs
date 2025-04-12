using Microsoft.AspNetCore.Http;

namespace RentIt.Application.Services.Interfaces;
public interface ICnhStorageService
{
    Task<string> SaveAsync(Guid deliverymanId, IFormFile file, CancellationToken cancellationToken = default);
}
