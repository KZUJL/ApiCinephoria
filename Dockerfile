# Image runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Image build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier uniquement le projet pour restore (optimise le cache)
COPY ApiCinephoria/ApiCinephoria.csproj ./ApiCinephoria/
RUN dotnet restore ./ApiCinephoria/ApiCinephoria.csproj

# Copier tout le reste
COPY . .
RUN dotnet publish ./ApiCinephoria/ApiCinephoria.csproj -c Release -o /app/publish

# Image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiCinephoria.dll"]
