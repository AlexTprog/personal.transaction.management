# ── Stage 1: build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore — copy only csproj files first to maximise layer cache hits
COPY personal.transaction.management.domain/personal.transaction.management.domain.csproj \
     personal.transaction.management.domain/
COPY personal.transaction.management.application/personal.transaction.management.application.csproj \
     personal.transaction.management.application/
COPY personal.transaction.management.infrastructure/personal.transaction.management.infrastructure.csproj \
     personal.transaction.management.infrastructure/
COPY personal.transaction.management.api/personal.transaction.management.api.csproj \
     personal.transaction.management.api/

RUN dotnet restore personal.transaction.management.api/personal.transaction.management.api.csproj

# Copy full source and publish
COPY . .
RUN dotnet publish personal.transaction.management.api/personal.transaction.management.api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Stage 2: runtime ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Run as non-root
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "personal.transaction.management.api.dll"]
