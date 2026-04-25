# personal.transaction.management

## Database Migrations

**Generate a migration** (run from the solution root):

```bash
dotnet ef migrations add <MigrationName>   --project personal.transaction.management.infrastructure   --startup-project personal.transaction.management.api   --output-dir Persistence/Migrations
```

**Apply migrations to the database:**

```bash
dotnet ef database update   --project personal.transaction.management.infrastructure   --startup-project personal.transaction.management.api
```

**Revert the last migration:**

```bash
dotnet ef migrations remove   --project personal.transaction.management.infrastructure   --startup-project personal.transaction.management.api
```

> `dotnet-ef` must be installed globally: `dotnet tool install --global dotnet-ef`

## Running with Docker Compose

```bash
# 1. Copy and fill in the required secrets
cp .env.example .env

# 2. Build and start all services
docker compose up --build

# API → http://localhost:8080
# Scalar UI → http://localhost:8080/scalar/v1
```

## Pending Reports

5. ¿Qué reportes quieres exponer?

Resumen por período (ingresos vs gastos del mes)
Gastos por categoría en un rango de fechas
Balance actual
Evolución mensual (últimos N meses)