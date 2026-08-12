# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy project file
COPY ["UNI_ASSETS.csproj", "./"]

# Restore dependencies
RUN dotnet restore "UNI_ASSETS.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish
RUN dotnet publish "UNI_ASSETS.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Koyeb will route traffic to the port configured for the service
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "UNI_ASSETS.dll"]