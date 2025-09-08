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

# Copier le dossier Data dans l'image
# Il sera disponible dans /app/Data
RUN mkdir -p /app/Data
COPY ApiCinephoria/Data /app/Data

# Publier l'application
RUN dotnet publish -c Release -o /app/publish

# Image finale
FROM base AS final
WORKDIR /app

# Copier l'application publiée
COPY --from=build /app/publish .

# Copier le dossier Data
COPY --from=build /app/Data ./Data

ENTRYPOINT ["dotnet", "ApiCinephoria.dll"]
