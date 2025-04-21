using RentIt.API.Extensions.DI;
using RentIt.Infrastructure.Persistence.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

MigrationManager.ApplyMigrations(app.Services);

app.UseHttpsRedirection();
app.UseGlobalExceptionHandling(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.MapControllers();

app.Run();

public partial class Program { }
