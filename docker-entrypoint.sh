#!/bin/bash
set -e

echo "========================================"
echo "Starting Clean Architecture Template API"
echo "========================================"

# Wait for PostgreSQL to be ready
echo "Waiting for PostgreSQL to be ready..."
MAX_RETRIES=30
RETRY_COUNT=0

until pg_isready -h db -U ${POSTGRES_USER:-postgres} -d ${POSTGRES_DB:-CleanArchitectureTemplateDb} > /dev/null 2>&1; do
  RETRY_COUNT=$((RETRY_COUNT+1))
  if [ $RETRY_COUNT -ge $MAX_RETRIES ]; then
    echo "PostgreSQL is not available after $MAX_RETRIES attempts. Exiting..."
    exit 1
  fi
  echo "PostgreSQL is unavailable - retry $RETRY_COUNT/$MAX_RETRIES (waiting 2s)"
  sleep 2
done

echo "PostgreSQL is ready!"

# Apply database migrations using dotnet-ef
echo "Applying database migrations..."

# Set connection string for EF Core
export ConnectionStrings__DefaultConnection="Host=db;Port=5432;Database=${POSTGRES_DB:-CleanArchitectureTemplateDb};Username=${POSTGRES_USER:-postgres};Password=${POSTGRES_PASSWORD:-postgres}"

# Build the projects first (required for dotnet-ef to find deps.json)
echo "Building projects for migrations..."
cd /app/src/CleanArchitectureTemplate.Infrastructure
dotnet build CleanArchitectureTemplate.Infrastructure.csproj -c Debug
cd /app/src/CleanArchitectureTemplate.API
dotnet build CleanArchitectureTemplate.API.csproj -c Debug

# Navigate to the Infrastructure project directory where migrations are located
cd /app/src/CleanArchitectureTemplate.Infrastructure

# Apply migrations using dotnet-ef
echo "Running: dotnet ef database update --startup-project ../CleanArchitectureTemplate.API"
dotnet ef database update --startup-project ../CleanArchitectureTemplate.API --verbose

if [ $? -eq 0 ]; then
  echo "Migrations applied successfully!"
else
  echo "Failed to apply migrations. Exiting..."
  exit 1
fi

# Return to app directory
cd /app

echo "========================================"
echo "Starting application..."
echo "========================================"

# Start the application
exec dotnet CleanArchitectureTemplate.API.dll
