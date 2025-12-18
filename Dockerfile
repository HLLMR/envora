# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file(s) and restore
COPY src/Envora.Web/Envora.Web.csproj ./src/Envora.Web/
RUN dotnet restore ./src/Envora.Web/Envora.Web.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/src/Envora.Web
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Envora.Web.dll"]


