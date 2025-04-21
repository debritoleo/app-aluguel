namespace RentIt.Application.Services.Interfaces;

public interface ICnhStorageService
{
    Task<string> SaveBase64Async(Guid deliverymanId, string base64Content, CancellationToken cancellationToken = default);
}
