using Microsoft.AspNetCore.Http;
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

    public async Task<string> SaveAsync(Guid deliverymanId, IFormFile file, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (extension != ".png" && extension != ".bmp")
            throw new InvalidOperationException("Formato inválido. Apenas PNG ou BMP são permitidos.");

        var fileName = $"{deliverymanId}{extension}";
        var fullPath = Path.Combine(_basePath, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return fileName;
    }
}
