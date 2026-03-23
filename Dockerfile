# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project file and restore dependencies
COPY StudentProject.csproj ./
RUN dotnet restore

# Copy remaining files and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Copy published files
COPY --from=build /app/publish .

# Expose port (Render uses PORT env var, default to 8080)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Set environment for production
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "StudentProject.dll"]