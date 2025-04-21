using Microsoft.Extensions.Options;
using RentIt.Application.Services.Interfaces;
using RentIt.Application.Settings;

namespace RentIt.Application.Services;

public class LocalCnhStorageService : ICnhStorageService
{
    private readonly string _basePath;

    public LocalCnhStorageService(IOptions<CnhStorageSettings> settings)
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), settings.Value.LocalPath);

        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveBase64Async(Guid deliverymanId, string base64Content, CancellationToken cancellationToken = default)
    {
        byte[] fileBytes;

        try
        {
            fileBytes = Convert.FromBase64String(base64Content);
        }
        catch
        {
            throw new InvalidOperationException("Dados inválidos");
        }

        var fileName = $"{deliverymanId}_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.png";

        var fullPath = Path.Combine(_basePath, fileName);

        try
        {
            await File.WriteAllBytesAsync(fullPath, fileBytes, cancellationToken);
            return fileName;
        }
        catch
        {
            throw new InvalidOperationException("Dados inválidos");
        }
    }
}
