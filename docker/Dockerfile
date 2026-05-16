# Multi-stage build for eShop .NET services.
# Build example:
#   docker build -f docker/Dockerfile \
#     --build-arg PROJECT_PATH=src/Catalog.API/Catalog.API.csproj \
#     --build-arg ASSEMBLY_NAME=Catalog.API \
#     -t eshop/catalog-api:latest .

ARG DOTNET_VERSION=10.0
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG PROJECT_PATH
WORKDIR /src

COPY Directory.Packages.props Directory.Build.props global.json eShop.slnx ./
COPY src/ src/

RUN dotnet restore "${PROJECT_PATH}"
RUN dotnet publish "${PROJECT_PATH}" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
ARG ASSEMBLY_NAME
ENV APP_DLL=${ASSEMBLY_NAME}.dll
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    ENABLE_HEALTH_CHECKS=true

EXPOSE 8080

# Health checks: use compose depends_on (postgres/redis/rabbitmq) or K8s probes.
# curl is not installed here to keep images small and avoid apt cache disk use during build.

ENTRYPOINT ["sh", "-c", "exec dotnet \"$APP_DLL\""]
