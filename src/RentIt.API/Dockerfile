FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["RentIt.API/RentIt.API.csproj", "RentIt.API/"]
COPY ["RentIt.Application/RentIt.Application.csproj", "RentIt.Application/"]
COPY ["RentIt.Domain/RentIt.Domain.csproj", "RentIt.Domain/"]
COPY ["RentIt.Infrastructure/RentIt.Infrastructure.csproj", "RentIt.Infrastructure/"]

RUN dotnet restore "RentIt.API/RentIt.API.csproj"

COPY . .

WORKDIR "/app/RentIt.API"
RUN dotnet publish "RentIt.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "RentIt.API.dll"]
