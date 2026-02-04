# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY PRN232.FUNewsManagement.API/*.csproj ./PRN232.FUNewsManagement.API/
COPY PRN232.FUNewsManagement.Models/*.csproj ./PRN232.FUNewsManagement.Models/
COPY PRN232.FUNewsManagement.Repo/*.csproj ./PRN232.FUNewsManagement.Repo/
COPY PRN232.FUNewsManagement.Services/*.csproj ./PRN232.FUNewsManagement.Services/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/PRN232.FUNewsManagement.API
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose port 8080 (default for .NET 8)
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Entry point
ENTRYPOINT ["dotnet", "PRN232.FUNewsManagement.API.dll"]
