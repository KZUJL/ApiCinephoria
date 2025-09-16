# 🎬 Cinephoria Back-end (C# .NET 8)

Cinephoria Back-end est l’API REST de la plateforme Cinephoria.  
Elle gère l’authentification, les réservations, la gestion des films, la base de données MySQL, ainsi que les données liées aux utilisateurs et employés.  

---

## 🚀 Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)  
- [MySQL](https://dev.mysql.com/downloads/) (version 8+)  
- [MongoDB](https://www.mongodb.com/try/download/community) (pour la gestion des réservations)  

---

## 📂 Installation

### Cloner le dépôt

```bash
git clone https://github.com/ton-org/CinephoriaApi.git
cd CinephoriaApi

Restaurer les dépendances :
dotnet restore


### Configuration des secrets avec .NET User Secrets

L’API Cinephoria utilise le gestionnaire de secrets intégré à .NET.  
Les secrets doivent être définis **avant de lancer l’API**, sinon l’application ne pourra pas démarrer.

Exécuter les commandes suivantes depuis la racine du projet (adapter le chemin si besoin) :

```bash
# Base de données MySQL
dotnet user-secrets set "ConnectionStrings:MYSQL_CONNECTION" "Server=localhost;Database=cinephoria;User=root;Password=<YOUR_MYSQL_PASSWORD>" --project ApiCinephoria/ApiCinephoria.csproj

# Base de données MongoDB
dotnet user-secrets set "ConnectionStrings:MONGODB_CONNECTION" "mongodb://<USER>:<PASSWORD>@localhost:27017/?authSource=reservation" --project ApiCinephoria/ApiCinephoria.csproj

# Mailjet (remplacer par vos propres clés)
dotnet user-secrets set "Mailjet:MAILJET_APIKEY" "<YOUR_MAILJET_APIKEY>" --project ApiCinephoria/ApiCinephoria.csproj
dotnet user-secrets set "Mailjet:MAILJET_APISECRET" "<YOUR_MAILJET_APISECRET>" --project ApiCinephoria/ApiCinephoria.csproj


Pour vérifier que les secrets ont bien été ajoutés :
dotnet user-secrets list --project ApiCinephoria/ApiCinephoria.csproj
⚠️ Pour tester l’envoi d’emails via Mailjet, vous devez créer un compte Mailjet et utiliser vos propres clés API.

## Lancer l’API en local

1. S'assurer que les secrets sont définis (voir section ci-dessus).
2. Lancer l'API depuis le projet ApiCinephoria :

```bash
dotnet run --project ApiCinephoria/ApiCinephoria.csproj

Ouvrir Swagger pour tester les endpoints de l'API :
https://localhost:7121/swagger/index.html

## Test

Exécuter les tests unitaires :
dotnet test

# Autres dépôts du projet Cinephoria

- [Cinephoria Front-end (Vue.js)](https://github.com/KZUJL/CinephoriaWeb)
- [Cinephoria Back-end (C# .NET)](https://github.com/ton-org/CinephoriaApi)
- [Cinephoria Mobile (Flutter)](https://github.com/KZUJL/CinephoriaMobileApp)
- [Cinephoria Desktop (C#)](https://github.com/KZUJL/CinephoriaDesktop)
