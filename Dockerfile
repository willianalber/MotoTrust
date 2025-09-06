# Use the official .NET 9.0 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Use the official .NET 9.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["MotoTrust.API/MotoTrust.API.csproj", "MotoTrust.API/"]
COPY ["MotoTrust.Application/MotoTrust.Application.csproj", "MotoTrust.Application/"]
COPY ["MotoTrust.Domain/MotoTrust.Domain.csproj", "MotoTrust.Domain/"]
COPY ["MotoTrust.Infrastructure/MotoTrust.Infrastructure.csproj", "MotoTrust.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "MotoTrust.API/MotoTrust.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/MotoTrust.API"
RUN dotnet build "MotoTrust.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MotoTrust.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "MotoTrust.API.dll"]
