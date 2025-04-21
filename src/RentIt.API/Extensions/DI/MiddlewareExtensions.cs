using RentIt.API.Extensions.Middlewares;

namespace RentIt.API.Extensions.DI;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
        => builder.UseMiddleware<ExceptionHandlingMiddleware>();
}

