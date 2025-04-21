using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentIt.Infrastructure;

namespace RentIt.Tests.Integration;

public abstract class BaseIntegrationTest : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
{
    private static bool _hasCleanedDatabase;

    protected readonly HttpClient Client;
    protected readonly IServiceProvider Services;

    protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
        Services = factory.Services;
    }

    public async Task InitializeAsync()
    {
        if (!_hasCleanedDatabase)
        {
            await ClearDatabaseAsync();
            _hasCleanedDatabase = true;
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

    protected async Task ClearDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Rentals.RemoveRange(await db.Rentals.ToListAsync());
        db.Deliverymen.RemoveRange(await db.Deliverymen.ToListAsync());
        db.Motorcycles.RemoveRange(await db.Motorcycles.ToListAsync());

        await db.SaveChangesAsync();
    }
}