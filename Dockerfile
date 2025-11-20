# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and project file
COPY *.sln .
COPY global.json .
COPY src/CleanArchitectureTemplate.API/*.csproj ./src/CleanArchitectureTemplate.API/
COPY src/CleanArchitectureTemplate.Application/*.csproj ./src/CleanArchitectureTemplate.Application/
COPY src/CleanArchitectureTemplate.Domain/*.csproj ./src/CleanArchitectureTemplate.Domain/
COPY src/CleanArchitectureTemplate.Infrastructure/*.csproj ./src/CleanArchitectureTemplate.Infrastructure/

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the source code
COPY src/. ./src/

# Build the application
RUN dotnet publish src/CleanArchitectureTemplate.API/CleanArchitectureTemplate.API.csproj -c Release -o out

# Build runtime image using SDK (needed for dotnet-ef tools)
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Install dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Install curl and postgresql-client for healthcheck and database operations
RUN apt-get update && apt-get install -y curl postgresql-client && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/out .

# Copy source code (needed for migrations)
COPY --from=build /app/src ./src
COPY --from=build /app/*.sln .
COPY --from=build /app/global.json .

# Copy entrypoint script
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

EXPOSE 80
EXPOSE 443

# Use entrypoint script to apply migrations before starting app
ENTRYPOINT ["/docker-entrypoint.sh"]