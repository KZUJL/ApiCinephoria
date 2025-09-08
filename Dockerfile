# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Définir l'URL Kestrel pour Fly.io
ENV ASPNETCORE_URLS=http://+:80

# SDK pour build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier csproj et restaurer
COPY ["ApiCinephoria/ApiCinephoria.csproj", "ApiCinephoria/"]
RUN dotnet restore "ApiCinephoria/ApiCinephoria.csproj"

# Copier tout le code
COPY . .
WORKDIR "/src/ApiCinephoria"
RUN dotnet publish -c Release -o /app/publish

# Image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiCinephoria.dll"]
