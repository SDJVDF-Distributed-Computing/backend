FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Backend.sln .
COPY Backend.API/Backend.API.csproj Backend.API/
COPY Backend.Application/Backend.Application.csproj Backend.Application/
COPY Backend.Domain/Backend.Domain.csproj Backend.Domain/
COPY Backend.Infrastructure/Backend.Infrastructure.csproj Backend.Infrastructure/
COPY Backend.Console/Backend.Console.csproj Backend.Console/

RUN dotnet restore Backend.API/Backend.API.csproj

COPY . .

RUN dotnet publish Backend.API/Backend.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app

COPY --from=build /app/publish .

RUN mkdir -p certs

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

ENTRYPOINT ["dotnet", "Backend.API.dll"]
